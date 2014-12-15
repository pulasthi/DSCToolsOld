using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Common.pviz
{
    [Serializable]
    [XmlType("plot")]
    public class Plot
    {
        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("pointsize")]
        public int PointSize { get; set; }

        [XmlElement("glyph")]
        public Glyph Glyph { get; set; }

        [XmlElement("camera")]
        public Camera Camera { get; set; }

    }
}
