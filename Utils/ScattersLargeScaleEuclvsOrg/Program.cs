using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;
using MPI;
using Salsa.Core;
using Salsa.Core.Blas;

#if USE_UINT16
using TDistance = System.UInt16;
#elif USE_INT16
using TDistance = System.Int16;
#else
using TDistance = System.Double;
#endif

namespace ScattersLargeScaleEuclvsOrg
{
    class Program
    {
        private static string _mat;
        private static string _pts;
        private static int _size;
        private static string _outdir;

        static double _xmaxbound = 1.0;
        static double _ymaxbound = 1.0;
        static int _xres = 50;
        static int _yres = 50;
        static double _alpha = 2.0;
        static double _pcutf = 0.85;
        static bool _zto1 = true;

        static double _xmaxWhole = double.NegativeInfinity;
        static double _xminWhole = double.PositiveInfinity;

        static double _ymaxWhole = double.NegativeInfinity;
        static double _yminWhole = double.PositiveInfinity;

        static double _xmaxSelected = double.NegativeInfinity;
        static double _xminSelected = double.PositiveInfinity;

        static double _ymaxSelected = double.NegativeInfinity;
        static double _yminSelected = double.PositiveInfinity;

        private static int _transfm;
        private static double _transfp, _distcut;

        private static readonly HashSet<int> SelectedCnums = new HashSet<int>();


        private static double _deltaxWhole, _deltayWhole, _deltaxSelected, _deltaySelected;

        // Surface area of each small 2D square
        private static double _deltasWhole, _deltasSelected;


