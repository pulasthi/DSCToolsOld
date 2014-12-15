using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bio;
using Bio.IO.FastA;

namespace PhySequenceClusters
{
    class Program
    {
        static void Main(string[] args)
        {
            string gbFasta = @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\sequences\74GenBank.txt";
            string wittiyaFasta =
                @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\sequences\unique_1130_allInOneFasta.txt";
            string krugerFasta =
                @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\sequences\unique_1016_SSU-ITS-LSU_alignment_Krueger_etal_2011.fa";

            string centerFasta = @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\sequences\sorted_unique_2149_GbWitKruReducedCenters_126ReducedCentersReverseComplemented.txt";

            string allFasta = @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\sequences\126+74+988+945_126reversed.fasta";

            string clusterFile =
                @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\sequences\126+74+988+945_126reversed_cluster.txt";

            using (FastAParser gbParser = new FastAParser(gbFasta),
                wittiyaParser = new FastAParser(wittiyaFasta),
                krugerParser = new FastAParser(krugerFasta),
                centerParser = new FastAParser(centerFasta),
                allParser = new FastAParser(allFasta))
            {
                HashSet<string> gbSeqs = new HashSet<string>();
                HashSet<string> wittiyaSeqs = new HashSet<string>();
                HashSet<string> krugerSeqs = new HashSet<string>();
                HashSet<string> centerSeqs = new HashSet<string>();
                PopulateHashSets(gbParser, gbSeqs);
                PopulateHashSets(wittiyaParser, wittiyaSeqs);
                PopulateHashSets(krugerParser, krugerSeqs);
                PopulateHashSets(centerParser, centerSeqs);

                IList<ISequence> allSeqs = allParser.Parse().ToList();
                using (StreamWriter writer = new StreamWriter(clusterFile))
                {
                    for (int i = 0; i < allSeqs.Count; i++)
                    {
                        string str = SeqToString(allSeqs[i]);
                        writer.WriteLine(i + "\t" +
                                         (gbSeqs.Contains(str)
                                              ? "GenBank"
                                              : (wittiyaSeqs.Contains(str)
                                                     ? "WittiyaLSU"
                                                     : (krugerSeqs.Contains(str)
                                                            ? "KrugerLSU+SSU+ITS"
                                                            : (centerSeqs.Contains(str) ? "ReducedRefinedCenter" : "Unknown")))));
                    }
                }
            }
            Console.WriteLine("Done.");
            Console.Read();
        }

        private static void PopulateHashSets(FastAParser parser, HashSet<string> hs)
        {
            foreach (var gbSeq in parser.Parse())
            {
                string str = SeqToString(gbSeq);
                if (!hs.Contains(str))
                {
                    hs.Add(str);
                }
            }
        }

        static string SeqToString(ISequence seq)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            return encoding.GetString(seq.ToArray()).ToUpper();
        }
    }
}
