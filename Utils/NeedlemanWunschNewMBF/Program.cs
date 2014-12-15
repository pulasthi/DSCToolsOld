using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bio;
using Bio.Algorithms.Alignment;
using Bio.IO.FastA;
using Bio.IO.GenBank;
using Bio.SimilarityMatrices;
using HPC.Utilities;

namespace NeedlemanWunschNewMBF
{
    class Program
    {
        static void Main(string[] args)
        {
            ISequence s1, s2;

            //            s1 = new Sequence(Alphabets.DNA, "CATCCAA");
            //            s2 = new Sequence(Alphabets.DNA, "AGAGATGGTTTT");

            // Sequences that give different alignments w.r.t. old MBF and BioJava update
            // s1 = new Sequence(Alphabets.DNA, "CTGGCACGGAGTTAGCCGGTGCTATTACTCTGGTACCGTCATCTGCCCTAAGGCTCTTTCGTCCCAGATTCAGAGGTTTACGATCCGAAACCTTCATCCCTCACGCGGCGTCGCTCCATCAGGCTTGCGCCCATTGTGGAAGATTCCTAACTGCTGCCTCCCGTAGGAGTGGGACCCGTGTCTCAGTGCCCCTGTGGCCGGCCACCCTCTCAGGCCGGCTATCCGTCGTCGCCTTGGTAGGCCTTTACCCCACCAACCAGCTGATGGAACGCAACCCCATCCAGAAGCGATAAATCTTTAGTGATGCACCACGGTGCATCACCACATCACGTATTAGCGGACCTTTCGGCCAGTTATTCGTGACTTCTGGGTAGGTCAGTTACGTGTTACTCACCCGTGCGCCACTCGGTCCGAAGACCGCGTTCGGCTTGCATGTCTTAAGCACGCCGCCAGCGTTCACCCTGAGCCAGGATCAAACTCTCTGAGACTTCCAAGGCACACAGGGGA");            
            // s2 = new Sequence(Alphabets.DNA, "CTTCACCCCAATCATCCATCCCACCTTAGGCGGCTGGCCCCTAAAAGGTTACCTCACCGACTTCGGGTGTTACAAACTCTCGTGGTGTGACGGGCGGTGTGTACAAGGCCCGGGAACGTATTCACCGCGGCGTGCTGATCCGCGATTACTAGCGATTCCGACTTCATGGAGGCGAGTTGCAGCCTCCAATCCGAACTGAGATCGGCTTTCAGAGATTAGCTTGCCGTCACCGGCTCGCAACTCGTTGTACCGACCATTGTAGCACGTGTGTAGCCCAGGTCATAAGGGGCATGATGATTTGACGTCATCCCCACCTTCCTCCGGTTTATTACCCGGCAGTCTCGCTAGAGTGCCCAACTTAATGATGGCAACTAACAATAAGGGTTGCGCTCGTTGCGGGACTTAACCCAACATCTCACGACACGAGCTGACGACAACCATGCACCACCTGTCTCCGATGTACCGAAGTAACTTCCTATCTCTAAGAATAGCATCGGGATGTCAAGACCT");
            
            // Another two sequences that screw up old MBF and BioJava update
            s1 = new Sequence(Alphabets.DNA, "TAGTGATGCACCACGGTGCATCACCACATCACGTATTAGCGGACCTTTCGGCCAGTTATTCGTGACTTCTGGGTAGGTCAGTTACGTGTTACTCACCCGTGCGCCACTCGGTCCGAAGACCGCGTTCGGCTTGCATGTCTTAAGCACGCCGCCAGCGTTCACCCTGAGCCAGGATCAAACTCTCTGAGACTTCCAAGGCACA");
            s2 = new Sequence(Alphabets.DNA, "CATCCCCACCTTCCTCCGGTTTATTACCCGGCAGTCTCGCTAGAGTGCCCAACTTAATGATGGCAACTAACAATAAGGGTTGCGCTCGTTGCGGGACTTAACCCAACATCTCACGACACGAGCTGACGACAACCATGCACCACCTGTCTCCGATGTACCGAAGTAACTTCCTATCTCTAAGAATAGCATCGGGATGTCA");
            Console.WriteLine("S1: " + s1);
            Console.WriteLine();
            Console.WriteLine("S2: " + s2);
            Console.WriteLine();




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
            SimilarityMatrix similarityMatrix = new SimilarityMatrix("EDNAFULL_newMBFstyle.txt");

            //            SimilarityMatrix similarityMatrix =
            //                new ConfigurationMgr().NeedlemanWunschSection.LoadSimilarityMatrix("EDNAFULL", MoleculeType.DNA);

            int go = -16;
            int ge = -4;

            NeedlemanWunschAligner aligner = new NeedlemanWunschAligner();
            IList<IPairwiseSequenceAlignment> psas = aligner.Align(similarityMatrix, go, ge, s1, s2);
            IPairwiseSequenceAlignment psa = psas[0]; // Take the first alignment
            IList<PairwiseAlignedSequence> pass = psa.PairwiseAlignedSequences;
            PairwiseAlignedSequence pas = pass[0]; // Take the first PairwisedAlignedSequence

            //            Console.WriteLine("S1 against S2");
            Console.WriteLine("aS1: ");
            PrintSequence(pas.FirstSequence);
            Console.WriteLine();
            Console.WriteLine("aS2: ");
            PrintSequence(pas.SecondSequence);

            Console.WriteLine("\n\nDistance S1 against S2");
            short d = ((Int16)((1.0f - ComputePercentIdentity(pas, true)) * Int16.MaxValue));
            Console.WriteLine(d);

            psas = aligner.Align(similarityMatrix, go, ge, s2, s1);
            psa = psas[0]; // Take the first alignment
            pass = psa.PairwiseAlignedSequences;
            pas = pass[0]; // Take the first PairwisedAlignedSequence

            //            Console.WriteLine("\nS2 against S1");
            Console.WriteLine("\n");
            Console.WriteLine("aS2: ");
            PrintSequence(pas.FirstSequence);
            Console.WriteLine();
            Console.WriteLine("aS1: ");
            PrintSequence(pas.SecondSequence);

            Console.WriteLine("\n\nDistance S2 against S1");
            d = ((Int16)((1.0f - ComputePercentIdentity(pas, true)) * Int16.MaxValue));
            Console.WriteLine(d);




            /*
             * Starting the pairwise alignment of 200 sequences
             */


            /*
            Console.WriteLine(@"Starting the pairwise alignment of 200 sequences");
            FastAParser parser = new FastAParser("200_fasta.txt");
            ISequence[] seqs = parser.Parse().ToArray();

            ISequence[] alignedSeqs = new ISequence[39800];
            double preciseTime = 0.0;
            HiPerfTimer preciseTimer;
            using (StreamWriter writer = new StreamWriter("MBF_new_alignments.txt"))
            {
                long count = 0;
                for (int i = 0; i < seqs.Length; i++)
                {
                    for (int j = i + 1; j < seqs.Length; j++)
                    {
                        preciseTimer = new HiPerfTimer();
                        preciseTimer.Start();
                        psas = aligner.Align(similarityMatrix, go, ge, seqs[i], seqs[j]);
                        preciseTimer.Stop();
                        preciseTime += preciseTimer.Duration;
                        psa = psas[0]; // Take the first alignment
                        pass = psa.PairwiseAlignedSequences;
                        pas = pass[0]; // Take the first PairwisedAlignedSequence
                        alignedSeqs[count++] = pas.FirstSequence;
                        alignedSeqs[count++] = pas.SecondSequence;
                    }
                }
                writer.WriteLine("Time to align 200 * (200 -1) / 2 sequences: " + ((preciseTime/1000000)/60) + "min");
                Console.WriteLine("Time to align 200 * (200 -1) / 2 sequences: " + ((preciseTime/1000000)/60) + "min");

                for (int i = 0; i < alignedSeqs.Length; i++)
                {
                    WriteSequence(alignedSeqs[i], writer);
                    writer.WriteLine();                    
                }
                Console.WriteLine("Done");
                //Time to align 200 * (200 -1) / 2 sequences: 9.26787999595602min

            }
            */
            Console.Read();
        }

