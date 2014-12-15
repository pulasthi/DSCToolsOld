using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bio;
using Bio.IO.FastA;
using Common;

namespace OldSelected7InLargeCog
{
    class Program
    {
        static void Main()
        {
            const string cog96KClusters = @"G:\SugarSyncSharedByMe\SalsaBio\cog\100k\sequences\clustAssign2_sorted_by_pnum_unique_95672(sbpu).txt";
            const string cog96KFasta = @"G:\SugarSyncSharedByMe\SalsaBio\cog\100k\sequences\cog_unique.fasta";
            
            const string cog183363Fasta = @"G:\SugarSyncSharedByMe\SalsaBio\cog\large\sequences\unique_183362_cog0303.fa";
            
            HashSet<string> sevenClusters = new HashSet<string>(new[] {"COG1126", "COG4608", "COG3839", "COG0444", "COG1131", "COG1136", "COG3842"});
            const string consensusCluster = "ConsensusCOG";
            const string discardedCluster = "DiscardedCOG";
            const int consensus = 4872;
            // Output file
            const string oldSeven96KPointsCluster = @"G:\SugarSyncSharedByMe\SalsaBio\cog\large\sequences\oldSeven96KPointsCluster.txt";

            using (StreamWriter writer = new StreamWriter(oldSeven96KPointsCluster))
            {
                using (SimpleClusterReader creader = new SimpleClusterReader(cog96KClusters))
                {
                    using (FastAParser parser96K = new FastAParser(cog96KFasta), 
                        parser183362 = new FastAParser(cog183363Fasta))
                    {
                        for (int i = 0; i < consensus; i++)
                        {
                            writer.WriteLine(i + "\t" + consensusCluster);
                        }

                        Hashtable oldSevenSeqStrToCogCluster = new Hashtable();
                        IList<ISequence> cog96KSeqs = parser96K.Parse().ToList();
                        while (!creader.EndOfStream)
                        {
                            Cluster c = creader.ReadCluster();
                            if (sevenClusters.Contains(c.Cstring))
                            {
                                ISequence seq = cog96KSeqs[c.Pnum];
                                oldSevenSeqStrToCogCluster.Add(SeqToString(seq), c.Cstring);
                            }
                        }

                        IList<ISequence> cog18362Seqs = parser183362.Parse().ToList();
                        for (int i = 0; i < cog18362Seqs.Count; i++)
                        {
                            string seqStr = SeqToString(cog18362Seqs[i]);
                            writer.WriteLine((consensus + i) + "\t" +
                                             (oldSevenSeqStrToCogCluster.ContainsKey(seqStr)
                                                  ? oldSevenSeqStrToCogCluster[seqStr]
                                                  : discardedCluster));
                        }
                    }
                }
            }
        }

        static string SeqToString(ISequence sequence)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            return encoding.GetString(sequence.ToArray()).ToUpper();
        }
    }
}
