using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Salsa.TestManager.Models
{
    public class SmithwatermanJob : BaseJob
    {
        private int _startIndex = 0;
        private int _endIndex = 35338;
        private string _timingFilePath = string.Empty;
        private string _fastaFilePath = string.Empty;
        private string _outputFolderPath = string.Empty;
        private bool _useRange = false;
        private short _gapOpenPenalty = 14;
        private short _gapExtensionPenalty = 4;

        internal SmithwatermanJob()
        {
        }

        public string TimingFilePath
        {
            get
            {
                return _timingFilePath;
            }
            set
            {
                value = value.Trim();

                if (_timingFilePath != value)
                {
                    _timingFilePath = value;
                    OnPropertyChanged("TimingFilePath");
                }
            }
        }

        public string FastaFilePath
        {
            get
            {
                return _fastaFilePath;
            }
            set
            {
                value = value.Trim();

                if (_fastaFilePath != value)
                {
                    _fastaFilePath = value;
                    OnPropertyChanged("FastaFilePath");
                }
            }
        }

        public string OutputFolderPath
        {
            get
            {
                return _outputFolderPath;
            }
            set
            {
                value = value.Trim();

                if (_outputFolderPath != value)
                {
                    _outputFolderPath = value;
                    OnPropertyChanged("OutputFolderPath");
                }
            }
        }

        public bool UseRange
        {
            get
            {
                return _useRange;
            }
            set
            {
                if (_useRange != value)
                {
                    _useRange = value;
                    OnPropertyChanged("UseRange");
                }
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
                if (_startIndex != value)
                {
                    _startIndex = value;
                    OnPropertyChanged("StartIndex");
                }
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
                if (_endIndex != value)
                {
                    _endIndex = value;
                    OnPropertyChanged("EndIndex");
                }
            }
        }

        public short GapOpenPenalty
        {
            get
            {
                return _gapOpenPenalty;
            }
            set
            {
                if (value != _gapOpenPenalty)
                {
                    _gapOpenPenalty = value;
                    OnPropertyChanged("GapOpenPenalty");
                }
            }
        }

        public short GapExtensionPenalty
        {
            get
            {
                return _gapExtensionPenalty;
            }
            set
            {
                if (value != _gapExtensionPenalty)
                {
                    _gapExtensionPenalty = value;
                    OnPropertyChanged("GapExtensionPenalty");
                }
            }
        }
    }
}
