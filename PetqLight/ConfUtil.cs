using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Salsa.Core.Configuration;
using Salsa.Core.Configuration.Sections;
using Salsa.Core.Bio.Algorithms;
using System.IO;


namespace PetqLight
{
    class ConfUtil
    {
        private static ConfigurationMgr _mgr;
        
        #region StaticConstructorWithDefaults

        static ConfUtil()
        {
            _mgr = new ConfigurationMgr();
            NeedlemanWunschSection nw = _mgr.NeedlemanWunschSection;
            SmithWatermanSection swg = _mgr.SmithWatermanSection;
            SmithWatermanMS swms = _mgr.SmithWatermanMS;
            ManxcatSection mds = _mgr.ManxcatSection;
            PairwiseSection pwc = _mgr.PairwiseSection;

            // NW Defaults
            nw.DistanceFunctionType = DistanceFunctionType.Kimura2;
            nw.GapExtensionPenalty = -8;
            nw.GapOpenPenalty = -20;
            nw.MoleculeType = MBF.MoleculeType.DNA;
            nw.ScoringMatrixName = "EDNAFULL";
            nw.WriteFullMatrix = true;
            nw.WriteAlignments = false;
            
            // SWMS Defaults
            swms.DistanceFunctionType = DistanceFunctionType.Kimura2;
            swms.GapExtensionPenalty = -20;
            swms.GapOpenPenalty = -8;
            swms.MoleculeType = MBF.MoleculeType.DNA;
            swms.ScoringMatrixName = "EDNAFULL";
            swms.WriteFullMatrix = true;
            swms.WriteAlignments = false;

            // SWG Defaults
            swg.DistanceFunctionType = DistanceFunctionType.Kimura2;
            swg.GapExtensionPenalty = 4;            
            swg.GapOpenPenalty = 14;
            swg.AlignmentType = AlignmentType.Nucleic;
            swg.ScoringMatrixName = "EDNAFULL";
            swg.WriteFullMatrix = true;
            swg.WriteAlignments = false;

            // MDS Defaults
            mds = _mgr.ManxcatSection;
            mds.ProcessingOption = 0;
            mds.DistanceProcessingOption = 1;
            mds.InitializationOption = 0;
            mds.WeightingOption = 0;
            mds.Write2Das3D = true;
            mds.LocalVectorDimension = 3;
            mds.VariedPointCriterion = "all";
            mds.FixedPointCriterion = "none";
            mds.RotationOption = 0;
            mds.InitializationLoops = 4;
            mds.Chisqnorm = 0;
            mds.DistanceFormula = 2;
            mds.FullSecondDerivativeOption = 0;
            mds.MinimumDistance = -0.001;
            mds.FunctionErrorCalcMultiplier = 10;
            mds.ChisqPrintConstant = 1;
            mds.Maxit = 80;
            mds.Nbadgo = 6;
            mds.ChisqChangePerPoint = 0.001;
            mds.FletcherRho = 0.25;
            mds.FletcherSigma = 0.75;
            mds.Omega = 1.25;
            mds.OmegaOption = 0;
            mds.QHighInitialFactor = 0.01;
            mds.QgoodReductionFactor = 0.5;
            mds.QLimitscalculationInterval = 1;
            mds.Extraprecision = 0.05;
            mds.AddonforQcomputation = 2;
            mds.InitialSteepestDescents = 0;
            mds.TimeCutmillisec = -1;
            mds.CGResidualLimit = 0.00001;
            mds.PowerIterationLimit = 200;
            mds.Eigenvaluechange = 0.001;
            mds.Eigenvectorchange = 0.001;
            mds.Derivtest = false;
            mds.MPIIOStrategy = 0;
            mds.HistogramBinCount = 100;
            mds.ExtraOption1 = 0;
            mds.DebugPrintOption = 2;
            mds.ConsoleDebugOutput = true;

            // PWC Defaults
            pwc.ProcessingOption = 0;
            pwc.TransformDimension = 4;
            pwc.MaxNcent = 4;
            pwc.Splitorexpandit = 1;
            pwc.MPIIOStrategy = 0;
            pwc.ToosmalltoSplit = 5;
            pwc.MinEigtest = -0.01;
            pwc.ConvergeIntermediateClusters = false;
            pwc.Waititerations = 10;
            pwc.Epsi_max_change = 0.001;
            pwc.InitialCoolingFactor = 0.9;
            pwc.FineCoolingFactor = 0.99;
            pwc.Eigenvaluechange = 0.001;
            pwc.Eigenvectorchange = 0.001;
            pwc.Iterationatend = 2000;
            pwc.ConvergenceLoopLimit = 2000;
            pwc.FreezingLimit = 0.002;
            pwc.PowerIterationLimit = 200;
        }

