using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBF2Java2Csharp
{
    unsafe public abstract class DynamicProgrammingPairwiseAligner
    {
        protected byte gapCode = 45;
        protected int internalGapOpenCost;
        protected SimilarityMatrix internalSimilarityMatrix;
        protected Sequence secondInputSequence;
        protected Sequence firstInputSequence;
        protected int internalGapExtensionCost;
        protected short numberOfCols;
        protected short numberOfRows;
        protected int* ixGapScore;
        protected int* maxScore;
        protected sbyte* fSource;
        protected int iyGapScore;
        protected int maxScoreDiagonal;

        /// <summary>
        /// Gets the name of the Aligner. Intended to be filled in
        /// by classes deriving from DynamicProgrammingPairwiseAligner class
        /// with the exact name of the Alignment algorithm.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the description of the Aligner. Intended to be filled in
        /// by classes deriving from DynamicProgrammingPairwiseAligner class
        /// with the exact details of the Alignment algorithm.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Pairwise alignment of two sequences using an affine gap penalty.  The various algorithms in derived classes (NeedlemanWunsch,
        /// SmithWaterman, and PairwiseOverlap) all use this general engine for alignment with an affine gap penalty.
        /// </summary>
        /// <param name="localSimilarityMatrix">Scoring matrix.</param>
        /// <param name="gapOpenPenalty">Gap open penalty (by convention, use a negative number for this.).</param>
        /// <param name="gapExtensionPenalty">Gap extension penalty (by convention, use a negative number for this.).</param>
        /// <param name="inputA">First input sequence.</param>
        /// <param name="inputB">Second input sequence.</param>
        /// <returns>A list of sequence alignments.</returns>
        public IList<AlignedData> Align(
                SimilarityMatrix localSimilarityMatrix,
                int gapOpenPenalty,
                int gapExtensionPenalty,
                Sequence inputA,
                Sequence inputB){


            // Initialize and perform validations for alignment
            // In addition, initialize gap extension penalty.
            SimpleAlignPrimer(localSimilarityMatrix, gapOpenPenalty, inputA, inputB);
            internalGapExtensionCost = gapExtensionPenalty;

            FillMatrixAffine();

            return Traceback();
        }

        private void SimpleAlignPrimer(SimilarityMatrix similarityMatrix, int gapPenalty, Sequence inputA, Sequence inputB) {
            ResetSpecificAlgorithmMemberVariables();

            // Set Gap Penalty and Similarity Matrix
            internalGapOpenCost = gapPenalty;

            // note that _gapExtensionCost is not used for linear gap penalty
            internalSimilarityMatrix = similarityMatrix;

            firstInputSequence = inputA;
            secondInputSequence = inputB;
        }

        /// <summary>
        /// Resets member variables that are unique to a specific algorithm.
        /// These must be reset for each alignment, initialization in the constructor
        /// only works for the first call to AlignSimple.  This routine is called at the beginning
        /// of each AlignSimple method.
        /// </summary>
        protected abstract void ResetSpecificAlgorithmMemberVariables();

        /// <summary>
        /// Fills matrix data for affine gap penalty implementation.
        /// </summary>
        unsafe protected void FillMatrixAffine()
        {
            numberOfCols = (short)(firstInputSequence.Count + 1);
            numberOfRows = (short)(secondInputSequence.Count + 1);
            int size = numberOfRows * numberOfCols;

            fixed (int* ixGapScoreE = &(new int[numberOfCols])[0], maxScoreE = &(new int[numberOfCols])[0])
            {
                ixGapScore = ixGapScoreE;
                maxScore = maxScoreE;
                fixed (sbyte* fSourceE = &(new sbyte[size])[0])
                {
                    fSource = fSourceE;
                    // Fill by rows
                    short row, col;
                    int cell;
                    // Set matrix bc along top row and left column.
                    SetRowBoundaryConditionAffine();

                    for (row = 1, cell = numberOfCols; row != numberOfRows; ++row)
                    {
                        SetColumnBoundaryConditionAffine(row, cell);
                        ++cell;
                        for (col = 1; col != numberOfCols; ++col, ++cell)
                        {
                            FillCellAffine(row, col, cell);
                        }
                    }

                    SetOptimalScoreAffine();
                }
            }
        }

        /// <summary>
        /// Sets boundary conditions for first row in dynamic programming matrix for affine gap penalty case.
        /// As in the FillCell methods, different algorithms will use different
        /// boundary conditions and will override this method.
        /// </summary>
        unsafe protected abstract void SetRowBoundaryConditionAffine();

        /// <summary>
        /// Sets boundary conditions for the zeroth column in dynamic programming
        /// matrix for affine gap penalty case.
        /// As in the FillCell methods, different algorithms will use different
        /// boundary conditions and will override this method.
        /// </summary>
        /// <param name="row">Row number of cell.</param>
        /// <param name="cell">Cell number.</param>
        unsafe protected abstract void SetColumnBoundaryConditionAffine(short row, int cell);

        /// <summary>
        /// Sets cell (row,col) of the matrix for affine gap implementation.  Different algorithms will use different scoring
        /// and traceback methods and therefore will override this method.
        /// Uses affine gap penalty.
        /// </summary>
        /// <param name="row">Row of cell to fill.</param>
        /// <param name="col">Col of cell to fill.</param>
        /// <param name="cell">Cell number.</param>
        unsafe protected abstract void FillCellAffine(short row, short col, int cell);

        /// <summary>
        /// Sets general case cell score and matrix elements for general affine gap case.
        /// </summary>
        /// <param name="row">Row of cell.</param>
        /// <param name="col">Col of cell.</param>
        /// <param name="cell">Cell number.</param>
        /// <returns>Score for cell.</returns>
        unsafe protected int SetCellValuesAffine(short row, short col, int cell) {
            int score;
            int extnScore, openScore;

            // _MaxScoreDiagonal is max(M[row-1,col-1], Iy[row-1,col-1], Iy[row-1,col-1])

            int diagScore = maxScoreDiagonal + internalSimilarityMatrix.GetValueAt(
                    secondInputSequence.Get(row - 1), firstInputSequence.Get(col - 1));

            // ~ Ix = _M[row - 1, col] + _gapOpenCost, _Ix[row - 1, col] + _gapExtensionCost);
            extnScore = ixGapScore[col] + internalGapExtensionCost;
            openScore = maxScore[col] + internalGapOpenCost;
            int scoreX = (extnScore >= openScore) ? extnScore : openScore;
            ixGapScore[col] = scoreX;

            // ~ Iy = Max(_M[row, col - 1] + _gapOpenCost, _Iy[row, col - 1] + _gapExtensionCost);
            extnScore = iyGapScore + internalGapExtensionCost;
            openScore = maxScore[col - 1] + internalGapOpenCost;
            iyGapScore = (extnScore >= openScore) ? extnScore : openScore;

            maxScoreDiagonal = maxScore[col];


            if (diagScore >= scoreX) {
                if (diagScore >= iyGapScore) {
                    score = diagScore;
                    fSource[cell] = SourceDirection.Diagonal;
                } else {
                    score = iyGapScore;
                    fSource[cell] = SourceDirection.Left;
                }
            } else {
                if (iyGapScore >= scoreX) {
                    score = iyGapScore;
                    fSource[cell] = SourceDirection.Left;
                } else {
                    score = scoreX;
                    fSource[cell] = SourceDirection.Up;
                }
            }

            return score;
        }

        /// <summary>
        /// Allows each algorithm to set optimal score at end of matrix construction
        /// Used for affine gap penalty.
        /// </summary>
        protected abstract void SetOptimalScoreAffine();

        unsafe protected abstract IList<AlignedData> Traceback();

        protected IList<AlignedData> CollateResults(Sequence inputA,
                                                   Sequence inputB,
                                                   int alignmentCount,
                                                   int optScore,
                                                   IList<byte[]> alignedSequences,
                                                   IList<int> offsets,
                                                   IList<short> startOffsets,
                                                   IList<short> endOffsets,
                                                   IList<int> insertions)
        {
            if (alignmentCount > 0)
            {
                IList<AlignedData> alignments = new List<AlignedData>(alignmentCount);
                Sequence seq;
                AlignedData ad;
                byte[] alignedA, alignedB;

                for (int i = 0; i < alignedSequences.Count; i += 2)
                {
                    ad  = new AlignedData(inputA, inputB);
                    alignedA = alignedSequences[i];
                    alignedB = alignedSequences[i + 1];
                    ad.Score = optScore;

                    seq = new Sequence(alignedA, inputA.Id);
                    ad.FirstAlignedSequence = seq;

                    seq = new Sequence(alignedB, inputB.Id);
                    ad.SecondAlignedSequence = seq;

                    ad.FirstOffset = offsets[i];
                    ad.SecondOffset = offsets[i + 1];

                    ad.FirstAlignedSequenceStartOffset = startOffsets[i];
                    ad.SecondAlignedSeqeunceStartOffset = startOffsets[i + 1];

                    ad.FirstAlignedSequenceEndOffset = endOffsets[i];
                    ad.SecondAlignedSeqeunceEndOffset = endOffsets[i + 1];

                    ad.FirstAlignedSequenceInsertionCount = insertions[i];
                    ad.SecondAlignedSeqeunceInsertionCount = insertions[i + 1];

                    alignments.Add(ad);
                }

                return alignments;
            }
            return new List<AlignedData>();
        }
    }
}
