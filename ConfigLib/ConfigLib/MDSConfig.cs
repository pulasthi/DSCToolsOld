using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigLib
{
    public class MDSConfig
    {
        public string DataLabelsFileName { get; set; }
        public string ReducedVectorOutputFileName { get; set; }
        public string ClusterDirectory { get; set; }
        public int DistanceProcessingOption { get; set; }
        public int InitializationOption { get; set; }
        public string InitializationFileName { get; set; }
        public int WeightingOption { get; set; }
        public string WeightingFileName { get; set; }
        public bool write2Das3D { get; set; }
        public int LocalVectorDimension { get; set; }
        public int selectedvariedpoints { get; set; }
        public string VariedPointCriterion { get; set; }
        public string selectedvariedpointfile { get; set; }
        public int selectedfixedpoints { get; set; }
        public string FixedPointCriterion { get; set; }
        public string selectedfixedpointfile { get; set; }
        public int ConversionOption { get; set; }
        public string ConversionInformation { get; set; }
        public int RotationOption { get; set; }
        public string RotationLabelsFileName { get; set; }
        public int InitializationLoops { get; set; }
        public int Chisqnorm { get; set; }
        public int DistanceFormula { get; set; }
        public int FullSecondDerivativeOption { get; set; }
        public float MinimumDistance { get; set; }
        public int FunctionErrorCalcMultiplier { get; set; }
        public int ChisqPrintConstant { get; set; }
        public int maxit { get; set; }
        public int nbadgo { get; set; }
        public float ChisqChangePerPoint { get; set; }
        public float FletcherRho { get; set; }
        public float FletcherSigma { get; set; }
        public float Omega { get; set; }
        public int OmegaOption { get; set; }
        public float QHighInitialFactor { get; set; }
        public float QgoodReductionFactor { get; set; }
        public int QLimitscalculationInterval { get; set; }
        public float extraprecision { get; set; }
        public int addonforQcomputation { get; set; }
        public int InitialSteepestDescents { get; set; }
        public int TimeCutmillisec { get; set; }
        public int CGResidualLimit { get; set; }
        public int PowerIterationLimit { get; set; }
        public float eigenvaluechange { get; set; }
        public float eigenvectorchange { get; set; }
        public bool derivtest { get; set; }
        public int RunNumber { get; set; }
        public string ResultDirectoryExtension { get; set; }
        public string TimingOutputFileName { get; set; }
        public string pattern { get; set; }
        public int ThreadCount { get; set; }
        public int NodeCount { get; set; }
        public int MPIperNodeCount { get; set; }
        public int HistogramBinCount { get; set; }
        public string Extradata1 { get; set; }
        public string Extradata2 { get; set; }
        public string Extradata3 { get; set; }
        public string Extradata4 { get; set; }
        public int ExtraOption1 { get; set; }
        public bool ConsoleDebugOutput { get; set; }
        public string Comment { get; set; }
    }
}
