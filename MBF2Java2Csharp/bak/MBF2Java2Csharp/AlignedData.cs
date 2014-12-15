using System;

namespace MBF2Java2Csharp
{
    public class AlignedData
    {
        Sequence _firstOriginalSequence;
        Sequence _secondOriginalSequence;
        Sequence _firstAlignedSequence;
        Sequence _secondAlignedSequence;

        int _firstAlignedSequenceStartOffset;
        int _secondAlignedSeqeunceStartOffset;
        int _firstAlignedSequenceEndOffset;
        int _secondAlignedSeqeunceEndOffset;

        int _firstOffset, _secondOffset;

        int _firstAlignedSequenceInsertionCount;
        int _secondAlignedSeqeunceInsertionCount;
        private int _score;

        public AlignedData(Sequence firstOriginalSequence, Sequence secondOriginalSequence)
        {
            _firstOriginalSequence = firstOriginalSequence;
            _secondOriginalSequence = secondOriginalSequence;
        }

        public Sequence FirstOriginalSequence
        {
            get { return _firstOriginalSequence; }
            set { _firstOriginalSequence = value; }
        }

        public Sequence SecondOriginalSequence
        {
            get { return _secondOriginalSequence; }
            set { _secondOriginalSequence = value; }
        }

        public Sequence FirstAlignedSequence
        {
            get { return _firstAlignedSequence; }
            set { _firstAlignedSequence = value; }
        }

        public Sequence SecondAlignedSequence
        {
            get { return _secondAlignedSequence; }
            set { _secondAlignedSequence = value; }
        }

        public int FirstAlignedSequenceStartOffset
        {
            get { return _firstAlignedSequenceStartOffset; }
            set { _firstAlignedSequenceStartOffset = value; }
        }

        public int SecondAlignedSeqeunceStartOffset
        {
            get { return _secondAlignedSeqeunceStartOffset; }
            set { _secondAlignedSeqeunceStartOffset = value; }
        }

        public int FirstAlignedSequenceEndOffset
        {
            get { return _firstAlignedSequenceEndOffset; }
            set { _firstAlignedSequenceEndOffset = value; }
        }

        public int SecondAlignedSeqeunceEndOffset
        {
            get { return _secondAlignedSeqeunceEndOffset; }
            set { _secondAlignedSeqeunceEndOffset = value; }
        }

        public int FirstOffset
        {
            get { return _firstOffset; }
            set { _firstOffset = value; }
        }

        public int SecondOffset
        {
            get { return _secondOffset; }
            set { _secondOffset = value; }
        }

        public int FirstAlignedSequenceInsertionCount
        {
            get { return _firstAlignedSequenceInsertionCount; }
            set { _firstAlignedSequenceInsertionCount = value; }
        }

        public int SecondAlignedSeqeunceInsertionCount
        {
            get { return _secondAlignedSeqeunceInsertionCount; }
            set { _secondAlignedSeqeunceInsertionCount = value; }
        }

        public int Score
        {
            get { return _score; }
            set { _score = value; }
        }


        public override string ToString()
        {
            return _firstAlignedSequence + Environment.NewLine + _secondAlignedSequence;
        }
    }
}
