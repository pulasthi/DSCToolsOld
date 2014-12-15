using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhylogeneticTreeToPlotViz
{
    class PVIZPoint: IComparable
    {
        int _id;
        double _x;
        double _y;

        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }
        double _z;

        public double Z
        {
            get { return _z; }
            set { _z = value; }
        }
        int _group;

        public int Group
        {
            get { return _group; }
            set { _group = value; }
        }
        string _label;

        public string Label
        {
            get { return _label; }
            set { _label = value; }
        }

        public PVIZPoint(int id, double x, double y, double z, int group, string label)
        {
            _id = id;
            _x = x;
            _y = y;
            _z = z;
            _group = group;
            _label = label;
        }

        public PVIZPoint(string id, string x, string y, string z, string group)
        {
            _id = int.Parse(id);
            _x = double.Parse(x);
            _y = double.Parse(y);
            _z = double.Parse(z);
            _group = int.Parse(group);
        }

        public PVIZPoint(int id, double x, double y, double z, int group)
        {
            _id = id;
            _x = x;
            _y = y;
            _z = z;
            _group = group;
        }

        public PVIZPoint()
        {
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public double X
        {
            get { return _x; }
            set { _x = value; }
        }

        public int CompareTo(object o)
        {
            // TODO Auto-generated method stub
            PVIZPoint dp = o as PVIZPoint;
            if (this.Id >= dp.Id)
                return 1;
            else
                return -1;
        }
    }
}
