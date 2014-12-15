using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PointsToClusterGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string pointsFile = args[0];
            int pointsFileBase = int.Parse(args[1]);
            int clusterFileBase = int.Parse(args[2]);
            string outFileName = args[3];


            int diff = clusterFileBase - pointsFileBase;

            char [] sep = new[]{' ','\t'};

            using (StreamReader reader = new StreamReader(pointsFile))
            {
                using (StreamWriter writer = new StreamWriter(Path.Combine(Path.GetDirectoryName(pointsFile), outFileName)))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] splits = line.Trim().Split(sep);
                        int cnum = int.Parse(splits[4]);

                        cnum += diff;

                        writer.WriteLine(splits[0] + "\t" + cnum);
                    }
                }
            }

        }
    }
}
