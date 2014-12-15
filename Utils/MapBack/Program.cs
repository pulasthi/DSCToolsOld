using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bio;
using Bio.IO.FastA;

namespace MapBack
{
    class Program
    {
        static void Main(string[] args)
        {
            int region = 2;
            string regionFasta = args[0];
//            string regionPoints = @"G:\SALSA2\Millions\680K_uniq\regions\dasmacof\" + region + @"\region_" + region + @"_points.txt";
            
            string sampleFasta =  args[1];
            
            string mappedRegionIdx = args[2];
//            string mappedRegionPoints = @"G:\SALSA2\Millions\680K_uniq\regions\sequences\sequencemappings\" + region + @"\region_" + region + "_sample_outsample_mapping_points.txt";


//            string regionFasta = @"G:\SALSA2\Millions\680K_uniq\29clus_based\sequences\15+17+22+3\hmp16sRNA_uniques_15+17+22+3.txt";
//            string regionPoints = @"G:\SALSA2\Millions\680K_uniq\29clus_based\(29)_{3,15,17,22}\DA-SMACOF\points.txt";
//            string sampleFasta = @"G:\SALSA2\Millions\680K_uniq\regions\sequences\sequencemappings\3\100k_sample_fasta_random_3.fa";

//            string mappedRegionIdx = @"G:\SALSA2\Millions\680K_uniq\regions\sequences\sequencemappings\3\region_3_sample_outsample_mapping.txt";
//            string mappedRegionIdx = @"G:\SALSA2\Millions\680K_uniq\29clus_based\(29)_{3,15,17,22}\DA-SMACOF\sequencemapped\region_3_sample_outsample_mapping.txt";
//            string mappedRegionPoints = @"G:\SALSA2\Millions\680K_uniq\regions\sequences\sequencemappings\3\region_3_sample_outsample_mapping_points.txt";
//            string mappedRegionPoints = @"G:\SALSA2\Millions\680K_uniq\29clus_based\(29)_{3,15,17,22}\DA-SMACOF\sequencemapped\region_3_sample_outsample_mapping_points.txt";


            Console.WriteLine("Working ...");
            IEnumerable<ISequence> sampleseqs;
            using (FastAParser sampleParser = new FastAParser(sampleFasta))
            {
                sampleseqs = sampleParser.Parse();

                Hashtable sampleSeqTable = new Hashtable();

                foreach (ISequence sampleseq in sampleseqs)
                {
                    sampleSeqTable.Add(sampleseq.ID, sampleseq);
                }

                IList<ISequence> regionseqs;
                using (FastAParser regionParser = new FastAParser(regionFasta))
                {
                    regionseqs = regionParser.Parse().ToList();


                    int sampleSeqCountInRegion = 0;

//                    using (StreamReader pointReader = new StreamReader(regionPoints))
//                    {
//                        string[] splits;
//                        using (StreamWriter pointWriter = new StreamWriter(mappedRegionPoints))
//                        {
                            using (StreamWriter idxWriter = new StreamWriter(mappedRegionIdx))
                            {
                                for (int i = 0; i < regionseqs.Count; i++)
                                {
//                                    splits = pointReader.ReadLine().Split('\t');
                                    ISequence regionSeq = regionseqs[i];
                                    if (sampleSeqTable.ContainsKey(regionSeq.ID))
                                    {
                                        idxWriter.WriteLine(i + "\t" + 1);
//                                        pointWriter.WriteLine(i + "\t" + splits[1] + "\t" + splits[2] + "\t" + splits[3] +
//                                                              "\t" + 1);
                                        sampleSeqCountInRegion++;
                                    }
                                    else
                                    {
                                        idxWriter.WriteLine(i + "\t" + 2);
//                                        pointWriter.WriteLine(i + "\t" + splits[1] + "\t" + splits[2] + "\t" + splits[3] +
//                                                              "\t" + 2);
                                    }
                                }
                                                        Console.WriteLine("Sample Sequence Count in Region " + region + ": " + sampleSeqCountInRegion);
//                                Console.WriteLine("Sample Sequence Count in Region 3: " + sampleSeqCountInRegion);
                            }
//                        }
//                    }
                }
            }
            Console.Read();
        }
    }
}
