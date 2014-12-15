using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bio;
using Bio.IO.FastA;

namespace COG303ClusterMapping
{
    class Program
    {
        static void Main()
        {
            const string cog183Kfasta = @"D:\Sali\InCloud\SugarSyncSharedByMe\SalsaBio\cog\large\sequences\unique_183362_cog0303.fa";
            const string unorderedCog303ClusterMapping = @"D:\Sali\InCloud\SugarSyncSharedByMe\SalsaBio\cog\large\sequences\cog0303_to_COG_clusters(unordered).txt";

            const string cog183KClusterMapping = @"D:\Sali\InCloud\SugarSyncSharedByMe\SalsaBio\cog\large\sequences\183362_pnums_to_cog_clusters.txt";

            using (var parser = new FastAParser(cog183Kfasta))
            {
                IList<ISequence> cogUnique183KSeqs = parser.Parse().ToList();
                var ht = new Hashtable();
                using (var reader = new StreamReader(unorderedCog303ClusterMapping))
                {
                    var sep = new[]{' ', '\t'};
                    while (!reader.EndOfStream)
                    {
                        string l = reader.ReadLine();
                        if (!string.IsNullOrEmpty(l))
                        {
                            string[] splits = l.Split(sep);
                            if (!ht.Contains(splits[0]))
                            {
                                ht.Add(splits[0], splits[1]);
                            }
                        }
                    }
                }

                using (var writer = new StreamWriter(cog183KClusterMapping))
                {
                    for (int i = 0; i < cogUnique183KSeqs.Count; i++)
                    {
                        writer.WriteLine(i +"\t" + (ht.Contains(cogUnique183KSeqs[i].ID) ? ht[cogUnique183KSeqs[i].ID] : "UnkownCOG"));
                    }
                }
            }
        }

        
    }
}
