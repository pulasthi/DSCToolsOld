using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using MBF2Java2Csharp;

namespace MBF2Java2CsharpAlignmentTester
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = @"C:\Users\sekanaya\Desktop\swgtester2\100k_sample_fasta_clusters_21+5_100.txt";
            string alignFile = @"C:\Users\sekanaya\Desktop\swgtester2\MBF2Java2Csharp_alignment.txt";
            string distFile = @"C:\Users\sekanaya\Desktop\swgtester2\MBF2Java2Csharp_dist.txt";
            string lenFile = @"C:\Users\sekanaya\Desktop\swgtester2\MBF2Java2Csharp_len.txt";

            IList<Sequence> seqs = SequenceParser.parse(file);
            int count = seqs.Count;

            SimilarityMatrix matrix = SimilarityMatrix.EDNAFULL;
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
                                IList<AlignedData> ads = aligner.Align(matrix, go, ge, seqs[i], seqs[j]);
                                short d = DistanceUtil.ComputePercentIdentityDistanceAsShort(ads[0]);
//                                writer.WriteLine(ads[0].ToString());
//                                writer.WriteLine();
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
    }
}
