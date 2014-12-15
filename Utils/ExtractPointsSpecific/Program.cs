using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace ExtractPointsSpecific
{
    class Program
    {
        static void Main(string[] args)
        {
            string extractFromFile = args[0];
            string specificPointsFile = args[1]; // has the format LocalPnum<TAB>PnumInOriginal. The EXTRACTED ORDER IS AS SAME AS POINTS ORDER IN THIS FILE
            string outputFile = args[2];

            Hashtable pnumToPointsTable = ParsePoints(extractFromFile);
            using (SimpleClusterReader reader = new SimpleClusterReader(specificPointsFile)) // not theoretcially a cluster file, but can use cluster reader
            {
                using (StreamWriter writer = new StreamWriter(outputFile))
                {
                    int count = 0;
                    while (!reader.EndOfStream)
                    {
                        Cluster c = reader.ReadCluster();
                        Point p = (Point) pnumToPointsTable[c.Cnum];
                        p.Index = count;
                        writer.WriteLine(p.SimplePointString());
                        ++count;
                    }
                }
            }
            Console.WriteLine("Done.");
            Console.Read();
        }

        private static Hashtable ParsePoints(string pointsFile)
        {
            using (SimplePointsReader reader = new SimplePointsReader(pointsFile))
            {
                Hashtable ht = new Hashtable();
                while (!reader.EndOfStream)
                {
                    Point p = reader.ReadPoint();
                    ht.Add(p.Index, p);
                }
                return ht;
            }
        }
    }
}
