using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bio;
using Bio.Algorithms.Alignment;
using Bio.IO.FastA;
using Bio.SimilarityMatrices;

namespace SWGMSQuickPairwise
{
    class Program
    {
        static void Main(string[] args)
        {
            string seqFile = @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\centers\420centers\reduced_126_center_sequences.txt";
//            string seqFile = @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\sequences\126+74_126reversed_refined_names_foxtofungi.fasta";
            string distFile = @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\distances\swg_pid\126\reduced_126_center_sequences.bin";
//            string distFile = @"G:\SugarSyncSharedByMe\SalsaBio\millions\phy\distances\swg_pid\126+74_126reversed\126+74_126rc_swg_edna_pid_from_scratch_c#.bin";
            int gapOpen = -16;
            int gapExt = -4;

            SimilarityMatrix mat = new SimilarityMatrix("EDNAFULL_newMBFstyle.txt");
            SmithWatermanAligner aligner = new SmithWatermanAligner();
            using (FastAParser parser = new FastAParser(seqFile))
            {
                IList<ISequence> seqs = parser.Parse().ToList();
                using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(distFile)))
                {
                    int count = 0;
                    for (int i = 0; i < seqs.Count; i++)
                    {
                        ISequence a = seqs[i];
                        for (int j = 0; j < seqs.Count; j++)
                        {
                            ISequence b = seqs[j];
                            if (!a.ID.Equals(b.ID))
                            {
                                IList<IPairwiseSequenceAlignment> psas = aligner.Align(mat, gapOpen, gapExt, a, b);
                                IPairwiseSequenceAlignment psa = psas[0]; // Take the first alignment
                                IList<PairwiseAlignedSequence> pass = psa.PairwiseAlignedSequences;
                                PairwiseAlignedSequence pas = pass[0]; // Take the first PairwisedAlignedSequence

                                writer.Write((Int16)((1.0f - ComputePercentIdentity(pas)) * Int16.MaxValue));

                            }
                            else
                            {
                                writer.Write((Int16)0);
                            }
                        }
                        if (++count % 20 == 0)
                        {
                            Console.WriteLine(count);
                        }
                    }
                }
            }
            Console.WriteLine("Done.");
            Console.Read();

        }

        static float ComputePercentIdentity(PairwiseAlignedSequence pas)
        {
            ISequence alignedSeqA = pas.FirstSequence;
            ISequence alignedSeqB = pas.SecondSequence;
            float identity = 0.0f;
            for (int i = 0; i < alignedSeqA.Count; i++)
            {
                char ca = Char.ToUpper((char)alignedSeqA[i]);
                char cb = Char.ToUpper((char)alignedSeqB[i]);
                if (ca == cb)
                {
                    identity++;
                }

            }
            return identity / alignedSeqA.Count;
        }

        static string SeqToString(ISequence seq)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            return encoding.GetString(seq.ToArray()).ToUpper();
        }
    }
}
