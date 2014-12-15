using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using Salsa.Core.Configuration.Sections;

namespace Salsa.Core.Configuration
{
    /* ConfiugrationMgr will be serialized as
     * <Configuration> root element*/
    [XmlRoot(ElementName="Configuration"),
    XmlInclude(typeof(ManxcatSection)), 
    XmlInclude(typeof(SmithWatermanSection)),
    XmlInclude(typeof(PairwiseSection))]

    public class ConfigurationMgr
    {
        private SectionCollection _sections;

        public ConfigurationMgr()
        {
            _sections = new SectionCollection();
        }

        public SectionCollection Sections
        {
            get
            {               
                return _sections;
            }            
        }

        public void SaveAs(string fileName)
        {
            StreamWriter writer = new StreamWriter(fileName);
            XmlSerializer serializer = new XmlSerializer(typeof(ConfigurationMgr));
            serializer.Serialize(writer, this);
            writer.Close();
        }


        public static ConfigurationMgr LoadConfiguration(string fileName)
        {
            ConfigurationMgr manager = null;
            StreamReader reader = new StreamReader(fileName);            
            XmlSerializer serializer = new XmlSerializer(typeof(ConfigurationMgr));
            manager = serializer.Deserialize(reader) as ConfigurationMgr;
            reader.Close();
            return manager;
        }        
    }
}
