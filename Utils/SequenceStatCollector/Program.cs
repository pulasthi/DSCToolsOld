using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MBF;
using MBF.IO.Fasta;
using Salsa.Core;

namespace SequenceStatCollector
{
    class Program
    {
        static void Main(string[] args)
        {

            //Load the command line args into our helper class which allows us to name arguments
            var pargs = new Arguments(args)
                            {
                                Usage = "Usage: SequenceStatCollector.exe /fastaFile=<string> /outDir=<string>"
                            };

            if (!pargs.CheckRequired(new[] { "fastaFile", "outDir"}))
            {
                Console.WriteLine(pargs.Usage);
                Console.Read();
                return;
            }

            Console.WriteLine("Working...\n");
            string fastaFile = pargs.GetValue<string>("fastaFile");
            IList<ISequence> seqs = new FastaParser().Parse(fastaFile);
            Hashtable ht = new Hashtable();

            string seqStr;

            string outDir = pargs.GetValue<string>("outDir");
            string fastaName = Path.GetFileNameWithoutExtension(fastaFile);

            string outFile = Path.Combine(outDir,
                                          fastaName + "_uniques.txt");
            FastaFormatter formatter = new FastaFormatter();
            using (StreamWriter writer = new StreamWriter(outFile))
            {
                foreach (ISequence seq in seqs)
                {
                    seqStr = seq.ToString();
                    if (ht.ContainsKey(seqStr))
                    {
                        ht[seqStr] = ((int) ht[seqStr]) + 1;
                    }
                    else
                    {
                        ht.Add(seqStr, 0);
                        formatter.Format(seq, writer);
                    }
                }
            }



            outFile = Path.Combine(outDir,
                                          fastaName + "_stat.txt");            
            using (StreamWriter writer = new StreamWriter(outFile))
            {
                int uniques = 0;
                int duplicateGroups = 0;
                int duplicateCount = 0;
                writer.WriteLine("DuplicateGroup\tDuplicateString\tNumOccurs\tDuplicateCount");
                foreach (DictionaryEntry kv in ht)
                {
                    if (((int)kv.Value) == 0)
                    {                        
                        uniques++;
                    }
                    else
                    {
                        duplicateGroups++;
                        writer.WriteLine(duplicateGroups + "\t" + kv.Key + "\t" + (((int)kv.Value) + 1) + "\t" + kv.Value);
                        duplicateCount += ((int) kv.Value);
                    }
                }
                writer.WriteLine("Uniques\t" + uniques);
                writer.WriteLine("DuplicateGroups\t" + duplicateGroups);
                writer.WriteLine("TotalDuplicateCount\t" + duplicateCount);
                writer.WriteLine("ReducibleUniques (Uniques + DuplicateGroups)\t" + (uniques + duplicateGroups));
                writer.WriteLine("TotalSequences\t" + (uniques + duplicateGroups + duplicateCount));
            }

            

            
//            int length = 0;
//            for (int i = 0; i < seqs.Count; i++)
//            {
//                var sequence = seqs[i];
//                length += sequence.Count;
//            }
//            Console.WriteLine(length/seqs.Count);
            Console.WriteLine("Done.");
            Console.Read();
        }
    }
}
