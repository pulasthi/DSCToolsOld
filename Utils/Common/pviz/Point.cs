using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Common.pviz
{
    [Serializable]
    [XmlType("point")]
    public class Point
    {
        [XmlElement("key")]
        public int Key { get; set; }

        [XmlElement("clusterkey")]
        public int ClusterKey { get; set; }

        [XmlElement("label")]
        public string Label { get; set; }

        [XmlElement("location")]
        public Location Location { get; set; }

        public Point Clone()
        {
            Point p = new Point
                          {
                              Key = Key,
                              ClusterKey = ClusterKey,
                              Label = Label,
                              Location = new Location(Location.X, Location.Y, Location.Z)
                          };
            return p;
        }
    }
}
