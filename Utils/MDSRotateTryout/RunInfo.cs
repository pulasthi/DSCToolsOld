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
        private string _appDir;
        private string _baseDir;
        private string _name;

        private string _clusterFile;

        private bool _autoIncr;
        private int _processes;

        private bool _isRunSuccess;

        #endregion

        #region Properties
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
        public int Processes
        {
            get
            {
                return _processes;
            }
            set
            {
                _processes = value;
            }
        }
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
