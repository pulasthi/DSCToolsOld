using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Common.pviz
{
    [Serializable]
    [XmlType("vertex")]
    public class Vertex
    {
        public Vertex()
        {
        }

        public Vertex(int key)
        {
            Key = key;
        }

        [XmlAttribute("key")]
        public int Key { get; set; }
    }
}
