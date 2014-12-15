using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bio;
using Bio.IO.FastA;

namespace OrderedDuplicateRemoverConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string sequenceFile = args[0];
            HashSet<string> uniqSeqStrings = new HashSet<string>();
            SortedList<int, ISequence> sortedUniqSeqs = new SortedList<int, ISequence>();
            using (FastAParser parser = new FastAParser(sequenceFile))
            {
                IList<ISequence> seqs = parser.Parse().ToList();

                for (int i = 0; i < seqs.Count; i++)
                {
                    ISequence seq = seqs[i];
                    string seqStr = SeqToString(seq);

                    if (!uniqSeqStrings.Contains(seqStr))
                    {
                        uniqSeqStrings.Add(seqStr);
                        sortedUniqSeqs.Add(i,seq);
                    }
                }
            }

            string sortedUniqSequenceFile = Path.Combine(Path.GetDirectoryName(sequenceFile),
                                                         "sorted_unique_" + sortedUniqSeqs.Count + "_" +
                                                         Path.GetFileName(sequenceFile));
            using (FastAFormatter formatter = new FastAFormatter(sortedUniqSequenceFile))
            {
                foreach (var kv in sortedUniqSeqs)
                {
                    formatter.Write(kv.Value);
                }
            }

        }

        static string SeqToString(ISequence seq)
        {
            ASCIIEncoding enc = new ASCIIEncoding();
            return enc.GetString(seq.ToArray()).ToUpper();
        }

        public struct OrderedSeq
        {
            private ISequence _seq;
            private int _idx;

            public OrderedSeq(ISequence seq, int idx)
            {
                _seq = seq;
                _idx = idx;
            }

            public ISequence Seq
            {
                get { return _seq; }
            }

            public int Idx
            {
                get { return _idx; }
            }
        }
    }
}
