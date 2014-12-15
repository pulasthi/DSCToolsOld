using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Salsa.Core.Configuration.Sections
{
    public class PairwiseSection : Section 
    {
        #region Constructors
        public PairwiseSection()
        {
        }
        #endregion

        #region Properties

        public override string Name
        {
            get
            {
                return "Pairwise";
            }
        }
        public string DataFileName
        {
            get
            {
                return _dataFileName;
            }
            set
            {
                _dataFileName = value;
            }
        }
        public string OutputDataFileName
        {
            get
            {
                return _outputDataFileName;
            }
            set
            {
                _outputDataFileName = value;
            }
        }
        public string InputDataFileName
        {
            get
            {
                return _inputDataFileName;
            }
            set
            {
                _inputDataFileName = value;
            }
        }
        public string InputFormatStyle
        {
            get
            {
                return _inputFormatStyle;
            }
            set
            {
                _inputFormatStyle = value;
            }
        }
        public string FamilySpecification
        {
            get
            {
                return _familySpecification;
            }
            set
            {
                _familySpecification = value;
            }
        }
        public double RandomInputCut
        {
            get
            {
                return _randomInputCut;
            }
            set
            {
                _randomInputCut = value;
            }
        }
        public string InputClusteredFileName
        {
            get
            {
                return _inputClusteredFileName;
            }
            set
            {
                _inputClusteredFileName = value;
            }
        }
        public string SelectedClusters
        {
            get
            {
                return _selectedClusters;
            }
            set
            {
                _selectedClusters = value;
            }
        }
        public int RunNumber
        {
            get
            {
                return _runNumber;
            }
            set
            {
                _runNumber = value;
            }
        }
        public int DataPoints
        {
            get
            {
                return _dataPoints;
            }
            set
            {
                _dataPoints = value;
            }
        }
        public int ProcessingOption
        {
            get
            {
                return _processingOption;
            }
            set
            {
                _processingOption = value;
            }
        }
        public int TransformDimension
        {
            get
            {
                return _transformDimension;
            }
            set
            {
                _transformDimension = value;
            }
        }
        public string BaseResultDirectoryName
        {
            get
            {
                return _baseResultDirectoryName;
            }
            set
            {
                _baseResultDirectoryName = value;
            }
        }
        public string ResultDirectoryExtension
        {
            get
            {
                return _resultDirectoryExtension;
            }
            set
            {
                _resultDirectoryExtension = value;
            }
        }
        public string TimingOutputFileName
        {
            get
            {
                return _timingOutputFileName;
            }
            set
            {
                _timingOutputFileName = value;
            }
        }
        public string RunSetLabel
        {
            get
            {
                return _runSetLabel;
            }
            set
            {
                _runSetLabel = value;
            }
        }
        public string ControlDirectoryName
        {
            get
            {
                return _controlDirectoryName;
            }
            set
            {
                _controlDirectoryName = value;
            }
        }
        public string SMACOFInputFileName
        {
            get
            {
                return _sMACOFInputFileName;
            }
            set
            {
                _sMACOFInputFileName = value;
            }
        }
        public string DataLabelsFileName
        {
            get
            {
                return _dataLabelsFileName;
            }
            set
            {
                _dataLabelsFileName = value;
            }
        }
        public string PointPropertiesFileName
        {
            get
            {
                return _pointPropertiesFileName;
            }
            set
            {
                _pointPropertiesFileName = value;
            }
        }
        public string FamilySMACOFFileName
        {
            get
            {
                return _familySMACOFFileName;
            }
            set
            {
                _familySMACOFFileName = value;
            }
        }
        public string ClusterSMACOFFileName
        {
            get
            {
                return _clusterSMACOFFileName;
            }
            set
            {
                _clusterSMACOFFileName = value;
            }
        }
        public int GroupCenterSize
        {
            get
            {
                return _groupCenterSize;
            }
            set
            {
                _groupCenterSize = value;
            }
        }
        public int MaxNcent
        {
            get
            {
                return _maxNcent;
            }
            set
            {
                _maxNcent = value;
            }
        }
        public int Splitorexpandit
        {
            get
            {
                return _splitorexpandit;
            }
            set
            {
                _splitorexpandit = value;
            }
        }
        public string Pattern
        {
            get
            {
                return _pattern;
            }
            set
            {
                _pattern = value;
            }
        }
        public int ThreadCount
        {
            get
            {
                return _threadCount;
            }
            set
            {
                _threadCount = value;
            }
        }
        public int NodeCount
        {
            get
            {
                return _nodeCount;
            }
            set
            {
                _nodeCount = value;
            }
        }
        public int MPIperNodeCount
        {
            get
            {
                return _mPIperNodeCount;
            }
            set
            {
                _mPIperNodeCount = value;
            }
        }
        public int MPIIOStrategy
        {
            get
            {
                return _mPIIOStrategy;
            }
            set
            {
                _mPIIOStrategy = value;
            }
        }
        public int ToosmalltoSplit
        {
            get
            {
                return _toosmalltoSplit;
            }
            set
            {
                _toosmalltoSplit = value;
            }
        }
        public double MinEigtest
        {
            get
            {
                return _minEigtest;
            }
            set
            {
                _minEigtest = value;
            }
        }
        public bool ConvergeIntermediateClusters
        {
            get
            {
                return _convergeIntermediateClusters;
            }
            set
            {
                _convergeIntermediateClusters = value;
            }
        }
        public int Waititerations
        {
            get
            {
                return _waititerations;
            }
            set
            {
                _waititerations = value;
            }
        }
        public double Epsi_max_change
        {
            get
            {
                return _epsi_max_change;
            }
            set
            {
                _epsi_max_change = value;
            }
        }
        public double InitialCoolingFactor
        {
            get
            {
                return _initialCoolingFactor;
            }
            set
            {
                _initialCoolingFactor = value;
            }
        }
        public double FineCoolingFactor
        {
            get
            {
                return _fineCoolingFactor;
            }
            set
            {
                _fineCoolingFactor = value;
            }
        }
        public double Eigenvaluechange
        {
            get
            {
                return _eigenvaluechange;
            }
            set
            {
                _eigenvaluechange = value;
            }
        }
        public double Eigenvectorchange
        {
            get
            {
                return _eigenvectorchange;
            }
            set
            {
                _eigenvectorchange = value;
            }
        }
        public int Iterationatend
        {
            get
            {
                return _iterationatend;
            }
            set
            {
                _iterationatend = value;
            }
        }
        public int ConvergenceLoopLimit
        {
            get
            {
                return _convergenceLoopLimit;
            }
            set
            {
                _convergenceLoopLimit = value;
            }
        }
        public double FreezingLimit
        {
            get
            {
                return _freezingLimit;
            }
            set
            {
                _freezingLimit = value;
            }
        }
        public int PowerIterationLimit
        {
            get
            {
                return _powerIterationLimit;
            }
            set
            {
                _powerIterationLimit = value;
            }
        }
        public int HistogramBinCount
        {
            get
            {
                return _histogramBinCount;
            }
            set
            {
                _histogramBinCount = value;
            }
        }
        public string Extradata1
        {
            get
            {
                return _extradata1;
            }
            set
            {
                _extradata1 = value;
            }
        }
        public string Extradata2
        {
            get
            {
                return _extradata2;
            }
            set
            {
                _extradata2 = value;
            }
        }
        public string Extradata3
        {
            get
            {
                return _extradata3;
            }
            set
            {
                _extradata3 = value;
            }
        }
        public string Extradata4
        {
            get
            {
                return _extradata4;
            }
            set
            {
                _extradata4 = value;
            }
        }
        public int ExtraOption1
        {
            get
            {
                return _extraOption1;
            }
            set
            {
                _extraOption1 = value;
            }
        }
        public int DebugPrintOption
        {
            get
            {
                return _debugPrintOption;
            }
            set
            {
                _debugPrintOption = value;
            }
        }
        public bool ConsoleDebugOutput
        {
            get
            {
                return _consoleDebugOutput;
            }
            set
            {
                _consoleDebugOutput = value;
            }
        }
        public int MinimumFeatureOverlap
        {
            get
            {
                return _minimumFeatureOverlap;
            }
            set
            {
                _minimumFeatureOverlap = value;
            }
        }
        public int DistanceUndefinedOption
        {
            get
            {
                return _distanceUndefinedOption;
            }
            set
            {
                _distanceUndefinedOption = value;
            }
        }
        public int DistanceInfiniteOption
        {
            get
            {
                return _distanceInfiniteOption;
            }
            set
            {
                _distanceInfiniteOption = value;
            }
        }
        public int DistanceChoice
        {
            get
            {
                return _distanceChoice;
            }
            set
            {
                _distanceChoice = value;
            }
        }
        #endregion

        #region Members
        private string _dataFileName = string.Empty;
        private string _outputDataFileName = string.Empty;
        private string _inputDataFileName = string.Empty;
        private string _inputFormatStyle = string.Empty;
        private string _familySpecification = string.Empty;
        private double _randomInputCut = 1.1;
        private string _inputClusteredFileName = string.Empty;
        private string _selectedClusters = string.Empty;
        private int _runNumber = 0;
        private int _dataPoints = 0;
        private int _processingOption = 0;
        private int _transformDimension = 4;
        private string _baseResultDirectoryName = string.Empty;
        private string _resultDirectoryExtension = string.Empty;
        private string _timingOutputFileName = "PairwiseClusteringTimings.xls";
        private string _runSetLabel = string.Empty;
        private string _controlDirectoryName = string.Empty;
        private string _sMACOFInputFileName = string.Empty;
        private string _dataLabelsFileName = string.Empty;
        private string _pointPropertiesFileName = string.Empty;
        private string _familySMACOFFileName = string.Empty;
        private string _clusterSMACOFFileName = string.Empty;
        private int _groupCenterSize = 10;
        private int _maxNcent = 20;
        private int _splitorexpandit = 1;
        private string _pattern = string.Empty;
        private int _threadCount = 1;
        private int _nodeCount = 32;
        private int _mPIperNodeCount = 24;
        private int _mPIIOStrategy = 0;
        private int _toosmalltoSplit = 50;
        private double _minEigtest = -0.01;
        private bool _convergeIntermediateClusters = false;
        private int _waititerations = 10;
        private double _epsi_max_change = 0.001;
        private double _initialCoolingFactor = 0.9;
        private double _fineCoolingFactor = 0.99;
        private double _eigenvaluechange = 0.001;
        private double _eigenvectorchange = 0.001;
        private int _iterationatend = 2000;
        private int _convergenceLoopLimit = 2000;
        private double _freezingLimit = 0.002;
        private int _powerIterationLimit = 200;
        private int _histogramBinCount = 100;
        private string _extradata1 = string.Empty;
        private string _extradata2 = string.Empty;
        private string _extradata3 = string.Empty;
        private string _extradata4 = string.Empty;
        private int _extraOption1 = 0;
        private int _debugPrintOption = 1;
        private bool _consoleDebugOutput = false;
        private int _minimumFeatureOverlap = 50;
        private int _distanceUndefinedOption = 0;
        private int _distanceInfiniteOption = 0;
        private int _distanceChoice = 3;
        #endregion
    }
}
