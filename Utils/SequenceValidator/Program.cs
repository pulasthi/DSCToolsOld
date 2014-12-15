using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MBF;
using MBF.IO;
using MBF.IO.Fasta;

namespace SequenceValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            Hashtable ht = new Hashtable();
            char [] ednafullAlphabet = new char[]{'A', 'T', 'G', 'C', 'S', 'W', 'R', 'Y', 'K', 'M', 'B', 'V', 'H', 'D', 'N', 'U'};
            for (int i = 0; i < ednafullAlphabet.Length; i++)
            {
                ht.Add(ednafullAlphabet[i], true);
            }

            string fasta =
//                @"C:\Users\sekanaya\Documents\Magic Briefcase\SALSA2\Millions\680K_100K(S)_uniq\Sequences\100k_sample_fasta.txt";
                @"C:\Users\sekanaya\Documents\Magic Briefcase\SALSA2\Millions\OriginalSequences\~400K\allreads.fna";
            string line;
            string name = string.Empty;
            char c;
            using (StreamReader reader = new StreamReader(fasta))
            {
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    if (line.StartsWith(">"))
                    {
                        name = line;
                    }
                    else
                    {
                        for (int i = 0; i < line.Length; i++)
                        {
                            c = line[i];
                            if (!ht.ContainsKey(c))
                            {
                                Console.WriteLine("BadChar " + c + " at " + i + " in " + name);
                            }
                        }
                    }

                }
            }

            Console.WriteLine("Done Manual.");

            ISequenceParser parser = new FastaParser();
            IList<ISequence> sequences = parser.Parse(fasta);
            ISequenceItem item;
            for (int i = 0; i < sequences.Count; i++)
            {
                ISequence sequence = sequences[i];
                for (int j = 0; j < sequence.Count; j++)
                {
                    item = sequence[j];
                    if (!ht.ContainsKey(item.Symbol))
                    {
                        Console.WriteLine("BadChar " + item.Symbol + " at " + j + " in " + sequence.DisplayID);
                    }
                }
            }

            Console.WriteLine("Done MBF.");
            Console.Read();
        }
    }
}
