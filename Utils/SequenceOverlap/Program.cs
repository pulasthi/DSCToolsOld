using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bio;
using Bio.IO.FastA;

namespace SequenceOverlap
{
    class Program
    {
        static void Main(string[] args)
        {
//            string prefix = "COG4608";
            string fastaOne =
//                @"G:\SugarSyncSharedByMe\SalsaBio\cog\100k\sequences\selected7\" + prefix+ "_cog_unique.fa";
//                @"G:\SugarSyncSharedByMe\SalsaBio\millions\420+74+1291\FASTA\74_fasta.txt";
                @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\sequences\unique_1016_SSU-ITS-LSU_alignment_Krueger_etal_2011.fa";
//                @"G:\SugarSyncSharedByMe\SalsaBio\cog\large\sequences\junk\cogAli_consensus.fa";
//                @"G:\SugarSyncSharedByMe\SalsaBio\cog\100k\sequences\cog_unique.fasta";
//                @"G:\SugarSyncSharedByMe\SalsaBio\cog\large\sequences\unique_183362_cog0303.fa";
//                @"G:\SugarSyncSharedByMe\SalsaBio\haixu\qiime_mother\mock_data\sequences\ArtificialGSFLXClean.fa";
//                @"G:\SugarSyncSharedByMe\SalsaBio\haixu\qiime_mother\mock_data\sequences\DivergentGSFLXClean.fa";
//            string fastaTwo = @"G:\SugarSyncSharedByMe\SalsaBio\cog\large\sequences\selected7\"+prefix +"_unique_183362_cog0303.fa";
//            string fastaTwo = @"G:\SugarSyncSharedByMe\SalsaBio\millions\420+74+1291\FASTA\1291_fasta.txt";
            string fastaTwo = @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\sequences\unique_934_1,081 Sequences.fasta";
//            string fastaTwo = @"G:\SugarSyncSharedByMe\SalsaBio\cog\large\sequences\junk\unique_183362_cog0303.fa";
//            string fastaTwo = @"G:\SugarSyncSharedByMe\SalsaBio\cog\large\sequences\unique_183362_cog0303.fa";
//            string fastaTwo = @"G:\SugarSyncSharedByMe\SalsaBio\cog\large\sequences\cogAli_consensus.fa";
//            string fastaTwo = @"G:\SugarSyncSharedByMe\SalsaBio\haixu\qiime_mother\mock_data\sequences\Artificial.fa";
//            string fastaTwo = @"G:\SugarSyncSharedByMe\SalsaBio\haixu\qiime_mother\mock_data\sequences\Divergent.fa";


            // bad way just checking
//            using (FastAParser pone = new FastAParser(fastaOne))
//            {
//                using (FastAParser ptwo = new FastAParser(fastaTwo))
//                {
//                    IList<ISequence> seqsOne = pone.Parse().ToList();
//                    IList<ISequence> seqsTwo = ptwo.Parse().ToList();
//                    int overlap = 0;
//                    foreach (ISequence seqOne in seqsOne)
//                    {
//                        foreach (ISequence seqTwo in seqsTwo)
//                        {
//                            if (SeqToString(seqOne).Equals(SeqToString(seqTwo)))
//                            {
//                                overlap++;
//                                
//                            }
//                        }
//                    }
//                    Console.WriteLine(overlap);
//                }
//            }

            string[] fileNames = new[]
                                     {
//                                         @"G:\SugarSyncSharedByMe\SalsaBio\millions\original_seqs\~1M(Haixu)\uniqs\length_gt200\random\allreads_uniques_gt200_446041_random.txt",
//                                         @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\sequences\74GenBank.txt",
//                                         @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\sequences\123.txt",
//                                         @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\centers\420centers\center_sequences.txt",
//                                         @"G:\SugarSyncSharedByMe\SalsaBio\millions\original_seqs\~1M(Haixu)\LSU copied 5_21_2012_1291seq\FASTA\allInOneFasta.txt",
//                                         @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\sequences\unique_1130_allInOneFasta.txt",
//                                         @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\sequences\126+74+988+945_126reversed.fasta",
//                                         @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\physequel\Fwd plans to make a tree\SSU-ITS-LSU_alignment_Krueger_etal_2011.fa",
//                                         @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\sequences\unique_1016_SSU-ITS-LSU_alignment_Krueger_etal_2011.fa",
//                                         @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\sequences\GbWitKru.txt",
//                                         @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\physequel\Fwd plans to make a tree\1,081 Sequences.fasta"
//                                         @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\sequences\unique_934_1,081 Sequences.fasta"
                                           @"G:\SugarSyncSharedByMe\SalsaBio\millions\original_seqs\~1M(Haixu)\uniqs\length_gt200\direct\allreads_uniques_gt200_446041.txt",
                                           @"G:\My Box Files\SalsaBio\millions\phy\updated_6.5.14\AM_fungi_qualScreened454Reads_toClusterWithReferenceSeqs.fasta"
                                     };
//            using (StreamWriter writer = new StreamWriter(@"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\sequences\junkoverlapmatrix.txt"))
            using (StreamWriter writer = new StreamWriter(@"G:\My Box Files\SalsaBio\millions\phy\updated_6.5.14\junkoverlapmatrix.txt"))
            {
//                string[] names = new string[] { "Haixu", "GenBank", "InitialCenters", "RefinedCenters", "WittiyaLSU", "LSU+SSU+ITS", "WittiyaLSU\\(LSU+SSU+ITS)" };
//                string[] names = new string[] { "WittiyaLSU", "LSU+SSU+ITS" };
//                string[] names = new string[] { "Ryan", "GbWitKru" };
                string[] names = new string[] { "446k", "87k" };
                writer.WriteLine("\t"+string.Join("\t", names));
                int count = 0;
                foreach (var f1 in fileNames)
                {
                    writer.Write(names[count]);
                    foreach (var f2 in fileNames)
                    {
                        if (!f1.Equals(f2))
                        {
                            writer.Write("\t" + FindOverlaps(f1, f2));
                        }
                        else
                        {
                            writer.Write("\tall");
                        }
                    }
                    writer.WriteLine();
                    ++count;
                }
            }
            Console.WriteLine("Done.");
            Console.Read();

        }

