using System;
using System.IO;
using Common;

namespace DoubleToInt16
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string doubleDistFile = args[0];
            int size = int.Parse(args[1]);
            string int16DistFile = Path.Combine(Path.GetDirectoryName(doubleDistFile) ?? string.Empty,
                                                Path.GetFileNameWithoutExtension(doubleDistFile) + "_int16_c#.bin");

            int maxi = -1, maxj = -1, mini = -1, minj = -1, minexi = -1, minexj = -1;
            double max = double.MinValue;
            double min = double.MaxValue;
            double minExcludingDiaganol = double.MaxValue;

            const double smallD = 1.0e-6;
            int lessThanSmallDCount = 0;
            using (var reader = new MatrixReader(doubleDistFile, MatrixType.Double, size))
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        double d = BitConverter.ToDouble(reader.Read(i, j), 0);
                        if (d > max)
                        {
                            max = d;
                            maxi = i;
                            maxj = j;
                        }
                        if (d < min)
                        {
                            min = d;
                            mini = i;
                            minj = j;
                        }
                        if (d < minExcludingDiaganol && i != j)
                        {
                            minExcludingDiaganol = d;
                            minexi = i;
                            minexj = j;
                        }
                        if (d < smallD) ++lessThanSmallDCount;
                        if (d < 0) throw new Exception("error: negative distance at " + i + ", " + j);
                    }
                }
            }

            Console.WriteLine("max: " + max + " row: " + maxi + " col: " + maxj);
            Console.WriteLine("min: " + min + " row: " + mini + " col: " + minj);
            Console.WriteLine("minExcludingDiagonal: " + minExcludingDiaganol + " row: " + minexi + " col: " + minexj);
            Console.WriteLine("SmallD: " + smallD + " count less than that: " + lessThanSmallDCount);

            using (var writer = new BinaryWriter(File.OpenWrite(int16DistFile)))
            {
                using (var reader = new MatrixReader(doubleDistFile, MatrixType.Double, size))
                {
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            Int16 d = (Int16) (BitConverter.ToDouble(reader.Read(i, j), 0)*Int16.MaxValue);
                            writer.Write(d);
                        }
                    }
                }
            }

            Console.WriteLine("done.");
            Console.Read();
        }
    }
}