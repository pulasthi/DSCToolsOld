using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatsMissing
{
    /// <summary>
    /// Represents a block within a 2D array
    /// </summary>
    [Serializable]
    public class Block
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlockPartition"/> class.
        /// </summary>
        /// <param name="rowRange">The row range.</param>
        /// <param name="colRange">The col range.</param>
        public Block(Range rowRange, Range colRange)
        {
            RowRange = rowRange;
            ColumnRange = colRange;
        }

        public bool IsDiagonal { get; set; }

        private int _rowBlockNumber;
        private int _columnBlockNumber;

        public int RowBlockNumber
        {
            get { return _rowBlockNumber; }
            set { _rowBlockNumber = value; }
        }

        public int ColumnBlockNumber
        {
            get { return _columnBlockNumber; }
            set { _columnBlockNumber = value; }
        }

        public void SetIndex(int rowBlockNumber, int columnBlockNumber)
        {
            _rowBlockNumber = rowBlockNumber;
            _columnBlockNumber = columnBlockNumber;

        }

        /// <summary>
        /// Transposes the row and column ranges
        /// </summary>
        /// <returns></returns>
        public Block Transpose()
        {
            Block b = new Block(ColumnRange, RowRange);
            return b;
        }

        public readonly Range RowRange;

        public readonly Range ColumnRange;

        public override string ToString()
        {
            return string.Format("[{0} {1}]", RowRange.ToString(), ColumnRange.ToString());
        }
    }
}
