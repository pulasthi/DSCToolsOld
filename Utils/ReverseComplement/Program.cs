using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bio;
using Bio.IO.FastA;
using Common;

namespace ReverseComplement
{
    class Program
    {
        static void Main(string[] args)
        {
            string fastaFile = @"G:\My Box Files\SalsaBio\larry\phyloshop\unique_14150_rnafragments_all.fa";
//            string fastaFile = @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\sequences\sorted_unique_2149_GbWitKruReducedCenters.txt";
            string clusterFile =
                @"G:\My Box Files\SalsaBio\larry\phyloshop\unique_14150_rnafragments_all_rev_direct_cluster.txt";
//                @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\sequences\sorted_unique_2149_GbWitKruReducedCenters_clusters.txt";
            string revComplementFastaFile =
                @"G:\My Box Files\SalsaBio\larry\phyloshop\unique_14150_rnafragments_all_rev_reversed.fa";
//                @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\sequences\sorted_unique_2149_GbWitKruReducedCenters_126ReducedCentersReverseComplemented.txt";

            HashSet<string> tags = new HashSet<string>(new[] { "rev" });
//            HashSet<string> tags = new HashSet<string>(new[] { "ReducedRefinedCenter"});


            using (FastAParser parser = new FastAParser(fastaFile))
            {
                IList<ISequence> seqs = parser.Parse().ToList();
                using (SimpleClusterReader reader = new SimpleClusterReader(clusterFile))
                {
                    while (!reader.EndOfStream)
                    {
                        Cluster c = reader.ReadCluster();
                        if (tags.Contains(c.Cstring))
                        {
                            seqs[c.Pnum] = seqs[c.Pnum].GetReverseComplementedSequence();
                        }
                    }
                }

                using (FastAFormatter formatter = new FastAFormatter(revComplementFastaFile))
                {
                    formatter.Write(seqs);
                }
            }

            Console.WriteLine("Done.");
            Console.Read();


        }
    }
}
