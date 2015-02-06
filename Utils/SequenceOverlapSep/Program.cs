using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bio;
using Bio.IO.FastA;

namespace SequenceOverlapSep
{
    class Program
    {
        static void Main(string[] args)
        {
            var subsetSequencesFile =
                @"E:\Sali\InCloud\IUBox\Box Sync\SalsaBio\16s\Sequences\uniques\100k\100k.equal.representation.Greenlandstability.trim.contigs.good.unique.pick.fasta";
            var fullSequencesFile =
                @"E:\Sali\InCloud\IUBox\Box Sync\SalsaBio\16s\Sequences\uniques\200k\200k.equal.representation.Greenlandstability.trim.contigs.good.unique.pick.fasta";
            var cleanFile = Path.Combine(Path.GetDirectoryName(fullSequencesFile) ?? String.Empty, "clean.fasta");
            using (var parser = new FastAParser(subsetSequencesFile))
            {
                var subSeqs = parser.Parse().ToList();
                var subSeqsSet = new HashSet<string>();
                foreach (var t in subSeqs)
                {
                    subSeqsSet.Add(SeqToString(t));
                }
                File.Copy(subsetSequencesFile, cleanFile, true);
                using (var writer = new StreamWriter(cleanFile, true))
                {
                    using (var fullParser = new FastAParser(fullSequencesFile))
                    {
                        var fullSeqs = fullParser.Parse().ToList();
                        for (int i = 0; i < fullSeqs.Count; i++)
                        {
                            var seq = fullSeqs[i];
                            var seqStr = SeqToString(seq);
                            if (subSeqsSet.Contains(seqStr)) continue;
                            writer.WriteLine(">"+seq.ID);
                            writer.WriteLine(seqStr);
                        }
                    }
                }
            }
        }

        static string SeqToString(IEnumerable<byte> sequence)
        {
            var encoding = new ASCIIEncoding();
            return encoding.GetString(sequence.ToArray()).ToUpper();
        }
    }
}
