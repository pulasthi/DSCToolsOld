using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bio;
using Bio.Algorithms.Alignment;
using Bio.IO.FastA;
using Bio.IO.GenBank;
using Bio.SimilarityMatrices;

namespace NeedlemanWunsch
{
    class Program
    {
        /// <summary>
        /// Method to check if COG 183K sequences result negative scores when aligned with NW: BLOSUM62 GO -9 GE -1
        /// Just use like 100 sequences first
        /// </summary>
        /// <param name="args"></param>
        static void CheckCogNw()
        {
            string cog183Kfa = @"G:\SugarSyncSharedByMe\SalsaBio\cog\large\sequences\unique_183362_cog0303.fa";
            string outdir = @"G:\SugarSyncSharedByMe\SalsaBio\cog\large\sequences\junkscorenegtest";

            SimilarityMatrix similarityMatrix = new SimilarityMatrix("BLOSUM62_newMBFstyle.txt");
            NeedlemanWunschAligner aligner = new NeedlemanWunschAligner();
            int go = -9;
            int ge = -1;
            
            using (FastAParser parser = new FastAParser(cog183Kfa))
            {
                using (StreamWriter writer = new StreamWriter(Path.Combine(outdir, "dist.txt")))
                {
                   

                    IList<ISequence> cogs = parser.Parse().ToList();
                    int max = 100;

                    for (int i = 0; i < max; i++)
                    {
                        if (cogs[i].Alphabet != Alphabets.Protein)
                        {
                            cogs[i] = new Sequence(Alphabets.AmbiguousProtein, SeqToString(cogs[i]));
                        }
                    }
                    for (int i = 0; i < max; i++)
                    {
                        for (int j = 0; j < max; j++)
                        {
                            IList<IPairwiseSequenceAlignment> psas = aligner.Align(similarityMatrix, go, ge, cogs[i], cogs[j]);
                            IPairwiseSequenceAlignment psa = psas[0]; // Take the first alignment
                            IList<PairwiseAlignedSequence> pass = psa.PairwiseAlignedSequences;
                            PairwiseAlignedSequence pas = pass[0]; // Take the first PairwisedAlignedSequence

                            writer.Write("\t" + pas.Score);
                        }
                        writer.WriteLine();
                        Console.WriteLine(i);
                    }
                }
            }
        }

        static string SeqToString(ISequence sequence)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            return encoding.GetString(sequence.ToArray()).ToUpper();
        }

