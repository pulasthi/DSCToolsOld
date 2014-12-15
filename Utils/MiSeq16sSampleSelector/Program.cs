using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bio;
using Bio.IO.FastA;

namespace MiSeq16sSampleSelector
{
    /// <summary>
    /// Note. ALWAYS check how many sequences were produced in the sample.
    /// This programs assumes that there are enough sequences from each group to pick an equally represented sample size.
    /// If this is not the case then the program needs to be changed to add extra sequences.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var countFile =
                @"G:\Box Sync\SalsaBio\16s\Sequences\uniques\Greenlandstability.trim.contigs.good.uchime.pick.count_table";
            var seqFile =
                @"G:\Box Sync\SalsaBio\16s\Sequences\uniques\Greenlandstability.trim.contigs.good.unique.pick.fasta";
            var sampleSize = 100000;
            var outSeqFile =
                @"G:\Box Sync\SalsaBio\16s\Sequences\uniques\100k.equal.representation.Greenlandstability.trim.contigs.good.unique.pick.fasta";
            var outMetaFile =
                @"G:\Box Sync\SalsaBio\16s\Sequences\uniques\100k.equal.representation.Greenlandstability.trim.contigs.good.unique.pick.metadata.txt";

            using (var reader = new StreamReader(countFile))
            {
                var sep = new[] {'\t'};
                if (reader.EndOfStream) return;
                var header = reader.ReadLine();
                if (string.IsNullOrEmpty(header)) return;
                var splits = header.Trim().Split(sep);
                if (splits.Length < 3) throw new Exception("invalid header, expects at least 1 group");
                var numGroups = splits.Length - 2;
                var groups = new string[numGroups];
                Array.Copy(splits, 2, groups, 0, numGroups);
                var seqToGroupTable = new Hashtable(); // stores sequence name and the depth group that it occurs the most times
               

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line)) continue;
                    splits = line.Trim().Split(sep);
                    if (splits.Length != numGroups+2) throw new Exception("invalid line, should have " + numGroups + " groups");
                    var max = int.MinValue;
                    var maxGroup = string.Empty;
                    for (var i = 2; i < splits.Length; ++i)
                    {
                        var count = int.Parse(splits[i]);
                        if (count < max) continue;
                        max = count;
                        maxGroup = groups[i - 2];
                    }
                    seqToGroupTable.Add(splits[0], maxGroup);
                }

                using (var fastaReader = new FastAParser(seqFile))
                {
                    var seqs = fastaReader.Parse().ToList();
                    using (StreamWriter writer = new StreamWriter(outSeqFile),
                        metaWriter = new StreamWriter(outMetaFile))
                    {
                        metaWriter.WriteLine("LocalIdx\tGlobalIdx\tSeqName\tSample");
                        var sampleMax = sampleSize/numGroups;
                        var groupToSampleCount = new Hashtable();
                        var outCount = 0;
                        for (var i = 0; i < seqs.Count; ++i)
                        {
                            var name = seqs[i].ID;
                            var group = seqToGroupTable[name];
                            if (!groupToSampleCount.Contains(group))
                            {
                                groupToSampleCount.Add(group, 1);
                            }
                            else
                            {
                                if (((int)groupToSampleCount[group]) == sampleMax) continue;
                                groupToSampleCount[group] = ((int)groupToSampleCount[group]) + 1;
                            }
                            
                            writer.WriteLine(">" + name);
                            writer.WriteLine(SeqToString(seqs[i]));
                            metaWriter.WriteLine(outCount + "\t" + i + "\t" + name + "\t" + group);
                            ++outCount;
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
