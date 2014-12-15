using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Xml;

namespace PhylogeneticTreeToPlotViz
{
    public class PVIZFileFormat
    {
        List<PVIZEdge> _edges;
        public List<PVIZEdge> Edges
        {
            get
            {
                return _edges;
            }
            set
            {
                _edges = value;
            }
        }
        List<PVIZCluster> _clusters;

        public List<PVIZCluster> Clusters
        {
            get { return _clusters; }
            set { _clusters = value; }
        }
        List<PVIZPoint> _points;

        internal List<PVIZPoint> Points
        {
            get { return _points; }
            set { _points = value; }
        }
        string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        int _pointsize;

        public int Pointsize
        {
            get { return _pointsize; }
            set { _pointsize = value; }
        }
        int _glyphVisible;

        public int GlyphVisible
        {
            get { return _glyphVisible; }
            set { _glyphVisible = value; }
        }
        int _glyphScale;

        public int GlyphScale
        {
            get { return _glyphScale; }
            set { _glyphScale = value; }
        }

        public PVIZFileFormat()
        {
            _edges = new List<PVIZEdge>();
            _clusters = new List<PVIZCluster>();
            _points = new List<PVIZPoint>();
            _title = "default";
            _pointsize = 2;
            _glyphVisible = 0;
            _glyphScale = 1;
        }

        public PVIZFileFormat(string filePath)
        {
            _edges = new List<PVIZEdge>();
            _clusters = new List<PVIZCluster>();
            _points = new List<PVIZPoint>();
            _title = Path.GetFileNameWithoutExtension(filePath);
            _pointsize = 2;
            _glyphVisible = 0;
            _glyphScale = 1;
            readFromFile(filePath);
        }

