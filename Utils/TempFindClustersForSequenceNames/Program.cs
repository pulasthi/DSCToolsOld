using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bio.IO.FastA;

namespace TempFindClustersForSequenceNames
{
    class Program
    {
        static void Main(string[] args)
        {
            var infoFile =
                @"G:\My Box Files\SalsaBio\millions\446K_uniq\reclassification\regions\MultiDimensionalScaling\FixedPointRuns\MDSasChisq\FixedPointsCollage\446041_sequences_100K_fixed_collage_points_w_orig_pnum_and_labels.txt";
            var fasta =
                @"G:\My Box Files\SalsaBio\millions\phy\updated_6.5.14\Sequences\uniqs\final_unique_56706_AM_fungi_referenceSeqs_plus_QualScreened454Reads_sequences.fa";
            var clusterFile = @"G:\My Box Files\SalsaBio\millions\phy\updated_6.5.14\Sequences\uniqs\final_unique_56706_AM_fungi_referenceSeqs_plus_QualScreened454Reads_clusters.txt";
            using (var parser = new FastAParser(fasta))
            {
                var seqs = parser.Parse().ToList();
                using (var reader = new StreamReader(infoFile))
                {
                    using (var writer = new StreamWriter(clusterFile))
                    {
                        var ht = new Hashtable();
                        var refCount = 864;
                        var other = @"Other";
                        var reference = @"Reference";
                        var sep = new[] {' ', '\t'};
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            if (string.IsNullOrEmpty(line)) continue;
                            var splits = line.Split(sep);
                            var cluster = splits[6];
                            var color = splits[7];
                            var seqName = splits[9].ToUpper();
                            if (!ht.Contains(seqName))
                            {
                                ht.Add(seqName, new []{cluster,color});
                            }
                            else
                            {
                                Console.WriteLine("error duplicate entry"); // can't happen
                            }
                        }

                        var count = 0;
                        foreach (var seq in seqs)
                        {
                            var seqName = seq.ID.ToUpper();
                            seqName = seqName.Substring(0, seqName.IndexOf('_'));
                            if (count < refCount)
                            {
                                writer.WriteLine(count + "\t" + reference + "\t0.0.0.0" );
                            }
                            else
                            {
                                writer.WriteLine(count + "\t" + (ht.Contains(seqName) ? ((string[])ht[seqName])[0] : other) + "\t" + (ht.Contains(seqName) ? ((string[])ht[seqName])[1] : "0.0.0.0"));
                            }
                            
                            ++count;
                        }
                    }
                }
            }
        }
    }
}
