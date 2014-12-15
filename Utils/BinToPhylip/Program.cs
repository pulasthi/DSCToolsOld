using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bio;
using Bio.IO.FastA;

namespace BinToPhylip
{
    class Program
    {
        static void Main(string[] args)
        {
            string fastaFile =
                @"G:\My Box Files\SalsaBio\millions\phy\lsu_w+k\sequences\AMF_Phylo_Seqs_599Nts_Apr17MAFFT_seqs.fasta";
            string distFile = @"G:\My Box Files\SalsaBio\millions\phy\lsu_w+k\trees\599Nts\swgPID\MDSasChisq_SMACOF_1821_10D.bin";
            int size = 1821; // CHANGE THIS TOO WHEN YOU CHANGE FILE
            string phylipDistFile =
                @"G:\My Box Files\SalsaBio\millions\phy\lsu_w+k\trees\599Nts\swgPID\MDSasChisq_SMACOF_1821_10D.toninja.phylip";

            bool isDouble = true; // CHANGE THIS TOO WHEN YOU CHANGE FILE

            //-------------------------------------------------------------------------------------------------------------------------------//

            using (StreamWriter writer = new StreamWriter(phylipDistFile))
            {
                using (BinaryReader reader = new BinaryReader(new FileStream(distFile,FileMode.Open)))
                {

                    using (FastAParser parser = new FastAParser(fastaFile))
                    {
                        IList<ISequence> seqs = parser.Parse().ToList();

                        writer.WriteLine("\t" + size);
                        for (int i = 0; i < size; i++)
                        {
//                            writer.Write(seqs[i].ID.Replace(' ','_').Replace('\t','_') + "\t");
                            writer.Write(seqs[i].ID.Replace(' ','_').Replace('\t','_').Replace(':','_').Replace(',','_') + "\t");
//                            writer.Write("seq" + i + "\t");
                            for (int j = 0; j < size; j++)
                            {
                                double d = isDouble ? reader.ReadDouble() : (double) reader.ReadInt16();
                                if (i == j)
                                {
                                    writer.Write("000000000\t");
                                    continue;
                                }
                                double dist = isDouble ? d : d/Int16.MaxValue;
                                // Doing what Ninja does (i.e.rounding to int) internally with floating point numbers
                                int integerDist = 100*(int) (((100000000*dist) + 50)/100); // This results in a number between [0 - 1E8]
                                writer.Write(integerDist.ToString("000000000") + "\t"); // Therefore, need to make room for 9 digits
                                


//                                writer.Write(Math.Round(dist, 9).ToString("0.000000000") + "\t");
                                
                            }
                            writer.WriteLine();
                            if (i%100 == 0)
                            {
                                Console.WriteLine("Row " + i + " done");
                            }
                        }
                    }
                }
            }
            Console.WriteLine("All Done.");
            Console.Read();

        }
    }
}