        #endregion

        public static ConfigurationMgr DefaultConfiguration { get { return _mgr; } }

        #region ConfigureLocalExecution

        public static ConfigurationMgr ConfigureLocalExecution(ConfigurationMgr mgr, string projectDir, string projectName, string inputFileName, int sequenceCount)
        {
            return ConfigureGenericExecution(mgr, projectDir, projectDir, projectName, inputFileName, sequenceCount);
        }

        #endregion

        #region ConfigureRemoteExecution

        public static ConfigurationMgr ConfigureRemoteExecution(ConfigurationMgr mgr, string headNode, string targetDir, string projectName, string inputFileName, int sequenceCount)
        {
            string headNodeDir = @"\\" + headNode + Path.DirectorySeparatorChar + targetDir.ElementAt(0) + "$" + targetDir.Substring(2);
            return ConfigureGenericExecution(mgr, targetDir, headNodeDir, projectName, inputFileName, sequenceCount);
        }

        #endregion

        #region ConfigureGenericExecution

        private static ConfigurationMgr ConfigureGenericExecution(ConfigurationMgr mgr, string localDir, string headNodeDir, string projectName, string inputFileName, int sequenceCount)
        {
            SmithWatermanSection swg = mgr.SmithWatermanSection;
            SmithWatermanMS swms = mgr.SmithWatermanMS;
            NeedlemanWunschSection nw = mgr.NeedlemanWunschSection;
            ManxcatSection mds = mgr.ManxcatSection;
            PairwiseSection pwc = mgr.PairwiseSection;
            
            char ds = Path.DirectorySeparatorChar;

            string localPath = localDir + ds + projectName;            
            string fastaFile = localPath + ds + Constants.InputDirName + ds + inputFileName;
            string distanceMatrixFile = localPath + ds + Constants.OutputDirName + ds + Constants.DistanceFileName;

            string headNodeOutPath = headNodeDir + ds + projectName + ds + Constants.OutputDirName;
            string indexFile = headNodeOutPath + ds + Constants.IndexFileName;


            // SWMS Addons
            swms.FastaFile = fastaFile;
            swms.DistanceMatrixFile = distanceMatrixFile;
            swms.IndexFile = indexFile;
            swms.TimingFile = headNodeOutPath + ds + Constants.SmithWatermanMSPrefix + Constants.TimingFileNameSuffix;
            swms.SummaryFile = headNodeOutPath + ds + Constants.SmithWatermanMSPrefix + Constants.SummaryFileNameSuffix;
            swms.SequenceCount = sequenceCount;

            // NW Addons
            nw.FastaFile = fastaFile;
            nw.DistanceMatrixFile = distanceMatrixFile;
            nw.IndexFile = indexFile;
            nw.TimingFile = headNodeOutPath + ds + Constants.NeedlemanWunschPrefix + Constants.TimingFileNameSuffix;
            nw.SummaryFile = headNodeOutPath + ds + Constants.NeedlemanWunschPrefix + Constants.SummaryFileNameSuffix;
            nw.SequenceCount = sequenceCount;

            // SWG Addons
            swg.FastaFile = fastaFile;
            swg.DistanceMatrixFile = distanceMatrixFile;
            swg.IndexFile = indexFile;
            swg.TimingFile = headNodeOutPath + ds + Constants.SmithWatermanPrefix + Constants.TimingFileNameSuffix;
            swg.SummaryFile = headNodeOutPath + ds + Constants.SmithWatermanPrefix + Constants.SummaryFileNameSuffix;
            swg.SequenceCount = sequenceCount;
            
            // MDS Addons
            mds.BaseResultDirectoryName = headNodeOutPath;
            mds.ControlDirectoryName = headNodeOutPath;
            mds.IndexFile = indexFile;
            mds.DistanceMatrixFile = distanceMatrixFile;
            mds.ReducedVectorOutputFileName = headNodeOutPath + ds + Constants.MultiDimensionalScalingPrefix + Constants.PointsFileNameSuffix;
            mds.TimingOutputFileName = headNodeOutPath + ds + Constants.MultiDimensionalScalingPrefix + Constants.TimingFileNameSuffix;
            mds.SummaryOutputFileName = headNodeOutPath + ds + Constants.MultiDimensionalScalingPrefix + Constants.SummaryFileNameSuffix;
            mds.DataPoints = sequenceCount;

            // PWC Addons
            pwc.DistanceMatrixFile = distanceMatrixFile;
            pwc.IndexFile = indexFile;
            pwc.TimingFile = headNodeOutPath + ds + Constants.PairwiseClusteringPrefix + Constants.TimingFileNameSuffix;
            pwc.SummaryFile = headNodeOutPath + ds + Constants.PairwiseClusteringPrefix + Constants.SummaryFileNameSuffix;
            pwc.ClusterFile = headNodeOutPath + ds + Constants.PairwiseClusteringPrefix + Constants.ClusterFileNameSuffix;
            pwc.DataPoints = sequenceCount;

            return _mgr;
        }

