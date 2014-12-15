using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBF2Java2Csharp
{
    public class SmithWatermanAligner : DynamicProgrammingPairwiseAligner
    {
        // SW begins traceback at cell with optimum score.  Use these variables
    // to track this in FillCell overrides.

    /// <summary>
    /// Stores details of all cells with best score.
    /// </summary>
    private IList<OptScoreCell> optScoreCells = new List<OptScoreCell>();

    /// <summary>
    /// Tracks optimal score for alignment.
    /// </summary>
    private int optScore = int.MinValue;


    public override string Name {
        get { return "Smith-Waterman"; } 
    }

    public override string Description {
        get { return "Pairwise local alignment"; }
    }

    /// <summary>
    /// Resets the members used to track optimum score and cell.
    /// </summary>
    protected override void ResetSpecificAlgorithmMemberVariables() {
        optScoreCells.Clear();
        optScore = int.MinValue;
    }

    /// <summary>
    /// Sets matrix boundary conditions for zeroth row in SmithWaterman alignment.
    /// Uses affine gap penalty.
    /// </summary>
    unsafe protected override void SetRowBoundaryConditionAffine() {
        for (short col = 0; col != numberOfCols; ++col) {
            ixGapScore[col] = int.MinValue / 2;
            maxScore[col] = 0;
            fSource[col] = SourceDirection.Stop; // no source for cells with 0
        }

        // Optimum score can be anywhere in the matrix.
        // These all have the same score, 0.
        // Track only cells with positive scores.
        optScore = 1;
        optScoreCells.Clear();
    }

    /// <summary>
    /// Sets matrix boundary conditions for zeroth column in SmithWaterman alignment.
    /// Uses affine gap penalty.
    /// </summary>
    /// <param name="row">Row number of cell.</param>
    /// <param name="cell">Cell number.</param>
    unsafe protected override void SetColumnBoundaryConditionAffine(short row, int cell) {
        iyGapScore = int.MinValue / 2;
        maxScoreDiagonal = maxScore[0];
        fSource[cell] = SourceDirection.Stop; // no source for cells with 0
    }

    /// <summary>
    /// Fills matrix cell specifically for SmithWaterman - Uses affine gap penalty.
    /// </summary>
    /// <param name="row">Row of cell.</param>
    /// <param name="col">Col of cell.</param>
    /// <param name="cell">Cell number.</param>
    unsafe protected override void FillCellAffine(short row, short col, int cell) {
        int score = SetCellValuesAffine(row, col, cell);

        // SmithWaterman does not use negative scores, instead, if score is < 0
        // set score to 0 and stop the alignment at that point.
        if (score < 0) {
            score = 0;
            fSource[cell] = SourceDirection.Stop;
        }

        maxScore[col] = score;

        // SmithWaterman traceback begins at cell with optimum score, save it here.
        if (score > optScore) {
            // New high score found. Clear old cell lists.
            // Update score and add this cell info
            optScoreCells.Clear();
            optScore = score;
            optScoreCells.Add(new OptScoreCell(row, col, cell));
        } else if (score == optScore) {
            // One more high scoring cell found.
            // Add cell info to opt score cell list
            optScoreCells.Add(new OptScoreCell(row, col, cell));
        }
    }

    /// <summary>
    /// Optimal score updated in fillCellAffine.
    /// So nothing to be done here.
    /// </summary>
    protected override void SetOptimalScoreAffine() {
    }

    unsafe protected override IList<AlignedData> Traceback() {
        IList<byte[]> alignedSequences = new List<byte[]>(optScoreCells.Count * 2);
        IList<int>offsets = new List<int>(optScoreCells.Count * 2);
        IList<short>startOffsets = new List<short>(optScoreCells.Count * 2);
        IList<short>endOffsets = new List<short>(optScoreCells.Count * 2);
        IList<int>insertions = new List<int>(optScoreCells.Count * 2);

        short col, row;
        foreach (OptScoreCell optCell in optScoreCells)
        {
            // need an array we can extend if necessary
            // aligned array will be backwards, may be longer than original sequence due to gaps
            int guessLen = Math.Max(firstInputSequence.Count, secondInputSequence.Count);

            // TODO: Remove below cast
            IList<Byte> alignedListA = new List<Byte>(guessLen);
            IList<Byte> alignedListB = new List<Byte>(guessLen);

            // Start at the optimum element of F and work backwards
            col = optCell.Column;
            row = optCell.Row;
            endOffsets.Add((short)(col - 1));
            endOffsets.Add((short)(row - 1));

            int colGaps = 0;
            int rowGaps = 0;

            bool done = false;

            int cell = optCell.Cell;
            while (!done) {
                // if next cell has score 0, we're done
                switch (fSource[cell]) {
                    case SourceDirection.Stop:
                        done = true;
                        break;

                    case SourceDirection.Diagonal:
                        // Diagonal, Aligned
                        alignedListA.Add(firstInputSequence.Get(col - 1));
                        alignedListB.Add(secondInputSequence.Get(row - 1));

                        col = (short) (col - 1);
                        row = (short) (row - 1);
                        cell = cell - numberOfCols - 1;
                        break;

                    case SourceDirection.Up:
                        // up, gap in A
                        alignedListA.Add(gapCode);
                        alignedListB.Add(secondInputSequence.Get(row - 1));

                        row = (short) (row - 1);
                        cell = cell - numberOfCols;
                        colGaps++;
                        break;

                    case SourceDirection.Left:
                        // left, gap in B
                        alignedListA.Add(firstInputSequence.Get(col - 1));
                        alignedListB.Add(gapCode);

                        col = (short) (col - 1);
                        cell = cell - 1;
                        rowGaps++;
                        break;

                    default:
                        // error condition, should never see this
                        String message = "SmithWatermanAligner : Bad source in traceback.";
                        throw new Exception(message);
                }
            }

            // Offset is start of alignment in input sequence with respect to other sequence.
            if (col - row >= 0) {
                offsets.Add(0);
                offsets.Add(col - row);
            } else {
                offsets.Add(-(col - row));
                offsets.Add(0);
            }

            startOffsets.Add(col);
            startOffsets.Add(row);

            insertions.Add(colGaps);
            insertions.Add(rowGaps);

            // prepare solution, copy diagnostic data, turn aligned sequences around, etc
            // Be nice, turn aligned solutions around so that they match the input sequences
            int i, j; // utility indices used to invert aligned sequences
            int len = alignedListA.Count;
            byte[] alignedA = new byte[len];
            byte[] alignedB = new byte[len];
            for (i = 0, j = len - 1; i < len; i++, j--) {
                alignedA[i] = alignedListA[j];
                alignedB[i] = alignedListB[j];
            }

            alignedSequences.Add(alignedA);
            alignedSequences.Add(alignedB);
        }

        return CollateResults(firstInputSequence, secondInputSequence,
                optScoreCells.Count, optScore, alignedSequences,
                offsets, startOffsets, endOffsets, insertions);
    }
    }
}