        private static int FindOverlaps(string fastaOne, string fastaTwo)
        {
            using (FastAParser parserOne = new FastAParser(fastaOne))
            {
                Hashtable ht = new Hashtable();
                List<ISequence> oneSeqs = parserOne.Parse().ToList();
                for (int i = 0; i < oneSeqs.Count; i++)
                {
                    string seqStr = SeqToString(oneSeqs[i]);
                    if (!ht.ContainsKey(seqStr))
                    {
                        ht.Add(seqStr, i);
                    }
                }

                using (FastAParser parserTwo = new FastAParser(fastaTwo))
                {
                    IList<ISequence> seqs = parserTwo.Parse().ToList();
                    int overlapCount = 0;
                    IList<ISequence> overlappingSeqs = new List<ISequence>();
                    string seqStr;
                 /*   using (
                        StreamWriter overlapClusterWriter =
                            new StreamWriter(Path.Combine(Path.GetDirectoryName(fastaTwo),
                                                          "[" + Path.GetFileNameWithoutExtension(fastaTwo) +
                                                          "]_overlap_cluster_with_[" +
                                                          Path.GetFileNameWithoutExtension(fastaOne) + "].txt")),
                                     fastaOrderWriter =
                                         new StreamWriter(Path.Combine(Path.GetDirectoryName(fastaTwo),
                                                                       "[" + Path.GetFileNameWithoutExtension(fastaTwo) +
                                                                       "]_overlap_order_with_[" +
                                                                       Path.GetFileNameWithoutExtension(fastaOne) + "].txt")),
                                     deltaWriter =
                                         new StreamWriter(Path.Combine(Path.GetDirectoryName(fastaTwo),
                                                                       "[" + Path.GetFileNameWithoutExtension(fastaTwo) +
                                                                       "]_minus_[" + Path.GetFileNameWithoutExtension(fastaOne) +
                                                                       "].txt")))*/
                    {
                        int count = 0;
                        foreach (ISequence seq in seqs)
                        {
                            seqStr = SeqToString(seq);
                            if (ht.ContainsKey(seqStr))
                            {
                                // seq overlaps with some sequence in the first fasta file
                                overlappingSeqs.Add(seq);
                                ++overlapCount;
                                /*overlapClusterWriter.WriteLine(count + "\t0");
                                fastaOrderWriter.WriteLine(overlapCount + "\t" + ht[seqStr]);*/
                            }
                            else
                            {
                                /*overlapClusterWriter.WriteLine(count + "\t1");
                                deltaWriter.WriteLine(">" + seq.ID);
                                deltaWriter.WriteLine(SeqToString(seq));*/
                            }
                            ++count;
                        }
                    }
                    Console.WriteLine(overlapCount);
                    /*if (overlappingSeqs.Count > 0)
                    {
                        using (
                            FastAFormatter formatter =
                                new FastAFormatter(Path.Combine(Path.GetDirectoryName(fastaTwo),
                                                                "[" + Path.GetFileNameWithoutExtension(fastaTwo) +
                                                                "]_overlap_with_[" + Path.GetFileNameWithoutExtension(fastaOne) +
                                                                "].fa")))
                        {
                            formatter.Write(overlappingSeqs);
                        }
                    }*/
                    return overlapCount;
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
