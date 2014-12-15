using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PwcVerifyCenters
{
    public class SimplePointsReader : IDisposable
    {
        private char[] _sep = new[] { ' ', '\t' };
        private StreamReader _reader;

        public SimplePointsReader(string file)
        {
            _reader = new StreamReader(File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read));
        }

        public Point ReadPoint()
        {
            string line;
            while (!_reader.EndOfStream)
            {
                line = _reader.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    string[] splits = line.Trim().Split(_sep);
                    if (splits.Length == 5)
                    {
                        return new Point(double.Parse(splits[1]), double.Parse(splits[2]), double.Parse(splits[3]),
                                         int.Parse(splits[0]), int.Parse(splits[4]));
                    }
                    else
                    {
                        // Assume it to be pnum<TAB>cnum format
                        return new Point(0.0, 0.0, 0.0, int.Parse(splits[0]), int.Parse(splits[1]));
                    }
                }
            }
            throw new EndOfStreamException("Couldn't find a valid point to read");
        }

        public bool EndOfStream
        {
            get { return _reader.EndOfStream; }
        }


        public void Dispose()
        {
            _reader.Close();
            _reader.Dispose();
        }
    }

    public class Point
    {
        private double _x;
        private double _y;
        private double _z;
        private int _index;
        private int _cluster;

        public Point(double x, double y, double z, int index, int cluster)
        {
            _x = x;
            _y = y;
            _z = z;
            _index = index;
            _cluster = cluster;
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
    }
}
