using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Common;

namespace TwoBand
{
    class Program
    {
        static void Main(string[] args)
        {
            string msaFile = args[0];
            string edgeSumFile = args[1];
            int size = int.Parse(args[2]);

            const double cut = 0.6;
            ArrayList seqs = ParseMSASequences(msaFile);
            int count = 0;
            using (MatrixReader reader = new MatrixReader(edgeSumFile, MatrixType.Int16, size))
            {
                using (StreamWriter writer = new StreamWriter(
                    Path.Combine(Path.GetDirectoryName(edgeSumFile) ?? string.Empty, "temp.txt")))
                {
                    Alphabet[] alphs = GenerateAlphabets();
                    
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            double d = ((double) BitConverter.ToInt16(reader.Read(i, j), 0))/Int16.MaxValue;
                            if (d > cut)
                            {
                                ++count;
                                string si = (string) seqs[i];
                                string sj = (string) seqs[j];
                                writer.WriteLine(i+"\t"+j+"\t"+d + "\t" +
                                                 string.Join("\t",
                                                             ComputePIDNGDistance(si, sj, alphs[0].Bases).Select(
                                                                 x => x.ToString(CultureInfo.InvariantCulture)).ToArray()));
//                                writer.WriteLine(si);
//                                writer.WriteLine(sj);
//                                writer.WriteLine();
                            }
                        }
                        Console.WriteLine("row " + i +" done");
                    }
                }

            }
            Console.WriteLine("Done.");
            Console.WriteLine("count of > cut: " + count);
            Console.Read();

        }

        private static ArrayList ParseMSASequences(string msaFile)
        {
            using (StreamReader reader = new StreamReader(msaFile))
            {
                ArrayList seqs = new ArrayList(2000);
                string seq = string.Empty;
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line)) continue;

                    if (line.StartsWith(">"))
                    {
                        if (!string.IsNullOrEmpty(seq))
                        {
                            seqs.Add(SanitizeLine(seq));
                            seq = string.Empty;
                        }
                    }
                    else
                    {
                        seq += SanitizeLine(line.Trim());
                    }
                }
                if (!string.IsNullOrEmpty(seq))
                {
                    seqs.Add(SanitizeLine(seq));
                }
                return seqs;
            }
        }

        static string SanitizeLine(string line)
        {
            StringBuilder sb = new StringBuilder(10000);
            const char gap = '-';
            const char tilde = '~';
            foreach (char c in line)
            {
                sb.Append(c == tilde ? gap : (c != gap ? char.ToUpper(c) : c));
            }
            return sb.ToString();
        }

        private static IEnumerable<int> ComputePIDNGDistance(string sa, string sb, HashSet<char> alphabet)
        {
            int charLenA = 0, charLenB = 0; // character lengths in MSA aligned sequences
            int length = sa.Length;
            int count = 0; // aligned (overlapping) length -- consider only character pairs that are in the alphabet
            int id = 0;
            for (int i = 0; i < length; i++)
            {
                if (alphabet.Contains(sa[i])) ++charLenA;
                if (alphabet.Contains(sb[i])) ++charLenB;
                if (alphabet.Contains(sa[i]) && alphabet.Contains(sb[i]))
                {
                    // both characters are in alphabet
                    ++count;
                    if (sa[i] == sb[i]) // assume all are in either upper or lower case 
                    {
                        ++id;
                    }
                }
            }
            double pidng = count == 0 ? 0.0 : ((double)id) / count; // some pairs in MSA have not common alignment, grrr.
            int minCharLen = Math.Min(charLenA, charLenB);
            double alignLenPercentage = minCharLen == 0 ? 0.0 : ((double)count) / minCharLen;
            double idLenPercentage = minCharLen == 0 ? 0.0 : ((double)id) / minCharLen;

//            return new[] { (1.0 - pidng), count, (1.0 - alignLenPercentage), (1.0 - idLenPercentage)};
            return new[] { count, id, charLenA, charLenB, length};
        }

        private static Alphabet[] GenerateAlphabets()
        {
            /*
                A - Adenine
                C - Cytosine
                T - Thymine
                G - Guanine
             */
            HashSet<char> strictDNAAlphabet = new HashSet<char>(new[] { 'A', 'C', 'T', 'G' });
            /*
                A - Adenine
                C - Cytosine
                M - A or C
                G - Guanine
                R - G or A
                S - G or C
                V - G or V or A
                T - Thymine
                W - A or T
                Y - T or C
                H - A or C or T
                K - G or T
                D - G or A or T
                B - G or T or C
                N - A or G or T or C.
               */
            HashSet<char> fullDNAAlphabet =
                new HashSet<char>(new[] { 'A', 'C', 'M', 'G', 'R', 'S', 'V', 'T', 'W', 'Y', 'H', 'K', 'D', 'B', 'N' });
            return new[] { new Alphabet("StrictDNA", strictDNAAlphabet), new Alphabet("FullDNA", fullDNAAlphabet) };
        }
    }

    struct Alphabet
    {
        public string Name { get; set; }
        public HashSet<char> Bases { get; set; }

        public Alphabet(string name, HashSet<char> bases)
            : this()
        {
            Name = name;
            Bases = bases;
        }


    }
}