        static void Main(string[] args)
        {
            CheckCogNw();
            return;
            ISequence s1, s2;

//            s1 = new Sequence(Alphabets.DNA, "CATCCAA");
//            s2 = new Sequence(Alphabets.DNA, "AGAGATGGTTTT");

            // Sequences that give different alignments w.r.t. this (old) MBF and BioJava update
//            s1 = new Sequence(Alphabets.DNA, "CTGGCACGGAGTTAGCCGGTGCTATTACTCTGGTACCGTCATCTGCCCTAAGGCTCTTTCGTCCCAGATTCAGAGGTTTACGATCCGAAACCTTCATCCCTCACGCGGCGTCGCTCCATCAGGCTTGCGCCCATTGTGGAAGATTCCTAACTGCTGCCTCCCGTAGGAGTGGGACCCGTGTCTCAGTGCCCCTGTGGCCGGCCACCCTCTCAGGCCGGCTATCCGTCGTCGCCTTGGTAGGCCTTTACCCCACCAACCAGCTGATGGAACGCAACCCCATCCAGAAGCGATAAATCTTTAGTGATGCACCACGGTGCATCACCACATCACGTATTAGCGGACCTTTCGGCCAGTTATTCGTGACTTCTGGGTAGGTCAGTTACGTGTTACTCACCCGTGCGCCACTCGGTCCGAAGACCGCGTTCGGCTTGCATGTCTTAAGCACGCCGCCAGCGTTCACCCTGAGCCAGGATCAAACTCTCTGAGACTTCCAAGGCACACAGGGGA");
//            s2 = new Sequence(Alphabets.DNA, "CTTCACCCCAATCATCCATCCCACCTTAGGCGGCTGGCCCCTAAAAGGTTACCTCACCGACTTCGGGTGTTACAAACTCTCGTGGTGTGACGGGCGGTGTGTACAAGGCCCGGGAACGTATTCACCGCGGCGTGCTGATCCGCGATTACTAGCGATTCCGACTTCATGGAGGCGAGTTGCAGCCTCCAATCCGAACTGAGATCGGCTTTCAGAGATTAGCTTGCCGTCACCGGCTCGCAACTCGTTGTACCGACCATTGTAGCACGTGTGTAGCCCAGGTCATAAGGGGCATGATGATTTGACGTCATCCCCACCTTCCTCCGGTTTATTACCCGGCAGTCTCGCTAGAGTGCCCAACTTAATGATGGCAACTAACAATAAGGGTTGCGCTCGTTGCGGGACTTAACCCAACATCTCACGACACGAGCTGACGACAACCATGCACCACCTGTCTCCGATGTACCGAAGTAACTTCCTATCTCTAAGAATAGCATCGGGATGTCAAGACCT");

            // Sequences that are at the two ends of a macro line analysis
//            s1 = new Sequence(Alphabets.DNA, "CATCCAAGAGATAGTTGATCGTCTGAGAC");
//            s1 = new Sequence(Alphabets.Protein, "MKKILTTTTTGGCCCMILLTLF");
            s1 = new Sequence(Alphabets.Protein, "PEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPEPE");
//            s2 = new Sequence(Alphabets.DNA, "AGAGATGACTTAGTAGAACGTAGCGGAGTTTT");
//            s2 = new Sequence(Alphabets.Protein, "MEAAARRGGRISGGCCTERH");
            s2 = new Sequence(Alphabets.Protein, "MGFYSGYSGGYSGGGYGSSFVLIVVLFILLIIVGATFLY");

//            Console.WriteLine("S1: " + s1);
//            Console.WriteLine();
//            Console.WriteLine("S2: " + s2);
//            Console.WriteLine();


            

            int[][] mat = {
                new[]{5, -4, -4, -4, -4, 1, 1, -4, -4, 1, -4, -1, -1, -1, -2, -4},
                new[]{-4, 5, -4, -4, -4, 1, -4, 1, 1, -4, -1, -4, -1, -1, -2, 5},
                new[]{-4, -4, 5, -4, 1, -4, 1, -4, 1, -4, -1, -1, -4, -1, -2, -4},
                new[]{-4, -4, -4, 5, 1, -4, -4, 1, -4, 1, -1, -1, -1, -4, -2, -4},
                new[]{-4, -4, 1, 1, -1, -4, -2, -2, -2, -2, -1, -1, -3, -3, -1, -4},
                new[]{1, 1, -4, -4, -4, -1, -2, -2, -2, -2, -3, -3, -1, -1, -1, 1},
                new[]{1, -4, 1, -4, -2, -2, -1, -4, -2, -2, -3, -1, -3, -1, -1, -4},
                new[]{-4, 1, -4, 1, -2, -2, -4, -1, -2, -2, -1, -3, -1, -3, -1, 1},
                new[]{-4, 1, 1, -4, -2, -2, -2, -2, -1, -4, -1, -3, -3, -1, -1, 1},
                new[]{1, -4, -4, 1, -2, -2, -2, -2, -4, -1, -3, -1, -1, -3, -1, -4},
                new[]{-4, -1, -1, -1, -1, -3, -3, -1, -1, -3, -1, -2, -2, -2, -1, -1},
                new[]{-1, -4, -1, -1, -1, -3, -1, -3, -3, -1, -2, -1, -2, -2, -1, -4},
                new[]{-1, -1, -4, -1, -3, -1, -3, -1, -3, -1, -2, -2, -1, -2, -1, -1},
                new[]{-1, -1, -1, -4, -3, -1, -1, -3, -1, -3, -2, -2, -2, -1, -1, -1},
                new[]{-2, -2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2},
                new[]{-4, 5, -4, -4, -4, 1, -4, 1, 1, -4, -1, -4, -1, -1, -2, 5}};
//            SimilarityMatrix similarityMatrix = new SimilarityMatrix(mat, "ATGCSWRYKMBVHDNU", "EDNAFULL", MoleculeType.DNA);
//            SimilarityMatrix similarityMatrix = new SimilarityMatrix("EDNAFULL_newMBFstyle.txt");
//            SimilarityMatrix similarityMatrix = new SimilarityMatrix("BLOSUM62_newMBFstyle.txt");
//            SimilarityMatrix similarityMatrix = new SimilarityMatrix("BLOSUM50_newMBFstyle.txt");
            SimilarityMatrix similarityMatrix = new SimilarityMatrix("PAM250_newMBFstyle.txt");

//            SimilarityMatrix similarityMatrix =
//                new ConfigurationMgr().NeedlemanWunschSection.LoadSimilarityMatrix("EDNAFULL", MoleculeType.DNA);

//            int go = -16;
            int go = -9;
//            int ge = -4;
            int ge = -1;

//            NeedlemanWunschAligner aligner = new NeedlemanWunschAligner();
            SmithWatermanAligner aligner = new SmithWatermanAligner();

            IList<IPairwiseSequenceAlignment> psas = aligner.Align(similarityMatrix, go, ge, s1, s2);
            IPairwiseSequenceAlignment psa = psas[0]; // Take the first alignment
            IList<PairwiseAlignedSequence> pass = psa.PairwiseAlignedSequences;
            PairwiseAlignedSequence pas = pass[0]; // Take the first PairwisedAlignedSequence

//            Console.WriteLine("S1 against S2");
            Console.WriteLine(ISeqToString(pas.FirstSequence));
            Console.WriteLine(ISeqToString(pas.SecondSequence));
            Console.WriteLine(pas.Score);

//            Console.WriteLine("\nEquality");
//            Console.WriteLine("aS1 == s1 ? " + pas.FirstSequence.SequenceEqual(s1));
//            Console.WriteLine("aS2 == s2 ? " + pas.SecondSequence.SequenceEqual(s2));

//            psas = aligner.Align(similarityMatrix, go, ge, s2, s1);
//            psa = psas[0]; // Take the first alignment
//            pass = psa.PairwiseAlignedSequences;
//            pas = pass[0]; // Take the first PairwisedAlignedSequence

//            Console.WriteLine("\nS2 against S1");
//            Console.WriteLine();
//            Console.WriteLine("aS2: " + pas.FirstSequence);
//            Console.WriteLine("aS1: " + pas.SecondSequence);

//            Console.WriteLine("\nDistance");
//            short d = ((Int16) ((1.0f - ComputePercentIdentity(pas, true))*Int16.MaxValue));
//            float d = 1.0f - ComputePercentIdentity(pas, true);
//            Console.WriteLine(d);


            // Testing ComputePercentIdentityShortSequence
//            ComputePercentIdentityShortSequence();

            Console.Read();

        }

