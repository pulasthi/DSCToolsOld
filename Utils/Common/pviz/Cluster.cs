using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Common.pviz
{
    [Serializable]
    [XmlType("cluster")]
    public class Cluster
    {
        [XmlElement("key")]
        public int Key { get; set; }

        [XmlElement("label")]
        public string Label { get; set; }

        [XmlElement("visible")]
        public int Visible { get; set; }

        [XmlElement("default")]
        public int Default { get; set; }

        [XmlElement("color")]
        public Color Color { get; set; }

        [XmlElement("size")]
        public float Size { get; set; }

        [XmlElement("shape")]
        public int Shape { get; set; }

        public Cluster Clone()
        {
            Cluster clone = new Cluster();
            clone.Key = Key;
            clone.Label = Label;
            clone.Visible = Visible;
            clone.Default = Default;
            clone.Color = Color;
            clone.Size = Size;
            clone.Shape = Shape;
            return clone;
        }
    }
}
