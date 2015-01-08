using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using MPI;
using Salsa.Core;
using Salsa.Core.Blas;

namespace VectorPairwise
{
    class Program
    {
        private static string _vectorFile;
        private static string _distFile;
        private static bool _normalize;

        private static int _size;
        private static double _dmax = double.MinValue;
        private static double _dmin = double.MaxValue;
        static void Main(string[] args)
        {
            // Load the command line args into our helper class which allows us to name arguments
            Arguments pargs = new Arguments(args)
            {
                Usage = "Usage: VectorPairwise.exe /config=<string>"
            };

            if (pargs.CheckRequired(new[] { "config" }) == false)
            {
                Console.WriteLine(pargs.Usage);
                return;
            }
            using (new MPI.Environment(ref args))
            {
                ReadConfiguration(pargs);
                IList<VectorPoint> vecs = ReadVectors();
                _size = vecs.Count;

                int rank = Communicator.world.Rank;
                int worldSize = Communicator.world.Size;

                Block[][] processToCloumnBlocks = BlockPartitioner.Partition(_size, _size, worldSize, worldSize);
                Block[] myColumnBlocks = processToCloumnBlocks[rank];

                PartialMatrix<double> myRowStrip =
                    new PartialMatrix<double>(myColumnBlocks[0].RowRange, new Range(0, _size - 1));


                ComputeDistanceBlocks(myRowStrip, myColumnBlocks, vecs);
                _dmin = Communicator.world.Allreduce(_dmin, Operation<double>.Min);
                _dmax = Communicator.world.Allreduce(_dmax, Operation<double>.Max);

                if (_dmax < 1) _normalize = false; // no need to normalize whe max distance is also less than 1

                if (rank == 0)
                {
                    Console.WriteLine("Min distance: " + _dmin);
                    Console.WriteLine("Max distance: " + _dmax);
                }

                WriteFullMatrixOnRank0(_distFile, _size, rank, myRowStrip, myColumnBlocks[0].RowRange, processToCloumnBlocks[0][0].RowRange, _normalize, _dmax);
                MPI.Communicator.world.Barrier();
                if (rank ==0)
                {
                    Console.WriteLine("Done.");
                }
            }
        }

