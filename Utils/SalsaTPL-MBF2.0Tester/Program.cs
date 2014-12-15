using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bio;
using Bio.IO.FastA;
using Bio.IO.GenBank;
using Bio.SimilarityMatrices;
using Salsa.Core.Configuration;

namespace SalsaTPL_MBF2._0Tester
{
    class Program
    {
        private static bool IsOdd(int value)
        {
            return (value & 1) == 1;
        }
        static void Main(string[] args)
        {
            Console.WriteLine(IsOdd(-1));
            int x = -1;
            int y = 2;
            Console.WriteLine(x/y);
            Console.Read();
            return;
                
//            ConfigurationMgr mgr = new ConfigurationMgr();
//            SimilarityMatrix matrix = mgr.SmithWatermanMS.LoadSimilarityMatrix("EDNAFULL", MoleculeType.DNA);
//            Console.WriteLine(matrix.Name);


            // Trying to parse sequences.

            string fasta =
//                @"C:\Users\sekanaya\Documents\Magic Briefcase\SALSA2\Millions\680K_100K(S)_uniq\Sequences\100k_sample_fasta_clusters_21+5.fa";
                @"\\Tempest.ads.iu.edu\e$\Salsa\saliya\millions\680K_100K(S)_Clus5+21\Input\100k_sample_fasta_clusters_5+21.fa";
            FastAParser parser = new FastAParser(fasta);
            IEnumerable<ISequence> seqs = parser.Parse();
            IList<ISequence> seqList = new List<ISequence>();
            foreach (ISequence sequence in seqs)
            {
                seqList.Add(sequence);
                Console.WriteLine(sequence[0]);
            }

            using (StreamReader reader = new StreamReader(fasta))
            {
                int count = 0;
                while(!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (line.StartsWith(">"))
                    {
                        if (!line.Substring(1).Equals(seqList[count].ID))
                        {
                            Console.WriteLine("bad order at " + count);
                        }
                        count++;
                    }
                }
            }

            Console.WriteLine("Done.");
            Console.Read();
        }
    }
}
