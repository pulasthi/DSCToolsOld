using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MSAtoFasta
{
    class Program
    {
        static void Main(string[] args)
        {
            var msaFile = args[0];
            var fastaFile = Path.Combine(Path.GetDirectoryName(msaFile) ?? string.Empty,
                                         Path.GetFileNameWithoutExtension(msaFile) + "_sequences.fa");
            ExtractSequences(msaFile,fastaFile);

        }

        private static void ExtractSequences(string msaFile, string fastaFile)
        {
            using (var writer = new StreamWriter(fastaFile))
            {
                using (var reader = new StreamReader(msaFile))
                {
                    string seq = string.Empty;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (string.IsNullOrEmpty(line)) continue;

                        if (line.StartsWith(">"))
                        {
                            if (!string.IsNullOrEmpty(seq))
                            {
                                writer.WriteLine(seq);
                                seq = string.Empty;
                            }
                            writer.WriteLine(line);
                        }
                        else
                        {
                            seq += SanitizeLine(line.Trim());
                        }
                    }
                    if (!string.IsNullOrEmpty(seq))
                    {
                        writer.WriteLine(seq);
                    }
                }
            }
        }

        static string SanitizeLine(string line)
        {
            var sb = new StringBuilder(10000);
            const char gap = '-';
            const char tilde = '~';
            foreach (var c in line)
            {
                if (c == tilde || c == gap) continue;
                sb.Append(char.ToUpper(c));
            }
            return sb.ToString();
        }

       
    }
}
