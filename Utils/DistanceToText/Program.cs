using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common;

namespace DistanceToText
{
    class Program
    {
        static void Main(string[] args)
        {
            string distanceFile = args[0];
            int size = int.Parse(args[1]);
            bool readAsDouble = bool.Parse(args[2]);

            using (MatrixReader reader = new MatrixReader(distanceFile, readAsDouble? MatrixType.Double : MatrixType.Int16, size))
            {
                string outFile = Path.Combine((Path.GetDirectoryName(distanceFile)??string.Empty),
                                              Path.GetFileNameWithoutExtension(distanceFile) + "_text.txt");
                using (StreamWriter writer = new StreamWriter(outFile))
                {
                    string template = "({0},{1})\t{2}";
                    for (int i = 0; i < size; ++i)
                    {
                        for (int j = 0; j < size; ++j)
                        {
                            double d = readAsDouble ? BitConverter.ToDouble(reader.Read(i, j), 0) : (((double)BitConverter.ToInt16(reader.Read(i, j), 0))/Int16.MaxValue);
                           
                            writer.Write(d+"\t");
                           /* if (readAsDouble)
                            {
                                if (j > i)
                                {
                                    writer.WriteLine(string.Format(template, i, j, reader.ReadDouble()));
                                }
                                else
                                {
                                    reader.ReadDouble(); // ignore the bottom triangle including diagonal
                                }
                            }
                            else
                            {
                                //                                if (j > i)
                                if (i == 322 || i == 613 || i == 2134 || i == 1806 || i == 22 || i == 2176 || i == 2815 ||
                                    i == 338 || i == 3322)
                                {
                                    writer.WriteLine(string.Format(template, i, j,
                                                                   (((double) reader.ReadInt16())/short.MaxValue)));
                                }
                                else
                                {
                                    reader.ReadInt16(); // ignore the bottom triangle including diagonal
                                }
                            }*/
                        }
                        writer.WriteLine();
                    }
                }
            }
            Console.WriteLine("Done.");
            Console.Read();
        }
    }
}
