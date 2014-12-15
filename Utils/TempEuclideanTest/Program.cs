using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common;

namespace TempEuclideanTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string pointsFile = args[0];
            using (VectorPointsReader reader = new VectorPointsReader(pointsFile))
            {
                IList<VectorPoint> points = new List<VectorPoint>();
                while (!reader.EndOfStream)
                {
                    points.Add(reader.ReadVectorPoint());
                }

                string outputFile = Path.Combine(Path.GetDirectoryName(pointsFile)??string.Empty,
                                             "euclidean_" + Path.GetFileName(pointsFile));

                double dmax = double.MinValue;
                double dmin = double.MaxValue;
                using (StreamWriter writer = new StreamWriter(outputFile))
                {
                    VectorPoint pi, pj;
                  
                    for (int i = 0; i < points.Count; i++)
                    {
                        pi = points[i];
                        for (int j = 0; j < points.Count; j++)
                        {
                            pj = points[j];
                            double d = pi.EuclidenDistanceTo(pj);
                            if (d > dmax) dmax = d;
                            if (d < dmin) dmin = d;
                            writer.Write(d+"\t");
                        }
                        writer.WriteLine();
                    }
                  
                }

                Console.WriteLine("dmax: " + dmax + " dmin: " + dmin);
                Console.Read();
            }
        }
    }
}
