using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common
{
    public class SimpleClusterReader : IDisposable
    {
        private readonly char[] _sep = new[] { ' ', '\t' };
        private readonly StreamReader _reader;

        public SimpleClusterReader(string file)
        {
            _reader = new StreamReader(File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read));
        }

        public Cluster ReadCluster()
        {
            while (!_reader.EndOfStream)
            {
                string line = _reader.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    string[] splits = line.Trim().Split(_sep);
                    int cnumPosition = splits.Length == 5 ? 4 : 1; 
                    int cnum;
                    if (int.TryParse(splits[cnumPosition], out cnum))
                    {
                        return new Cluster(int.Parse(splits[0]), cnum);
                    }
                    return new Cluster(int.Parse(splits[0]), splits[cnumPosition]);
                }
            }
            throw new EndOfStreamException("Couldn't find a valid cluster to read");
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

    public class Cluster
    {
        public int Pnum { get; set; }
        public int Cnum { get; set; }
        public string Cstring { get; set; }

       public Cluster(int pnum, string cstring)
        {
            Pnum = pnum;
            Cstring = cstring;
        }

        public Cluster(int pnum, int cnum)
        {
            Pnum = pnum;
            Cnum = cnum;
        }
    }

}
