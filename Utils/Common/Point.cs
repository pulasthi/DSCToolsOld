using System;
using System.Xml.Linq;

namespace Common
{
    public class Point
    {
        private double _x;
        private double _y;
        private double _z;
        private int _index;
        private int _cluster;
        private string _label;
        private double[] _coords;

        public Point(double x, double y, double z, int index, int cluster) : this(x, y, z, index, cluster, string.Empty){}


        public Point(double x, double y, double z, int index, int cluster, string label)
        {
            _x = x;
            _y = y;
            _z = z;
            _index = index;
            _cluster = cluster;
            _label = label;
            this._coords = new double[] {x,y,z};
        }

        public Point(double[] coords, int index, int cluster)
        {
            this._coords = coords;
            this._index = index;
            this._cluster = cluster;
            if (_coords.Length > 0 && _coords.Length <= 3)
            {
                _x = _coords[0];
                _y = _coords.Length > 1 ? _coords[1] : 0.0;
                _z = _coords.Length > 2 ? _coords[2] : 0.0;
            }
        }

        public double X
        {
            get { return _x; }
            set { _x = value; }
        }

        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public double Z
        {
            get { return _z; }
            set { _z = value; }
        }

        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        public int Cluster
        {
            get { return _cluster; }
            set { _cluster = value; }
        }

        public string Label
        {
            get { return _label; }
            set { _label = value; }
        }

        public double[] Coords
        {
            get { return _coords; }
            set { _coords = value; }
        }

        public XElement ToPvizPointElement()
        {
            return new XElement("point",
                              new XElement("key", _index),
                              new XElement("clusterkey", _cluster),
                              new XElement("label", _label),
                              new XElement("location",
                                           new XAttribute("x", _x),
                                           new XAttribute("y", _y),
                                           new XAttribute("z", _z)));
        }

        public Point Clone()
        {
            return new Point(_x, _y, _z, _index, _cluster, _label);
        }

        public string SimplePointString()
        {
            return Index + "\t" + X + "\t" + Y + "\t" + Z + "\t" + Cluster;
        }

        public string PositionString()
        {
            return X + "\t" + Y + "\t" + Z;
        }

        public double DistanceTo(Point p)
        {
            double[] pcoords = p.Coords;
            double d = 0.0;
            for (int i = 0; i < _coords.Length; i++)
            {
                d += Math.Pow(_coords[i] - pcoords[i], 2);
            }
            return Math.Sqrt(d);
        }
    }
}
