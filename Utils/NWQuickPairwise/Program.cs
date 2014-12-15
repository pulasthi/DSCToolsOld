using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bio;
using Bio.Algorithms.Alignment;
using Bio.IO.FastA;
using Bio.SimilarityMatrices;

namespace NWQuickPairwise
{
    class Program
    {
        static void Main(string[] args)
        {
            string seqFile = @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\sequences\unique_2439_GbWitKruCenters.txt";
            string distFile = @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\sequences\unique_2439_GbWitKruCenters_nw_mmnorm_pairwise_dist.txt";
            int gapOpen = -16;
            int gapExt = -4;
            int twoGapOpen = 2*gapOpen;

            SimilarityMatrix mat = new SimilarityMatrix("EDNAFULL_newMBFstyle.txt");
            NeedlemanWunschAligner aligner = new NeedlemanWunschAligner();
            using (FastAParser parser = new FastAParser(seqFile))
            {
                IList<ISequence> seqs = parser.Parse().ToList();
                using (StreamWriter writer = new StreamWriter(distFile))
                {
                    int count = 0;
                    writer.WriteLine("\t" + seqs.Count);
                    foreach (var a in seqs)
                    {
                        writer.Write(a.ID + "\t");
                        foreach (var b in seqs)
                        {
                            if (!a.ID.Equals(b.ID))
                            {
                                IList<IPairwiseSequenceAlignment> psas = aligner.Align(mat, gapOpen, gapExt, a, b);
                                IPairwiseSequenceAlignment psa = psas[0]; // Take the first alignment
                                IList<PairwiseAlignedSequence> pass = psa.PairwiseAlignedSequences;
                                PairwiseAlignedSequence pas = pass[0]; // Take the first PairwisedAlignedSequence

                                int maxS = Math.Max(SelfAlignedScore(a, mat), SelfAlignedScore(b, mat));
                                int minS = twoGapOpen + ((int)a.Count + (int)b.Count)*gapExt; // Note. This needs to be replaced with zero if SW is used instead of NW
                                int s = (int)pas.Score;
                                double mmnorm = (s - minS)*1.0/(maxS - minS);

                                writer.Write(Math.Round(mmnorm,5) + "\t");
                            } 
                            else
                            {
                                writer.Write("0.00000\t");
                            }
                        }
                        writer.WriteLine();
                        if (++count % 100 == 0)
                        {
                            Console.WriteLine(count);
                        }
                    }
                }
            }
            Console.WriteLine("Done.");
            Console.Read();

        }

        static int SelfAlignedScore(IEnumerable<byte> seq, SimilarityMatrix mat)
        {
            return seq.Sum(b => mat[b, b]);
        }

        static  string SeqToString(ISequence seq)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            return encoding.GetString(seq.ToArray()).ToUpper();
        }
    }
}
