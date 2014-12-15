using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Common.pviz
{
    [Serializable]
    [XmlType("glyph")]
    public class Glyph
    {
        public Glyph()
        {
        }

        public Glyph(int visible, float scale)
        {
            Visible = visible;
            Scale = scale;
        }
        [XmlElement("visible")]
        public int Visible { get; set; }

        [XmlElement("scale")]
        public float Scale { get; set; }
    }
}