        private static float ComputePercentIdentity(PairwiseAlignedSequence pas, bool overAlignedRegion)
        {
            ISequence alignedSeqA = pas.FirstSequence;
            ISequence alignedSeqB = pas.SecondSequence;

            Console.WriteLine("\nAligned Sequence Lengths");
            Console.WriteLine("SeqA Length " + alignedSeqA.Count);
            Console.WriteLine("SeqB Length " + alignedSeqB.Count);

            int start, end;

            if (overAlignedRegion)
            {
                /* Modifying percent identity calculation only for the aligned portion.*/

                // First non gap index
                start = (int) Math.Max(alignedSeqA.IndexOfNonGap(), alignedSeqB.IndexOfNonGap());
                // Last non gap index
                end = (int) Math.Min(alignedSeqA.LastIndexOfNonGap(), alignedSeqB.LastIndexOfNonGap());
            }
            else
            {
                /* Percent identity over full aligned sequence length */
                start = 0;
                end = (int) (alignedSeqA.Count - 1);
            }

            float identity = 0.0f;
            for (int i = start; i <= end; i++)
            {
                if (alignedSeqA[i] == alignedSeqB[i])
                {
                    identity++;
                }

            }
            return identity / ((end - start) + 1);
        }


        /// <summary>
        /// The idea of this computation is to consider the aligned length over the original short 
        /// sequence out of sa, sb. A hypothetical example is as follows.
        /// sa: ACTTGAGGGACC
        /// sb: ACTGAAC
        /// 
        /// After aligining,
        /// 
        /// aligned sa       : --ACTT--GAG-GGA--CC
        /// aligned sb       : ACTG----------AA-C-
        /// considered region:   |--------------|
        /// mismatches       : 3
        /// divisor          : 5 (length in the aligned region corresponding to the original short sequence)
        /// 
        /// We will count the mimatches w.r.t. aligned sb (since sb is the orignal short sequence), ignoring any 
        /// end gaps and gaps in aligned sj.
        ///
        /// </summary>
        /// <param name="pas">Pairwise aligned sequence</param>
        /// <param name="sa">First original sequences used in the alignment, this corresponds to the FirstSequence in pas </param>
        /// <param name="sb">The second original sequence used in the alignment, this corresponds to the SecondSequence in pas</param>
        /// <returns></returns>
        private static float ComputePercentIdentityShortSequence()
        {
            ISequence sa = new Sequence(Alphabets.DNA, "ACTTGAGGGACC");
            ISequence sb = new Sequence(Alphabets.DNA, "ACTGAAC");

            ISequence alignedSeqA = new Sequence(Alphabets.DNA, "--ACTT--GAG-GGA--CC");
            ISequence alignedSeqB = new Sequence(Alphabets.DNA, "ACTG----------AA-C-");

            ISequence alignedShortSeq = sa.Count <= sb.Count ? alignedSeqA : alignedSeqB;
            IAlphabet alphabet = alignedShortSeq.Alphabet;

            // Identifying aligned region
            int firstNonGapIdx = (int) Math.Max(alignedSeqA.IndexOfNonGap(), alignedSeqB.IndexOfNonGap());
            int lastNonGapIdx = (int) Math.Min(alignedSeqA.LastIndexOfNonGap(), alignedSeqB.LastIndexOfNonGap());

            float identity = 0.0f;
            int regionLength = 0;
            for (int i = firstNonGapIdx; i <= lastNonGapIdx; i++)
            {
                if (alphabet.CheckIsGap(alignedShortSeq[i]))
                {
                    continue;
                }

                if (alignedSeqA[i] == alignedSeqB[i])
                {
                    identity++;
                }
                regionLength++;
            }
            return identity / regionLength;

        }

        private static string ISeqToString(ISequence seq)
        {
            string s = string.Empty;
            for (int i = 0; i < seq.Count; i++)
            {
                s += (char)seq[i];
            }
            return s;
        }

    }
}
