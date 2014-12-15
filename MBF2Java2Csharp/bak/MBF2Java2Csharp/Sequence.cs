using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBF2Java2Csharp
{
    public class Sequence
    {
        private readonly string _id = string.Empty;
        private readonly byte[] _sequenceData = null;
        private readonly short _count = 0;

        public Sequence(string sequence): this(sequence, "") 
        {}

        public Sequence(byte[] sequenceData):this(sequenceData, "")
        {}

        public Sequence(byte[] sequenceData, string id)
        {
            _sequenceData = sequenceData;
            _count = (short)sequenceData.Length;
            _id = id;
        }
        public Sequence(string sequence, string id)
        {
            _id = id;
            //Note. Assumes sequence is an ASCII which will get encoded one byte per one character
            _sequenceData = new ASCIIEncoding().GetBytes(sequence);
            _count = (short) sequence.Length;
        }

        public string Id
        {
            get {return _id;}
        }

        public byte Get(int i)
        {
            return _sequenceData[i];
        }

        public short Count
        {
            get { return _count; }
        }

        public override string ToString()
        {
            return new ASCIIEncoding().GetString(_sequenceData);
        }
    }
}