        static void Main(string[] args)
        {
            // Load the command line args into our helper class which allows us to name arguments
            Arguments pargs = new Arguments(args)
                                  {
                                      Usage = "Usage: ScatterLargeScaleEuclvsOrg.exe /config=<string>"
                                  };

            if (pargs.CheckRequired(new[] { "config" }) == false)
            {
                Console.WriteLine(pargs.Usage);
                return;
            }

            using (new MPI.Environment(ref args))
            {
                ReadConfiguration(pargs);

                Hashtable pnumToPointTable = new Hashtable();
                ReadPoints(pnumToPointTable); // It's OK to read all the points by everyone as it won't take much space.

                int rank = Communicator.world.Rank;
                int worldSize = Communicator.world.Size;
                Block[][] processToCloumnBlocks = BlockPartitioner.Partition(_size, _size, worldSize, worldSize);
                Block[] myColumnBlocks = processToCloumnBlocks[rank];

                PartialMatrix<TDistance> myRowStripMatrixForOriginal = new PartialMatrix<TDistance>(myColumnBlocks[0].RowRange, new Range(0, _size - 1));

                ReadDistanceBlocks(rank, myRowStripMatrixForOriginal, myColumnBlocks, pnumToPointTable);

                // Wait till everyone is done reading their blocks and have local min and max values
                Communicator.world.Barrier();
                Console.WriteLine("All are done reading distance blocks");
                _xminWhole = Communicator.world.Allreduce(_xminWhole, Operation<double>.Min);
                _xmaxWhole = Communicator.world.Allreduce(_xmaxWhole, Operation<double>.Max);
                _yminWhole = Communicator.world.Allreduce(_yminWhole, Operation<double>.Min);
                _ymaxWhole = Communicator.world.Allreduce(_ymaxWhole, Operation<double>.Max);

                _xminSelected = Communicator.world.Allreduce(_xminSelected, Operation<double>.Min);
                _xmaxSelected = Communicator.world.Allreduce(_xmaxSelected, Operation<double>.Max);
                _yminSelected = Communicator.world.Allreduce(_yminSelected, Operation<double>.Min);
                _ymaxSelected = Communicator.world.Allreduce(_ymaxSelected, Operation<double>.Max);

                // Output min/max
                if (rank ==0)
                {
                    Console.WriteLine("xmaxwhole:{0} xminwhole{1} ymaxwhole{2} yminwhole{3}", _xmaxWhole, _xminWhole, _ymaxWhole, _yminWhole);
                    Console.WriteLine("xmaxselected:{0} xminselected{1} ymaxselected{2} yminselected{3}", _xmaxSelected, _xminSelected, _ymaxSelected, _yminSelected);
                }

                // global xmax, xmin, ymax, and ymin should be set by now
                _deltaxWhole = (_xmaxWhole - _xminWhole) / _xres;
                _deltayWhole = (_ymaxWhole - _yminWhole) / _yres;
                _deltasWhole = _deltaxWhole * _deltayWhole;

                _deltaxSelected = (_xmaxSelected - _xminSelected) / _xres;
                _deltaySelected = (_ymaxSelected - _yminSelected) / _yres;
                _deltasSelected = _deltaxSelected * _deltaySelected;

                Hashtable cellsForWholeSample = new Hashtable();
                Hashtable cellsForSelectedClusters = new Hashtable();
                GeneratePartialHistograms(cellsForWholeSample, cellsForSelectedClusters, rank, myRowStripMatrixForOriginal, myColumnBlocks, pnumToPointTable);

                // Wait till everyone is done their part of histogramming
                Communicator.world.Barrier();
                Console.WriteLine("All are done generating partial histograms");

                Hashtable[] cellsArrayForWholeSample = new Hashtable[worldSize];
                Hashtable[] cellsArrayForSelectedClusters = new Hashtable[worldSize];
                Communicator.world.Gather(cellsForWholeSample, 0, ref cellsArrayForWholeSample);
                Communicator.world.Gather(cellsForSelectedClusters, 0, ref cellsArrayForSelectedClusters);

                if (rank == 0)
                {
                    // Rank 0 should have all the cells from each process by now.

                    Console.WriteLine("Rank 0 starting to write density data file for whole sample");
                    GenerateDensityDataFile(cellsArrayForWholeSample, _xmaxWhole, _xminWhole, _ymaxWhole, _yminWhole, _deltaxWhole, _deltayWhole, _deltasWhole, "whole-");
                    Console.WriteLine("Rank 0 done writing density data file for whole sample");
                    Console.WriteLine("Rank 0 starting to write density data file for selected clusters");
                    GenerateDensityDataFile(cellsArrayForSelectedClusters, _xmaxSelected, _xminSelected, _ymaxSelected, _yminSelected, _deltaxSelected, _deltaySelected, _deltasSelected, "selected-");
                    Console.WriteLine("Rank 0 done writing density data file for selected clusters");
                }
            }
        }

