using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistanceComparator
{
    public class RowData
    {
        private string _pair;
        private double _originalDistance;
        private double _geometricDistance;

        public RowData()
        {
        }

        public RowData(string pair, double geometricDistance)
        {
            _pair = pair;
            _geometricDistance = geometricDistance;
        }

        public RowData(string pair, double originalDistance, double geometricDistance)
        {
            _pair = pair;
            _originalDistance = originalDistance;
            _geometricDistance = geometricDistance;
        }

        public string Pair
        {
            get { return _pair; }
            set { _pair = value; }
        }

        public double OriginalDistance
        {
            get { return _originalDistance; }
            set { _originalDistance = value; }
        }

        public double GeometricDistance
        {
            get { return _geometricDistance; }
            set { _geometricDistance = value; }
        }
    }
}
