using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Bio;
using Bio.Algorithms.Alignment;
using Bio.IO.FastA;
using Bio.IO.GenBank;
using Bio.SimilarityMatrices;
using Salsa.Core.Configuration;
using Salsa.Core.Configuration.Sections;

namespace MBF2AlignmentTester
{
    class Program
    {
        static void Main(string[] args)
        {
            //            JalignerReadCompare();
            //            Console.Read();
            //            return;

            string file = @"C:\Users\sekanaya\Desktop\swgtester2\100k_sample_fasta_clusters_21+5_100.txt";
            string alignFile = @"C:\Users\sekanaya\Desktop\swgtester2\MBF2_alignment.txt";
            string distFile = @"C:\Users\sekanaya\Desktop\swgtester2\MBF2_dist.txt";
            string lenFile = @"C:\Users\sekanaya\Desktop\swgtester2\MBF2_len.txt";

            IList<ISequence> seqs = new FastAParser(file).Parse().ToList();
            int count = seqs.Count;

            SmithWatermanMS swgmssection = new ConfigurationMgr().SmithWatermanMS;
            SimilarityMatrix matrix = swgmssection.LoadSimilarityMatrix("EDNAFULL", MoleculeType.DNA);
            int go = -16;
            int ge = -4;

            SmithWatermanAligner aligner = new SmithWatermanAligner();

            Console.WriteLine("Working ...");
            Stopwatch timer = new Stopwatch();
            using (StreamWriter lenWriter = new StreamWriter(lenFile))
            {
                using (StreamWriter distWriter = new StreamWriter(distFile))
                {
                    using (StreamWriter writer = new StreamWriter(alignFile))
                    {
                        timer.Start();
                        for (int i = 0; i < count; i++)
                        {
                            for (int j = 0; j < count; j++)
                            {
                                IList<IPairwiseSequenceAlignment> psas = aligner.Align(matrix, go, ge, seqs[i], seqs[j]);
//                                IPairwiseSequenceAlignment psa = psas[0]; // Take the first alignment
//                                IList<PairwiseAlignedSequence> pass = psa.PairwiseAlignedSequences;
//                                PairwiseAlignedSequence pas = pass[0]; // Take the first PairwisedAlignedSequence
//                                WriteAlignment(pas, writer);
//                                distWriter.WriteLine((short) (1.0f - ComputePercentIdentity(pas))*short.MaxValue));
//                                distWriter.WriteLine(1.0f - ComputePercentIdentity(pas));
//                                distWriter.WriteLine(ComputePercentIdentity(pas));
//                                lenWriter.WriteLine(pas.FirstSequence.Count);
                            }
                            Console.WriteLine("Row " + i + " done.");
                        }
                        timer.Stop();
                        Console.WriteLine("Total Time: " + timer.ElapsedMilliseconds + "ms");
                    }
                    Console.WriteLine("Done.");
                    Console.Read();
                }
            }
        }

        public static void WriteAlignment(PairwiseAlignedSequence pas, StreamWriter writer)
        {
            for (int i = 0; i < pas.FirstSequence.Count; i++ )
            {
                writer.Write(""+ (char)pas.FirstSequence[i]);
            }
            writer.WriteLine();
            for (int i = 0; i < pas.SecondSequence.Count; i++)
            {
                writer.Write("" + (char)pas.SecondSequence[i]);
            }
            writer.WriteLine(Environment.NewLine);
        }

        static float ComputePercentIdentity(PairwiseAlignedSequence pas)
        {
            ISequence alignedSeqA = pas.FirstSequence;
            ISequence alignedSeqB = pas.SecondSequence;
            float identity = 0.0f;
            for (int i = 0; i < alignedSeqA.Count; i++)
            {
                if (alignedSeqA[i] == alignedSeqB[i])
                {
                    identity++;
                }

            }
//            return identity / alignedSeqA.Count;
            return identity;
        }

        public static void JalignerReadCompare()
        {
            string jalignerfasta =
                @"C:\Users\sekanaya\Desktop\swgtester2\100k_sample_fasta_clusters_21+5_100_JAlignerParserOut.txt";
            string originalfasta = @"C:\Users\sekanaya\Desktop\swgtester2\100k_sample_fasta_clusters_21+5_100.txt";

            IList<ISequence> jalignerSeqs = new FastAParser(jalignerfasta).Parse().ToList();
            IList<ISequence> orgSeqs = new FastAParser(originalfasta).Parse().ToList();

            if (jalignerSeqs.Count != orgSeqs.Count)
            {
                Console.WriteLine("Bad - org count " + orgSeqs.Count + " jalignercount " + jalignerSeqs.Count);
                return;
            }

            for (int i = 0; i < orgSeqs.Count; i++)
            {
                ISequence orgseq = orgSeqs[i];
                ISequence jalignerseq = jalignerSeqs[i];
                if (orgseq.ID != jalignerseq.ID)
                {
                    Console.WriteLine("Bad - org seq id " + orgseq.ID + " jaligner id " + jalignerseq.ID);
                    break;
                }

                if (!orgseq.SequenceEqual(jalignerseq))
                {
                    Console.WriteLine("Bad - org seq != jaligner seq");
                    break;
                }
            }
            Console.WriteLine("Done.");
        }
    }
}
