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
        private readonly ReaderSignature readVector = null;
        private delegate VectorPoint ReaderSignature();
        
        public string Header { get; set; }

        public VectorPointsReader(string file)
        {
            _reader = new StreamReader(File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read));
            readVector = HeadlessVectorReader;
        }

        public VectorPointsReader(string file, bool ignoreFirstRow, bool ignoreFirstCol)
        {
            _reader = new StreamReader(File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read));
            if (ignoreFirstRow && !_reader.EndOfStream)
            {
                Header = _reader.ReadLine();
            }
            if (ignoreFirstCol)
            {
                readVector = IgnoreFirstColVectorReader;
            }
            else
            {
                readVector = HeadlessVectorReader;
            }
        }

        public VectorPoint ReadVectorPoint()
        {
            return readVector();
        }

        private VectorPoint IgnoreFirstColVectorReader()
        {
            while (!_reader.EndOfStream)
            {
                string line = _reader.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    var splits = line.Trim().Split(_sep);
                    double[] vec = splits.Skip(1).Select(double.Parse).ToArray();
                    return new VectorPoint(vec, splits[0]);
                }
            }
            throw new EndOfStreamException("Couldn't find a valid point to read");
        }
        private VectorPoint HeadlessVectorReader()
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
