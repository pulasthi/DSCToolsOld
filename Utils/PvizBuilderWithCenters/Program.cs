using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PvizBuilderWithCenters
{
    class Program
    {
        public static void Main(string[] args)
        {
//            PvizBuilder builder = new PvizBuilder(args[0],args[1]);
//            builder.BuildOverallBestTopN(8);

            TestCodeBeforeHandingOutToDrFox();
        }

        public static void TestCodeBeforeHandingOutToDrFox()
        {
            /* Generate all types of center clusters per cluster 
             * 
             * Center clusters are,
             *  1. Original Min Mean
             *  2. MDS Min Mean
             *  3. MDS Center of Gravity (CoG)
             *  4. Overall Best
             *  5. Bucket Fraction 0
             *     Bucket Fraction 1 and so on
             *     
             * Number of center points to include in each center type = n 
             * n <= N, which is the number of center points found for each center type by PWC
             * N is specified through NumberOfCenters parameter in PWC
             * 
             * Assumes a center file from a PWC center finding run
             * Assumes a points file, which has each point mapped to its cluster in the format 
             *  PointNumber<TAB>Xcoord<TAB>Ycoord<TAB>Zcoord<TAB>ClusterNumber
             */

            string centerFileWithNoBucketFractions =
                @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\haixu_release\plots\mega_regions\1\refined_r1-CenterFile-M19-C19.txt";
            string pointsFileWithClusterInfo =
                @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\haixu_release\plots\mega_regions\1\refined_haixu_1(19)_zeroidx.txt";

            int numberOfCenterPointsToIncludeInEachCenterType = 3; 

            List<Color> matlab50Colors = GenerateMatlab50Colors();

            XElement clustersElement = new XElement("clusters");
            XElement pointsElement = new XElement("points");

            HashSet<int> existingCnumsSet = new HashSet<int>();
            Hashtable existingPointsTable = new Hashtable();
            int maxpnum = -1;
            ProcessPointsFile(pointsFileWithClusterInfo, clustersElement, pointsElement, ref maxpnum, existingPointsTable, existingCnumsSet, matlab50Colors);

            Hashtable groupTable = new Hashtable();
            ProcessCenterFile(centerFileWithNoBucketFractions, groupTable);

            CreatePlotMethodThree(Path.GetDirectoryName(pointsFileWithClusterInfo),
                                Path.GetFileNameWithoutExtension(pointsFileWithClusterInfo),
                                clustersElement, pointsElement, maxpnum, existingPointsTable,
                                existingCnumsSet, matlab50Colors, groupTable, numberOfCenterPointsToIncludeInEachCenterType);
            Console.WriteLine("Done plot one.");
        }

        private static void CreatePlotMethodThree(string outDir, string outNamePrefix, XElement clustersElement, XElement pointsElement, int maxpnum, Hashtable existingPointsTable, HashSet<int> existingCnumsSet, List<Color> matlab50Colors, Hashtable groupTable, int numberOfCenterPointsToIncludeInEachCenterType)
        {
            int max = existingCnumsSet.Max() + 1;
            foreach (DictionaryEntry kv in groupTable)
            {
                var group = (int) kv.Key;
                var methodTable = (Hashtable) kv.Value;
                int methodCount = methodTable.Count;
                int tempCount = methodCount;
                foreach (DictionaryEntry mc in methodTable)
                {
                    var method = (string) mc.Key;

                    var clusterNumberForCenterType = group*methodCount + (methodCount - tempCount--) + max;
                    var centerTypeName = group + "." + method + ".centerpoints";
                    clustersElement.Add(CreateClusterElement(clusterNumberForCenterType, centerTypeName,
                                                             matlab50Colors[group%matlab50Colors.Count], false, 2.0));

                    var cps = (List<CenterInfo>) mc.Value;
                    // Picking the topmost n point for each method
                    for (int i = 0; i < numberOfCenterPointsToIncludeInEachCenterType; i++)
                    {
                        CenterInfo cp = cps[i];
                        Point p = (Point)existingPointsTable[cp.Pnum];
                        pointsElement.Add(CreatePointElement(++maxpnum, clusterNumberForCenterType,
                                                             ("cluster:" + group + "-idx:" + p.Index + "method:" +
                                                              method),
                                                             p.X, p.Y, p.Z));
                    }
                }
            }

            string pvizName = outNamePrefix + "_with_" + numberOfCenterPointsToIncludeInEachCenterType + "_center(s)_for_each_center_type";
            XElement plotElement = CreatePlotElement(pvizName, true);
            XElement plotvizElement = new XElement("plotviz");
            plotvizElement.Add(plotElement);
            plotvizElement.Add(clustersElement);
            plotvizElement.Add(pointsElement);
            string pvizFile = Path.Combine(outDir, pvizName + ".pviz");
            plotvizElement.Save(pvizFile);

        }


        static void MainOld(string[] args)
        {
            List<Color> matlab50Colors = GenerateMatlab50Colors();

            string centerFile = args[0];
//                @"C:\Users\sekanaya\Desktop\pwc-centers\haixu_region_6(11)_centers_nobuckets\CenterFile-M11-C11.txt";
            string pointsFile = args[1];
//                @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\haixu_release\plots\mega_regions\6\haixu_region_6(11)_zeroidx.txt";

            XElement clustersElement = new XElement("clusters");
            XElement pointsElement = new XElement("points");

            HashSet<int> existingCnumsSet = new HashSet<int>();
            Hashtable existingPointsTable = new Hashtable();
            int maxpnum = -1;
            ProcessPointsFile(pointsFile, clustersElement, pointsElement, ref maxpnum, existingPointsTable, existingCnumsSet, matlab50Colors);

            Hashtable groupTable = new Hashtable();
            ProcessCenterFile(centerFile, groupTable);


            /* This method creates a plot with centers for each cluster. Centers are the top points in each category, i.e.
             * 1. Original Min Mean
             * 2. MDS Min Mean
             * 3. Nearest Point to Center of Mass
             * 4. 3 bucket choices
             * 5. Overall best
             */
//            CreatePlotMethodOne(Path.GetDirectoryName(pointsFile), Path.GetFileNameWithoutExtension(pointsFile), CloneElement(clustersElement), CloneElement(pointsElement), maxpnum, existingPointsTable, existingCnumsSet, matlab50Colors, groupTable);
//            Console.WriteLine("Done plot one.");


            /* Produces 5 Plots covering
             * 1. Original Min Mean
             * 2. MDS Min Mean
             * 3. Nearest Point to Center of Mass
             * 4. 3 bucket choices
             * 5. Overall best (only those with Source=Both)
             */
            string[] methods = new[]
                                   {
//                                       "SmallestDistanceMeans",
//                                       "SmallestMDSDistanceMeans",
//                                       "SmallestMDSCoG",
//                                       "LargestCountsBucket", 
                                       "OverallBest"
                                   };
            foreach (string method in methods)
            {
                CreatePlotMethodTwo(Path.GetDirectoryName(pointsFile), Path.GetFileNameWithoutExtension(pointsFile),
                                    CloneElement(clustersElement), CloneElement(pointsElement), maxpnum,
                                    existingPointsTable, existingCnumsSet, matlab50Colors, groupTable, method);
                Console.WriteLine("Done " + method);
                
            }


            Console.ReadKey();
        }

        private static void CreatePlotMethodTwo(string outDir, string outNamePrefix, XElement clustersElement, XElement pointsElement, int maxpnum, Hashtable existingPointsTable, HashSet<int> existingCnumsSet, List<Color> matlab50Colors, Hashtable groupTable, string method)
        {
            int max = existingCnumsSet.Max() + 1;

            foreach (DictionaryEntry gm in groupTable)
            {
                int group = (int) gm.Key;
                double clusterGlyphSize = 2.0;

                if (!method.Equals("LargestCountsBucket") && !method.Equals("OverallBest"))
                {
                    List<CenterInfo> cps = (List<CenterInfo>)((Hashtable)gm.Value)[method];
                    maxpnum = AddCenterPointsToPointsElement(group, max, cps, existingPointsTable, pointsElement, maxpnum, -1);
                }
                else if (method.Equals("LargestCountsBucket"))
                {
                    Hashtable methodTable = (Hashtable) gm.Value;
                    int bucketNum = 0;
                    string fullMethod = method + "-" + bucketNum;
                    while (methodTable.ContainsKey(fullMethod))
                    {
                        List<CenterInfo> cps = (List<CenterInfo>) methodTable[fullMethod];
                        maxpnum = AddCenterPointsToPointsElement(group, max, cps, existingPointsTable, pointsElement, maxpnum, bucketNum);
                        fullMethod = method + "-" + ++bucketNum;
                    }
                }
                else if (method.Equals("OverallBest"))
                {
                    List<CenterInfo> cps = ((List<CenterInfo>) ((Hashtable) gm.Value)[method]);
                    // Top 8 (or all if less than 8) points in overall best
                    List<CenterInfo> temp = new List<CenterInfo>();
                    for (int i = 0; i < 8 && i < cps.Count; ++i )
                    {
                        temp.Add(cps[i]);
                    }
                    maxpnum = AddCenterPointsToPointsElement(group, max, temp, existingPointsTable, pointsElement, maxpnum, -1);
                    
                    // Note. you can't have this in the code simultaneously with top8 at the moment
                    // Souce=Both points in overall beset 
//                    temp = cps.Where(cp => cp.Source.Equals("Both")).ToList();
//                    maxpnum = AddCenterPointsToPointsElement(group, max, temp, existingPointsTable, pointsElement, maxpnum, -1);
                }
                clustersElement.Add(CreateClusterElement(group + max, (method + " " + group), matlab50Colors[group % matlab50Colors.Count], false, clusterGlyphSize));
            }

            XElement plotElement = CreatePlotElement(outNamePrefix + "_with_" + method + "_centers", true);
            XElement plotvizElement = new XElement("plotviz");
            plotvizElement.Add(plotElement);
            plotvizElement.Add(clustersElement);
            plotvizElement.Add(pointsElement);
            string pvizFile = Path.Combine(outDir, outNamePrefix + "_with_" + method + "_centers.pviz");
            plotvizElement.Save(pvizFile);
        }

        private static int AddCenterPointsToPointsElement(int group, int max, IEnumerable<CenterInfo> cps, Hashtable existingPointsTable, XElement pointsElement, int maxpnum, int bucket)
        {
            int count = 0;
            foreach (CenterInfo cp in cps)
            {
                Point p = (Point) existingPointsTable[cp.Pnum];
                if (bucket >= 0)
                {
                    pointsElement.Add(CreatePointElement(++maxpnum, (group + max),
                                                     ("cluster:" + group + "-idx:" + p.Index + "-bucket:" + bucket + "-rank:" + count++),
                                                     p.X, p.Y, p.Z));
                }
                else
                {
                    pointsElement.Add(CreatePointElement(++maxpnum, (group + max),
                                                         ("cluster:" + group + "-idx:" + p.Index + "-rank:" + count++),
                                                         p.X, p.Y, p.Z));
                }
            }
            return maxpnum;
        }

        private static void CreatePlotMethodOne(string outDir, string outNamePrefix, XElement clustersElement, XElement pointsElement, int maxpnum, Hashtable existingPointsTable, HashSet<int> existingCnumsSet, List<Color> matlab50Colors, Hashtable groupTable)
        {
            int max = existingCnumsSet.Max() + 1;
            HashSet<int> centerCnumsSet = new HashSet<int>();
            foreach (DictionaryEntry kv in groupTable)
            {
                int group = (int)kv.Key;
                if (!centerCnumsSet.Contains(group))
                {
                    centerCnumsSet.Add(group);
                    clustersElement.Add(CreateClusterElement(group + max, group + ".centers", matlab50Colors[group % matlab50Colors.Count], false, 2.0));
                }

                Hashtable methodTable = (Hashtable) kv.Value;
                foreach (DictionaryEntry mc in methodTable)
                {
                    string method = (string) mc.Key;
                    List<CenterInfo> cps = (List<CenterInfo>)mc.Value;
                    // Picking the topmost point for each method
                    CenterInfo top = cps[0];
                    Point topPoint = (Point) existingPointsTable[top.Pnum];
                    pointsElement.Add(CreatePointElement(++maxpnum, (group + max),
                                                         ("cluster:" + group + "-idx:" + topPoint.Index + "method:" + method),
                                                         topPoint.X, topPoint.Y, topPoint.Z));
                }
            }

            XElement plotElement = CreatePlotElement(outNamePrefix + "_with_centers", true);
            XElement plotvizElement = new XElement("plotviz");
            plotvizElement.Add(plotElement);
            plotvizElement.Add(clustersElement);
            plotvizElement.Add(pointsElement);
            string pvizFile = Path.Combine(outDir, outNamePrefix+"_with_centers.pviz");
            plotvizElement.Save(pvizFile);
            
        }

        private static void ProcessPointsFile(string pointsFile, XElement clusters, XElement points, ref int maxpnum, Hashtable pointsTable, HashSet<int> clusterNumbers, List<Color> matlab50Colors)
        {
            using (StreamReader reader = new StreamReader(pointsFile))
            {
                while (!reader.EndOfStream)
                {
                    Point p = ReadPointLine(reader.ReadLine().Trim());
                    if (maxpnum < p.Index)
                    {
                        maxpnum = p.Index;
                    }
                    pointsTable.Add(p.Index, p);
                    if (!clusterNumbers.Contains(p.Cluster))
                    {
                        clusterNumbers.Add(p.Cluster);
                        clusters.Add(CreateClusterElement(p.Cluster, p.Cluster.ToString(), matlab50Colors[p.Cluster % matlab50Colors.Count], false, 0.1));
                    }
                    points.Add(CreatePointElement(p.Index,p.Cluster,string.Empty, p.X, p.Y, p.Z));
                }
            }
        }

        private static List<Color> GenerateMatlab50Colors()
        {
            using (StreamReader reader = new StreamReader("Matlab50.txt"))
            {
                List<Color> colors = new List<Color>();
                char[] sep = new[] { ' ', '\t' };
                string[] splits;
                string split;
                int startIdx = 3;
                int r, g, b, a;
                while (!reader.EndOfStream)
                {
                    splits = reader.ReadLine().Trim().Split(sep);

                    split = splits[0];
                    r = int.Parse(split.Substring(startIdx, (split.Length - (startIdx + 1))));

                    split = splits[1];
                    g = int.Parse(split.Substring(startIdx, (split.Length - (startIdx + 1))));

                    split = splits[2];
                    b = int.Parse(split.Substring(startIdx, (split.Length - (startIdx + 1))));

                    split = splits[3];
                    a = int.Parse(split.Substring(startIdx, (split.Length - (startIdx + 1))));

                    colors.Add(Color.FromArgb(a, r, g, b));
                }
                return colors;
            }
        }

        private static Point ReadPointLine(string line)
        {
            char[] sep = new[] { ' ', '\t' };
            string[] splits = line.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            Point p = new Point(double.Parse(splits[1]), double.Parse(splits[2]), double.Parse(splits[3]),int.Parse(splits[0]),int.Parse(splits[4]));
            return p;
        }

        private static CenterInfo ReadCenterLine(string line)
        {
            char[] sep = new[] { ' ', '\t' };
            char[] eqsep = new[] { '=' };
            string[] splits = line.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            int pnum = int.Parse(splits[0].Split(eqsep)[1]);
            double measure = double.Parse(splits[1].Split(eqsep)[1]);
            int methodIdx = 2;
            string source = string.Empty;
            double count = 0.0;
            if (splits[2].StartsWith("Count"))
            {
                methodIdx = 4;
                count = double.Parse(splits[2].Split(eqsep)[1]);
                source = splits[3].Split(eqsep)[1];
            } 
            string method =  splits[methodIdx].Split(eqsep)[1];
            int group = int.Parse(splits[methodIdx + 1].Split(eqsep)[1]);
            string seqName = splits[methodIdx + 2].Split(eqsep)[1];
            for (int i = methodIdx + 3; i < splits.Length - 4; ++i)
            {
                seqName += (" " + splits[i]);
            }
            int seqLength = int.Parse(splits[splits.Length - 4].Split(eqsep)[1]);
            return new CenterInfo(pnum, measure, method, group, seqName, seqLength, source, count);
        }

        private static void ProcessCenterFile(string centerFile, Hashtable groupTable)
        {
            using (StreamReader reader = new StreamReader(centerFile))
            {
                while (!reader.EndOfStream)
                {
                    CenterInfo cp = ReadCenterLine(reader.ReadLine());
                    AddToGroupTable(groupTable, cp);
                }
            }
        }

        private static void AddToGroupTable(Hashtable groupTable, CenterInfo cp)
        {
            if (groupTable.ContainsKey(cp.Cluster))
            {
                Hashtable methodTable = (Hashtable) groupTable[cp.Cluster];
                if (methodTable.ContainsKey(cp.Method))
                {
                    // Need a list to maintain the order of points
                    List<CenterInfo> cps = (List<CenterInfo>) methodTable[cp.Method];
                    cps.Add(cp);
                }
                else
                {
                    // Need a list to maintain the order of points
                    List<CenterInfo> cps = new List<CenterInfo> { cp };
                    methodTable[cp.Method] = cps;
                }
            }
            else
            {
                // Need a list to maintain the order of points
                List<CenterInfo> cps = new List<CenterInfo> {cp};
                Hashtable methodTable = new Hashtable();
                methodTable[cp.Method] = cps;
                groupTable[cp.Cluster] = methodTable;
            }
        }


        private static XElement CreatePlotElement(string name, bool glyphVisible)
        {
            XElement plot =
                new XElement("plot",
                             new XElement("title", name),
                             new XElement("pointsize", 1),
                             new XElement("glyph",
                                          new XElement("visible", glyphVisible? 1: 0),
                                          new XElement("scale", 1)),
                             new XElement("camera",
                                          new XElement("focumode",0),
                                          new XElement("focus",
                                                       new XAttribute("x",0),
                                                       new XAttribute("y",0),
                                                       new XAttribute("z",0))));
            return plot;
        }

        private static XElement CreateClusterElement(int key, string label, Color color, bool isDefault, double size)
        {
            XElement cluster =
                new XElement("cluster",
                             new XElement("key", key),
                             new XElement("label", label),
                             new XElement("visible", 1),
                             new XElement("default", isDefault ? 1 : 0),
                             new XElement("color",
                                          new XAttribute("r", color.R),
                                          new XAttribute("g", color.G),
                                          new XAttribute("b", color.B),
                                          new XAttribute("a", color.A)),
                             new XElement("size", size));
            return cluster;
        }

        private static XElement CreatePointElement(int key, int clusterKey, string label, double x, double y, double z)
        {
            XElement point =
                new XElement("point",
                             new XElement("key", key),
                             new XElement("clusterkey", clusterKey),
                             new XElement("label", label),
                             new XElement("location",
                                          new XAttribute("x", x),
                                          new XAttribute("y", y),
                                          new XAttribute("z", z)));
            return point;
        }

        // Sweet recursive method to clone an XElement
        // grabbed from http://blogs.msdn.com/b/ericwhite/archive/2009/01/28/manually-cloning-linq-to-xml-trees.aspx
        private static XElement CloneElement(XElement element)
        {
            return new XElement(element.Name,
                                element.Attributes(),
                                element.Nodes().Select(n =>
                                                           {
                                                               XElement e = n as XElement;
                                                               if (e != null)
                                                                   return CloneElement(e);
                                                               return n;
                                                           }
                                    )
                );
        }
    }
}
