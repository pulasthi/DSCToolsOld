using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common
{
     public class VectorPointsReader : IDisposable
    {
        private readonly char[] _sep = new[] { ',', '\t' };
        private readonly StreamReader _reader;

        public VectorPointsReader(string file)
        {
            _reader = new StreamReader(File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read));
        }

        public VectorPoint ReadVectorPoint()
        {
            while (!_reader.EndOfStream)
            {
                string line = _reader.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    double[] vec = line.Trim().Split(_sep).Select(double.Parse).ToArray();
                    return new VectorPoint(vec);
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
