using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;
using Common.pviz;
using Cluster = Common.pviz.Cluster;
using Point = Common.pviz.Point;

namespace FixedPointsCollage
{
    internal class Program
    {
        private static List<System.Drawing.Color> matlab50Colors = new List<System.Drawing.Color>();


        private static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: FixedPointsCollage.exe mode infofile");
                Console.WriteLine("\nPress any key to exit.");
                return;
            }


            LoadMatlab50Colors();


            string mode = args[0];
            string infotxt = args[1];
            bool isWithCenters = bool.Parse(mode);
            string suffix = isWithCenters ? "_w_centers" : string.Empty;
            if (!string.IsNullOrEmpty(infotxt))
            {
                using (var reader = new StreamReader(infotxt))
                {
                    var sep = new[] { '\t' };
                    string line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        string[] splits = line.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                        if (splits.Length == 2)
                        {
                            string outfile = splits[1];

                            if (outfile.StartsWith("\""))
                            {
                                outfile = outfile.Substring(1, outfile.Length - 2);
                                outfile = Path.Combine(Path.GetDirectoryName(outfile) ?? string.Empty,
                                                       Path.GetFileNameWithoutExtension(outfile) + suffix + ".pviz");
                            }
                            string outfile2 = Path.Combine(Path.GetDirectoryName(outfile) ?? string.Empty,
                                                           Path.GetFileNameWithoutExtension(outfile) +
                                                           "_by_regions" + suffix + ".pviz");

                            var pviz1Plot = new Plot
                            {
                                Title = Path.GetFileNameWithoutExtension(outfile),
                                Glyph = new Glyph(0, 1.0f),
                                PointSize = 1
                            };
                            var pviz2Plot = new Plot
                            {
                                Title =
                                    Path.GetFileNameWithoutExtension(outfile) + "_by_regions" + suffix,
                                Glyph = new Glyph(0, 1.0f),
                                PointSize = 1
                            };

                            var pviz1Clusters = new List<Cluster>();
                            var pviz2Clusters = new List<Cluster>();
                            var pviz1Points = new List<Point>();
                            var pviz2Points = new List<Point>();

                            var pviz1 = new PvizModel { Plot = pviz1Plot, Clusters = pviz1Clusters, Points = pviz1Points };
                            var pviz2 = new PvizModel { Plot = pviz2Plot, Clusters = pviz2Clusters, Points = pviz2Points };

                            var regionLocalCenterPnumToLabelTables = new Dictionary<int, Dictionary<int, string>>();
                            var regionLocalCenterPnumToCategoryTable = new Dictionary<int, int>();
                            
                            if (isWithCenters)
                            {
                                line = reader.ReadLine();
                                if (!string.IsNullOrEmpty(line))
                                {
                                    splits = line.Split(sep);
                                    if (splits.Length == 2)
                                    {
                                        string centerInfoFile = splits[1];
                                        if (centerInfoFile.StartsWith("\""))
                                        {
                                            centerInfoFile = centerInfoFile.Substring(1, centerInfoFile.Length - 2);
                                        }
                                        using (var centerInfoReader = new StreamReader(centerInfoFile))
                                        {
                                            while (!centerInfoReader.EndOfStream)
                                            {
                                                line = centerInfoReader.ReadLine();
                                                if (!string.IsNullOrEmpty(line))
                                                {
                                                    splits = line.Split(sep);
                                                    int region;
                                                    int localSeqNum;
                                                    string label = splits[1];
                                                    ExtractInfoFromCenterLabel(label, out region, out localSeqNum);
                                                    if (!regionLocalCenterPnumToLabelTables.ContainsKey(region))
                                                    {
                                                        regionLocalCenterPnumToLabelTables.Add(region, new Dictionary<int, string>());
                                                        
                                                    }
                                                    regionLocalCenterPnumToLabelTables[region][localSeqNum] = label;
                                                }
                                            }
                                        }
                                    }
                                }

                                var centerPointCategories = new HashSet<int>();
                                line = reader.ReadLine();
                                if (!string.IsNullOrEmpty(line))
                                {
                                    splits = line.Split(sep);
                                    if (splits.Length == 2)
                                    {
                                        string centerInfoFile = splits[1];
                                        if (centerInfoFile.StartsWith("\""))
                                        {
                                            centerInfoFile = centerInfoFile.Substring(1, centerInfoFile.Length - 2);
                                        }
                                        using (var centerInfoReader = new StreamReader(centerInfoFile))
                                        {
                                            while (!centerInfoReader.EndOfStream)
                                            {
                                                line = centerInfoReader.ReadLine();
                                                if (!string.IsNullOrEmpty(line))
                                                {
                                                    splits = line.Split(sep);
                                                    int category = int.Parse(splits[2]);
                                                    regionLocalCenterPnumToCategoryTable.Add(int.Parse(splits[1]), category);
                                                    if (!centerPointCategories.Contains(category))
                                                    {
                                                        centerPointCategories.Add(category);
                                                    }
                                                }
                                            }
                                        }

                                    }
                                }

                                foreach (var pvizCluster in centerPointCategories.Select(centerPointCategory => new Cluster
                                                                                                                    {
                                                                                                                        Key = centerPointCategory,
                                                                                                                        Label = "LSU_Centers_Category_" + centerPointCategory,
                                                                                                                        Visible = 1,
                                                                                                                        Default = 0,
                                                                                                                        Color = new Color(matlab50Colors[0]),
                                                                                                                        Size = 2.0f + centerPointCategory
                                                                                                                    }))
                                {
                                    pviz1Clusters.Add(pvizCluster);
                                    pviz2Clusters.Add(pvizCluster);
                                }
                                
                            }

                            reader.ReadLine(); // ignore first empty line
                            while (!reader.EndOfStream)
                            {
                                HandleRegion(reader, pviz1Points, pviz1Clusters, pviz2Points, pviz2Clusters,
                                             isWithCenters, regionLocalCenterPnumToLabelTables,
                                             regionLocalCenterPnumToCategoryTable);
                            }

                            pviz1.SaveAs(outfile);
                            pviz2.SaveAs(outfile2);
                        }
                    }
                }
            }
            
        }

        private static void ExtractInfoFromCenterLabel(string label, out int region, out int localSeqNum)
        {
            var sep = new[] {'-'};
            string[] splits = label.Split(sep);
            region = int.Parse(splits[0].Substring(2));
            localSeqNum = int.Parse(splits[2].Substring(7));
        }


        private static void LoadMatlab50Colors()
        {
            if (File.Exists("Matlab50.txt"))
            {
                using (StreamReader reader = new StreamReader("Matlab50.txt"))
                {
                    matlab50Colors = new List<System.Drawing.Color>();
                    char[] sep = new[] {' ', '\t'};
                    string[] splits;
                    string split;
                    int startIdx = 3, endIdx;
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

                        matlab50Colors.Add(System.Drawing.Color.FromArgb(a, r, g, b));
                    }
                }
            }
        }


        private static void HandleRegion(StreamReader reader, List<Point> pviz1Points, List<Cluster> pviz1Clusters, List<Point> pviz2Points, List<Cluster> pviz2Clusters,
            bool isWithCenters, Dictionary<int, Dictionary<int, string>> regionLocalCenterPnumToLabelTables, Dictionary<int, int> regionLocalCenterPnumToCategoryTable)
        {
            var sep = new[] {'\t'};
            int region = -1, numfixed = -1;
            string clustersFile = string.Empty, fixedptsFile = string.Empty;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    string[] splits = line.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                    if (splits.Length == 2)
                    {
                        string value = splits[1];
                        
                        switch (splits[0])
                        {
                            case "region":
                                region = int.Parse(value);
                                break;
                            case "clusters":
                                clustersFile = value;
                                break;
                            case "fixedpts":
                                fixedptsFile = value;
                                break;
                            case "numfixed":
                                numfixed = int.Parse(value);
                                break;
                            default:
                                throw new Exception("Undefined parameter: " + splits[0]);
                        }

                    }
                }
                else
                {
                    break;
                }
            }

            if (clustersFile.StartsWith("\""))
            {
                clustersFile = clustersFile.Substring(1, clustersFile.Length - 2);
            }

            if (fixedptsFile.StartsWith("\""))
            {
                fixedptsFile = fixedptsFile.Substring(1, fixedptsFile.Length - 2);
            }
            using (var creader = new SimpleClusterReader(clustersFile))
            {
                int pnum = pviz1Points.Count == 0 ? 0 : (pviz1Points.Max(p => p.Key) + 1);
                int cnum1 = pviz1Clusters.Count == 0 ? 0 : (pviz1Clusters.Max(c => c.Key)) + 1;
                int cnum2 = pviz2Clusters.Count == 0 ? 0 : (pviz2Clusters.Max(c => c.Key)) + 1;

                var localClusterTable = new Hashtable();
                using (var preader = new SimplePointsReader(fixedptsFile))
                {
                    while (!creader.EndOfStream && !preader.EndOfStream)
                    {
                        Common.Cluster c = creader.ReadCluster();
                       
                        if (!localClusterTable.Contains(c.Cnum))
                        {
                            localClusterTable.Add(c.Cnum, cnum1 + c.Cnum);
                            var pvizCluster = new Cluster
                            {
                                Key = cnum1 + c.Cnum,
                                Label = "R" + region + "_C" + c.Cnum,
                                Visible = 1,
                                Default = 0,
                                Color = new Color(matlab50Colors[c.Cnum % 50])
                            };

                            pviz1Clusters.Add(pvizCluster);
                        }

                        int globalCnum = (int)localClusterTable[c.Cnum];

                        
                        Common.Point p = preader.ReadPoint();
                        var pvizPoint = new Point
                                            {
                                                Label = "R" + region + "_C" + c.Cnum + "_P" + p.Index,
                                                Key = pnum++,
                                                ClusterKey = globalCnum,
                                                Location = new Location(p.X, p.Y, p.Z)
                                            };
                        pviz1Points.Add(pvizPoint);

                        // preparing point for second pviz
                        pvizPoint = pvizPoint.Clone();
                        globalCnum = cnum2;
                        pvizPoint.ClusterKey = globalCnum;
                        pviz2Points.Add(pvizPoint);

                        if (isWithCenters && regionLocalCenterPnumToLabelTables[region].ContainsKey(p.Index))
                        {
                            // this point is a center point as well
                            pvizPoint = new Point
                            {
                                Label = "LSU_Center_R:" + region + "_C:" + c.Cnum + "_P:" + p.Index + "_CAT:" + regionLocalCenterPnumToCategoryTable[p.Index],
                                Key = pnum++,
                                ClusterKey = regionLocalCenterPnumToCategoryTable[p.Index],
                                Location = new Location(p.X, p.Y, p.Z)
                            };
                            pviz1Points.Add(pvizPoint);
                            pviz2Points.Add(pvizPoint);
                        }

                    }

                    // Second Pviz clusters -- just one cluster per region
                    var pviz2Cluster = new Cluster
                                           {
                                               Key = cnum2,
                                               Label = "R" + region,
                                               Visible = 1,
                                               Default = 0,
                                               Color = new Color(matlab50Colors[region%50]),
                                               Size = 1,
                                               Shape = 0
                                           };

                    pviz2Clusters.Add(pviz2Cluster);
                }
            }

        }
    }
}