        #endregion

        #region CloneSection

        public static Section CloneSection(Section original, ApplicationType appType)
        {
            // Creates a new instance of the original type
            Section clone = (Section)original.GetType().GetConstructor(Type.EmptyTypes).Invoke(null);
            CopySection(original, clone, appType, true);
            return clone;
        }

        #endregion

        #region CopySection

        public static void CopySection(Section from, Section to, ApplicationType appType, bool includeIO)
        {
            if (appType == ApplicationType.NW)
            {
                CopyNeedlemanWunsch(from as NeedlemanWunschSection, to as NeedlemanWunschSection, includeIO);
            }
            else if (appType == ApplicationType.SWMS)
            {
                CopySmithWatermanMS(from as SmithWatermanMS, to as SmithWatermanMS, includeIO);
            }
            else if (appType == ApplicationType.SWG)
            {
                CopySmithWaterman(from as SmithWatermanSection, to as SmithWatermanSection, includeIO);
            }
            else if (appType == ApplicationType.PWC)
            {
                CopyPairwiseClustering(from as PairwiseSection, to as PairwiseSection, includeIO);
            }
            else if (appType == ApplicationType.MDS)
            {
                CopyMultiDimensionalScaling(from as ManxcatSection, to as ManxcatSection, includeIO);
            }
        }

        private static void CopyNeedlemanWunsch(NeedlemanWunschSection from, NeedlemanWunschSection to, bool includeIO) 
        {
            if (includeIO)
            {
                to.DistanceMatrixFile = from.DistanceMatrixFile;
                to.FastaFile = from.FastaFile;
                to.IndexFile = from.IndexFile;
                to.SummaryFile = from.SummaryFile;
                to.TimingFile = from.TimingFile;
                to.WriteAlignmentsFile = from.WriteAlignmentsFile;
            }
            to.DistanceFunctionType = from.DistanceFunctionType;
            to.GapExtensionPenalty = from.GapExtensionPenalty;
            to.GapOpenPenalty = from.GapOpenPenalty;
            to.MoleculeType = from.MoleculeType;
            to.NodeCount = from.NodeCount;
            to.ProcessPerNodeCount = from.ProcessPerNodeCount;
            to.ScoringMatrixName = from.ScoringMatrixName;
            to.SequenceCount = from.SequenceCount;
            to.WriteAlignments = from.WriteAlignments;
            to.WriteFullMatrix = from.WriteFullMatrix;
        }

