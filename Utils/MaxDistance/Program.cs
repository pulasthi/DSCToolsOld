using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bio;
using Bio.Algorithms.Alignment;
using Bio.IO.FastA;
using Bio.SimilarityMatrices;
using Common;

namespace MaxDistance
{
    public class Program
    {
        private static void Main(string[] args)
        {
            string fastaFile =
                @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\sequences\unique_2439_fasta_reversed_Ryans_with_Ns_removed.txt";
//            string pidDistance = @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\heatmaps\2439.416ReverseComplemented\distances\2439_swg_pid_c#.bin";
            string pidDistance = @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\distances\swg_pid\2439\unique_2439_fasta_reversed_Ryans_with_Ns_removed_pid_c#.bin";
            string scoreDistance =
                @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\heatmaps\2439.416ReverseComplemented\distances\2439_swg_scoreAveLocal_c#.bin";

            using (FastAParser parser = new FastAParser(fastaFile))
            {
                IList<ISequence> seqs = parser.Parse().ToList();
                int size = seqs.Count;
                using (MatrixReader pidReader = new MatrixReader(pidDistance, MatrixType.Int16, size), scoreReader = new MatrixReader(scoreDistance, MatrixType.Int16, size))
                {
                    Int16 dPidMax = -1;
                    IList<long> pnums = new List<long>();
                    IList<Int16> scoreDistances = new List<short>();

                    for (int i = 0; i < size; ++i)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            Int16 distPid = BitConverter.ToInt16(pidReader.Read(i, j), 0);
                            Int16 distScore = BitConverter.ToInt16(scoreReader.Read(i, j), 0);

                          
                            if (distPid > dPidMax)
                            {
                                dPidMax = distPid;
                                pnums = new List<long> {((long) i)*size + j};
                                scoreDistances = new List<short>{distScore};
                            }
                            else if (distPid == dPidMax)
                            {
                                pnums.Add(((long)i) * size + j);
                                scoreDistances.Add(distScore);
                            }

                        }
                    }

                    // TODO: Testing only
//                    dPidMax = BitConverter.ToInt16(pidReader.Read(1568, 1875), 0);
//                    scoreDistances[0] = BitConverter.ToInt16(scoreReader.Read(1568, 1875), 0);
//                    pnums[0] = 2418438;
//                    pnums[0] = 3826227;


                    SmithWatermanAligner aligner = new SmithWatermanAligner();
                    SimilarityMatrix ednafull = new SimilarityMatrix("EDNAFULL_newMBFstyle.txt");
                    int go = -16, ge = -4;

                    Console.WriteLine("(1-PID) Max: " + ((double)dPidMax / Int16.MaxValue));
                    Console.WriteLine();

                    

                    for (int i = 0; i < pnums.Count; i++)
                    {
                        long pnum = pnums[i];
                        int row = (int)(pnum/size);
                        int col = (int) (pnum%size);
                        Console.WriteLine("(" + row +"," + col + ") -- " + ((double)scoreDistances[i]/Int16.MaxValue));

                        ISequence inputA = seqs[row];
                        ISequence inputB = seqs[col];

                        IList<IPairwiseSequenceAlignment> psas = aligner.Align(ednafull, go, ge, inputA, inputB);
                        IPairwiseSequenceAlignment psa = psas[0]; // Take the first alignment
                        IList<PairwiseAlignedSequence> pass = psa.PairwiseAlignedSequences;
                        PairwiseAlignedSequence pas = pass[0]; // Take the first PairwisedAlignedSequence
                        WriteAlignment(pas);
                        double tempDPid = ((int)((1.0 - ComputePercentIdentity(pas)) * Int16.MaxValue) / (double)Int16.MaxValue); // to mimic the conversion (loss of precision) when writing as short
                        Console.WriteLine("Calculated (1-PID): " + tempDPid);
                        double tempDAvgLocalScore = ((int)((1.0 - ComputeAvgLocalScore(pas, ednafull)) * Int16.MaxValue) / (double)Int16.MaxValue); // to mimic the conversion (loss of precision) when writing as short
                        Console.WriteLine("Calculated (1-AvgLocalScore): " + tempDAvgLocalScore);
                        Console.WriteLine("\n\n");

                    }
                }

                
            }

            Console.WriteLine("done.");
            Console.Read();

        }

        private static void WriteAlignment(PairwiseAlignedSequence pas)
        {
            Console.WriteLine(SeqToString(pas.FirstSequence));
            Console.WriteLine(SeqToString(pas.SecondSequence));
            Console.WriteLine();
        }

        static int SelfAlignedScore(IEnumerable<byte> seq, SimilarityMatrix mat)
        {
            int s = 0;
            byte gap = (byte) '-';
            foreach (byte b in seq)
            {
                if (b != gap)
                {
                    s += mat[b, b];
                }
            }
            return s;
//            return seq.Sum(b => mat[b, b]);
        }

        static string SeqToString(ISequence seq)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            return encoding.GetString(seq.ToArray()).ToUpper();
        }
        static float ComputePercentIdentity(PairwiseAlignedSequence pas)
        {
            ISequence alignedSeqA = pas.FirstSequence;
            ISequence alignedSeqB = pas.SecondSequence;
            float identity = 0.0f;
            for (int i = 0; i < alignedSeqA.Count; i++)
            {
                char ca = Char.ToUpper((char) alignedSeqA[i]);
                char cb = Char.ToUpper((char) alignedSeqB[i]);
                if (ca == cb)
                {
                    identity++;
                }

            }
            return identity / alignedSeqA.Count;
        }

        static float ComputeAvgLocalScore(PairwiseAlignedSequence pas, SimilarityMatrix mat)
        {
            ISequence alignedSeqA = pas.FirstSequence;
            ISequence alignedSeqB = pas.SecondSequence;
            float avgLocalScore = pas.Score/
                                  (((float) (SelfAlignedScore(alignedSeqA, mat) + SelfAlignedScore(alignedSeqB, mat)))/2);
            return avgLocalScore;
        }
    }
}
    