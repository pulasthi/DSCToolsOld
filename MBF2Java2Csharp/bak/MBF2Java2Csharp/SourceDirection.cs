using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBF2Java2Csharp
{
    public static class SourceDirection
    {
        /// <summary> Source was up and left from current cell. </summary>
        public const sbyte Diagonal = 0;

        /// <summary> Source was up from current cell. </summary>
        public const sbyte Up = 1;

        /// <summary> Source was left of current cell. </summary>
        public const sbyte Left = 2;

        /// <summary> During traceback, stop at this cell (used by SmithWaterman). </summary>
        public const sbyte Stop = -1;

        /// <summary> Error code, if cell has code Invalid error has occurred. </summary>
        public const sbyte Invalid = -2;
    }
}
