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
    /// This programs tries to pick an equally represented sample size from each group.
    /// If there are not enough sequences in some groups to contribute equally then sequences from other groups
    /// are added to fill in the difference. When filling the difference, the groups that have more than to contribute
    /// equally to the output are iterated in a round-robin fashion. However if difference is > # groups then round-robin
    /// will happen multiple times and the order in which round-robin will pick groups may not necessary be the same in each
    /// iteration because overflow groups are stored in a hashtable where doing a foreach on the keys doesn't gurantee this.
    ///
    /// In summary running this program twice for the same dataset where this overflow logic is invoked may produce different
    /// sequence files.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var countFile =
                @"G:\Box Sync\SalsaBio\16s\Sequences\uniques\Greenlandstability.trim.contigs.good.uchime.pick.count_table";
            var seqFile =
                @"G:\Box Sync\SalsaBio\16s\Sequences\uniques\Greenlandstability.trim.contigs.good.unique.pick.fasta";
            var sampleSize = 200000;
            var outSeqFile =
                @"G:\Box Sync\SalsaBio\16s\Sequences\uniques\200k\200k.equal.representation.Greenlandstability.trim.contigs.good.unique.pick.fasta";
            var outMetaFile =
                @"G:\Box Sync\SalsaBio\16s\Sequences\uniques\200k\200k.equal.representation.Greenlandstability.trim.contigs.good.unique.pick.metadata.txt";

            using (var reader = new StreamReader(countFile))
            {
                var sep = new[] { '\t' };
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
                    if (splits.Length != numGroups + 2) throw new Exception("invalid line, should have " + numGroups + " groups");
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
                        var sampleMax = sampleSize / numGroups;
                        var groupToSampleCount = new Hashtable();
                        var groupToOverFlowSeqIds = new Hashtable();
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
                                if (((int)groupToSampleCount[group]) == sampleMax)
                                {
                                    if (!groupToOverFlowSeqIds.Contains(group))
                                    {
                                        var overFlowIds = new List<int> { i };
                                        groupToOverFlowSeqIds.Add(group, overFlowIds);
                                    }
                                    else
                                    {
                                        var overFlowIds = (List<int>)groupToOverFlowSeqIds[group];
                                        overFlowIds.Add(i);
                                    }
                                    continue;
                                }
                                groupToSampleCount[group] = ((int)groupToSampleCount[group]) + 1;
                            }

                            writer.WriteLine(">" + name);
                            writer.WriteLine(SeqToString(seqs[i]));
                            metaWriter.WriteLine(outCount + "\t" + i + "\t" + name + "\t" + group);
                            ++outCount;
                        }

                        if (outCount >= sampleSize) return;

                        // Time to call in the overflow sequences :)
                        var diff = sampleSize - outCount;
                        var loopCount = 0;
                        var seqCount = 0;
                        while (diff != 0)
                        {
                            foreach (var overFlowGroup in groupToOverFlowSeqIds.Keys)
                            {
                                var overFlowIds = (List<int>)groupToOverFlowSeqIds[overFlowGroup];

                                if (overFlowIds.Count <= loopCount) continue;

                                var overFlowId = overFlowIds[loopCount];
                                var overFlowSeq = seqs[overFlowId];
                                writer.WriteLine(">" + overFlowSeq.ID);
                                writer.WriteLine(SeqToString(overFlowSeq));
                                metaWriter.WriteLine((outCount + seqCount) + "\t" + overFlowId + "\t" + overFlowSeq.ID + "\t" + overFlowGroup);
                                ++seqCount;
                                --diff;

                                if (diff == 0) break;
                            }
                            ++loopCount;
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
