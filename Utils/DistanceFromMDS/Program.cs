using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bio;
using Bio.IO.FastA;
using Common;
using Common.pviz;
using Point = Common.pviz.Point;

namespace DistanceFromMDS
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = args[0]; // assume this is not a tree pviz if isPviz
            string seqsFile = args[1];
            bool isPviz = bool.Parse(args[2]);

            string dir = Path.GetDirectoryName(file) ?? string.Empty;
            string phylipDist = Path.Combine(dir,
                                             Path.GetFileNameWithoutExtension(file) + "_3d_dist.phylip");
            string binDist = Path.Combine(dir,
                                            Path.GetFileNameWithoutExtension(file) + "_3d_dist_c#.bin");


            Hashtable ptable = new Hashtable();
            PopulatePtable(isPviz, file, ptable);

            using (FastAParser parser = new FastAParser(seqsFile))
            {
                IList<ISequence> seqs = parser.Parse().ToList();
                Console.WriteLine("Working ...");
                double dmax = FindMax(ptable, isPviz); 
                WriteDistances(binDist, phylipDist, isPviz, ptable, seqs, dmax);
            }
            Console.WriteLine("done.");
            Console.Read();



        }

        private static double FindMax(Hashtable ptable, bool isPviz)
        {
            double dmax = double.MinValue;
            int count = ptable.Count;
            for (int i = 0; i < count; i++)
            {
                var pi = ptable[i];
                for (int j = 0; j < count; j++)
                {
                    var pj = ptable[j];
                    double dist = isPviz
                                      ? EuclideanDistance((Point) pi, (Point) pj)
                                      : ((Common.Point) pi).DistanceTo((Common.Point) pj);
                    if (dist > dmax)
                    {
                        dmax = dist;
                    }                 
                }
            }
            return dmax;
        }

        private static void WriteDistances(string binDist, string phylipDist, bool isPviz, Hashtable ptable, IList<ISequence> seqs, double dmax)
        {
            bool normalize = dmax > 1.0;
            Console.WriteLine("normalize: " + normalize);
            using (BinaryWriter binWriter = new BinaryWriter(File.OpenWrite(binDist)))
            {
                using (StreamWriter writer = new StreamWriter(phylipDist))
                {
                    int count = ptable.Count;
                    writer.WriteLine("\t" + count);
                    for (int i = 0; i < count; i++)
                    {
                        var pi = ptable[i];
                        ISequence si = seqs[i];
                        writer.Write(
                            si.ID.Replace(' ', '_').Replace('\t', '_').Replace(':', '_').Replace(',', '_') +
                            "\t");
                        for (int j = 0; j < count; j++)
                        {
                            if (i == j)
                            {
                                writer.Write("000000000\t");
                                binWriter.Write((Int16) 0);
                                continue;
                            }
                            var pj = ptable[j];
                            double dist = isPviz
                                              ? EuclideanDistance((Point) pi, (Point) pj)
                                              : ((Common.Point) pi).DistanceTo((Common.Point) pj);
                            dist = normalize ? dist/dmax : dist;
                            // Doing what Ninja does (i.e.rounding to int) internally with floating point numbers
                            int integerDist = 100*(int) (((100000000*dist) + 50)/100);
                            // This results in a number between [0 - 1E8]
                            writer.Write(integerDist.ToString("000000000") + "\t");
                            // for the binary version write dist * Int16.MaxValue
                            binWriter.Write((Int16) (dist*Int16.MaxValue));

                        }
                        writer.WriteLine();
                    }
                }
            }
        }

        private static void PopulatePtable(bool isPviz, string file, Hashtable ptable)
        {
            if (isPviz)
            {
                PvizModel pviz = PvizModel.LoadPviz(file);
                List<Point> points = pviz.Points;
                foreach (var p in points)
                {
                    if (ptable.ContainsKey(p.Key))
                    {
                        // can't happen for simple pviz files
                        throw new Exception("duplicate points");
                    }
                    ptable.Add(p.Key, p);
                }
            }
            else
            {
                using (SimplePointsReader reader = new SimplePointsReader(file))
                {
                    while (!reader.EndOfStream)
                    {
                        Common.Point p = reader.ReadPoint();
                        if (ptable.ContainsKey(p.Index))
                        {
                            // can't happen for simple pviz files
                            throw new Exception("duplicate points");
                        }
                        ptable.Add(p.Index, p);
                    }
                }
            }
        }


        static double EuclideanDistance(Point p1, Point p2)
        {
            Location l1 = p1.Location, l2 = p2.Location;
            return Math.Sqrt(Math.Pow((l1.X - l2.X), 2) + Math.Pow((l1.Y - l2.Y), 2) + Math.Pow((l1.Z - l2.Z), 2));
        }


            /*double dist = ((double) d)/Int16.MaxValue;
                                // Doing what Ninja does (i.e.rounding to int) internally with floating point numbers
                                int integerDist = 100*(int) (((100000000*dist) + 50)/100); // This results in a number between [0 - 1E8]
                                writer.Write(integerDist.ToString("000000000") + "\t"); // Therefore, need to make room for 9 digits*/
    }
}
