using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Common.pviz
{
    [Serializable]
    [XmlRoot(ElementName = "plotviz")]
    public class PvizModel
    {
        [XmlElement("plot")]
        public Plot Plot { get; set; }

        [XmlArray("clusters"), XmlArrayItem(typeof(Cluster), ElementName = "cluster")]
        public List<Cluster> Clusters { get; set; }

        [XmlArray("points"), XmlArrayItem(typeof(Point), ElementName = "point")]
        public List<Point> Points { get; set; }

        [XmlArray("edges"), XmlArrayItem(typeof(Edge), ElementName = "edge")]
        public List<Edge> Edges { get; set; }

        public void SaveAs(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PvizModel));
                serializer.Serialize(writer, this);
                writer.Close();
            }
        }

        public static PvizModel LoadPviz(string fileName)
        {
            PvizModel pvizModel = null;

            using (StreamReader reader = new StreamReader(fileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PvizModel));
                pvizModel = serializer.Deserialize(reader) as PvizModel;
                reader.Close();
            }

           return pvizModel;
        }

        
    }
}