        private static void GenerateDensityDataFile(IEnumerable<Hashtable> cellsArray, double xmax, double xmin, double ymax, double ymin, double deltax, double deltay, double deltas, string prefix)
        {
            Hashtable xHist = new Hashtable(_xres);
            Hashtable yHist = new Hashtable(_yres);

            long cellmax = 0, count = 0;
            Hashtable finalcells = new Hashtable();
            foreach (Hashtable t in cellsArray)
            {
                foreach (DictionaryEntry kv in t)
                {
                    string cell = (string)kv.Key;
                    string[] splits = cell.Split(',');
                    int cellx = int.Parse(splits[0]);
                    int celly = int.Parse(splits[1]);
                    long v = (long)kv.Value;
                    finalcells[cell] = finalcells.ContainsKey(cell) ? ((long)finalcells[cell]) + v : v;
                    xHist[cellx] = xHist.ContainsKey(cellx) ? ((long) xHist[cellx]) + v : v;
                    yHist[celly] = yHist.ContainsKey(celly) ? ((long) yHist[celly]) + v : v;
                    if ((long)finalcells[cell] > cellmax)
                    {
                        cellmax = (long)finalcells[cell];
                    }
                    count += v;
                }
            }

            double cellmean = ((double)count) / (_xres * _yres);
            double power = cellmax < (_alpha * cellmean) ? 1.0 : (Math.Log(_alpha) / Math.Log(cellmax / cellmean));
            // Constant value by which the number of points in a 2D square is multiplied.
            // The resulting value is independent of the total number of points as well as 
            // the x,y resolution. The mult value is a factor changing the z value scale.
            double c = _zto1 ? (1.0 / cellmax) : (1.0 / (count * deltas));

            // Output density values
            Console.WriteLine(new string('*', 40));
            Console.WriteLine("DataSet\t" + prefix);
            Console.WriteLine("Zto1\t" + _zto1);
            Console.WriteLine("Count\t" + count);
            Console.WriteLine("Deltas\t" + deltas);
            Console.WriteLine("CellMean\t" + cellmean);
            Console.WriteLine("CellMax\t" + cellmax);
            Console.WriteLine("Power\t" + power);
            Console.WriteLine("Const\t" + c);
            for (int i = 0; i < 10; i++)
            {
                double density = i / 10.0;
                double densityToCount = Math.Pow(density, (1 / power)) / c;
                Console.WriteLine(density + "\t" + densityToCount);
            }
            Console.WriteLine(new string('*', 40));


            int xpointcount = 2 * _xres;
            int ypointcount = 2 * _yres;

            string outFile = Path.Combine(_outdir, prefix+
                                          Path.GetFileNameWithoutExtension(_pts) + "-Vs-" +
                                          Path.GetFileNameWithoutExtension(_mat) + ".txt");

            string xHistOutFile = Path.Combine(_outdir,
                                               prefix + "xHist-" + Path.GetFileNameWithoutExtension(_pts) + "-Vs-" +
                                               Path.GetFileNameWithoutExtension(_mat) + ".txt");

            string yHistOutFile = Path.Combine(_outdir,
                                               prefix + "yHist-" + Path.GetFileNameWithoutExtension(_pts) + "-Vs-" +
                                               Path.GetFileNameWithoutExtension(_mat) + ".txt");
            using (StreamWriter writer = new StreamWriter(outFile), 
                xHistWriter = new StreamWriter(xHistOutFile), yHistWriter= new StreamWriter(yHistOutFile))
            {
                writer.WriteLine("#xcoord\tycoord\thistogramValue");
                xHistWriter.WriteLine("#xval\thistogramvalue");
                yHistWriter.WriteLine("#yval\thistogramvalue");

                // Generating x histogram
                double xoffset = xmin + 0.5*deltax;
                for (int i = 0; i < _xres; ++i)
                {
                    double xcoord = xoffset + i*deltax;
                    xHistWriter.WriteLine(xcoord +"\t"+ (xHist.ContainsKey(i) ? xHist[i] : 0L));
                }

                // Generating y histogram
                double yoffset = ymin + 0.5 * deltay;
                for (int i = 0; i < _yres; ++i)
                {
                    double ycoord = yoffset + i * deltay;
                    yHistWriter.WriteLine(ycoord + "\t" + (yHist.ContainsKey(i) ? yHist[i] : 0L));
                }

                for (int i = 0; i < xpointcount; i++)
                {
                    double x = xmin + ((IsOdd(i) ? (i + 1) / 2 : i / 2) * deltax);
                    int cellx = IsOdd(i) ? (i - 1) / 2 : i / 2;

                    for (int j = 0; j < ypointcount; j++)
                    {
                        double y = ymin + ((IsOdd(j) ? (j + 1) / 2 : j / 2) * deltay);
                        int celly = IsOdd(j) ? (j - 1) / 2 : j / 2;
                        string cell = cellx + "," + celly;

                        double cellvalue = Math.Pow(((finalcells.ContainsKey(cell) ? (long)finalcells[cell] : 0L) * c), power);

                        // todo: commented for now
                        // cellvalue = cellvalue > pcutf ? pcutf : cellvalue < ncutf ? ncutf : cellvalue;
                        cellvalue = cellvalue > _pcutf ? _pcutf : cellvalue;

                        writer.WriteLine(x + "\t" + y + "\t" + cellvalue);
                    }
                    writer.WriteLine();
                }

                // Fill up the remaining region from beyond x=xmax and y=ymax as zero 
                writer.WriteLine();
                writer.WriteLine(xmin + "\t" + ymax + "\t" + 0.0);
                writer.WriteLine(xmin + "\t" + _ymaxbound + "\t" + 0.0);
                writer.WriteLine();
                writer.WriteLine(xmax + "\t" + ymax + "\t" + 0.0);
                writer.WriteLine(xmax + "\t" + _ymaxbound + "\t" + 0.0);
                writer.WriteLine();
                writer.WriteLine(xmax + "\t" + ymin + "\t" + 0.0);
                writer.WriteLine(xmax + "\t" + _ymaxbound + "\t" + 0.0);
                writer.WriteLine();
                writer.WriteLine(_xmaxbound + "\t" + ymin + "\t" + 0.0);
                writer.WriteLine(_xmaxbound + "\t" + _ymaxbound + "\t" + 0.0);
            }
        }

