using System;
using System.IO;

namespace Common
{
    public class SimplePointsReader : IDisposable
    {
        private readonly char[] _sep = new[] { ' ', '\t' };
        private readonly StreamReader _reader;

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
                        // usual 3D point
                        return new Point(double.Parse(splits[1]), double.Parse(splits[2]), double.Parse(splits[3]),
                                         int.Parse(splits[0]), int.Parse(splits[4]));
                    }
                    if (splits.Length > 2)
                    {
                        string[] tmp = new string[splits.Length - 2];
                        Array.Copy(splits, 1, tmp, 0, splits.Length - 2);
                        double[] coords = Array.ConvertAll<string, double>(tmp, double.Parse);
                        return new Point(coords, int.Parse(splits[0]), int.Parse(splits[splits.Length - 1]));
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
}
