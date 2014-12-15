using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistanceAverager
{
    class Program
    {
        /*static void Main(string[] args)
        {
            var f1 = args[0];
            var f2 = args[1];
            var size = int.Parse(args[2]);
            var isBigEndian = bool.Parse(args[3]);
            var numThreads = int.Parse(args[4]);

            Parallel.For(0, numThreads, (threadNum) =>
                                            {
                                                var div = size/numThreads;
                                                var remainder = size%numThreads;
                                                var numRows = div + (threadNum < remainder ? 1 : 0);
                                                var startRow = (threadNum < remainder)
                                                                   ? threadNum*(div + 1)
                                                                   : remainder + threadNum*div;
                                                var offset = ((long) startRow)*size*2; // 2 for Int16 size in bytes
                                                var length = numRows*size*2; // 2 for Int16 size in bytes

                                                // Create the memory-mapped file. 
                                                using (MemoryMappedFile mmf1 = MemoryMappedFile.CreateFromFile(f1, FileMode.Open, "mat1"), 
                                                    mmf2 = MemoryMappedFile.CreateFromFile(f2, FileMode.Open, "mat2"))
                                                {
                                                    using (MemoryMappedViewAccessor accessor1 = mmf1.CreateViewAccessor(offset, length, MemoryMappedFileAccess.ReadWrite),
                                                        accessor2 = mmf2.CreateViewAccessor(offset, length, MemoryMappedFileAccess.Read))
                                                    {
                                                        // Make changes to the view. 
                                                        /*for (long i = 0; i < length; i += colorSize)
                                                        {
                                                            accessor1.ReadByte()Read(i, out color);
                                                            color.Brighten(10);
                                                            accessor.Write(i, ref color);
                                                        }#1#
                                                    }
                                                }
                                            });
        }

        static Func<int,double> Averager(MemoryMappedViewAccessor ac1, MemoryMappedViewAccessor ac2, bool isBigEndian)
        {
            if (isBigEndian)
            {
                // Java style 
                return new Func<int, double>()
            }
        }*/

        /* Averager for bigendian (Java style) distance matrices */
        public static void Main(string[] args)
        {
            var f1 = args[0];
            var f2 = args[1];
            var size = int.Parse(args[2]);
            var resultName = args[3];
            var result = Path.Combine(Path.GetDirectoryName(f1) ?? string.Empty, resultName);
            using (BinaryReader r1 = new BinaryReader(File.OpenRead(f1)), r2 = new BinaryReader(File.OpenRead(f2)))
            {
                using (var writer = new BinaryWriter(File.OpenWrite(result)))
                {
                    long length = ((long) size)*size*2;
                    for (int i = 0; i < size; ++i)
                    {
                        var rowLengthInBytes = size*2;
                        var row1 = r1.ReadBytes(rowLengthInBytes);
                        var row2 = r2.ReadBytes(rowLengthInBytes);
                        for (int j = 0; j < rowLengthInBytes; j+=2)
                        {
                            var tmp = row1[j];
                            row1[j] = row1[j + 1];
                            row1[j + 1] = tmp;
                            var d1 = BitConverter.ToInt16(row1, j);
                            
                            tmp = row2[j];
                            row2[j] = row2[j + 1];
                            row2[j + 1] = tmp;
                            var d2 = BitConverter.ToInt16(row2, j);

                            Int16 avg = (Int16) (((((double) d1)/Int16.MaxValue + ((double) d2)/Int16.MaxValue)/2)* Int16.MaxValue);
                            writer.Write(avg);
                        }
                        if (i%1000 == 0)
                        {
                            Console.WriteLine("Row " + i + " done");
                        }
                    }
                }
            }
        }
    }
}
