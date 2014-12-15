using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bio;
using Bio.IO.FastA;

namespace SequenceSetDifference
{
    class Program
    {
        /// <summary>
        /// Finds the sequences that are in sequenceFile and within the given length range, but not in subsetmappingFile 
        /// subsetMappingFile has the format local#\torig#\tseqname\tlength (there's a header line, which should be ignored when reading data)
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var sequenceFile = args[0];
            var subsetMappingFile = args[1];
            var minLength = int.Parse(args[2]);
            var maxLength = int.Parse(args[3]);

            using (var parser = new FastAParser(sequenceFile))
            {
                var seqs = parser.Parse().ToList();
                var subsetIndices = new HashSet<int>();
                using (var reader = new StreamReader(subsetMappingFile))
                {
                    var sep = new[] {'\t'};
                    reader.ReadLine(); // ignore header line
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (string.IsNullOrEmpty(line)) continue;
                        var splits = line.Split(sep);
                        if (splits.Length != 4) continue;
                        subsetIndices.Add(int.Parse(splits[1]));
                    }
                }
                var remainingCount = seqs.Count - subsetIndices.Count;
                var remainingFile = Path.Combine(Path.GetDirectoryName(subsetMappingFile) ?? string.Empty,
                                                 "remaining_" + remainingCount + "_of_" + Path.GetFileName(sequenceFile));

                int validCount = 0;
                using (var writer = new StreamWriter(new BufferedStream(new FileStream(remainingFile,FileMode.Create,FileAccess.Write))))
                {
                    for (int i = 0; i < seqs.Count; i++)
                    {
                        if (subsetIndices.Contains(i)) continue;
                        var seq = seqs[i];
                        if (seq.Count < minLength || seq.Count > maxLength) continue;
                        ++validCount;
                        writer.WriteLine(">" + seq.ID);
                        writer.WriteLine(SeqToString(seq));
                    }
                    writer.Close();
                }
                Console.WriteLine("Number of sequences in " + Path.GetFileNameWithoutExtension(sequenceFile) +
                                  " but not in  " + Path.GetFileNameWithoutExtension(subsetMappingFile) +
                                  " within range [" + minLength + ", " + maxLength + "] is " + validCount);
                Console.Read();
            }
        }

        private static string SeqToString(IEnumerable<byte> seq)
        {
            var asciiEncoding = new ASCIIEncoding();
            return asciiEncoding.GetString(seq.ToArray()).ToUpper();
        }
    }
}
