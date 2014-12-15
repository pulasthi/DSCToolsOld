
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Salsa.Core.Configuration
{
    
    public class ManxcatConfigurationSection : ConfigurationSection
    {
        #region Constructors
        public ManxcatConfigurationSection()
        {
        }
        #endregion

        #region Properties

        public override string SectionName
        {
            get
            {
                return "Manxcat";
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
        public string ReducedVectorOutputFileName
        {
            get
            {
                return _reducedVectorOutputFileName;
            }
            set
            {
                _reducedVectorOutputFileName = value;
            }
        }
        public string ClusterDirectory
        {
            get
            {
                return _clusterDirectory;
            }
            set
            {
                _clusterDirectory = value;
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
        public int DistanceProcessingOption
        {
            get
            {
                return _distanceProcessingOption;
            }
            set
            {
                _distanceProcessingOption = value;
            }
        }
        public int InitializationOption
        {
            get
            {
                return _initializationOption;
            }
            set
            {
                _initializationOption = value;
            }
        }
        public string InitializationFileName
        {
            get
            {
                return _initializationFileName;
            }
            set
            {
                _initializationFileName = value;
            }
        }
        public int WeightingOption
        {
            get
            {
                return _weightingOption;
            }
            set
            {
                _weightingOption = value;
            }
        }
        public string WeightingFileName
        {
            get
            {
                return _weightingFileName;
            }
            set
            {
                _weightingFileName = value;
            }
        }
        public bool Write2Das3D
        {
            get
            {
                return _write2Das3D;
            }
            set
            {
                _write2Das3D = value;
            }
        }
        public int LocalVectorDimension
        {
            get
            {
                return _localVectorDimension;
            }
            set
            {
                _localVectorDimension = value;
            }
        }
        public string Selectedvariedpoints
        {
            get
            {
                return _selectedvariedpoints;
            }
            set
            {
                _selectedvariedpoints = value;
            }
        }
        public string VariedPointCriterion
        {
            get
            {
                return _variedPointCriterion;
            }
            set
            {
                _variedPointCriterion = value;
            }
        }
        public string Selectedvariedpointfile
        {
            get
            {
                return _selectedvariedpointfile;
            }
            set
            {
                _selectedvariedpointfile = value;
            }
        }
        public string Selectedfixedpoints
        {
            get
            {
                return _selectedfixedpoints;
            }
            set
            {
                _selectedfixedpoints = value;
            }
        }
        public string FixedPointCriterion
        {
            get
            {
                return _fixedPointCriterion;
            }
            set
            {
                _fixedPointCriterion = value;
            }
        }
        public string Selectedfixedpointfile
        {
            get
            {
                return _selectedfixedpointfile;
            }
            set
            {
                _selectedfixedpointfile = value;
            }
        }
        public string ConversionOption
        {
            get
            {
                return _conversionOption;
            }
            set
            {
                _conversionOption = value;
            }
        }
        public string ConversionInformation
        {
            get
            {
                return _conversionInformation;
            }
            set
            {
                _conversionInformation = value;
            }
        }
        public int RotationOption
        {
            get
            {
                return _rotationOption;
            }
            set
            {
                _rotationOption = value;
            }
        }
        public string RotationLabelsFileName
        {
            get
            {
                return _rotationLabelsFileName;
            }
            set
            {
                _rotationLabelsFileName = value;
            }
        }
        public int InitializationLoops
        {
            get
            {
                return _initializationLoops;
            }
            set
            {
                _initializationLoops = value;
            }
        }
        public int Chisqnorm
        {
            get
            {
                return _chisqnorm;
            }
            set
            {
                _chisqnorm = value;
            }
        }
        public int DistanceFormula
        {
            get
            {
                return _distanceFormula;
            }
            set
            {
                _distanceFormula = value;
            }
        }
        public int FullSecondDerivativeOption
        {
            get
            {
                return _fullSecondDerivativeOption;
            }
            set
            {
                _fullSecondDerivativeOption = value;
            }
        }
        public double MinimumDistance
        {
            get
            {
                return _minimumDistance;
            }
            set
            {
                _minimumDistance = value;
            }
        }
        public int FunctionErrorCalcMultiplier
        {
            get
            {
                return _functionErrorCalcMultiplier;
            }
            set
            {
                _functionErrorCalcMultiplier = value;
            }
        }
        public int ChisqPrintConstant
        {
            get
            {
                return _chisqPrintConstant;
            }
            set
            {
                _chisqPrintConstant = value;
            }
        }
        public int Maxit
        {
            get
            {
                return _maxit;
            }
            set
            {
                _maxit = value;
            }
        }
        public int Nbadgo
        {
            get
            {
                return _nbadgo;
            }
            set
            {
                _nbadgo = value;
            }
        }
        public double ChisqChangePerPoint
        {
            get
            {
                return _chisqChangePerPoint;
            }
            set
            {
                _chisqChangePerPoint = value;
            }
        }
        public double FletcherRho
        {
            get
            {
                return _fletcherRho;
            }
            set
            {
                _fletcherRho = value;
            }
        }
        public double FletcherSigma
        {
            get
            {
                return _fletcherSigma;
            }
            set
            {
                _fletcherSigma = value;
            }
        }
        public double Omega
        {
            get
            {
                return _omega;
            }
            set
            {
                _omega = value;
            }
        }
        public int OmegaOption
        {
            get
            {
                return _omegaOption;
            }
            set
            {
                _omegaOption = value;
            }
        }
        public double QHighInitialFactor
        {
            get
            {
                return _qHighInitialFactor;
            }
            set
            {
                _qHighInitialFactor = value;
            }
        }
        public double QgoodReductionFactor
        {
            get
            {
                return _qgoodReductionFactor;
            }
            set
            {
                _qgoodReductionFactor = value;
            }
        }
        public int QLimitscalculationInterval
        {
            get
            {
                return _qLimitscalculationInterval;
            }
            set
            {
                _qLimitscalculationInterval = value;
            }
        }
        public double Extraprecision
        {
            get
            {
                return _extraprecision;
            }
            set
            {
                _extraprecision = value;
            }
        }
        public int AddonforQcomputation
        {
            get
            {
                return _addonforQcomputation;
            }
            set
            {
                _addonforQcomputation = value;
            }
        }
        public int InitialSteepestDescents
        {
            get
            {
                return _initialSteepestDescents;
            }
            set
            {
                _initialSteepestDescents = value;
            }
        }
        public int TimeCutmillisec
        {
            get
            {
                return _timeCutmillisec;
            }
            set
            {
                _timeCutmillisec = value;
            }
        }
        public double CGResidualLimit
        {
            get
            {
                return _cGResidualLimit;
            }
            set
            {
                _cGResidualLimit = value;
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
        public bool Derivtest
        {
            get
            {
                return _derivtest;
            }
            set
            {
                _derivtest = value;
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
        public string Comment
        {
            get
            {
                return _comment;
            }
            set
            {
                _comment = value;
            }
        }
        #endregion

        #region Members
        private string _dataFileName = string.Empty;
        private string _dataLabelsFileName = string.Empty;
        private string _reducedVectorOutputFileName = "Points.txt";
        private string _clusterDirectory = string.Empty;
        private int _dataPoints = 0;
        private int _processingOption = 0;
        private int _distanceProcessingOption = 0;
        private int _initializationOption = 0;
        private string _initializationFileName = string.Empty;
        private int _weightingOption = 0;
        private string _weightingFileName = string.Empty;
        private bool _write2Das3D = true;
        private int _localVectorDimension = 3;
        private string _selectedvariedpoints = string.Empty;
        private string _variedPointCriterion = "all";
        private string _selectedvariedpointfile = string.Empty;
        private string _selectedfixedpoints = string.Empty;
        private string _fixedPointCriterion = "none";
        private string _selectedfixedpointfile = string.Empty;
        private string _conversionOption = string.Empty;
        private string _conversionInformation = string.Empty;
        private int _rotationOption = 0;
        private string _rotationLabelsFileName = string.Empty;
        private int _initializationLoops = 1;
        private int _chisqnorm = 2;
        private int _distanceFormula = 1;
        private int _fullSecondDerivativeOption = 0;
        private double _minimumDistance = -0.001;
        private int _functionErrorCalcMultiplier = 10;
        private int _chisqPrintConstant = 1;
        private int _maxit = 80;
        private int _nbadgo = 6;
        private double _chisqChangePerPoint = 0.001;
        private double _fletcherRho = 0.25;
        private double _fletcherSigma = 0.75;
        private double _omega = 1.25;
        private int _omegaOption = 0;
        private double _qHighInitialFactor = 0.01;
        private double _qgoodReductionFactor = 0.5;
        private int _qLimitscalculationInterval = 1;
        private double _extraprecision = 0.05;
        private int _addonforQcomputation = 2;
        private int _initialSteepestDescents = 0;
        private int _timeCutmillisec = -1;
        private double _cGResidualLimit = 1E-05;
        private int _powerIterationLimit = 200;
        private double _eigenvaluechange = 0.001;
        private double _eigenvectorchange = 0.001;
        private bool _derivtest = false;
        private int _runNumber = 27;
        private string _baseResultDirectoryName = string.Empty;
        private string _resultDirectoryExtension = string.Empty;
        private string _timingOutputFileName = "ManxcatMDSTimings.xls";
        private string _runSetLabel =string.Empty;
        private string _controlDirectoryName = string.Empty;
        private string _pattern = string.Empty;
        private int _threadCount = 24;
        private int _nodeCount = 30;
        private int _mPIperNodeCount = 1;
        private int _mPIIOStrategy = 0;
        private int _histogramBinCount = 100;
        private string _extradata1 = string.Empty;
        private string _extradata2 = string.Empty;
        private string _extradata3 = string.Empty;
        private string _extradata4 = string.Empty;
        private int _extraOption1 = 0;
        private int _debugPrintOption = 2;
        private bool _consoleDebugOutput = true;
        private string _comment = string.Empty;
        #endregion

    }
}
