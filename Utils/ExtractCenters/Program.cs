using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bio;
using Bio.IO.FastA;
using Common.pviz;

namespace ExtractCenters
{
    internal class Program
    {
        // Assumptions: one center point per center type
        private static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: ExtractCenters <info.txt>");
                Console.WriteLine("Press any key to exit");
                Console.Read();
                return;
            }

            using (var infoReader = new StreamReader(args[0]))
            {
                var outdir = string.Empty;
                int count = 0;
                StreamWriter allCenterSeqsWriter=null, allCenterSeqsClusterWriter=null, allCenterSeqsClusterByRegionWriter=null, allCenterSeqsPointInfoWriter=null;
                while (!infoReader.EndOfStream)
                {
                    var line = infoReader.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        if (line.StartsWith("outdir"))
                        {
                            outdir = GetValue(line);
                            
                            string allCenterSeqsFile = Path.Combine(outdir, "All_center_seqs.fa");
                            string allCenterSeqsClusterFile=Path.Combine(outdir, "All_center_seqs_cluster.txt");
                            string allCenterSeqsClusterByRegionFile = Path.Combine(outdir,"All_center_seqs_cluster_by_region.txt");
                            string allCenterSeqsPointInfoFile = Path.Combine(outdir,"All_center_seqs_point_info.txt");
                            
                            allCenterSeqsWriter = new StreamWriter(allCenterSeqsFile);
                            allCenterSeqsClusterWriter = new StreamWriter(allCenterSeqsClusterFile);
                            allCenterSeqsClusterByRegionWriter = new StreamWriter(allCenterSeqsClusterByRegionFile);
                            allCenterSeqsPointInfoWriter = new StreamWriter(allCenterSeqsPointInfoFile);
                        }
                        else if (line.StartsWith("region"))
                        {
                            int region = int.Parse(GetValue(line));
                            HandleRegion(region, infoReader, outdir, allCenterSeqsWriter, allCenterSeqsClusterWriter,
                                             allCenterSeqsClusterByRegionWriter, allCenterSeqsPointInfoWriter, ref count);
                            Console.WriteLine("Region " + region + " done.");
                            
                        }
                    }
                }
                if (allCenterSeqsClusterByRegionWriter != null)
                {
                    allCenterSeqsClusterByRegionWriter.Flush();
                    allCenterSeqsClusterByRegionWriter.Close();
                    allCenterSeqsClusterByRegionWriter.Dispose();
                }

                if (allCenterSeqsClusterWriter != null)
                {
                    allCenterSeqsClusterWriter.Flush();
                    allCenterSeqsClusterWriter.Close();
                    allCenterSeqsClusterWriter.Dispose();
                }

                if (allCenterSeqsWriter != null)
                {
                    allCenterSeqsWriter.Flush();
                    allCenterSeqsWriter.Close();
                    allCenterSeqsWriter.Dispose();
                }

