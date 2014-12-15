using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigLib
{
    public class SWConfig 
    {
        /* Mandatory properties */
        public string InputFile { get; set; }
        public string OutputFile { get; set; }
        public string TimingFile { get; set; }
        public int NodeCount { get; set; }

        /* Optional properties */
        public bool WriteFullMatrix { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public float GapOpen { get; set; }
        public float GapExtension { get; set; }
    }
}
