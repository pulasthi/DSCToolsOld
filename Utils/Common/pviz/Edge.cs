using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Common.pviz
{
    [Serializable]
    [XmlType("edge")]
    public class Edge
    {
        public Edge()
        {
        }

        public Edge(int key)
        {
            Key = key;
        }

        

        [XmlElement("key")]
        public int Key { get; set; }

        [XmlArray("vertices"), XmlArrayItem(typeof(Vertex), ElementName = "vertex")]
        public List<Vertex> Vertices { get; set; }

    }
}
