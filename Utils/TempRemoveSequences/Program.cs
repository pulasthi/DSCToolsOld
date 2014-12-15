using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bio.IO.FastA;

namespace TempRemoveSequences
{
    class Program
    {
        private static string SeqToString(IEnumerable<byte> seq)
        {
            var asciiEncoding = new ASCIIEncoding();
            return asciiEncoding.GetString(seq.ToArray()).ToUpper();
        }

        static void Main(string[] args)
        {
            var sequenceFile =
                @"C:\Users\sekanaya\Downloads\final_unique_56706_AM_fungi_referenceSeqs_plus_QualScreened454Reads_sequences.fa";
            var removeSequences =
                @"C:\Users\sekanaya\Downloads\ChimeraSeqNames_UCHIME_USEARCH_Derep_AMFCluster_2baseInSlidWindBelowThresh.txt";
            var finalSequenceFile = Path.Combine(Path.GetDirectoryName(sequenceFile) ?? string.Empty,
                                                 "clean_final_unique_{0}_AM_fungi_referenceSeqs_plus_QualScreened454Reads_sequences.fa");
            var statFile = Path.Combine(Path.GetDirectoryName(sequenceFile) ?? string.Empty,
                                        "stat_clean_final_unique_{0}_AM_fungi_referenceSeqs_plus_QualScreened454Reads_sequences.txt");

            using (var parser = new FastAParser(sequenceFile))
            {
                var seqs = parser.Parse().ToList();
                using (var reader = new StreamReader(removeSequences))
                {
                    var idx = 0;
                    var removedCount = 0;
                    using (var writer = new StreamWriter(finalSequenceFile))
                    {
                        using (var statWriter = new StreamWriter(statFile))
                        {
                            var toRemoveNames = new HashSet<string>();
                            while (!reader.EndOfStream)
                            {
                                var name = reader.ReadLine();
                                if (string.IsNullOrEmpty(name)) continue;
                                name = name.Trim();
                                toRemoveNames.Add(name.ToUpper());
                            }

                            var sep = new [] {'_'};
                            
                            foreach (var seq in seqs)
                            {
                                var splits = seq.ID.Split(sep);
                                if (splits.Length < 1) throw new Exception("Error in sequence name " + seq.ID);
                                var seqName = splits[0].ToUpper();
                                if (toRemoveNames.Contains(seqName))
                                {
                                    statWriter.WriteLine(idx +"\t" + seq.ID);
                                    ++removedCount;
                                }
                                else
                                {
                                    writer.WriteLine(">" + seq.ID);
                                    writer.WriteLine(SeqToString(seq));
                                }
                                ++idx;
                            }
                            statWriter.WriteLine("Removed " + removedCount  + " sequences");
                            
                        }
                    }
                    File.Move(finalSequenceFile, string.Format(finalSequenceFile, (idx - removedCount)));
                    File.Move(statFile, string.Format(statFile, (idx - removedCount)));
                }
            }
        }
    }
}