        private static void CopySmithWatermanMS(SmithWatermanMS from, SmithWatermanMS to, bool includeIO)
        {
            if (includeIO)
            {
                to.DistanceMatrixFile = from.DistanceMatrixFile;
                to.FastaFile = from.FastaFile;
                to.IndexFile = from.IndexFile;
                to.SummaryFile = from.SummaryFile;
                to.TimingFile = from.TimingFile;
                to.WriteAlignmentsFile = from.WriteAlignmentsFile;
            }

            to.DistanceFunctionType = from.DistanceFunctionType;
            to.GapExtensionPenalty = from.GapExtensionPenalty;
            to.GapOpenPenalty = from.GapOpenPenalty;
            to.MoleculeType = from.MoleculeType;
            to.NodeCount = from.NodeCount;
            to.ProcessPerNodeCount = from.ProcessPerNodeCount;
            to.ScoringMatrixName = from.ScoringMatrixName;
            to.SequenceCount = from.SequenceCount;
            to.WriteAlignments = from.WriteAlignments;
            to.WriteFullMatrix = from.WriteFullMatrix;
        }

        private static void CopySmithWaterman(SmithWatermanSection from, SmithWatermanSection to, bool includeIO)
        {
            if (includeIO)
            {
                to.DistanceMatrixFile = from.DistanceMatrixFile;
                to.FastaFile = from.FastaFile;
                to.IndexFile = from.IndexFile;
                to.SummaryFile = from.SummaryFile;
                to.TimingFile = from.TimingFile;
                to.WriteAlignmentsFile = from.WriteAlignmentsFile;
            }

            to.AlignmentType = from.AlignmentType;
            to.DistanceFunctionType = from.DistanceFunctionType;
            to.GapExtensionPenalty = from.GapExtensionPenalty;
            to.GapOpenPenalty = from.GapOpenPenalty;
            to.NodeCount = from.NodeCount;
            to.ProcessPerNodeCount = from.ProcessPerNodeCount;
            to.ScoringMatrixName = from.ScoringMatrixName;
            to.SequenceCount = from.SequenceCount;
            to.WriteAlignments = from.WriteAlignments;
            to.WriteFullMatrix = from.WriteFullMatrix;
            to.WritePartialMatrix = from.WritePartialMatrix;
        }

        private static void CopyPairwiseClustering(PairwiseSection from, PairwiseSection to, bool includeIO)
        {
            if (includeIO)
            {
                to.ClusterFile = from.ClusterFile;
                to.IndexFile = from.IndexFile;
                to.DistanceMatrixFile = from.DistanceMatrixFile;
                to.SummaryFile = from.SummaryFile;
                to.TimingFile = from.TimingFile;

            }

            to.Comments = from.Comments;
            to.ConsoleDebugOutput = from.ConsoleDebugOutput;
            to.ConvergeIntermediateClusters = from.ConvergeIntermediateClusters;
            to.ConvergenceLoopLimit = from.ConvergenceLoopLimit;
            to.DataPoints = from.DataPoints;
            to.DebugPrintOption = from.DebugPrintOption;
            to.Eigenvaluechange = from.Eigenvaluechange;
            to.Eigenvectorchange = from.Eigenvectorchange;
            to.Epsi_max_change = from.Epsi_max_change;
            to.FineCoolingFactor = from.FineCoolingFactor;
            to.FreezingLimit = from.FreezingLimit;
            to.InitialCoolingFactor = from.InitialCoolingFactor;
            to.Iterationatend = from.Iterationatend;
            to.MaxNcent = from.MaxNcent;
            to.MinEigtest = from.MinEigtest;
            to.MPIIOStrategy = from.MPIIOStrategy;
            to.MPIperNodeCount = from.MPIperNodeCount;
            to.NodeCount = from.NodeCount;
            to.Pattern = from.Pattern;
            to.PowerIterationLimit = from.PowerIterationLimit;
            to.ProcessingOption = from.ProcessingOption;
            to.Splitorexpandit = from.Splitorexpandit;
            to.ThreadCount = from.ThreadCount;
            to.ToosmalltoSplit = from.ToosmalltoSplit;
            to.TransformDimension = from.TransformDimension;
            to.Waititerations = from.Waititerations;
        }

