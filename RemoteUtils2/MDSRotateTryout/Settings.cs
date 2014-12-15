using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace MDSTryout
{
    public class Settings
    {
        #region Members

        private string _defaultPlotVizExe = @"C:\Program Files (x86)\PVIZ3\bin\pviz3.exe";
        private string _defaultManxcatAppDirectory = @"E:\salsa\svn\SalsaTPL\bin\Release";
        private string _defaultPairwiseAppDirectory = @"E:\salsa\svn\SalsaTPL\bin\Release";
        private string _defaultSpongeAppDirectory = @"D:\salsa\svn\SalsaTPL\Salsa.SequentialSponge\bin\Debug";
        private string _defaultBaseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private string _defaultTargetDirectory = @"C:\remote\gcf";
        #endregion

        #region Properties
        public string DefaultManxcatAppDirectory
        {
            get
            {
                return _defaultManxcatAppDirectory;
            }
            set
            {
                _defaultManxcatAppDirectory = value;
            }
        }

        public string DefaultPairwiseAppDirectory
        {
            get { return _defaultPairwiseAppDirectory; }
            set { _defaultPairwiseAppDirectory = value; }
        }

        public string DefaultSpongeAppDirectory
        {
            get { return _defaultSpongeAppDirectory; }
            set { _defaultSpongeAppDirectory = value; }
        }

        public string DefaultBaseDirectory
        {
            get
            {
                return _defaultBaseDirectory;
            }
            set
            {
                _defaultBaseDirectory = value;
            }
        }

        public static string SettingsFileName
        {
            get
            {
                return "settings.xml";
            }
        }

        public string DefaultPlotVizExe
        {
            get { return _defaultPlotVizExe; }
            set { _defaultPlotVizExe = value; }
        }

        public string DefaultTargetDirectory
        {
            get { return _defaultTargetDirectory; }
            set { _defaultTargetDirectory = value; }
        }

        #endregion

        #region Methods

        

        public void SaveAs(string fname)
        {

            using (StreamWriter writer = new StreamWriter(fname))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                serializer.Serialize(writer, this);
                writer.Close();
            }
        }

        public void Save()
        {
            SaveAs(SettingsFileName);
        }

        public static Settings LoadSettings(string fname)
        {
            Settings settings = null;

            using (StreamReader reader = new StreamReader(fname))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                settings = serializer.Deserialize(reader) as Settings;
                reader.Close();
            }

            return settings;
        }

        public Settings CloneSettings()
        {
            Settings clone = new Settings();
            clone.DefaultManxcatAppDirectory = this.DefaultManxcatAppDirectory;
            clone.DefaultBaseDirectory = this.DefaultBaseDirectory;
            clone._defaultTargetDirectory = this._defaultTargetDirectory;
            return clone;
        }

        public static void CopySettings(Settings from, Settings to)
        {
            to.DefaultManxcatAppDirectory = from.DefaultManxcatAppDirectory;
            to._defaultBaseDirectory = from._defaultBaseDirectory;
            to._defaultTargetDirectory = from._defaultTargetDirectory;
        }
        #endregion



    }
}
