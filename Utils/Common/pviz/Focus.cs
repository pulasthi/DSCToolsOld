using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Common.pviz
{
    [Serializable]
    [XmlType("focus")]
    public class Focus
    {
        public Focus()
        {
            X = Y = Z = 0.0f;
        }

        public Focus(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        [XmlAttribute("x")]
        public float X { get; set; }
        [XmlAttribute("y")]
        public float Y { get; set; }
        [XmlAttribute("z")]
        public float Z { get; set; }

    }
}
