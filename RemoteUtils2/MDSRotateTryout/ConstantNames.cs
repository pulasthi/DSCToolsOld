using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDSTryout
{
    class ConstantNames
    {
        public static readonly string ManxcatAppDirGroupBxText = @"Manxcat App Directory";
        public static readonly string PairwiseAppDirGroupBxText = @"Pairwise Clustering App Directory";
        public static string SpongeAppDirGroupBxText = @"Sequential Sponge App Directory";

        public static readonly string ManxcatRunInfoGroupBxText = @"Manxcat Run Info";
        public static readonly string PairwiseRunInfoGroupBxText = @"Pairwise Clustering Run Info";
        public static string SpongeRunInfoGroupBxText = @"Sequential Sponge Run Info";

        public static readonly string ManxcatFileGroupBxText = "Point Clusters (Optional)";
        public static readonly string PairwiseFileGroupBxText = "Point Coordinates (Optional)";

        public static readonly string ManxcatConfigurationGroupBxText = @"Manxcat Configuration";
        public static readonly string PairwiseConfigurationGroupBxText = @"Pairwise Clustering Configuration";
        public static string SpongConfigurationGroupBxText = @"Sequential Sponge Configuration";
        
        public static readonly string ManxcatFileLabelText = @"Cluster File:";
        public static readonly string PairwiseFileLabelText = @"Coordinates File:";

        public static readonly string DefaultManxcatTaskName = "Manxcat";
        public static readonly string DefaultPairwiseTaskName = "PairwiseClustering";
        public static string DefaultSpongeTaskName = @"SequentialSponge";

        public static readonly string PairwiseClusterFileNameFormat = "{0}-M{1}-C{2}.txt";
        public static readonly string PairwiseCenterFileNameFormat = "CenterFile-M{0}-C{1}.txt";

        public static readonly string DefaultManxcatRunNamePrefix = "Manxcat_Run_";
        public static readonly string DefaultPairwiseRunNamePrefix = "Pairwise_Run_";
        public static string DefaultSpongeRunNamePrefix = "Sponge_Run_";
        
    }
}
