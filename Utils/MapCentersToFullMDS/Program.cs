using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bio;
using Bio.IO.FastA;
using Common.pviz;

namespace MapCentersToFullMDS
{
    class Program
    {
        static void Main(string[] args)
        {
            /* Note. All sequences are unique 
             *       The final pviz has center sequences IN ORDER AS THEY ARE IN CENTER SEQUENCE FILE*/ 

            string centerSequences =
                @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\centers\420centers\center_sequences.txt";
//                @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\centers\420centers\reduced_126_center_sequences.txt";
            string allSequences =
                @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\haixu_release\sequences\initial\allreads_uniques_gt200_446041_random.txt";
            string allPointsPlot =
                @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\haixu_release\plots\initial\haixu_446041_mega_regions_zeroidx.pviz";
            string centerMappedPlot =
                @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\centers\420centers\haixu_446041_mega_regions_with_420_centers_zeroidx.pviz";
//                @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\centers\420centers\haixu_446041_mega_regions_with_126_centers_zeroidx.pviz";
            Hashtable centerSeqToInfoTable = ParseCenterSequences(centerSequences);
            Hashtable allSeqsToPnumTable = ParseSeqs(allSequences);
            PvizModel pviz = PvizModel.LoadPviz(allPointsPlot);
            List<Cluster> clusters = pviz.Clusters;
            int numberOfMegaRegions = clusters.Count;
            for (int i = 0; i < numberOfMegaRegions; ++i)
            {
                Cluster centerCluster = clusters[i].Clone();
                centerCluster.Label = centerCluster.Key + ".centers";
                centerCluster.Shape = 4;
                centerCluster.Key = clusters.Count;
                clusters.Add(centerCluster);
            }


            Hashtable pvizPointNumToPointTable = ParsePvizPoints(pviz.Points);
            int allPointCount = pvizPointNumToPointTable.Count;
            foreach (DictionaryEntry kv in centerSeqToInfoTable)
            {
                string seqStr = (string) kv.Key;
                IList<CenterSeqIdInfo> centerInfos = (IList<CenterSeqIdInfo>) kv.Value;
                int pnum = (int)allSeqsToPnumTable[seqStr];
                foreach (CenterSeqIdInfo centerInfo in centerInfos)
                {
                    Point point = ((Point)pvizPointNumToPointTable[pnum]).Clone();
                    point.Key = allPointCount+centerInfo.CenterSeqNum; // Guarantees center sequences are indexed in order they appear in the sequence file
                    point.ClusterKey = centerInfo.Region + numberOfMegaRegions;
                    point.Label = "Region:" + centerInfo.Region + "-Cluster:" + centerInfo.Cluster + "-Method:" +
                                  centerInfo.Method;
                    pvizPointNumToPointTable.Add(point.Key, point);
                }
            }
            pviz.Points = OrderPvizPoints(pvizPointNumToPointTable);

            pviz.SaveAs(centerMappedPlot);
            Console.WriteLine("Done. \nPress any key");
            Console.Read();
        }

        private static List<Point> OrderPvizPoints(Hashtable pvizPointNumToPointTable)
        {
            List<Point> points = new List<Point>();
            for (int i = 0; i < pvizPointNumToPointTable.Count; ++i )
            {
                points.Add((Point)pvizPointNumToPointTable[i]);
            }
            return points;
        }

        private static Hashtable ParsePvizPoints(List<Point> points)
        {
            Hashtable ht = new Hashtable();
            foreach (Point point in points)
            {
                ht.Add(point.Key, point);
            }
            return ht;
        }

        private static Hashtable ParseSeqs(string sequenceFile)
        {
            using (FastAParser parser = new FastAParser(sequenceFile))
            {
                IList<ISequence> seqs = parser.Parse().ToList();
                Hashtable ht = new Hashtable();
                for (int i =0; i < seqs.Count; ++i)
                {
                    ISequence seq = seqs[i];
                    ht.Add(SeqToString(seq), i);
                }
                return ht;
            } 
        }

        private static Hashtable ParseCenterSequences(string centerSequences)
        {
            using (FastAParser parser = new FastAParser(centerSequences))
            {
                IEnumerable<ISequence> seqs = parser.Parse();
                Hashtable ht = new Hashtable();
                int count = 0;
                foreach (ISequence seq in seqs)
                {
                    /* sequence ID format 
                     * G4P2R5E01CJXF8 length=217 xy=0929_3974 region=1 run=R_2011_06_22_18_25_07_ FoxRegion=0 FoxCluster=0 FoxMethod=SmallestDistanceMeans
                     */
                    string seqStr = SeqToString(seq);
                    if (ht.Contains(seqStr))
                    {
                        IList<CenterSeqIdInfo> infos = (IList<CenterSeqIdInfo>) ht[seqStr];
                        infos.Add(ParseCenterSeqId(seq.ID, count));
                    }
                    else
                    {
                        IList<CenterSeqIdInfo> centerInfos = new List<CenterSeqIdInfo>();
                        centerInfos.Add(ParseCenterSeqId(seq.ID,count));
                        ht.Add(seqStr, centerInfos);
                    }
                    ++count;
                }
                return ht;
            }
        }

        static CenterSeqIdInfo ParseCenterSeqId(string id, int seqNum)
        {
            CenterSeqIdInfo info = new CenterSeqIdInfo();
            info.CenterSeqNum = seqNum;
            int idx = id.LastIndexOf('_');
            info.Id = id.Substring(0, idx + 1);
            string[] splits = id.Substring(idx + 1).Split(new[]{'=',' '});
            info.Region = int.Parse(splits[2]);
            info.Cluster = int.Parse(splits[4]);
            info.Method = splits[6];
            return info;
        }

        static string SeqToString(IEnumerable<byte> seq)
        {
            ASCIIEncoding enc = new ASCIIEncoding();
            return enc.GetString(seq.ToArray()).ToUpper();
        } 
    }

    class CenterSeqIdInfo
    {
        public string Id { get; set; }
        public int Region { get; set; }
        public int Cluster { get; set; }
        public string Method { get; set; }
        public int CenterSeqNum { get; set; }
    }
}

