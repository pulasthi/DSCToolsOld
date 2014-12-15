using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBF2Java2Csharp
{
    class OptScoreCell
    {
        /// <summary>
        /// Column number of cell with optimal score.
        /// </summary>
        public short Column;

        /// <summary>
        /// Row number of cell with optimal score.
        /// </summary>
        public short Row;

        /// <summary>
        /// Cell number of cell with optimal score.
        /// </summary>
        public int Cell;

        /// <summary>
        /// Initializes a new instance of the OptScoreCell struct.
        /// Creates best score cell with the input position values.
        /// </summary>
        /// <param name="row">Row Number.</param>
        /// <param name="column">Column Number.</param>
        public OptScoreCell(short row, short column)
        {
            Row = row;
            Column = column;
            Cell = 0;
        }

        /// <summary>
        /// Initializes a new instance of the OptScoreCell struct.
        /// Creates best score cell with the input position values.
        /// </summary>
        /// <param name="row">Row Number.</param>
        /// <param name="column">Column Number.</param>
        /// <param name="cell">Cell Number.</param>
        public OptScoreCell(short row, short column, int cell):this(row, column)
        {
            Cell = cell;
        }

        /// <summary>
        /// A == Operator.
        /// </summary>
        /// <param name="cell">cell to compare.</param>
        /// <returns>Result of comparison.</returns>
        public bool ValEq(OptScoreCell cell)
        {
            return
                    Row == cell.Row &&
                            Column == cell.Column &&
                            Cell == cell.Cell;
        }

        /// <summary>
        /// A != Operator.
        /// </summary>
        /// <param name="cell">cell to compare.</param>
        /// <returns>Result of comparison.</returns>
        public bool ValNotEq(OptScoreCell cell)
        {
            return !(this.ValEq(cell));
        }

        /// <summary>
        /// Override Equals method.
        /// </summary>
        /// <param name="obj">Object for comparison.</param>
        /// <returns>Result of comparison.</returns>
        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            OptScoreCell other = (OptScoreCell) obj;
            return this.ValEq(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