        private static void WriteFullMatrixOnRank0(string fileName, int size, int rank, PartialMatrix<double> partialMatrix, Range myRowRange, Range rootRowRange, bool normalize, double dmax)
        {
            FileStream fileStream = null;
            BinaryWriter writer = null;

            int a = size / MPI.Communicator.world.Size;
            int b = size % MPI.Communicator.world.Size;

            /*
             * A note on row ranges and assigned process numbers.
             * First b number of process will have (a + 1) number of rows each.
             * The rest will have only 'a' number of rows. So if a row number, j,
             * falls inside the first set, i.e. j < (b * (a + 1)), then the rank 
             * of the process that handles this row is equal to the integer division
             * of j / (a + 1). Else, i.e. j >= (b * (a + 1)) then that row is 
             * in the second set of processes. Thus, the rank of the process handling
             * this row is equal to the integer calculation of b + [(j - (b * (a + 1)) / a]
             */

            int numOfRowsPerReceive = a;

            Range nextRowRange = null;

            if (rank == 0)
            {
                fileStream = File.Create(fileName, 4194304);
                writer = new BinaryWriter(fileStream);

                // I am rank0 and I am the one who will fill the fullMatrix. So let's fill what I have already.
                for (int i = partialMatrix.GlobalRowStartIndex; i <= partialMatrix.GlobalRowEndIndex; i++)
                {
                    double[] values = partialMatrix.GetRowValues(i);
                    foreach (double value in values)
                    {
                        writer.Write((Int16)((normalize? value/dmax : value)*Int16.MaxValue));
                    }
                }
            }



            // For all the remaining rows that rank0 does not have receive in blocks of rows
            for (int i = rootRowRange.EndIndex + 1; i < size; )
            {
                if (rank == 0)
                {
                    // I am rank0 and let's declare the next row range that I want to receive.
                    int end = i + numOfRowsPerReceive - 1;
                    end = end >= size ? size - 1 : end;
                    nextRowRange = new Range(i, end);
                }

                // Announce everyone about the next row ranges that rank0 has declared.
                MPI.Communicator.world.Broadcast<Range>(ref nextRowRange, 0);

                if (rank == 0)
                {
                    /* I am rank0 and now let's try to receive the declared next row range from others */

                    // A variable to hold the rank of the process, which has the row that I am (rank0) going to receive
                    int processRank;

                    double[] values;
                    for (int j = nextRowRange.StartIndex; j <= nextRowRange.EndIndex; j++)
                    {
                        // Let's find the process that has the row j.
                        processRank = j < (b * (a + 1)) ? j / (a + 1) : b + ((j - (b * (a + 1))) / a);

                        // For each row that I (rank0) require I will receive from the process, which has that row.
                        values = MPI.Communicator.world.Receive<double[]>(processRank, 100);

                        // Set the received values in the fullMatrix
                        foreach (double value in values)
                        {
                            writer.Write((Int16) ((normalize ? value/dmax : value)*Int16.MaxValue));
                        }
                    }
                }
                else
                {
                    /* I am just an ordinary process and I am ready to give rank0 whatever the row it requests if I have that row */

                    // find the intersection of the row ranges of what I (the ordinary process) have and what rank0 wants and then send those rows to rank0
                    if (myRowRange.IntersectsWith(nextRowRange))
                    {
                        Range intersection = myRowRange.GetIntersectionWith(nextRowRange);
                        for (int k = intersection.StartIndex; k <= intersection.EndIndex; k++)
                        {
                            MPI.Communicator.world.Send<double[]>(partialMatrix.GetRowValues(k), 0, 100);
                        }
                    }
                }

                i += numOfRowsPerReceive;
            }

            // I am rank0 and I came here means that I wrote full matrix to disk. So I will clear the writer and stream.
            if (rank == 0)
            {
                writer.Flush();
                fileStream.Close();
                writer.Close();
            }

        }

        private static void ComputeDistanceBlocks(PartialMatrix<double> myRowStrip, Block[] myColumnBlocks, IList<VectorPoint> vecs)
        {
            foreach (Block block in myColumnBlocks)
            {
                for (int r = block.RowRange.StartIndex; r <= block.RowRange.EndIndex; ++r)
                {
                    VectorPoint vr = vecs[r];
                    for (int c = block.ColumnRange.StartIndex; c <= block.ColumnRange.EndIndex; ++c)
                    {
                        VectorPoint vc = vecs[c];
                        double dist = vr.EuclidenDistanceTo(vc);
                        myRowStrip[r, c] = dist;
                        if (dist > _dmax)
                        {
                            _dmax = dist;
                        }

                        if (dist < _dmin)
                        {
                            _dmin = dist;
                        }
                    }
                }
            }
        }


        private static IList<VectorPoint> ReadVectors()
        {
            IList<VectorPoint> vecs = new List<VectorPoint>();
            using (VectorPointsReader reader = new VectorPointsReader(_vectorFile))
            {
                while (!reader.EndOfStream)
                {
                    vecs.Add(reader.ReadVectorPoint());
                }
            }
            return vecs;
        }

        private static void ReadConfiguration(Arguments pargs)
        {
            string config = pargs.GetValue<string>("config");

            /* Reading parameters file */
            using (StreamReader reader = new StreamReader(config))
            {
                char[] sep = new[] { ' ', '\t' };
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    // Skip null/empty and comment lines
                    if (!string.IsNullOrEmpty(line) && !line.StartsWith("#"))
                    {
                        string[] splits = line.Trim().Split(sep);
                        if (splits.Length >= 2)
                        {
                            string value = splits[1];
                            switch (splits[0])
                            {
                                case "VectorFile":
                                    _vectorFile = value;
                                    break;
                                case "DistFile":
                                    _distFile = value;
                                    break;
                                case "Normalize":
                                    _normalize = bool.Parse(value);
                                    break;
                                default:
                                    throw new Exception("Invalide line in configuration file: " + line);
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid line in configuration file: " + line);
                        }
                    }
                }
            }
        }
    }
}
