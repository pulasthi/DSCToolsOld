using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using Salsa.Core.Configuration;

namespace Test4
{
    /// <summary>
    /// This would serve as the container of project
    /// info. Once serialized it should produce an XML
    /// with all the info in parallel to the folder
    /// structure.
    /// </summary>
    public class Project
    {
        public const string APPS = "apps";
        public const string CONFIG = "config";
        public const string INPUT = "input";
        public const string OUTPUT = "output";
        private bool _dirty;

        private string _projectName;
        private string _folderPath; // todo:rename this to _projectDir(v2)
        private string _appsDir;
        private string _inputDir;
        private string _outputDir;
        private string _configDir;

        private string _inputFile;
        private string _configFile;

        private int _pageNum;
        private int _appIndex;
        private int _headNodeIndex;
        private int [] _nodes;
        private int _sectionIndex;

        private ConfigurationMgr _mgr;

        #region Constructors
        public Project(string folderPath, string projectName)
        {
            this._projectName = projectName;
            this._folderPath = folderPath;
            this._appsDir =
                Path.Combine(Path.Combine(_folderPath, _projectName), APPS);
            this._inputDir =
                Path.Combine(Path.Combine(_folderPath, _projectName), INPUT);
            this._outputDir =
                Path.Combine(Path.Combine(_folderPath, _projectName), OUTPUT);
            this._configDir =
                Path.Combine(Path.Combine(_folderPath, _projectName), CONFIG);
            this._configFile = Path.Combine(_configDir, "config.xml");
            _mgr = new ConfigurationMgr();
            _mgr.SaveAs(_configFile);

            _dirty = true;
        }
        public Project()
        { }
        #endregion

        #region Properties
        public string ProjectName
        {
            get
            {
                return _projectName;
            }
            set
            {
                //_dirty = value.Equals(_projectName) ? _dirty : true;
                _projectName = value;

            }

        }
        
        public string FolderPath
        {
            get
            {
                return _folderPath;
            }
            set
            {
                //_dirty = value.Equals(_folderPath) ? _dirty : true;
                _folderPath = value;
            }
        }
                
        public int PageNum
        {
            get
            {
                return _pageNum;
            }
            set
            {
                //_dirty = _pageNum == value ? _dirty : true;
                _pageNum = value;
            }
        }
       
        public int AppIndex
        {
            get
            {
                return _appIndex;
            }
            set
            {
                //_dirty = _appIndex == value ? _dirty : true;
                _appIndex = value;
            }
        }
        public int HeadNodeIndex
        {
            get
            {
                return _headNodeIndex;
            }
            set
            {
                //_dirty = _headNodeIndex == value ? _dirty : true;
                _headNodeIndex = value;
            }
        }
        
        [XmlIgnore]
        public bool Dirty
        {
            get
            {
                return _dirty;
            }
            set
            {
                _dirty = value;
            }
        }

        public int[] Nodes
        {
            get
            {
                return _nodes;
            }
            set
            {
                // dirty flag is handled by UI
                _nodes = value;
            }
        }

        public string AppDir
        {
            get
            {
                return _appsDir;
            }
            set
            {
                // how not to give someone set this?
                _appsDir = value; 
            }
        }

        public string InputDir
        {
            get
            {
                return _inputDir;
            }
            set
            {
                // how not to give someone set this?
                _inputDir = value;
            }
        }

        public string OutputDir
        {
            get
            {
                return _outputDir;
            }
            set
            {
                // how not to give someone set this?
                _outputDir = value;
            }
        }

        public string ConfigDir
        {
            get
            {
                return _configDir;
            }
            set
            {
                // how not to give someone set this?
                // I only want this for the XML deserializer to work
                _configDir = value;
            }
        }

        public string InputFile
        {
            get
            {
                return _inputFile;
            }
            set
            {
                _inputFile = value;
            }
        }

        public string ConfigFile
        {
            get
            {
                return _configFile;
            }
            set
            {
                _configFile = value;
            }
        }

        public int SectionIndex
        {
            get
            {
                return _sectionIndex;
            }
            set
            {
                _sectionIndex = value;
            }
        }

        #endregion

        public void Save()
        {
            string fileName = _projectName + ".pet";
            fileName = Path.Combine(Path.Combine(_folderPath, _projectName), fileName);
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Project));
                serializer.Serialize(writer, this);
                writer.Close();
            }
            _mgr.SaveAs(_configFile);
            _dirty = false;
        }

        public ConfigurationMgr Manager
        {
            get
            {
                return _mgr;
            }
        }

        public void UpdateManager(string path)
        {
            _mgr = ConfigurationMgr.LoadConfiguration(path,false);
        }
        

        public static Project Load(string path)
        {
            Project project = null;
            using (StreamReader reader = new StreamReader(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Project));
                project = serializer.Deserialize(reader) as Project;
                project._dirty = false; // clear dirty flag
                reader.Close();
            }
            project._mgr = ConfigurationMgr.LoadConfiguration(project.ConfigFile, false);
            return project;
        }

    }
}
