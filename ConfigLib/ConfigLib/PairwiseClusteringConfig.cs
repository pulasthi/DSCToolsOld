using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigLib
{
    public class PairwiseClusteringConfig
    {
        public string OutputDataFileName { get; set; }
        public string InputDataFileName { get; set; }
        public string InputFormatStyle { get; set; }
        public string FamilySpecification { get; set; }
        public float RandomInputCut { get; set; }
        public string InputClusteredFileName { get; set; }
        public string SelectedClusters { get; set; }
        public int RunNumber { get; set; }
        public int TransformDimension { get; set; }
        public string ResultDirectoryExtension { get; set; }
        public string TimingOutputFileName { get; set; }
        public string SMACOFInputFileName { get; set; }
        public string DataLabelsFileName { get; set; }
        public string PointPropertiesFileName { get; set; }
        public string FamilySMACOFFileName { get; set; }
        public string ClusterSMACOFFileName { get; set; }
        public int GroupCenterSize { get; set; }
        public int maxNcent { get; set; }
        public int splitorexpandit { get; set; }
        public string pattern { get; set; }
        public int ThreadCount { get; set; }
        public int NodeCount { get; set; }
        public int MPIperNodeCount { get; set; }
        public int ToosmalltoSplit { get; set; }
        public float MinEigtest { get; set; }
        public bool ConvergeIntermediateClusters { get; set; }
        public int Waititerations { get; set; }
        public float Epsi_max_change { get; set; }
        public float InitialCoolingFactor { get; set; }
        public float FineCoolingFactor { get; set; }
        public float eigenvaluechange { get; set; }
        public float eigenvectorchange { get; set; }
        public int Iterationatend { get; set; }
        public int ConvergenceLoopLimit { get; set; }
        public float FreezingLimit { get; set; }
        public int PowerIterationLimit { get; set; }
        public int HistogramBinCount { get; set; }
        public string Extradata1 { get; set; }
        public string Extradata2 { get; set; }
        public string Extradata3 { get; set; }
        public string Extradata4 { get; set; }
        public int ExtraOption1 { get; set; }
        public bool ConsoleDebugOutput { get; set; }
        public int MinimumFeatureOverlap { get; set; }
        public int DistanceUndefinedOption { get; set; }
        public int DistanceInfiniteOption { get; set; }
        public int DistanceChoice { get; set; }
    }
}
