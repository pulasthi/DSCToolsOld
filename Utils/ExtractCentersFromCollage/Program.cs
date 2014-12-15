using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using Common.pviz;

namespace ExtractCentersFromCollage
{
    class Program
    {
        static void Main(string[] args)
        {
            string fixPointCollagePlot =
                @"G:\My Box Files\SalsaBio\millions\446K_uniq\reclassification\regions\MultiDimensionalScaling\FixedPointRuns\MDSasChisq\FixedPointsCollage\446041_sequences_100K_fixed_collage.pviz";
            string centerInfoFile =
                @"G:\My Box Files\SalsaBio\millions\446K_uniq\reclassification\regions\MultiDimensionalScaling\FixedPointRuns\MDSasChisq\PlotsWithCenters\All_center_seqs_point_info.txt";
            string fixedCenterPoints =
                @"G:\My Box Files\SalsaBio\millions\446K_uniq\reclassification\regions\MultiDimensionalScaling\FixedPointRuns\MDSasChisq\PlotsWithCenters\fixed_center_points.txt";

            using (var centerReader = new StreamReader(centerInfoFile))
            {
                
                //R#->C#->SeqNum#->PvizPoint
                var rToCtoSeqNumToPointTable =  ParseCenterInfo(centerInfoFile);

                PvizModel pviz = PvizModel.LoadPviz(fixPointCollagePlot);
                var sep = new[] {'_'};
                pviz.Points.ForEach(p =>
                                        {
                                            var splits = p.Label.Split(sep);
                                            var r = int.Parse(splits[0].Substring(1));
                                            var c = int.Parse(splits[1].Substring(1));
                                            var pnum = int.Parse(splits[2].Substring(1));
                                            if (rToCtoSeqNumToPointTable.ContainsKey(r))
                                            {
                                                if (rToCtoSeqNumToPointTable[r].ContainsKey(c))
                                                {
                                                    if ((int)(rToCtoSeqNumToPointTable[r][c].First) == pnum)
                                                    {
                                                        rToCtoSeqNumToPointTable[r][c].Second = p;
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Missing cluster: " + c +
                                                                      " should be a cluster with bad status");
                                                }
                                            }
                                            else
                                            {
                                                throw new Exception("Can't happen, region " + r +
                                                                    " should have been in this table");
                                            }

                                        });
                using (var writer = new StreamWriter(fixedCenterPoints))
                {
                    int[] regions = rToCtoSeqNumToPointTable.Keys.ToArray();
                    Array.Sort(regions);
                    foreach (int region in regions)
                    {
                        int[] clusters = rToCtoSeqNumToPointTable[region].Keys.ToArray();
                        Array.Sort(clusters);
                        foreach (int cluster in clusters)
                        {
                            Pair pair = rToCtoSeqNumToPointTable[region][cluster];
                            Location location = ((Point) pair.Second).Location;
                            writer.WriteLine(region + "\t" + cluster + "\t" + pair.First + "\t" + location.X+"\t" +location.Y+"\t"+location.Z);
                        }
                    }

                }
            }
        }

        private static Dictionary<int,Dictionary<int,Pair>> ParseCenterInfo(string centerInfoFile)
        {
            var tabsep = new[] { '\t' };
            var hyphensep = new[] { '-' };
            var colonsep = new[] { ':' };
            var rToCtoSeqNumToPointTable = new Dictionary<int, Dictionary<int, Pair>>();
            using (var centerReader = new StreamReader(centerInfoFile))
            {
                while (!centerReader.EndOfStream)
                {
                    var line = centerReader.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        var content = line.Split(tabsep)[1];
                        if (content != null)
                        {
                            var hyphensplits = content.Split(hyphensep);
                            if (hyphensplits.Length == 6)
                            {
                                int r = int.Parse(hyphensplits[0].Split(colonsep)[1]);
                                int c = int.Parse(hyphensplits[1].Split(colonsep)[1]);
                                int seqNum = int.Parse(hyphensplits[2].Split(colonsep)[1]);

                                if (!rToCtoSeqNumToPointTable.ContainsKey(r))
                                {
                                    rToCtoSeqNumToPointTable[r] = new Dictionary<int, Pair>();
                                }

                                if (!rToCtoSeqNumToPointTable[r].ContainsKey(c))
                                {
                                    rToCtoSeqNumToPointTable[r][c] = new Pair();
                                }

                                if (rToCtoSeqNumToPointTable[r][c].First == null)
                                {
                                    rToCtoSeqNumToPointTable[r][c].First = seqNum;
                                }
                                else
                                {
                                    throw new Exception("Can't happen two centers for same cluster with refined centers");
                                }
                            }
                            else
                            {
                                throw new Exception("Wrong number of hyphensplits");
                            }
                        }
                        else
                        {
                            throw new Exception("Content is null");
                        }
                    }
                }
            }
            return rToCtoSeqNumToPointTable;
        }
    }
}
