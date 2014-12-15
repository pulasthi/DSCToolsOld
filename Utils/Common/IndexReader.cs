using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common
{
    public class IndexReader : IDisposable
    {
        private readonly char[] _sep = new[] {' ', '\t' };
        private readonly StreamReader _reader;

        public IndexReader(string file)
        {
            _reader = new StreamReader(File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read));
        }

        public IndexEntry ReadIndexEntry()
        {
            while (!_reader.EndOfStream)
            {
                string line = _reader.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    string[] splits = line.Trim().Split(_sep);
                    int length,pnum;
                    if (int.TryParse(splits[splits.Length - 1], out length) && int.TryParse(splits[0], out pnum))
                    {
                        return new IndexEntry(pnum, splits[1], length);
                    }
                }
            }
            throw new EndOfStreamException("Couldn't find a valid index entry to read");
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

    public class IndexEntry
    {
        public int Pnum { get; set; }
        public string Name { get; set; }
        public int Length { get; set; }
        public IndexEntry(int pnum, string name, int length)
        {
            Pnum = pnum;
            Name = name;
            Length = length;
        }
    }
}
