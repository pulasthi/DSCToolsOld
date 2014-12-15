using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetqLight
{
    public class Constants
    {
        #region ApplicationNames

        public static string SmithWatermanName { get { return "Smith-Waterman"; } }

        public static string SmithWatermanMSName { get { return "Smith-Waterman (Microsoft Implementation)"; } }

        public static string NeedlemanWunschName { get { return "Needleman-Wunsch"; } }

        public static string PairwiseClusteringName { get { return "Pairwise Clustering"; } }

        public static string MultiDimensionalScalingName { get { return "Multi-Dimensional Scaling"; } }

        #endregion

        #region ApplicationPrefixes

        internal static string SmithWatermanPrefix
        {
            get
            {
                return "swg-";
            }
        }

        internal static string SmithWatermanMSPrefix
        {
            get
            {
                return "swms-";
            }
        }

        internal static string NeedlemanWunschPrefix
        {
            get
            {
                return "nw-";
            }
        }

        internal static string PairwiseClusteringPrefix
        {
            get
            {
                return "pwc-";
            }
        }

        internal static string MultiDimensionalScalingPrefix
        {
            get
            {
                return "mds-";
            }
        }

        #endregion

        #region InputOutputFileNames

        internal static string ConfigFileName { get { return "config.xml"; } }
        internal static string IndexFileName { get { return "index.txt"; } }
        internal static string DistanceFileName { get { return "distance.bin"; } }
        internal static string TimingFileNameSuffix { get { return "timing.txt"; } }
        internal static string SummaryFileNameSuffix { get { return "summary.txt"; } }
        internal static string ClusterFileNameSuffix { get { return "cluster.txt"; } }
        internal static string PointsFileNameSuffix { get { return "points.txt"; } }

        #endregion

        #region BatFileNames

        internal static string SetupBatFileName { get { return "setup.bat"; } }
        internal static string GoBatFileName { get { return "go.bat"; } }
        internal static string GoLocalBatFileName { get { return "golocal.bat"; } }
        internal static string CleanupBatFileName { get { return "cleanup.bat"; } }

        #endregion

        #region DefaultDirectories

        internal static string AppsDirName { get { return "Apps"; } }
        internal static string ConfigDirName { get { return "Config"; } }
        internal static string InputDirName { get { return "Input"; } }
        internal static string OutputDirName { get { return "Output"; } }
        internal static string BinariesDirName { get { return "binaries"; } }

        #endregion

        #region ExecutableNames

        internal static string SmithWatermanMSExe { get { return "Salsa.SmithWatermanMS.exe"; } }
        internal static string SmithWatermanExe { get { return "Salsa.SmithWatermanTPL.exe"; } }
        internal static string NeedlemanWunschExe { get { return "Salsa.NeedlemanWunschTPL.exe"; } }
        internal static string PairwiseClusteringExe { get { return "Salsa.PairwiseClusteringTPL.exe"; } }
        internal static string MultiDimensionalScalingExe { get { return "ManxcatMDS.exe"; } }

        #endregion

        #region LocalMPIExecCommands

        private static string _commonLocalMpiexec = "mpiexec -np {0} {1} /configFile={2} /nodeCount=1";
        private static string _commonLocalMpiexecWithThreads = _commonLocalMpiexec + " /threadCount=1";            
        internal static string LocalSequenceAlignMpiexec
        {
            get
            {
                return _commonLocalMpiexec;
            }
        }

        internal static string LocalPairwiseClusteringMpiexec
        {
            get
            {
                return _commonLocalMpiexecWithThreads;
            }
        }

        internal static string LocalMultiDimensionalScalingMpiexec
        {
            get
            {
                return _commonLocalMpiexecWithThreads;
            }
        }

        #endregion

        #region GetTitleForApplication

        internal static string GetTitleForApplication(ApplicationType at)
        {
            switch(at)
            {
                case ApplicationType.SWG:
                    return SmithWatermanName;
                case ApplicationType.SWMS:
                    return SmithWatermanMSName;
                case ApplicationType.NW:
                    return NeedlemanWunschName;
                case ApplicationType.MDS:
                    return MultiDimensionalScalingName;
                case ApplicationType.PWC:
                    return PairwiseClusteringName;
                default:
                    return ApplicationType.UNDEFINED.ToString();
            }

        }

        #endregion

    }
}
