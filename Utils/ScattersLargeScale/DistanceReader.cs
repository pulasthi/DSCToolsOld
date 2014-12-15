using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace ScattersLargeScale
{
    public class DistanceReader : IDisposable
    {
        private MatrixReader _matrixReader;
        private IList<Point> _pointsTable = new List<Point>();
        private bool _readPoints;
        private MatrixType _matrixType;

        public DistanceReader(string file, MatrixType matrixType, int cols, bool readPoints)
        {
            _readPoints = readPoints;
            _matrixType = matrixType;
            if (readPoints)
            {
                using (var reader = new SimplePointsReader(file))
                {
                    while (!reader.EndOfStream)
                    {
                        Point p = reader.ReadPoint();
                        _pointsTable.Add(p);
                    }
                }
            }
            else
            {
                _matrixReader = new MatrixReader(file, matrixType, cols);
            }
        }

        public byte[] ReadDistanceFromMatrix(int row, int col)
        {
            if (_readPoints)
            {
                throw new Exception("DistanceReader is instantiated to read from points file, but called the matrix file read");
            }
            return _matrixReader.Read(row, col);
        }

        public double ReadDistanceFromPointsFile(int row, int col)
        {
            if (!_readPoints)
            {
                throw new Exception("DistanceReader is instantiated to read from matrix file, but called the points file read");
            }
            Point rowPoint = _pointsTable[row];
            Point colPoint = _pointsTable[col];
            return rowPoint.DistanceTo(colPoint);
        }
        

        public void Dispose()
        {
            if (!_readPoints && _matrixReader != null)
            {
                _matrixReader.Dispose();
            }
        }
    }
}
