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
        private string _name;

        public VectorPoint(double[] vec)
        {
            _vec = vec;
            _length = vec.Count();
        }

        public VectorPoint(double[] vec, string name) : this(vec)
        {
            _name = name;
        }

        public double[] Vec
        {
            get { return _vec; }
        }

        public int Length
        {
            get { return _length; }
        }

        public string Name
        {
            get { return _name; }
        }

        public int NonZerComponentCount
        {
            get
            {
                return _vec.Count(t => t != 0.0);
            }
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
