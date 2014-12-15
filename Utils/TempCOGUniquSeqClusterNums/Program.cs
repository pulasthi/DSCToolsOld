using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bio;
using Bio.IO.FastA;

namespace TempCOGUniquSeqClusterNums
{
    class Program
    {
        static void Main(string[] args)
        {
//            OriginalMethod();
            MethodTwo();
        }

        private static void MethodTwo()
        {
            string fullClusterFile =
//                @"G:\SugarSyncSharedByMe\SalsaBio\cog\100k\sequences\junkcheckofclusassign\clustAssign2.txt";
                @"G:\SugarSyncSharedByMe\SalsaBio\cog\large\sequences\183362_pnums_to_cog_clusters_detailed.txt";
            string uniqueFastAFile =
//                @"G:\SugarSyncSharedByMe\SalsaBio\cog\100k\sequences\junkcheckofclusassign\cog_unique.fasta";
                @"G:\SugarSyncSharedByMe\SalsaBio\cog\large\sequences\unique_183362_cog0303.fa";


            Hashtable seqToCogMappingTable = new Hashtable();
            Hashtable cognameToNumberTable = new Hashtable();
            using (StreamReader reader = new StreamReader(fullClusterFile))
            {
                char[] sep = new []{'\t'};
                string[] splits;
                int cnum = 0;
                while (!reader.EndOfStream)
                {
                    splits = reader.ReadLine().Trim().Split(sep);
                    // Format for 100k cog is pnum<tab>seqname<tab>cogname<tab>seqlength<tab>somebooleanvalue
//                    seqToCogMappingTable.Add(splits[1], splits[2]);

                    // Format for 100k cog is pnum<tab>seqname<tab>cogname<tab>seqlength
                    seqToCogMappingTable.Add(splits[1], splits[2]);
                    if (!cognameToNumberTable.Contains(splits[2]))
                    {
                        cognameToNumberTable.Add(splits[2], cnum++);
                    }
                }
            }

            using (FastAParser parser = new FastAParser(uniqueFastAFile))
            {
                IList<ISequence> uniqSeqs = parser.Parse().ToList();
                string outputFile = Path.Combine(Path.GetDirectoryName(fullClusterFile),
                                                 Path.GetFileNameWithoutExtension(fullClusterFile) + "_detailed_uniqs_in_(" +
                                                 Path.GetFileName(uniqueFastAFile) + ")_order.txt");
                using (StreamWriter writer = new StreamWriter(outputFile))
                {
                    for (int i = 0; i < uniqSeqs.Count; i++)
                    {
                        ISequence sequence = uniqSeqs[i];
                        string cogName =(string) seqToCogMappingTable[sequence.ID.Trim()];
                        writer.WriteLine(i + "\t" + sequence.ID.Trim() + "\t" + cogName + "\t" + cognameToNumberTable[cogName] );
//                        writer.WriteLine(i + "\t" + seqToCogMappingTable[sequence.ID.Trim()]);
                    }
                }
            }

        }

        private static void OriginalMethod()
        {
            //            string allClustersTxt = @"G:\SugarSyncSharedByMe\SalsaBio\cog\100k\sequences\cog.full.clusters.txt";
            //            string allClustersTxt = @"G:\SugarSyncSharedByMe\SalsaBio\cog\100k\sequences\clustAssign2_sorted_by_pnum.txt";
            string allClustersTxt = @"G:\SugarSyncSharedByMe\SalsaBio\cog\100k\sequences\junkcheckofclusassign(sbpu)\clustAssign2_sbp.txt";
            
            string allSequencesFasta = @"G:\SugarSyncSharedByMe\SalsaBio\cog\100k\sequences\cog.fasta";
            string uniqSequencesFasta = @"G:\SugarSyncSharedByMe\SalsaBio\cog\100k\sequences\cog_unique.fasta";
            
//            string outUniquClusterTxt = @"G:\SugarSyncSharedByMe\SalsaBio\cog\100k\sequences\cog.unique.95672.clusters.txt";
//            string outUniquClusterTxt = @"G:\SugarSyncSharedByMe\SalsaBio\cog\100k\sequences\clustAssign2_sorted_by_pnum_unique_95672.txt";
            string outUniquClusterTxt = @"G:\SugarSyncSharedByMe\SalsaBio\cog\100k\sequences\junkcheckofclusassign(sbpu)\clustAssign2_sorted_by_pnum_unique_95672.txt";

            using (FastAParser allParser = new FastAParser(allSequencesFasta),
                               uniqParser = new FastAParser(uniqSequencesFasta))
            {
                using (StreamReader allClusReader = new StreamReader(allClustersTxt))
                {
                    using (StreamWriter uniqClusWriter = new StreamWriter(outUniquClusterTxt))
                    {
                        IList<ISequence> allSeqs = allParser.Parse().ToList();
                        IList<ISequence> uniqSeqs = uniqParser.Parse().ToList();

                        char[] sep = new[] {'\t', ' '};
                        int count = 0;
                        Hashtable ht = new Hashtable();
                        int c;
                        string cl;
                        while (!allClusReader.EndOfStream)
                        {
//                            c = int.Parse(allClusReader.ReadLine().Split(sep)[1]);
                            cl = allClusReader.ReadLine().Split(sep)[2];
                            if (!ht.ContainsKey(allSeqs[count].ID))
                            {
                                ht.Add(allSeqs[count].ID,cl);
                            }
                            else
                            {
//                                if (((int)ht[allSeqs[count].ID]) != c)
                                if (!((string)ht[allSeqs[count].ID]).Equals(cl))
                                {
//                                    Console.WriteLine(allSeqs[count].ID + " is assigned to " + ((int)ht[allSeqs[count].ID]) + " and " + c);
                                    Console.WriteLine(allSeqs[count].ID + " is assigned to " + ((string)ht[allSeqs[count].ID]) + " and " + cl);
                                }
                            }
                            ++count;
                        }

                        for (int i = 0; i < uniqSeqs.Count; i++)
                        {
                            ISequence sequence = uniqSeqs[i];
//                            uniqClusWriter.WriteLine(i + "\t" + ht[sequence.ID] +"\t" +sequence.ID);
                            uniqClusWriter.WriteLine(i + "\t" + ht[sequence.ID]);
                        }
                        Console.WriteLine("done.");
                        Console.Read();

                    }
                }
            }
        }
    }
}