                if (allCenterSeqsPointInfoWriter != null)
                {
                    allCenterSeqsPointInfoWriter.Flush();
                    allCenterSeqsPointInfoWriter.Close();
                    allCenterSeqsPointInfoWriter.Dispose();
                }
                Console.WriteLine("All Done.\nPress any key to exit");
                Console.Read();
            }
        }

        private static void HandleRegion(int region, StreamReader infoReader, string outdir, StreamWriter allCenterSeqsWriter,
                                         StreamWriter allCenterSeqsClusterWriter, StreamWriter allCenterSeqsClusterByRegionWriter, StreamWriter allCenterSeqsPointInfoWriter, ref int count)
        {
            var centerTypeNames = new[] {"MinDistMean", "MinMDSMean", "MDSCoG"};
            var centerPlot = string.Empty;
            var fasta = string.Empty;
            var centerStatus = new Dictionary<int, string[]>();
            var sep = new[] {'\t'};
            while (!infoReader.EndOfStream)
            {
                var line = infoReader.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    if (line.StartsWith("centerswithfixed"))
                    {
                        centerPlot = GetValue(line);
                    }
                    else if (line.StartsWith("fasta"))
                    {
                        fasta = GetValue(line);
                    }
                    else if (line.StartsWith("cnum"))
                    {
                    }
                    else
                    {
                        var splits = line.Split(sep);
                        if (splits.Length == 4)
                        {
                            centerStatus[int.Parse(splits[0])] = new[] { splits[1], splits[2], splits[3] };
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            string regionCenterStatusFile = Path.Combine(outdir, "R" + region + "_center_status.txt");
            using (StreamWriter regionCenterStatusWriter = new StreamWriter(regionCenterStatusFile))
            {
                regionCenterStatusWriter.WriteLine("CNum\tCenterType\tStatus\tSeqNum\tLength");
                using (var parser = new FastAParser(fasta))
                {
                    IList<ISequence> seqs = parser.Parse().ToList();
                    PvizModel pviz = PvizModel.LoadPviz(centerPlot);
                    Dictionary<int,int> nonCenterCnumToNumPointsTable = new Dictionary<int, int>();
                    Dictionary<int, int[]> cnumToCenterPointsTable = new Dictionary<int, int[]>();
                    foreach (Point point in pviz.Points)
                    {
                        string label = point.Label;
                        if (!string.IsNullOrEmpty(label))
                        {
                            // A center point
                            int centerType = GetCenterTypeFromPointLabel(label); // 0=OveralBest, 1=MinDistMean, 2=MinMDSMean, 3=MDSCoG
                            if (centerType > 0)
                            {
                                // Avoid OverallBest center
                                centerType -= 1; // now 0=MinDistMean, 1=MinMDSMean, 2=MDSCoG
                                int relatedCnum = GetRelatedCnumFromPointLabel(label);
                                if (!cnumToCenterPointsTable.ContainsKey(relatedCnum))
                                {
                                    var pnums = new int[3];
                                    var pnum = GetPnumFromPointLabel(point);
                                    pnums[centerType] = pnum;
                                    cnumToCenterPointsTable.Add(relatedCnum, pnums);
                                }
                                else
                                {
                                    cnumToCenterPointsTable[relatedCnum][centerType] = GetPnumFromPointLabel(point);
                                }
                            }
                        }
                        else
                        {
                            // A non center point, should be associated with a non center cluster 
                            if (!nonCenterCnumToNumPointsTable.ContainsKey(point.ClusterKey))
                            {
                                nonCenterCnumToNumPointsTable.Add(point.ClusterKey, 1);
                            }
                            else
                            {
                                nonCenterCnumToNumPointsTable[point.ClusterKey]++;
                            }
                        }

                        
                    }

                    foreach (KeyValuePair<int, string[]> kv in centerStatus)
                    {
                        int cnum = kv.Key;
                        string[] status = kv.Value;

                        if (!"NA".Equals(status[0]))
                        {
                            // A good cluster, not debris
                            long longestCenterSeqLength = -1;
                            ISequence longestCenterSeq = null;
                            int longestCenterType = -1;
                            int longestCenterSeqNum = -1;
                            for (int i = 0; i < 3; i++)
                            {
                                if (!"BAD".Equals(status[i]))
                                {
                                    int seqNum = cnumToCenterPointsTable[cnum][i];
                                    ISequence seq = seqs[seqNum];
                                    if (seq.Count > longestCenterSeqLength)
                                    {
                                        longestCenterSeqLength = seq.Count;
                                        longestCenterSeq = seq;
                                        longestCenterType = i;
                                        longestCenterSeqNum = seqNum;
                                    }
                                }
                            }

                            if (longestCenterSeq != null)
                            {
                                string seqLabel = longestCenterSeq.ID + "_R" + region + "_C" + cnum + "_" +
                                                  centerTypeNames[longestCenterType];
                                regionCenterStatusWriter.WriteLine(cnum + "\t" + centerTypeNames[longestCenterType] + "\t" + status[longestCenterType] + "\t" + longestCenterSeqNum + "\t" + longestCenterSeqLength);
                                allCenterSeqsWriter.WriteLine(">"+seqLabel);
                                allCenterSeqsWriter.WriteLine(SeqToString(longestCenterSeq));
                                allCenterSeqsClusterWriter.WriteLine(count + "\t" + "R" + region + "C" + cnum + "_" +
                                                                     centerTypeNames[longestCenterType]);
                                allCenterSeqsClusterByRegionWriter.WriteLine(count + "\t" + region);
                                allCenterSeqsPointInfoWriter.WriteLine(count++ + "\tR:" + region + "|C:" + cnum + "|SeqNum:" + longestCenterSeqNum + "|Points:" + nonCenterCnumToNumPointsTable[cnum] + "|Status:" + status[longestCenterType] + "|Len:" + longestCenterSeqLength);
                            }
                            else
                            {
                                // Can't happen
                                throw new Exception("No good center sequence found for cluster");
                            }
                        }
                        else
                        {
                            regionCenterStatusWriter.WriteLine(cnum + "\tNA\tDebris\tNA\tNA");
                        }
                    }
                }
            }
        }

        private static int GetRelatedCnumFromPointLabel(string label)
        {
            int idx1 = label.IndexOf(':')+1;
            int idx2 = label.IndexOf('-');
            return int.Parse(label.Substring(idx1, idx2 - idx1));
        }

        private static int GetCenterTypeFromPointLabel(string label)
        {
            int length = label.Length;
            char last = label[length - 1];
            char fourteen = label[length - 14];
            return last == 't' ? 0 : (last == 'G' ? 3 : (fourteen == 'S' ? 2 : 1));
        }

        static string SeqToString(ISequence sequence)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            return encoding.GetString(sequence.ToArray()).ToUpper();
        }

        private static int GetPnumFromPointLabel(Point point)
        {
            int idx1 = point.Label.IndexOf("idx:", System.StringComparison.Ordinal) + 4;
            int idx2 = point.Label.IndexOf("method", StringComparison.Ordinal);
            int pnum = int.Parse(point.Label.Substring(idx1, (idx2 - idx1)));
            return pnum;
        }

        private static string GetValue(string line)
        {
            var sep = new[] {'\t'};
            var splits = line.Split(sep);
            if (splits.Length == 2)
            {
                return splits[1];
            }
            throw new Exception("Invalid line: " + line);
        }
    }
}
