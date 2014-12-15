using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MBF2Java2Csharp
{
    public class SequenceParser
    {
        public static IList<Sequence> parse(string file)
        {
            List<Sequence> seqs = new List<Sequence>();
            using (StreamReader reader = new StreamReader(file))
            {
                string line;
                string seqstr = string.Empty;
                Sequence seq;
                string name = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        if (line.StartsWith(">"))
                        {
                            seq = new Sequence(seqstr, name);
                            seqs.Add(seq);
                            name = line;
                            seqstr = string.Empty;
                            continue;
                        }
                        seqstr += line;
                    }
                }
                seq = new Sequence(seqstr, name);
                seqs.Add(seq);
                return seqs;
            }
        }
    }
}
