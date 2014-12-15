using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhylogeneticTreeToPlotViz;

namespace DiscrepancyColorer
{
    class Program
    {
        class RainbowRgb
        {
            public int R, G, B;

            public RainbowRgb(int r, int g, int b)
            {
                R = r;
                G = g;
                B = b;
            }
        }
        static void Main(string[] args)
        {
            var referenceTree = new PVIZFileFormat(args[0]);
            var rotatedTree = new PVIZFileFormat(args[1]);
            var size = int.Parse(args[2]);

            var referenceClusters = referenceTree.Clusters;
            var rotatedClusters = rotatedTree.Clusters;

            

            var colors = new RainbowRgb[]
                             {
                                 new RainbowRgb(255, 0, 0), // red
                                 new RainbowRgb(255,93,0),
                                 new RainbowRgb(255,185,0),
                                 new RainbowRgb(232,255,0),
                                 new RainbowRgb(139,255,0),
                                 new RainbowRgb(46,255,0),
                                 new RainbowRgb(0,255,46),
                                 new RainbowRgb(0,255,139),
                                 new RainbowRgb(0,255,232),
                                 new RainbowRgb(0,185,255),
                                 new RainbowRgb(0,93,255),
                                 new RainbowRgb(0,0,255), // blue
                             };

            var cnumToPoints = new Dictionary<int, PVIZPoint[]>();
            AddToTable(cnumToPoints, 0, referenceTree.Points, size);
            AddToTable(cnumToPoints, 1, rotatedTree.Points, size);

            var cnumToCluster = new Dictionary<int, PVIZCluster>();
            HashClusters(cnumToCluster,rotatedClusters, size);

            for (int i = 0; i < size; i++)
            {
                var refP = cnumToPoints[i][0];
                var rotP = cnumToPoints[i][1];
                double diff =
                    Math.Sqrt(Math.Pow(refP.X - rotP.X, 2) + Math.Pow(refP.Y - rotP.Y, 2) + Math.Pow(refP.Z - rotP.Z, 2));
                var clusterColor = cnumToCluster[i].Color;
                RainbowRgb pickedColor = null;
                if (diff > 0.35)
                {
                    pickedColor = colors[0];
                } 
                else if (diff <= 0.35 && diff > 0.3)
                {
                    pickedColor = colors[1];
                }
                else if (diff <= 0.3 && diff > 0.25)
                {
                    pickedColor = colors[2];
                }
                else if (diff <= 0.25 && diff > 0.2)
                {
                    pickedColor = colors[3];
                }
                else if (diff <= 0.2 && diff > 0.15)
                {
                    pickedColor = colors[4];
                }
                else if (diff <= 0.15 && diff > 0.1)
                {
                    pickedColor = colors[5];
                }
                else if (diff <= 0.1 && diff > 0.05)
                {
                    pickedColor = colors[6];
                }
                else if (diff <= 0.05 && diff > 0.04)
                {
                    pickedColor = colors[7];
                }
                else if (diff <= 0.04 && diff > 0.03)
                {
                    pickedColor = colors[8];
                }
                else if (diff <= 0.03 && diff > 0.02)
                {
                    pickedColor = colors[9];
                }
                else if (diff <= 0.02 && diff > 0.01)
                {
                    pickedColor = colors[10];
                }
                else if (diff <= 0.01)
                {
                    pickedColor = colors[11];
                }
                else
                {
                    pickedColor = new RainbowRgb(255,255,255);// white
                }
                clusterColor.R = pickedColor.R;
                clusterColor.G = pickedColor.G;
                clusterColor.B = pickedColor.B;

            }

            rotatedTree.writeToFile(Path.Combine(Path.GetDirectoryName(args[1])??string.Empty, Path.GetFileNameWithoutExtension(args[1]) + "_discrepancy_colored.pviz"));
        }

        public static void HashClusters(Dictionary<int,PVIZCluster> cnumToCluster, IList<PVIZCluster> clusters, int size)
        {
            foreach (PVIZCluster cluster in clusters)
            {
                int c = int.Parse(cluster.Key);
                if (c >= size) continue;
                cnumToCluster.Add(c,cluster);
            }
        }

        public static void AddToTable(Dictionary<int,PVIZPoint[]> table, int idx, IList<PVIZPoint> points, int size )
        {
            // assuming one cluster per point
            foreach (PVIZPoint p in points)
            {
                if (p.Id >= size) continue; // points of interest are within [0,size-1] in their IDs

                int c = p.Group;
                if (!table.ContainsKey(c))
                {
                    table[c] = new PVIZPoint[2];
                    
                }
                table[c][idx] = p;
            }
            
        }
    }
}