        private static void WriteSequence(ISequence seq, StreamWriter writer)
        {
            long length = seq.Count;
            for (int i = 0; i < length; i++)
            {
                writer.Write((char)seq[i]);
            }
            
        }

        private static void PrintSequence(ISequence seq)
        {
            long length = seq.Count;
            for (int i = 0; i < length; i++)
            {
                Console.Write((char)seq[i]);
            }
        }


        private static float ComputePercentIdentity(PairwiseAlignedSequence pas, bool overAlignedRegion)
        {
            ISequence alignedSeqA = pas.FirstSequence;
            ISequence alignedSeqB = pas.SecondSequence;


            long start, end;

            if (overAlignedRegion)
            {
                /* Modifying percent identity calculation only for the aligned portion.*/

                // First non gap index
                start = Math.Max(alignedSeqA.IndexOfNonGap(), alignedSeqB.IndexOfNonGap());
                // Last non gap index
                end = Math.Min(alignedSeqA.LastIndexOfNonGap(), alignedSeqB.LastIndexOfNonGap());
            }
            else
            {
                /* Percent identity over full aligned sequence length */
                start = 0;
                end = alignedSeqA.Count - 1;
            }

            float identity = 0.0f;
            for (long i = start; i <= end; i++)
            {
                if (alignedSeqA[i] == alignedSeqB[i])
                {
                    identity++;
                }

            }
            return identity / ((end - start) + 1);
        }
    }
}
