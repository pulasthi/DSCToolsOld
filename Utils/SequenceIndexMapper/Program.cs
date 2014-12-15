using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bio;
using Bio.IO.FastA;

namespace SequenceIndexMapper
{
    class Program
    {
        static void Main(string[] args)
        {
            // Assumes unique sequence files
            string orignalSequenceFile = args[0];
            string subSetFile = args[1];
            string outDir = args[2];

            string compareBySeqOutput = Path.Combine(outDir, "PnumsBySeq_Org[" +
                                          Path.GetFileNameWithoutExtension(orignalSequenceFile) + "]_Sub[" +
                                          Path.GetFileNameWithoutExtension(subSetFile) + "].txt");

            string compareBySeqClusterOutput = Path.Combine(outDir, "PnumsBySeq_Org[" +
                                          Path.GetFileNameWithoutExtension(orignalSequenceFile) + "]_Sub[" +
                                          Path.GetFileNameWithoutExtension(subSetFile) + "]_cluster.txt");

            Console.Write("Working ... ");
            using (FastAParser originalSeqParser = new FastAParser(orignalSequenceFile))
            {
                IList<ISequence> originalSeqs = originalSeqParser.Parse().ToList();
                Hashtable seqToOriginalPnum = new Hashtable(originalSeqs.Count);
                for (int i = 0; i < originalSeqs.Count; i++)
                {
                    seqToOriginalPnum.Add(SeqToString(originalSeqs[i]), i);
                }
                using (FastAParser subSetParser = new FastAParser(subSetFile))
                {
                    IList<ISequence> subSetSeqs = subSetParser.Parse().ToList();
                    using (StreamWriter writerBySeqOutput = new StreamWriter(compareBySeqOutput),
                        writerBySeqClusterOutput = new StreamWriter(compareBySeqClusterOutput))
                    {
                        HashSet<int> originalPnumsForSubSetSeqs = new HashSet<int>();
                        for (int i = 0; i < subSetSeqs.Count; i++)
                        {
                            string subSetSeqString = SeqToString(subSetSeqs[i]);
                            if (!seqToOriginalPnum.ContainsKey(subSetSeqString))
                            {
                                throw new Exception("Error: Couldn't find seqeunce " + i + " in " + orignalSequenceFile);
                            }
                            int orignalPnum = (int)seqToOriginalPnum[subSetSeqString];
                            writerBySeqOutput.WriteLine(i +"\t" + orignalPnum );
                            originalPnumsForSubSetSeqs.Add(orignalPnum);
                        }

                        for (int i = 0; i < originalSeqs.Count; ++i)
                        {
                            writerBySeqClusterOutput.WriteLine(i +"\t" + (originalPnumsForSubSetSeqs.Contains(i) ? 0 : 1));
                        }
                    }
                }
            }
            Console.WriteLine("Done.");
            Console.Read();
        }

        static string SeqToString(IEnumerable<byte> seq)
        {
            ASCIIEncoding enc = new ASCIIEncoding();
            return enc.GetString(seq.ToArray()).ToUpper();
        }
    }
}
