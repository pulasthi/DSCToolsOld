using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bio;
using Bio.IO.FastA;

namespace ReduceCenters
{
    class Program
    {
        static void Main(string[] args)
        {
            string reductionFile =
                @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\centers\420centers\reduced_centers.txt";
            string sequenceFile =
                @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\centers\420centers\center_sequences.txt";

            HashSet<string> tokeep = new HashSet<string>();
            using (FastAParser parser = new FastAParser(sequenceFile))
            {
                IList<ISequence> allSeqs = parser.Parse().ToList();

                using (StreamReader reader = new StreamReader(reductionFile))
                {
                    char[] sep = new[] {' ', '\t'};
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line))
                        {
                            string[] splits = line.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                            if (!"NULL".Equals(splits[2]))
                            {
                                tokeep.Add(splits[0] + "."  +splits[1] + "." + splits[2].ToUpper());
                            }
                        }
                        
                    }
                }

                IList<ISequence> reducedSeqs = allSeqs.Where(seq => tokeep.Contains(ParseCenterSeqId(seq.ID))).ToList();

                string reducedSequenceFile = Path.Combine(Path.GetDirectoryName(sequenceFile),
                                                          "reduced_" + reducedSeqs.Count + "_" +
                                                          Path.GetFileName(sequenceFile));
                using (FastAFormatter formatter = new FastAFormatter(reducedSequenceFile))
                {
                    formatter.Write(reducedSeqs);
                }
            }

        }

        // Returns a string of the format, region.cluster.method
        static string ParseCenterSeqId(string id)
        {
            char[] sep = new[] { ' ', '\t' };
            string[] splits = id.Split(sep);
            if (splits.Length != 8) throw new Exception("Invalid number of splits in id: " + id);
            return splits[5].Substring(10) + "." + splits[6].Substring(11) + "." + splits[7].Substring(10).ToUpper();
        }

        
    }
}
