using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace ConfigLib
{
    [XmlRoot(ElementName="Configuration", 
        Namespace="http://pervasivetechnologylabs.iu.edu")]
    public class Configuration
    {
        private static XmlSerializer s;
        
        [XmlAttribute(AttributeName="name")]
        public string Name {get; set;}

        public GlobalConfig Global { get; set; }
        public SWConfig SmithWaterman { get; set; }
        public PairwiseClusteringConfig PairwiseClustering { get; set; }
        public MDSConfig MDS { get; set; }

        public Configuration()
        {
            s = new XmlSerializer(typeof(Configuration));
        }

        public void SerializeToFile(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Create);
            s.Serialize(fs, this);
        }

        public static Configuration BuildFromFile(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open);
            return s.Deserialize(fs) as Configuration;
        }

    }
}