        private static void ReadPoints(Hashtable pnumToPointTable)
        {
            using (SimplePointsReader reader = new SimplePointsReader(_pts))
            {
                while (!reader.EndOfStream)
                {
                    Point p = reader.ReadPoint();
                    // Assume p.Index is unique
                    pnumToPointTable.Add(p.Index, p);
                }
            }
        }

        private static void GeneratePartialHistograms(Hashtable cellsForWholeSample, Hashtable cellsForSelectedClusters,
            int rank, PartialMatrix<TDistance> myRowStripMatrixForOriginal, Block[] myBlocks, Hashtable pnumToPointTable)
        {
            for (int i = 0; i < myBlocks.Length; ++i)
            {
                Block block = myBlocks[i];
                // todo - remove after testing
//                if ((rank == i) || (rank > i && !IsOdd(rank + i)) || (rank < i && IsOdd(rank + i)))
                {
                    // Interesting block for this process
                    if (rank == i)
                    {
                        // Diagonal block
                        for (int r = block.RowRange.StartIndex; r <= block.RowRange.EndIndex; ++r)
                        {
                            // todo - remove after testing
                            // for (int c = block.ColumnRange.StartIndex; c < r; ++c) 
                            for (int c = block.ColumnRange.StartIndex; c <= block.ColumnRange.EndIndex; ++c)
                            {
                                // todo - remove after testing
                                if (r == c) continue;

                                // Each pair in block
                                double xval = ((double)myRowStripMatrixForOriginal[r, c])/TDistance.MaxValue;

                                // Skip if either point r or c is not in the points list (possible with deleted points when run with Manxcat)
                                if (!pnumToPointTable.ContainsKey(r) || !pnumToPointTable.ContainsKey(c)) continue;

                                double yval = GetEuclideanDistance(pnumToPointTable[r] as Point,
                                                                    pnumToPointTable[c] as Point);
                                // ignore values xvals greater than distcut when distcut is specified
                                if (_distcut > -1 && xval > _distcut) continue; 

                                if (_transfm > -1)
                                {
                                    xval = Transform(xval);
                                }
                                UpdateCells(xval, yval, _xmaxWhole, _xminWhole, _ymaxWhole, _yminWhole, _deltaxWhole, _deltayWhole, cellsForWholeSample);

                                // We consider only intra cluster pairs (p1,p2) where both p1,p2 belong to one cluster
                                // when doing the combined histogram of selected clusters. So we will only check if p1,p2 belong 
                                // to the same cluster in our set of selected clusters
                                int rCnum = ((Point)pnumToPointTable[r]).Cluster;
                                int cCnum = ((Point)pnumToPointTable[c]).Cluster;
                                if (SelectedCnums.Contains(rCnum) && SelectedCnums.Contains(cCnum) && rCnum == cCnum)
                                {
                                    UpdateCells(xval, yval, _xmaxSelected, _xminSelected, _ymaxSelected, _yminSelected, _deltaxSelected, _deltaySelected, cellsForSelectedClusters);
                                }
                            }
                        }
                    }
                    else
                    {
                        // Non diagonal block
                        for (int r = block.RowRange.StartIndex; r <= block.RowRange.EndIndex; ++r)
                        {
                            // todo - remove after testing
                            // for (int c = block.ColumnRange.StartIndex; c < r; ++c) 
                            for (int c = block.ColumnRange.StartIndex; c <= block.ColumnRange.EndIndex; ++c)
                            {
                                // Each pair in block
                                double xval = ((double)myRowStripMatrixForOriginal[r, c]) / TDistance.MaxValue;

                                // Skip if either point r or c is not in the points list (possible with deleted points when run with Manxcat)
                                if (!pnumToPointTable.ContainsKey(r) || !pnumToPointTable.ContainsKey(c)) continue;

                                double yval = GetEuclideanDistance(pnumToPointTable[r] as Point,
                                                                    pnumToPointTable[c] as Point);
                                // ignore values xvals greater than distcut when distcut is specified
                                if (_distcut > -1 && xval > _distcut) continue;

                                if (_transfm > -1)
                                {
                                    xval = Transform(xval);
                                }
                                UpdateCells(xval, yval, _xmaxWhole, _xminWhole, _ymaxWhole, _yminWhole, _deltaxWhole, _deltayWhole, cellsForWholeSample);

                                // We consider only intra cluster pairs (p1,p2) where both p1,p2 belong to one cluster
                                // when doing the combined histogram of selected clusters. So we will only check if p1,p2 belong 
                                // to the same cluster in our set of selected clusters
                                int rCnum = ((Point)pnumToPointTable[r]).Cluster;
                                int cCnum = ((Point)pnumToPointTable[c]).Cluster;
                                if (SelectedCnums.Contains(rCnum) && SelectedCnums.Contains(cCnum) && rCnum == cCnum)
                                {
                                    UpdateCells(xval, yval, _xmaxSelected, _xminSelected, _ymaxSelected, _yminSelected, _deltaxSelected, _deltaySelected, cellsForSelectedClusters);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static double Transform(double xval)
        {
            if (_transfm == 10)
            {
                xval= Math.Min(1.0, xval);
                return Math.Pow(xval, _transfp);
            }
            return xval;
        }

        private static void ReadDistanceBlocks(int rank, PartialMatrix<TDistance> myRowStripMatrixForOriginal, Block[] myColumnBlocks, Hashtable pnumToPointTable)
        {
#if USE_UINT16
            using (MatrixReader matAReader = new MatrixReader(_mat, MatrixType.UInt16, _size))
#elif USE_INT16
            using (MatrixReader matReader = new MatrixReader(_mat, MatrixType.Int16, _size))
#else
            using (MatrixReader matAReader = new MatrixReader(_mat, MatrixType.Double, _size))
#endif
            {
                for (int i = 0; i < myColumnBlocks.Length; ++i)
                {
                    Block block = myColumnBlocks[i];
                    // todo - remove after testing
//                    if ((rank == i) || (rank > i && !IsOdd(rank + i)) || (rank < i && IsOdd(rank + i)))
                    {
                        // Interesting block for this process
                        if (rank == i)
                        {
                            // Diagonal block
                            for (int r = block.RowRange.StartIndex; r <= block.RowRange.EndIndex; ++r) // block.RowRange should be equivalent to myRowStripMatrix's row range
                            {
                                // todo - remove after testing
                                // for (int c = block.ColumnRange.StartIndex; c < r; ++c) 
                                for (int c = block.ColumnRange.StartIndex; c <= block.ColumnRange.EndIndex; ++c)
                                {
                                    // todo - remove after testing
                                    if (r == c) continue;

                                    // Each pair in block
                                    TDistance t = FromArrayToTdist(matReader.Read(r, c));
                                    myRowStripMatrixForOriginal[r, c] = t;

                                    double x = ((double) t)/TDistance.MaxValue;

                                    // Skip if either point r or c is not in the points list (possible with deleted points when run with Manxcat)
                                    if (!pnumToPointTable.ContainsKey(r) || !pnumToPointTable.ContainsKey(c)) continue;

                                    double y = GetEuclideanDistance(pnumToPointTable[r] as Point,
                                                                    pnumToPointTable[c] as Point);

                                    // ignore values xvals greater than distcut when distcut is specified
                                    if (_distcut > -1 && x > _distcut) continue;

                                    if (_transfm > -1)
                                    {
                                        x = Transform(x);
                                    }

                                    UpdateMinMax(x, y, ref _xmaxWhole, ref _xminWhole, ref _ymaxWhole, ref _yminWhole);

                                    // We consider only intra cluster pairs (p1,p2) where both p1,p2 belong to one cluster
                                    // when doing the combined histogram of selected clusters. So we will only check if p1,p2 belong 
                                    // to the same cluster in our set of selected clusters
                                    int rCnum = ((Point)pnumToPointTable[r]).Cluster;
                                    int cCnum = ((Point)pnumToPointTable[c]).Cluster;
                                    if (SelectedCnums.Contains(rCnum) && SelectedCnums.Contains(cCnum) && rCnum == cCnum)
                                    {
                                        UpdateMinMax(x, y, ref _xmaxSelected, ref _xminSelected, ref _ymaxSelected, ref _yminSelected);
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Non diagonal block
                            for (int r = block.RowRange.StartIndex; r <= block.RowRange.EndIndex; ++r)
                            {
                                // todo - remove after testing
                                // for (int c = block.ColumnRange.StartIndex; c < r; ++c) 
                                for (int c = block.ColumnRange.StartIndex; c <= block.ColumnRange.EndIndex; ++c)
                                {
                                    // Each pair in block
                                    TDistance t = FromArrayToTdist(matReader.Read(r, c));
                                    myRowStripMatrixForOriginal[r, c] = t;

                                    double x = ((double)t) / TDistance.MaxValue;

                                    // Skip if either point r or c is not in the points list (possible with deleted points when run with Manxcat)
                                    if (!pnumToPointTable.ContainsKey(r) || !pnumToPointTable.ContainsKey(c)) continue;

                                    double y = GetEuclideanDistance(pnumToPointTable[r] as Point,
                                                                    pnumToPointTable[c] as Point);

                                    // ignore values xvals greater than distcut when distcut is specified
                                    if (_distcut > -1 && x > _distcut) continue;

                                    if (_transfm > -1)
                                    {
                                        x = Transform(x);
                                    }

                                    UpdateMinMax(x, y, ref _xmaxWhole, ref _xminWhole, ref _ymaxWhole, ref _yminWhole);

                                    // We consider only intra cluster pairs (p1,p2) where both p1,p2 belong to one cluster
                                    // when doing the combined histogram of selected clusters. So we will only check if p1,p2 belong 
                                    // to the same cluster in our set of selected clusters
                                    int rCnum = ((Point)pnumToPointTable[r]).Cluster;
                                    int cCnum = ((Point)pnumToPointTable[c]).Cluster;
                                    if (SelectedCnums.Contains(rCnum) && SelectedCnums.Contains(cCnum) && rCnum == cCnum)
                                    {
                                        UpdateMinMax(x, y, ref _xmaxSelected, ref _xminSelected, ref _ymaxSelected, ref _yminSelected);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void UpdateMinMax(double x, double y, ref double xmax, ref double xmin, ref double ymax, ref double ymin)
        {
            if (x > xmax)
            {
                xmax = x;
            }

            if (x < xmin)
            {
                xmin = x;
            }

            if (y > ymax)
            {
                ymax = y;
            }

            if (y < ymin)
            {
                ymin = y;
            }
        }

        private static void UpdateCells(double x, double y, double xmax, double xmin, double ymax, double ymin,
            double deltax, double deltay, Hashtable cells)
        {
            // cell number based on zero index from bottom left corner
            // if x is equal to xmax then it's placed in the last cell, which is xres-1 in zero based index
            // same is done for y when y == ymax
            int cellx = x == xmax ? _xres - 1 : (int)Math.Floor((x - xmin) / deltax);
            int celly = y == ymax ? _yres - 1 : (int)Math.Floor((y - ymin) / deltay);

            if (x > xmax || y > ymax || x < xmin || y < ymin)
            {
                // now this should never be reached
                throw new Exception("bad(1)-> x: " + x + " y: " + y + " xmax: " + xmax + " xmin: " + xmin + " ymax: " +
                                    ymax + " ymin: " + ymin);
            }

            if (cellx >= _xres || celly >= _yres)
            {
                // now this should never be reached
                throw new Exception("bad(2)-> x: " + x + " y:" + y + " xmax: " + xmax + " xmin: " + xmin + " ymax: " +
                                    ymax + " ymin: " + ymin + " cellx: " + cellx + " celly: " + celly);
            }

            string cell = cellx + "," + celly;
            cells[cell] = cells.ContainsKey(cell) ? ((long)cells[cell]) + 1 : 1L;
        }

        private static TDistance FromArrayToTdist(byte[] xarr)
        {
#if USE_UINT16
           return BitConverter.ToUInt16(xarr, 0);
#elif USE_INT16
           return BitConverter.ToInt16(xarr, 0);
#else
           return BitConverter.ToDouble(xarr, 0);
#endif
        }

        static double GetEuclideanDistance(Point p, Point q)
        {
            return Math.Sqrt(Math.Pow((p.X - q.X), 2) + Math.Pow((p.Y - q.Y), 2) + Math.Pow((p.Z - q.Z), 2));
        }

        private static void ReadConfiguration(Arguments pargs)
        {
            string config = pargs.GetValue<string>("config");
            using (StreamReader reader = new StreamReader(config))
            {
                char[] sep = new[] { ' ', '\t' };
                /* Reading parameters file */
// ReSharper disable PossibleNullReferenceException
                _mat = reader.ReadLine().Trim().Split(sep)[1];
                _pts = reader.ReadLine().Trim().Split(sep)[1];
                _size = int.Parse(reader.ReadLine().Trim().Split(sep)[1]);
                _outdir = reader.ReadLine().Trim().Split(sep)[1];

                _xmaxbound = double.Parse(reader.ReadLine().Trim().Split(sep)[1]);
                _ymaxbound = double.Parse(reader.ReadLine().Trim().Split(sep)[1]);
                _xres = int.Parse(reader.ReadLine().Trim().Split(sep)[1]);
                _yres = int.Parse(reader.ReadLine().Trim().Split(sep)[1]);
                _alpha = double.Parse(reader.ReadLine().Trim().Split(sep)[1]);
                string value = reader.ReadLine().Trim().Split(sep)[1];
                _pcutf = "+inf".Equals(value) ? double.MaxValue : double.Parse(value);
                value = reader.ReadLine().Trim().Split(sep)[1];
                _zto1 = bool.Parse(value);
                value = reader.ReadLine().Trim().Split(sep)[1];
                _transfm = int.Parse(value);
                value = reader.ReadLine().Trim().Split(sep)[1];
                _transfp = _transfm > -1 ? double.Parse(value) : -1.0;
                value = reader.ReadLine().Trim().Split(sep)[1];
                _distcut = double.Parse(value);
                value = reader.ReadLine().Trim().Split(sep)[1];
// ReSharper restore PossibleNullReferenceException
                if(!string.IsNullOrEmpty(value) && !"none".Equals(value))
                {
                    foreach (int c in value.Trim().Split(new[] { ',' }).Select(x => int.Parse(x)))
                    {
                        if (!SelectedCnums.Contains(c)){
                            SelectedCnums.Add(c);
                        }
                    }   
                }
            }
        }

        private static bool IsOdd(int value)
        {
            return (value & 1) == 1;
        }
    }
}
