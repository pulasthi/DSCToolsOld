using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bio;
using Bio.IO.FastA;

namespace ExtractRandoms
{
    class Program
    {
        static void Main(string[] args)
        {
            var fasta =
                @"G:\My Box Files\SalsaBio\alaska\sequences\overlaps\uniques\unique_694493_GSF647-R1-trimmed_1.fa";
            var randomMapping =
                @"G:\My Box Files\SalsaBio\alaska\sequences\overlaps\uniques\100k\unique_694493_GSF647-R2-trimmed_1_100000_mapping.txt";
            var output = Path.Combine(Path.GetDirectoryName(fasta) ?? string.Empty,
                                      "unique_694493_GSF647-R1-trimmed_1_100000.txt");

            using (var writer = new StreamWriter(output))
            {
                using (var parser = new FastAParser(fasta))
                {
                    using (var reader = new StreamReader(randomMapping))
                    {
                        var sep = new[] {'\t'};
                        IList<ISequence> seqs = parser.Parse().ToList();
                        reader.ReadLine();// ignore header line
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            if (string.IsNullOrEmpty(line)) continue;
                            var splits = line.Split(sep);
                            var seqNum = int.Parse(splits[1]);
                            var seq = seqs[seqNum];
                            writer.WriteLine(">" + seq.ID);
                            writer.WriteLine(SeqToString(seq));
                        }
                    }
                }
            }
        }

        private static string SeqToString(IEnumerable<byte> seq)
        {
            var asciiEncoding = new ASCIIEncoding();
            return asciiEncoding.GetString(seq.ToArray()).ToUpper();
        }
    }
}
