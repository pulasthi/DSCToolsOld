using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bio;
using Bio.IO.FastA;

namespace DuplicateCountTwoDatasets
{
    class Program
    {
        private static void Main(string[] args)
        {
            var file1 = args[0];
            var file2 = args[1];
            var uniqFile1 = Path.Combine(Path.GetDirectoryName(file1) ?? string.Empty,
                                         "paired_unique_{0}_" + Path.GetFileNameWithoutExtension(file1) + ".fa");
            var uniqFile2 = Path.Combine(Path.GetDirectoryName(file2) ?? string.Empty,
                                         "paired_unique_{0}_" + Path.GetFileNameWithoutExtension(file2) + ".fa");
            var parsers = new FastAParser[2];
            parsers[0] = new FastAParser(file1);
            parsers[1] = new FastAParser(file2);
            var uniqueCount = 0;
            using (
                StreamWriter writer1 =
                    new StreamWriter(new BufferedStream(new FileStream(uniqFile1, FileMode.Create, FileAccess.Write))),
                             writer2 =
                                 new StreamWriter(
                                     new BufferedStream(new FileStream(uniqFile2, FileMode.Create, FileAccess.Write))))
            {
                var hs1 = new HashSet<string>();
                var hs2 = new HashSet<string>();
                Console.WriteLine("parsing...");
                var ops = new ParallelOptions {MaxDegreeOfParallelism = 2};
                var seqs = new IList<ISequence>[2];
                Parallel.For(0, ops.MaxDegreeOfParallelism, ops, index =>
                                                                     {
                                                                         seqs[index] = parsers[index].Parse().ToList();
                                                                     });

                Console.WriteLine("  done -- Total seqs1 " + seqs[0].Count + " Total seqs2 " + seqs[1].Count);
                if (seqs[0].Count != seqs[1].Count)
                {
                    Console.WriteLine("Error: sequence counts are not equal");
                }
                else
                {
                    int duplicateCount = 0;
                    int count = 0;
                    for (int i = 0; i < seqs[0].Count; ++i )
                    {
                        ++count;
                        if (count % 1000 == 0) Console.WriteLine(count);
                        var seq1 = seqs[0][i];
                        var seq2 = seqs[1][i];
                        var str1 = SeqToString(seq1);
                        var str2 = SeqToString(seq2);
                        if (hs1.Contains(str1) && hs2.Contains(str2))
                        {
                            ++duplicateCount;
                        }
                        else
                        {
                            if (!hs1.Contains(str1))
                            {
                                hs1.Add(str1);
                            }
                            if (!hs2.Contains(str2))
                            {
                                hs2.Add(str2);
                            }
                            ++uniqueCount;
                            writer1.WriteLine(">" + seq1.ID);
                            writer1.WriteLine(str1);
                            
                            writer2.WriteLine(">" + seq2.ID);
                            writer2.WriteLine(str2);
                        }
                    }
                    Console.WriteLine("dups: " + duplicateCount);
                    Console.WriteLine("uniqs: " + uniqueCount);
                    writer1.Close();
                    writer2.Close();
                    File.Move(uniqFile1, string.Format(uniqFile1, uniqueCount));
                    File.Move(uniqFile2, string.Format(uniqFile2, uniqueCount));
                    
                }
            }

            foreach (var parser in parsers)
            {
                parser.Close();
                parser.Dispose();
            }
            
            Console.WriteLine("done.");
            Console.Read();
            
        }

        private static string SeqToString(IEnumerable<byte> seq)
        {
            var asciiEncoding = new ASCIIEncoding();
            return asciiEncoding.GetString(seq.ToArray()).ToUpper();
        }
    }
}
