using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhylogeneticTreeToPlotViz
{
    public class PVIZCluster : IComparable
    {
        string _key;
        string _label;
        string _visible;
        string _default;
        Color _color;
        int _size;
        int _shape;

        public PVIZCluster()
        {
            _color = new Color();
        }
        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }
        public string Label
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
            }
        }
        public string Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                _visible = value;
            }
        }
        public string Default
        {
            get
            {
                return _default;
            }
            set
            {
                _default = value;
            }
        }
        public int Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
            }
        }
        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }

        public void setColor(int r, int g, int b)
        {
            _color.R = r;
            _color.G = g;
            _color.B = b;
        }

        public int Shape
        {
            get
            {
                return _shape;
            }
            set
            {
                _shape = value;
            }
        }

        public int CompareTo(object o)
        {
            // TODO Auto-generated method stub
            PVIZCluster dp = o as PVIZCluster;
            if (int.Parse(this.Key) >= int.Parse(dp.Key))
                return 1;
            else
                return -1;
        }
    }

    public class Color
    {
        int _r;
        int _g;
        int _b;
        int _a;
        public Color()
        {
        }
        public Color(int r, int g, int b, int a)
        {
            _r = r;
            _g = g;
            _b = b;
            _a = a;
        }
        public int R
        {
            get
            {
                return _r;
            }
            set
            {
                _r = value;
            }
        }
        public int G
        {
            get
            {
                return _g;
            }
            set
            {
                _g = value;
            }
        }
        public int B
        {
            get
            {
                return _b;
            }
            set
            {
                _b = value;
            }
        }
        public int A
        {
            get
            {
                return _a;
            }
            set
            {
                _a = value;
            }
        }

        
    }
}
