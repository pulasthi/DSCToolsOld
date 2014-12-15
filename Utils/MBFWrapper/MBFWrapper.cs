using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using MBF;
using MBF.Algorithms.Alignment;
using MBF.IO.Fasta;
using MBF.SimilarityMatrices;
using Salsa.Core.Configuration.Sections;

namespace MBFWrapper
{
    public class MBFWrapper
    {
        public static Int16 CalculateDistance(ISequence sequenceA, ISequence sequenceB, SimilarityMatrix scoringMatrix, int gapOpen, int gapExt, bool overAlignedRegion)
        {
            var aligner = new NeedlemanWunschAligner();
            IList<IPairwiseSequenceAlignment> psas = aligner.Align(scoringMatrix, gapOpen, gapExt, sequenceA, sequenceB);
            IPairwiseSequenceAlignment psa = psas[0]; // Take the first alignment
            IList<PairwiseAlignedSequence> pass = psa.PairwiseAlignedSequences;
            PairwiseAlignedSequence pas = pass[0]; // Take the first PairwisedAlignedSequence
            return ((Int16)((1.0f - ComputePercentIdentity(pas, overAlignedRegion))*Int16.MaxValue));
        }

        private static float ComputePercentIdentity(PairwiseAlignedSequence pas, bool overAlignedRegion)
        {
            ISequence alignedSeqA = pas.FirstSequence;
            ISequence alignedSeqB = pas.SecondSequence;


            int start, end;
            
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
            for (int i = start; i <= end; i++)
            {
                if (alignedSeqA[i].Symbol == alignedSeqB[i].Symbol)
                {
                    identity++;
                }

            }            
            return identity / ((end - start) + 1);
        }

        public static IList<ISequence> ParseFASTA(string path)
        {
            return new FastaParser().Parse(path);
        }

        public enum SimilarityMatrices
        {
            EDNAFULL
        }

        public static SimilarityMatrix LoadSimilarityMatrix(SimilarityMatrices matrix, MoleculeType moleculeType)
        {
            return new NeedlemanWunschSection().LoadSimilarityMatrix(matrix.ToString(), moleculeType);
        }

    }
}
