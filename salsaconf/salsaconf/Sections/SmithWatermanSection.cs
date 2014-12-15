using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Salsa.Core.Configuration.Sections
{
    public class SmithWatermanSection : Section
    {
        #region Constructors
        public SmithWatermanSection()
        { }
        #endregion

        #region Members
        private string _inputFile;
        private string _outputFile;
        private string _timingFile;
        private int _nodeCount;
        private bool _writeFullMatrix;
        private int _startIndex;
        private int _endIndex;
        private float _gapOpen;
        private float _gapExtension;
        #endregion

        #region Properties
        public override string Name
        {
            get
            {
                return "SmithWaterman";
            }
        }

        /* Mandatory properties */
        public string InputFile
        {
            get
            {
                return _inputFile;
            }
            set
            {
                _inputFile = value;
            }
        }

        public string OutputFile
        {
            get
            {
                return _outputFile;
            }
            set
            {
                _outputFile = value;
            }
        }
        public string TimingFile
        {
            get
            {
                return _timingFile;
            }
            set
            {
                _timingFile = value;
            }
        }
        public int NodeCount
        {
            get
            {
                return _nodeCount;
            }
            set
            {
                _nodeCount = value;
            }
        }

        /* Optional properties */
        public bool WriteFullMatrix
        {
            get
            {
                return _writeFullMatrix;
            }
            set
            {
                _writeFullMatrix = value;
            }
        }
        public int StartIndex
        {
            get
            {
                return _startIndex;
            }
            set
            {
                _startIndex = value;
            }
        }
        public int EndIndex
        {
            get
            {
                return _endIndex;
            }
            set
            {
                _endIndex = value;
            }
        }
        public float GapOpen
        {
            get
            {
                return _gapOpen;
            }
            set
            {
                _gapOpen = value;
            }
        }
        public float GapExtension
        {
            get
            {
                return _gapExtension;
            }
            set
            {
                _gapExtension = value;
            }
        }
        #endregion
    }
}
