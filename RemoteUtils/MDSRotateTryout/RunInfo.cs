using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace MDSTryout
{
    public class RunInfo
    {
        #region Members

        private RunType _runType;
        // Local directory containing executables
        private string _appDir;
        // Local directory containing the project directory, i.e. parent directory
        private string _baseDir;
        // Project name
        private string _name;
        // Local path to cluster file
        private string _clusterFile;

        private bool _autoIncr;
        private int _processCount;
        private int _threadCount;

        private bool _isRunSuccess;

        private bool _isRunCompleted;
        private int _outLineReadCount;
        private int _errLineReadCount;

        private string _localOutStatusFile;
        private string _localErrStatusFile;

        // todo:  beta2 - remove these and use localstatus properties above
        private string _localOutFile;
        private string _localErrFile;

        #region RemoteRunMembers

        private string _hnProjectDir;
        private string _cnProjectDir;
        private int _nodeCount;
        private int _maxCores;
        private string _hnName;
        private string _nodesList;
        private string _hostsList;
        private string _targetDir;

        // Todo: improve this to store actual names of nodes instead of indices
        /* The following assumes clusters won't change */
        
        // Index of the headnode in the headnode list
        private int _hnIndex;
        // Indices of selected compute nodes
        private int[] _cnIndices;
        

        // Job and tasks details
//        private List<int> _setupTaskIds;
//        private int _manxcatTaskId;
        private int _jobId;

        #endregion

        #endregion

        #region Properties

        public RunType RunType
        {
            get { return _runType; }
            set { _runType = value; }
        }

        public bool IsRunSuccess
        {
            get { return _isRunSuccess; }
            set { _isRunSuccess = value; }
        }

        public string AppDir 
        { 
            get
            {
                return _appDir; ;
            }
            set
            {
                _appDir = value;
            }
        }
        public string BaseDir
        { 
            get
            {
                return _baseDir; ;
            }
            set
            {
                _baseDir = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string ClusterFile
        {
            get
            {
                return _clusterFile;
            }
            set
            {
                _clusterFile = value;
            }
        }

        public bool AutoIncr
        { 
            get
            {
                return _autoIncr;
            }
            set
            {
                _autoIncr = value;
            }
        }
        public int ProcessCount
        {
            get
            {
                return _processCount;
            }
            set
            {
                _processCount = value;
            }
        }

        public int ThreadCount
        {
            get { return _threadCount; }
            set { _threadCount = value; }
        }

        public int HnIndex
        {
            get { return _hnIndex; }
            set { _hnIndex = value; }
        }

        public int[] CnIndices
        {
            get { return _cnIndices; }
            set { _cnIndices = value; }
        }

        public int JobId
        {
            get { return _jobId; }
            set { _jobId = value; }
        }

        public int NodeCount
        {
            get { return _nodeCount; }
            set { _nodeCount = value; }
        }

        public int MaxCores
        {
            get { return _maxCores; }
            set { _maxCores = value; }
        }

        public string HnProjectDir
        {
            get { return _hnProjectDir; }
            set { _hnProjectDir = value; }
        }

        public string CnProjectDir
        {
            get { return _cnProjectDir; }
            set { _cnProjectDir = value; }
        }

        public string NodesList
        {
            get { return _nodesList; }
            set { _nodesList = value; }
        }

        public string HostsList
        {
            get { return _hostsList; }
            set { _hostsList = value; }
        }

        public string HnName
        {
            get { return _hnName; }
            set { _hnName = value; }
        }

        public bool IsRunCompleted
        {
            get { return _isRunCompleted; }
            set { _isRunCompleted = value; }
        }

        public int OutLineReadCount
        {
            get { return _outLineReadCount; }
            set { _outLineReadCount = value; }
        }

        public int ErrLineReadCount
        {
            get { return _errLineReadCount; }
            set { _errLineReadCount = value; }
        }

        public string LocalOutStatusFile
        {
            get { return _localOutStatusFile; }
            set { _localOutStatusFile = value; }
        }

        public string LocalErrStatusFile
        {
            get { return _localErrStatusFile; }
            set { _localErrStatusFile = value; }
        }


        // todo: beta2 - remove these and use localstatus properties above
        public string LocalOutFile
        {
            get { return _localOutFile; }
            set { _localOutFile = value; }
        }

        public string LocalErrFile
        {
            get { return _localErrFile; }
            set { _localErrFile = value; }
        }

        public string TargetDir
        {
            get { return _targetDir; }
            set { _targetDir = value; }
        }

//        public List<int> SetupTaskIds
//        {
//            get { return _setupTaskIds; }
//            set { _setupTaskIds = value; }
//        }
//
//        public int ManxcatTaskId
//        {
//            get { return _manxcatTaskId; }
//            set { _manxcatTaskId = value; }
//        }

        #endregion

        #region Constructors
        public RunInfo()
        {
            
        }
        #endregion

        public void SaveAs(string fname)
        {
            using (StreamWriter writer = new StreamWriter(fname))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RunInfo));
                serializer.Serialize(writer, this);
                writer.Close();
            } 
        }

        public static RunInfo LoadRunInfo(string fname)
        {
            RunInfo runinfo = null;

            using (StreamReader reader = new StreamReader(fname))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RunInfo));
                runinfo = serializer.Deserialize(reader) as RunInfo;
                reader.Close();
            }

            return runinfo;
        }
    }
}
