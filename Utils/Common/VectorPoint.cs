using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class VectorPoint
    {
        private double[] _vec;
        private int _length;

        public VectorPoint(double[] vec)
        {
            _vec = vec;
            _length = vec.Count();
        }

        public double[] Vec
        {
            get { return _vec; }
        }

        public int Length
        {
            get { return _length; }
        }


        public double EuclidenDistanceTo(VectorPoint p)
        {
            int len = _vec.Length;
            double[] pvec = p.Vec;
            double d = 0.0;
            for (int i = 0; i < len; i++)
            {
                d += Math.Pow((_vec[i] -pvec[i]),2);
            }
            return Math.Sqrt(d);
        }
    }
}
