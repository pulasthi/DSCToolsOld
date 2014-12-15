using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigLib
{
    public class GlobalConfig
    {
        public string DataFileName { get; set; }
        public int DataPoints { get; set; }
        public int ProcessingOption { get; set; }
        public string BaseResultDirectoryName { get; set; }
        public string RunSetLabel { get; set; }
        public string ControlDirectoryName { get; set; }
        public int MPIIOStrategy { get; set; }
        public int DebugPrintOption { get; set; }

    }
}
