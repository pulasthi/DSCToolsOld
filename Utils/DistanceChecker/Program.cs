using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DistanceChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            string distFile1 = args[0];
            string distFile2 = args[1];
            int bigc = int.Parse(args[2]);

            Int16 d1, d2;
            Console.WriteLine("Working ...");
            long badcount = 0;
            using (BinaryReader d1reader = new BinaryReader(File.OpenRead(distFile1)))
            {
                using (BinaryReader d2reader = new BinaryReader(File.OpenRead(distFile2)))
                {
                    for (int i = 0; i < bigc; i++)
                    {
                        for (int j =0; j < bigc; j++)
                        {
                            d1 = d1reader.ReadInt16();
                            d2 = d2reader.ReadInt16();
                            if (i!=j && d1 != d2)
                            {
//                                double dd1 = (((double)d1)/short.MaxValue);
//                                double dd2 = (((double)d2)/short.MaxValue);
//                                if (Math.Abs(dd1 - dd2) < 0.1)
//                                {
//                                    continue;
//                                }
                                badcount++;
                                Console.WriteLine("Bad dist at seq pair: (" + i + "," + j + ")");
                                Console.WriteLine("  d1: " + d1 + "  d2: " + d2);
//                                Console.WriteLine("Bad count: " + badcount);
                            }
                        }
                    }
                }
            }
            Console.WriteLine("Done.");
            Console.Read();
        }
    }
}