        private static void CopyMultiDimensionalScaling(ManxcatSection from, ManxcatSection to, bool includeIO)
        {
            if (includeIO)
            {
                to.BaseResultDirectoryName = from.BaseResultDirectoryName;
                to.ControlDirectoryName = from.ControlDirectoryName;
                to.ClusterDirectory = from.ClusterDirectory;
                to.DataLabelsFileName = from.DataLabelsFileName;
                to.DistanceMatrixFile = from.DistanceMatrixFile;
                to.IndexFile = from.IndexFile;
                to.InitializationFileName = from.InitializationFileName;
                to.ReducedVectorOutputFileName = from.ReducedVectorOutputFileName;
                to.ResultDirectoryExtension = from.ResultDirectoryExtension;
                to.RotationLabelsFileName = from.RotationLabelsFileName;
                to.Selectedfixedpointfile = from.Selectedfixedpointfile;
                to.Selectedvariedpointfile = from.Selectedvariedpointfile;
                to.SummaryOutputFileName = from.SummaryOutputFileName;
                to.TimingOutputFileName = from.TimingOutputFileName;
                to.WeightingFileName = from.WeightingFileName;
            }

            to.AddonforQcomputation = from.AddonforQcomputation;
            to.CGResidualLimit = from.CGResidualLimit;
            to.ChisqChangePerPoint = from.ChisqChangePerPoint;
            to.Chisqnorm = from.Chisqnorm;
            to.ChisqPrintConstant = from.ChisqPrintConstant;
            to.Comment = from.Comment;
            to.ConsoleDebugOutput = from.ConsoleDebugOutput;
            to.ConversionInformation = from.ConversionInformation;
            to.ConversionOption = from.ConversionOption;
            to.DataPoints = from.DataPoints;
            to.DebugPrintOption = from.DebugPrintOption;
            to.Derivtest = from.Derivtest;
            to.DistanceFormula = from.DistanceFormula;
            to.DistanceProcessingOption = from.DistanceProcessingOption;
            to.Eigenvaluechange = from.Eigenvaluechange;
            to.Eigenvectorchange = from.Eigenvectorchange;
            to.Extradata1 = from.Extradata1;
            to.Extradata2 = from.Extradata2;
            to.Extradata3 = from.Extradata3;
            to.Extradata4 = from.Extradata4;
            to.ExtraOption1 = from.ExtraOption1;
            to.Extraprecision = from.Extraprecision;
            to.FixedPointCriterion = from.FixedPointCriterion;
            to.FletcherRho = from.FletcherRho;
            to.FletcherSigma = from.FletcherSigma;
            to.FullSecondDerivativeOption = from.FullSecondDerivativeOption;
            to.FunctionErrorCalcMultiplier = from.FunctionErrorCalcMultiplier;
            to.HistogramBinCount = from.HistogramBinCount;
            to.InitializationLoops = from.InitializationLoops;
            to.InitializationOption = from.InitializationOption;
            to.InitialSteepestDescents = from.InitialSteepestDescents;
            to.LocalVectorDimension = from.LocalVectorDimension;
            to.Maxit = from.Maxit;
            to.MinimumDistance = from.MinimumDistance;
            to.MPIIOStrategy = from.MPIIOStrategy;
            to.MPIperNodeCount = from.MPIperNodeCount;
            to.Nbadgo = from.Nbadgo;
            to.NodeCount = from.NodeCount;
            to.Omega = from.Omega;
            to.OmegaOption = from.OmegaOption;
            to.Pattern = from.Pattern;
            to.PowerIterationLimit = from.PowerIterationLimit;
            to.ProcessingOption = from.ProcessingOption;
            to.QgoodReductionFactor = from.QgoodReductionFactor;
            to.QHighInitialFactor = from.QHighInitialFactor;
            to.QLimitscalculationInterval = from.QLimitscalculationInterval;
            to.RotationOption = from.RotationOption;
            to.RunNumber = from.RunNumber;
            to.RunSetLabel = from.RunSetLabel;
            to.Selectedfixedpoints = from.Selectedfixedpoints;
            to.Selectedvariedpoints = from.Selectedvariedpoints;
            to.ThreadCount = from.ThreadCount;
            to.TimeCutmillisec = from.TimeCutmillisec;
            to.VariedPointCriterion = from.VariedPointCriterion;
            to.WeightingOption = from.WeightingOption;
            to.Write2Das3D = from.Write2Das3D;
        }

        #endregion
    }
}
