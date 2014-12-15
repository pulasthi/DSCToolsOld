using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistanceComparator
{
    public class NegativeRowData
    {
        private string _pair;
        private double _originalDistance;

        public NegativeRowData()
        {
        }

        public NegativeRowData(string pair, double originalDistance)
        {
            _pair = pair;
            _originalDistance = originalDistance;
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
    }
}