        public void readFromFile(string filePath)
        {
            var doc = XDocument.Load(filePath);

            var clusters = from clusterElement in doc.Descendants("cluster")
                           select new PVIZCluster()
                           {
                               Key = clusterElement.Element("key").Value,
                               Label = clusterElement.Element("label").Value,
                               Color = new Color(
                                   (int)clusterElement.Element("color").Attribute("r"),
                                   (int)clusterElement.Element("color").Attribute("g"),
                                   (int)clusterElement.Element("color").Attribute("b"),
                                   (int)clusterElement.Element("color").Attribute("a")),
                               Size = (int)clusterElement.Element("size"),
                               Shape = (int)clusterElement.Element("shape"),
                               Visible = clusterElement.Element("visible").Value,
                               Default = clusterElement.Element("default").Value
                           };

            var points = (from pointElement in doc.Descendants("point")
                          select new PVIZPoint()
                          {
                              Id = (int)pointElement.Element("key"),
                              Group = (int)pointElement.Element("clusterkey"),
                              Label = pointElement.Element("label").Value,

                              X = (double)pointElement.Element("location").Attribute("x"),
                              Y = (float)pointElement.Element("location").Attribute("y"),
                              Z = (float)pointElement.Element("location").Attribute("z")
                          });

            var edges = (from pointElement in doc.Descendants("edge")
                         select new PVIZEdge()
                         {
                             Key = (int)pointElement.Element("key"),
                             VertexElements = pointElement.Element("vertices").Elements("vertex")
                         });
            foreach (var cluster in clusters)
                _clusters.Add(cluster);
            foreach (var point in points)
                _points.Add(point);
            foreach (var edge in edges)
                _edges.Add(edge);
        }
        /*
        public PVIZFileFormat readFromFile(string filePath)
        {
            FileStream stream = new FileStream(filePath, FileMode.Open);
            XmlReader reader = new XmlTextReader(stream);

            PVIZCluster cluster = null;
            PVIZPoint p_point = null;
            PVIZFileFormat pvizFileFormat = new PVIZFileFormat();

            bool plotFlag = false;
            bool clusterFlag = false;
            bool pointFlag = false;
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        if (reader.Name.Equals("plot"))
                        {
                            plotFlag = true;
                        }
                        if (plotFlag == true)
                        {
                            if (reader.Name.Equals("title"))
                            {
                                reader.Read();
                                pvizFileFormat.Title = reader.Value;
                            }
                            if (reader.Name.Equals("pointsize"))
                            {
                                reader.Read();
                                pvizFileFormat.Pointsize = int.Parse(reader.Value);
                            }
                            if (reader.Name.Equals("visible"))
                            {
                                reader.Read();
                                pvizFileFormat.GlyphVisible = int.Parse(reader.Value);
                            }
                            if (reader.Name.Equals("scale"))
                            {
                                reader.Read();
                                pvizFileFormat.GlyphScale = int.Parse(reader.Value);
                            }
                        }
                        if (reader.Name.Equals("clusters"))
                        {
                            plotFlag = false;
                            clusterFlag = true;
                        }
                        if (clusterFlag == true)
                        {
                            if (reader.Name.Equals("cluster"))
                            {
                                cluster = new PVIZCluster();
                            }
                            if (reader.Name.Equals("key"))
                            {
                                reader.Read();
                                cluster.Key = reader.Value;
                            }
                            if (reader.Name.Equals("label"))
                            {
                                reader.Read();
                                if (reader.NodeType != XmlNodeType.EndElement)
                                    cluster.Label = reader.Value;
                            }
                            if (reader.Name.Equals("visible"))
                            {
                                reader.Read();
                                cluster.Visible = reader.Value;
                            }
                            if (reader.Name.Equals("default"))
                            {
                                reader.Read();
                                cluster.Default = reader.Value;
                            }
                            if (reader.Name.Equals("color"))
                            {
                                cluster.Color.R = int.Parse(reader.GetAttribute(0));
                                cluster.Color.G = int.Parse(reader.GetAttribute(1));
                                cluster.Color.B = int.Parse(reader.GetAttribute(2));
                                cluster.Color.A = int.Parse(reader.GetAttribute(3));
                            }
                            if (reader.Name.Equals("size"))
                            {
                                reader.Read();
                                cluster.Size = int.Parse(reader.Value);
                            }
                            if (reader.Name.Equals("shape"))
                            {
                                reader.Read();
                                cluster.Shape = int.Parse(reader.Value);
                            }
                        }

                        if (reader.Name.Equals("points"))
                        {
                            clusterFlag = false;
                            pointFlag = true;
                        }
                        if (pointFlag == true)
                        {
                            if (reader.Name.Equals("point"))
                            {
                                p_point = new PVIZPoint();
                            }
                            if (reader.Name.Equals("key"))
                            {
                                reader.Read();
                                p_point.Id = int.Parse(reader.Value);
                            }
                            if (reader.Name.Equals("label"))
                            {
                                reader.Read();
                                if (reader.NodeType != XmlNodeType.EndElement)
                                    p_point.Label = reader.Value;
                            }
                            if (reader.Name.Equals("clusterkey"))
                            {
                                reader.Read();
                                p_point.Group = int.Parse(reader.Value);
                            }
                            if (reader.Name.Equals("location"))
                            {
                                p_point.X = double.Parse(reader.GetAttribute(0));
                                p_point.Y = double.Parse(reader.GetAttribute(1));
                                p_point.Z = double.Parse(reader.GetAttribute(2));
                            }
                        }
                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        if (reader.Name.Equals("cluster"))
                            pvizFileFormat.Clusters.Add(cluster);
                        if (reader.Name.Equals("point"))
                            pvizFileFormat.Points.Add(p_point);
                        break;
                }
            }
            reader.Close();
            return pvizFileFormat;
        }
         * */
        public void writeToFile(string filePath)
        {
            XDocument doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("PVIZ 3 input format"),
                new XElement("plotviz",
                    new XElement("plot",
                        new XElement("title", _title),
                        new XElement("pointsize", _pointsize),
                        new XElement("glyph",
                            new XElement("visible", _glyphVisible),
                            new XElement("scale", _glyphScale))),
                        new XElement("clusters",
                            from cluster in _clusters
                            select new XElement("cluster",
                                new XElement("key", cluster.Key),
                                new XElement("label", cluster.Label),
                                new XElement("visible", cluster.Visible),
                                new XElement("default", cluster.Default),
                                new XElement("color", new XAttribute("r", cluster.Color.R),
                                    new XAttribute("g", cluster.Color.G),
                                    new XAttribute("b", cluster.Color.B),
                                    new XAttribute("a", cluster.Color.A)),
                                new XElement("size", cluster.Size),
                                new XElement("shape", cluster.Shape))),
                        new XElement("points",
                            from point in _points
                            select new XElement("point",
                                new XElement("key", point.Id),
                                new XElement("clusterkey", point.Group),
                                new XElement("label", point.Label),
                                new XElement("location", new XAttribute("x", point.X),
                                    new XAttribute("y", point.Y),
                                    new XAttribute("z", point.Z)))),
                        new XElement("edges",
                            from edge in _edges
                            select new XElement("edge",
                                new XElement("key", edge.Key),
                                new XElement("vertices",
                                    from vertex in edge.Vertices
                                    select new XElement("vertex", new XAttribute("key", vertex)))))));
            doc.Save(filePath);
        }
    }
}
