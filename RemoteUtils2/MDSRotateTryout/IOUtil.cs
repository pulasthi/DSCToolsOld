using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MDSTryout
{
    public class IOUtil
    {
        public static readonly string KnownMatricesFileName = "known-matrices.txt";

        public static readonly string RemoteInputDirName = "input";
        public static readonly string RemoteAppsDirName = "apps";
        public static readonly string RemoteConfigFileName = "remote-config.xml";
        public static readonly string LocalConfigName = "local-config.xml";
        public static readonly string LocalOutStatusFileName = "local-out-status.txt";
        public static readonly string LocalErrStatusFileName = "local-err-status.txt";

        // todo: beta2 - remove these and use localstatus files above
        public static readonly string LocalOutFileName = "local-out.txt";
        public static readonly string LocalErrFileName = "local-err.txt";

        public static readonly string LocalPlotTxtFileName = "local-plot";

        public static readonly string RunInfoExt = ".infx";
        public static readonly string TextExt = ".txt";

        public static readonly string ManxcatWholeDensityFile = "whole-density.txt";
        public static readonly string ManxcatWholeXHistFile = "whole-xHist.txt";
        public static readonly string ManxcatWholeYHistFile = "whole-yHist.txt";
        public static readonly string ManxcatWholePlotFile = "whole-plot.txt";

        public static readonly string ManxcatSelectedDensityFile = "selected-density.txt";
        public static readonly string ManxcatSelectedInterDensityFile = "selected-inter-density.txt";
        public static readonly string ManxcatSelectedXHistFile = "selected-xHist.txt";
        public static readonly string ManxcatSelectedInterXHistFile = "selected-inter-xHist.txt";
        public static readonly string ManxcatSelectedYHistFile = "selected-yHist.txt";
        public static readonly string ManxcatSelectedInterYHistFile = "selected-inter-yHist.txt";
        public static readonly string ManxcatSelectedPlotFile = "selected-plot.txt";
        public static string ManxcatSelectedInterPlotFile = "selected-inter-plot.txt";

        public static readonly string ManxcatHtmlFile = "index.html";
        public static readonly string ManxcatCSSFile = "style.css";

        public static readonly string ManxcatSimplePointsFileNamePrefix = "SIMPLE";
        public static readonly string ManxcatColonPointsFileNameSuffix = "Colon";
        public static readonly string ManxcatGroupPointsFileNameSuffix = "Group";

        public static readonly string ManxcatOutFileName = "manxcat-out.txt";
        public static readonly string ManxcatErrFileName = "manxcat-err.txt";
        public static readonly string ManxcatExeName = "ManxcatMDS.exe";

        public static readonly string ManxcatDataLabelsFilePrefix = @"dlbl_";
        public static readonly string ManxcatDistanceFilePrefix = @"dist_";
        public static readonly string ManxcatClusterFilePrefix = @"clust_";
        public static readonly string ManxcatIndexFilePrefix = @"indx_";
        public static readonly string ManxcatInitializationFilePrefix = @"init_";
        public static readonly string ManxcatRotationLabelsFilePrefix = @"rlbl_";
        public static readonly string ManxcatSelectedFixedPointsFilePrefix = @"sfix_";
        public static readonly string ManxcatSelectedVariedPointsFilePrefix = @"svar_";
        public static readonly string ManxcatWeightingFilePrefix = @"wgtn_";
        public static readonly string ManxcatScalingFilePrefix = @"scal_";

        public static readonly string PairwiseOutFileName = "pairwise-out.txt";
        public static readonly string PairwiseErrFileName = "pairwise-err.txt";
        public static readonly string PairwiseExeName = "Salsa.PairwiseClusteringTPL.exe";

        public static readonly string PairwiseDistanceFilePrefix = @"dist_";
        public static readonly string PairwiseIndexFilePrefix = @"indx_";
        public static readonly string PairwiseLabelFilePrefix = @"lbl_";
        public static readonly string PairwiseMdsFilePrefix = @"mds_";
        public static readonly string PairwiseClusterNumberFilePrefix = @"cnum_";

        public static string SpongeOutFileName = @"sponge-out.txt";
        public static string SpongeErrFileName = @"sponge-err.txt";
        public static string SpongeExeName = @"Salsa.SequentialSponge.exe";

        public static readonly string SpongeDistanceFilePrefix = @"dist_";
        public static readonly string SpongeLabelsFilePrefix = @"lbl_";

        public static readonly string PairwiseDefaultClustFileName = "cluster.txt";
        public static readonly string PairwiseDefaultTimingFileName = "pwc-timing.txt";
        public static readonly string PairwiseDefaultSummaryFilename = "pwc-summary.txt";
        public static string PairwiseDefaultCenterPlotFileName = "pwc-plot.pviz";

        public static readonly string SpongeDefaultClustFileName = "cluster.txt";
        public static readonly string SpongeDefaultTimingFileName = "sponge-timing.txt";
        public static readonly string SpongeDefaultSummaryFilename = "sponge-summary.txt";
        


        public static void CopyFiles(string fromDir, string toDir)
        {
            string[] files = Directory.GetFiles(fromDir);
            foreach (string t in files)
            {
                if (!t.EndsWith(".tmp"))
                {
                    File.Copy(t, Path.Combine(toDir, Path.GetFileName(t)), true);
                }
            }
        }
    }
}
