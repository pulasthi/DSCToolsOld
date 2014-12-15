using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bio;
using Bio.IO.FastA;

namespace DuplicateCount
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var file = args[0];
            var uniqFile = Path.Combine(Path.GetDirectoryName(file) ?? string.Empty,
                                        "unique_{0}_" + Path.GetFileNameWithoutExtension(file) + ".fa");
            using (var parser = new FastAParser(file))
            {
                int uniqueCount = 0;
                using (var writer = new StreamWriter(new BufferedStream(new FileStream(uniqFile,FileMode.Create,FileAccess.Write))))
                {
                    var hs = new HashSet<string>();
                    Console.WriteLine("parsing...");
                    IList<ISequence> seqs = parser.Parse().ToList();
                    Console.WriteLine("  done -- Total seqs " + seqs.Count);
                    int duplicateCount = 0;
                    int count = 0;
                    foreach (var seq in seqs)
                    {
                        ++count;
                        if (count%1000 == 0) Console.WriteLine(count);
                        string str = SeqToString(seq);
                        if (hs.Contains(str))
                        {
                            ++duplicateCount;
                        }
                        else
                        {
                            hs.Add(str);
                            ++uniqueCount;
                            writer.WriteLine(">"+seq.ID);
                            writer.WriteLine(str);
                        }
                    }
                    Console.WriteLine("dups: " + duplicateCount);
                    Console.WriteLine("uniqs: " + uniqueCount);
                    writer.Close();
                }
                File.Move(uniqFile, string.Format(uniqFile, uniqueCount));
                Console.WriteLine("done.");
                Console.Read();
            }
        }

        private static string SeqToString(ISequence seq)
        {
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            return asciiEncoding.GetString(seq.ToArray()).ToUpper();
        }
    }
}
