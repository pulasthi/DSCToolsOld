using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Common.pviz
{
    [Serializable]
    [XmlType("location")]
    public class Location
    {
        public Location()
        {
        }

        public Location(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        [XmlAttribute("x")]
        public double X { get; set; }
        [XmlAttribute("y")]
        public double Y { get; set; }
        [XmlAttribute("z")]
        public double Z { get; set; }
    }
}
