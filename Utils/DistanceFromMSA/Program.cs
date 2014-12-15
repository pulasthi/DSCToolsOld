using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bio;
using Bio.IO.FastA;
using Bio.SimilarityMatrices;

namespace DistanceFromMSA
{
    class Program
    {
        struct Alphabet
        {
            public string Name { get; set; }
            public HashSet<char> Bases { get; set; }

            public Alphabet(string name, HashSet<char> bases) : this()
            {
                Name = name;
                Bases = bases;
            }

            
        }
        static void Main(string[] args)
        {
            string msaFile =
                @"G:\My Box Files\SalsaBio\millions\phy\lsu_w+k\sequences\with454\AMF_Phylo_454OptimizedLength_with_Haixu_Wittaya_454_clusters_trimToBackboneSeqLength.fasta";
            string resultsDir = @"G:\My Box Files\SalsaBio\millions\phy\lsu_w+k\distmsa\pidng\with454";

            Alphabet[] alphs = GenerateAlphabets();

            ArrayList seqs = ParseMSASequences(msaFile);
            
            /* more consistency stuff */
//            Console.WriteLine(seqs[0]);
//            Console.WriteLine();
//            Console.WriteLine(seqs[seqs.Count -1]);
//            CheckParsingConsistency(msaFile, seqs);


            foreach (var alph in alphs)
            {
                string distanceType = "PIDNG";
                string msaDistanceFile = Path.Combine(resultsDir,
                                                      Path.GetFileNameWithoutExtension(msaFile) + "_" + distanceType +
                                                      "_" + alph.Name + "dist.bin");
                string msaAlignLenFile = Path.Combine(resultsDir,
                                                      Path.GetFileNameWithoutExtension(msaFile) + "_" + distanceType +
                                                      "_" + alph.Name + "alignlen.bin");
                string msaAlignLenPercentageFile = Path.Combine(resultsDir,
                                                      Path.GetFileNameWithoutExtension(msaFile) + "_" + distanceType +
                                                      "_" + alph.Name + "alignlen.percentage.bin");

                string msaIdLenPercentageFile = Path.Combine(resultsDir,
                                                     Path.GetFileNameWithoutExtension(msaFile) + "_" + distanceType +
                                                     "_" + alph.Name + "idlen.percentage.bin");

                using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(msaDistanceFile))) 
//                    alignLenWriter = new BinaryWriter(File.OpenWrite(msaAlignLenFile)),
//                    alignLenPercentageWriter = new BinaryWriter(File.OpenWrite(msaAlignLenPercentageFile)),
//                    idLenPercentageWriter = new BinaryWriter(File.OpenWrite(msaIdLenPercentageFile)))
                {

                    for (int i = 0; i < seqs.Count; i++)
                    {
                        string si = seqs[i] as string;
                        for (int j = 0; j < seqs.Count; j++)
                        {
                            string sj = seqs[j] as string;

                            Int16[] pidngData = ComputePIDNGDistance(si, sj, alph.Bases);
                            writer.Write(i == j ? (Int16)0 : pidngData[0]);
//                            alignLenWriter.Write(pidngData[1]);
//                            alignLenPercentageWriter.Write(pidngData[2]);
//                            idLenPercentageWriter.Write(pidngData[3]);
                        }
                        Console.WriteLine("Row " + i + " done.");
                    }
                }
                Console.WriteLine("Alphabet: " + alph.Name + " done.");
            }

           

            

            Console.WriteLine("Done.");
            Console.Read();
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
            return new[] {new Alphabet("StrictDNA", strictDNAAlphabet), new Alphabet("FullDNA", fullDNAAlphabet)};
        }

        private static Int16[] ComputePIDNGDistance(string sa, string sb, HashSet<char> alphabet )
        {
            int charLenA = 0, charLenB = 0; // character lengths in MSA aligned sequences
            int length = sa.Length;
            int count = 0; // aligned (overlapping) length -- consider only character pairs that are in the alphabet
            int id = 0;
            for (int i = 0; i < length; i++)
            {
//                if (alphabet.Contains(sa[i])) ++charLenA;
//                if (alphabet.Contains(sb[i])) ++charLenB;
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
            double pidng = count == 0 ? 0.0 : ((double) id)/count; // some pairs in MSA have not common alignment, grrr.
//            int minCharLen = Math.Min(charLenA, charLenB);
//            double alignLenPercentage = minCharLen == 0 ? 0.0 : ((double) count)/minCharLen;
//            double idLenPercentage = minCharLen == 0 ? 0.0 : ((double) id)/minCharLen;

//            return new[] { (Int16)((1.0 - pidng) * Int16.MaxValue), (Int16)count, (Int16)((1.0 - alignLenPercentage) * Int16.MaxValue), (Int16)((1.0 - idLenPercentage) * Int16.MaxValue) };
            return new[] { (Int16)((1.0 - pidng) * Int16.MaxValue)};
        }

        private static Int16 ComputePIDDistance(string sa, string sb, int go, int ge, SimilarityMatrix mat, HashSet<char> alphabet)
        {
            /* Issues I have
             A A C - -
             C T - T -
             should it be taken up to last T or first T
             * 
             *
             A A T C A -
             C - T - - -
             taken up to last T or last A?
             * 
             *
             C T - G - T -
             C - A - C - -
             taken up to last T or first C?
             * 
             * 
             not sure if these are possible, but if so what should be the decision
             
             */

            char gap = '-';
            int length = sa.Length;

            int startIdx = -1, endIdx = -1;
            bool inAlignment = false, inGapA = false, inGapB = false;
            for (int i = 0; i < length; i++)
            {
                if (!inAlignment)
                {
                    if (gap != sa[i] && gap != sb[i])
                    {
                        startIdx = endIdx = i;
                        inAlignment = true;
                        inGapA = inGapB = false;
                    }
                }
                else
                {
                    if (gap == sa[i] && gap == sb[i])
                    {
                        /*Compute partial PID for fraction form [startIdx,endIdx]*/
                        inAlignment = inGapA = inGapB = false;
                        startIdx = endIdx = -1;
                    }
                    else if (gap == sa[i])
                    {
                    }

                }
            
            }
            return 0;
        }

        private static void CheckParsingConsistency(string msaFile, ArrayList seqs)
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(Path.GetDirectoryName(msaFile), "junk.txt")))
            using (StreamWriter writer2 = new StreamWriter(Path.Combine(Path.GetDirectoryName(msaFile), "junk2.txt")))
            {
                foreach (var seq in seqs)
                {
                    writer.WriteLine(seq);
                }

                using (StreamReader reader = new StreamReader(msaFile))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line) && !line.StartsWith(">"))
                        {
                            writer2.WriteLine(SanitizeLine(line));
                        }
                    }
                }
            }
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

        static void SanitizeFasta(string file, char toRemove)
        {
            string dir = Path.GetDirectoryName(file) ?? string.Empty;
            string sanitizedFile = Path.Combine(dir,
                                                Path.GetFileNameWithoutExtension(file) + "_" + toRemove + "s_removed.fa");
            using (FastAParser parser = new FastAParser(file))
            {
                ISequence[] seqs = parser.Parse().ToArray();
                
                using (FastAFormatter formatter = new FastAFormatter(sanitizedFile))
                {
                    foreach (var seq in seqs)
                    {
                        byte[] sanitizedBytes =
                            seq.Where(b => char.ToUpper((char) b) != char.ToUpper(toRemove)).ToArray();
                        var s = new Sequence(Alphabets.DNA, sanitizedBytes, false);
                        s.ID = seq.ID;
                        formatter.Write(s);
                    }
                }
            }
        }

      
    }
}
