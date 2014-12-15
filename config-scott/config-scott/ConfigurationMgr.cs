using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Salsa.Core.Configuration
{
    [XmlRoot("Configuration")]
    [XmlInclude(typeof(ManxcatConfigurationSection)),
        XmlInclude(typeof(PairwiseConfigurationSection))]
    public class ConfigurationMgr
    {
        
        
        public ConfigurationMgr()
        {
            _sections = new ConfigurationSectionCollection(this);
        }
        
        public ConfigurationSectionCollection Sections
        {
            get
            {
                return _sections;
            }
        }
       
        public void Save(string fileName)
        {
            using (var writer = XmlWriter.Create(fileName))
            {
                List<Type> extraTypes = new List<Type>();
                foreach (ConfigurationSection section in Sections)
                {
                    if (section.GetType().IsSubclassOf(typeof(ConfigurationSection)))
                    {
                        extraTypes.Add(section.GetType());
                    }
                }


                //XmlSerializer serializer = new XmlSerializer(typeof(ConfigurationMgr), extraTypes.ToArray());
                XmlSerializer serializer = new XmlSerializer(typeof(ConfigurationMgr));
                serializer.Serialize(writer, this);
            }
        }
       
        public static ConfigurationMgr Load(string fileName)
        {
            ConfigurationMgr manager = null;

            using (StreamReader reader = new StreamReader(fileName))
            {
                Type[] types = new Type[2];
                types[0] = typeof(ManxcatConfigurationSection);
                types[1] = typeof(PairwiseConfigurationSection);
                //XmlSerializer serializer = new XmlSerializer(typeof(ConfigurationMgr), types);
                XmlSerializer serializer = new XmlSerializer(typeof(ConfigurationMgr));
                manager = serializer.Deserialize(reader) as ConfigurationMgr;               
            }
            

            return manager;
        }

        #region Members
        private ConfigurationSectionCollection _sections;
        #endregion
    }
}