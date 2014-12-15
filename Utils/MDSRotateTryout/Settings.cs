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

        private string _defPlotVizExe = @"C:\Program Files (x86)\Indiana University\PlotViz v2\Salsa.PlotViz.exe";
        private string _defMDSAppDir = @"E:\salsa\svn\SalsaTPL\bin\Release";
        private string _defBaseDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        #endregion

        #region Properties
        public string DefaultMDSAppDirectory
        {
            get
            {
                return _defMDSAppDir;
            }
            set
            {
                _defMDSAppDir = value;
            }
        }

        public string DefaultBaseDirectory
        {
            get
            {
                return _defBaseDir;
            }
            set
            {
                _defBaseDir = value;
            }
        }

        public static string SettingsFileName
        {
            get
            {
                return "settings.xml";
            }
        }

        public string DefPlotVizExe
        {
            get { return _defPlotVizExe; }
            set { _defPlotVizExe = value; }
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
            clone.DefaultMDSAppDirectory = this.DefaultMDSAppDirectory;
            clone.DefaultBaseDirectory = this.DefaultBaseDirectory;
            return clone;
        }

        public static void CopySettings(Settings from, Settings to)
        {
            to.DefaultMDSAppDirectory = from.DefaultMDSAppDirectory;
            to._defBaseDir = from._defBaseDir;
        }
        #endregion



    }
}
