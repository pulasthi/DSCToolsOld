using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace MDSTryout
{
    public class PlotVizDataFile
    {
        private PlotVizDataFileClusterCollection _clusters;
        private PlotVizDataFilePointCollection _points;
        private PlotVizDataFileEdgeCollection _edges;
        private PlotVizDataFileTreeNodeCollection _nodes;
        private PlotVizDataFileTreeClusterCollection _treeclusters;

        public PlotVizDataFile()
        {
            _clusters = new PlotVizDataFileClusterCollection();
            _points = new PlotVizDataFilePointCollection();
            _edges = new PlotVizDataFileEdgeCollection();
            _nodes = new PlotVizDataFileTreeNodeCollection();
            _treeclusters = new PlotVizDataFileTreeClusterCollection();

        }

        public PlotVizDataFile(IEnumerable<PlotVizDataFileCluster> clusters, IEnumerable<PlotVizDataFilePoint> points, IEnumerable<PlotVizDataFileEdge> edges, IEnumerable<PlotVizDataFileTreeNode> nodes, IEnumerable<PlotVizDataFileTreeCluster> treeclusters)
        {
            _clusters = new PlotVizDataFileClusterCollection(clusters);
            _points = new PlotVizDataFilePointCollection(points);
            _edges = new PlotVizDataFileEdgeCollection(edges);
            _nodes = new PlotVizDataFileTreeNodeCollection(nodes);
            _treeclusters = new PlotVizDataFileTreeClusterCollection(treeclusters);
        }

        public PlotVizDataFileClusterCollection Clusters
        {
            get
            {
                return _clusters;
            }
        }

        public PlotVizDataFileTreeClusterCollection TreeClusters
        {
            get
            {
                return _treeclusters;
            }
        }

        public PlotVizDataFilePointCollection Points
        {
            get
            {
                return _points;
            }
        }

        public PlotVizDataFileEdgeCollection Edges
        {
            get
            {
                return _edges;
            }
        }

        public PlotVizDataFileTreeNodeCollection Nodes
        {
            get
            {
                return _nodes;
            }
        }

        public void Save(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLower();

            if ((ext == ".pviz") || (ext == ".xml"))
            {
                SaveAsXml(fileName);
            }
            else
            {
                SaveAsText(fileName);
            }
        }

        public static PlotVizDataFile Load(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLower();

            if (ext == ".pviz")
            {
                return LoadFromXml(fileName);
            }
            else
            {
                return LoadFromText(fileName);
            }
        }

        private void SaveAsText(string fileName)
        {
            using (TextWriter writer = File.CreateText(fileName))
            {
                writer.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}",
                    "FileIndex",
                    "X",
                    "Y",
                    "Z",
                    "ClusterKey",
                    "Label");

                foreach (var point in Points)
                {
                    writer.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}",
                        point.Key,
                        point.Coordinate.X,
                        point.Coordinate.Y,
                        point.Coordinate.Z,
                        point.ClusterKey,
                        point.Label);
                }
            }
        }

        private static PlotVizDataFile LoadFromText(string fileName)
        {
            PlotVizDataFile dataFile = new PlotVizDataFile();

            using (StreamReader reader = new StreamReader(fileName))
            {
                bool isFirst = true;
                bool isPreviousVersion = true;

                while (reader.EndOfStream == false)
                {
                    string line = reader.ReadLine();

                    if (isFirst && line.StartsWith("FileIndex"))
                    {
                        isFirst = false;
                        isPreviousVersion = false;
                        continue;
                    }

                    string[] fields = line.Split(new char[] { '\t', ' ' });

                    PlotVizDataFileCoordinate coord = new PlotVizDataFileCoordinate();
                    coord.X = Convert.ToSingle(fields[1]);
                    coord.Y = Convert.ToSingle(fields[2]);
                    coord.Z = Convert.ToSingle(fields[3]);

                    PlotVizDataFilePoint point = new PlotVizDataFilePoint();
                    point.Key = Convert.ToInt32(fields[0]);
                    point.Coordinate = coord;
                    point.ClusterKey = Convert.ToInt32(fields[4]);

                    if (!isPreviousVersion)
                    {
                        point.Label = fields[5];
                    }

                    if (dataFile.Clusters.Contains(point.ClusterKey) == false)
                    {
                        dataFile.Clusters.Add(new PlotVizDataFileCluster(point.ClusterKey));
                    }

                    dataFile.Points.Add(point);
                }
            }

            return dataFile;
        }

        private void SaveAsXml(string fileName)
        {
            var doc = new XDocument(
                new XElement("plotviz",
                    new XElement("clusters",
                        from cluster in _clusters
                        select new XElement("cluster",
                            new XElement("key", cluster.Key),
                            new XElement("label", cluster.Label),
                            new XElement("color",
                                new XAttribute("r", cluster.Color.R),
                                new XAttribute("g", cluster.Color.G),
                                new XAttribute("b", cluster.Color.B),
                                new XAttribute("a", cluster.Color.A)),
                            new XElement("size", cluster.Size))),
                    new XElement("points",
                        from point in _points
                        select new XElement("point",
                            new XElement("key", point.Key),
                            new XElement("clusterkey", point.ClusterKey),
                            new XElement("label", point.Label),
                            new XElement("location",
                                new XAttribute("x", point.Coordinate.X),
                                new XAttribute("y", point.Coordinate.Y),
                                new XAttribute("z", point.Coordinate.Z)))),
                    new XElement("TreeClusters",
                        from treecluster in _treeclusters
                        select new XElement("TreeCluster",
                            new XElement("key", treecluster.Key),
                            new XElement("label", treecluster.Label),
                            new XElement("color",
                                new XAttribute("r", treecluster.Color.R),
                                new XAttribute("g", treecluster.Color.G),
                                new XAttribute("b", treecluster.Color.B),
                                new XAttribute("a", treecluster.Color.A)),
                            new XElement("size", treecluster.Size))),
                    new XElement("TreeNodes",
                        from node in _nodes
                        select new XElement("TreeNode",
                            new XElement("key", node.Key),
                            new XElement("clusterkey", node.ClusterKey),
                            new XElement("label", node.Label),
                            new XElement("min",
                                new XAttribute("x", node.MIN.X),
                                new XAttribute("y", node.MIN.Y),
                                new XAttribute("z", node.MIN.Z)),
                            new XElement("max",
                                new XAttribute("x", node.MAX.X),
                                new XAttribute("y", node.MAX.Y),
                                new XAttribute("z", node.MAX.Z))))));
            doc.Save(fileName);
        }

        private static PlotVizDataFile LoadFromXml(string fileName)
        {
            var doc = XDocument.Load(fileName);

            var clusters = from clusterElement in doc.Descendants("cluster")
                           select new PlotVizDataFileCluster()
                           {
                               Key = (int)clusterElement.Element("key"),
                               Label = clusterElement.Element("label").Value,
                               Color = new PlotVizDataFileColor(
                                   (int)clusterElement.Element("color").Attribute("r"),
                                   (int)clusterElement.Element("color").Attribute("g"),
                                   (int)clusterElement.Element("color").Attribute("b"),
                                   (int)clusterElement.Element("color").Attribute("a")),
                               Size = (float)clusterElement.Element("size")
                           };

            var points = (from pointElement in doc.Descendants("point")
                          select new PlotVizDataFilePoint()
                          {
                              Key = (int)pointElement.Element("key"),
                              ClusterKey = (int)pointElement.Element("clusterkey"),
                              Label = pointElement.Element("label").Value,
                              Coordinate = new PlotVizDataFileCoordinate(
                                  (float)pointElement.Element("location").Attribute("x"),
                                  (float)pointElement.Element("location").Attribute("y"),
                                  (float)pointElement.Element("location").Attribute("z")),
                              MetaDataElements = pointElement.Elements("metadata")
                          });

            var edges = (from pointElement in doc.Descendants("edge")
                         select new PlotVizDataFileEdge()
                         {
                             Key = (int)pointElement.Element("key"),
                             VertexElements = pointElement.Element("vertices").Elements("vertex")
                         });

            var treeclusters = from treeclusterElement in doc.Descendants("TreeCluster")
                               select new PlotVizDataFileTreeCluster()
                               {
                                   Key = (int)treeclusterElement.Element("key"),
                                   Label = treeclusterElement.Element("label").Value,
                                   Color = new PlotVizDataFileColor(
                                       (int)treeclusterElement.Element("color").Attribute("r"),
                                       (int)treeclusterElement.Element("color").Attribute("g"),
                                       (int)treeclusterElement.Element("color").Attribute("b"),
                                       (int)treeclusterElement.Element("color").Attribute("a")),
                                   Size = (float)treeclusterElement.Element("size")
                               };

            var nodes = (from nodeElement in doc.Descendants("TreeNode")
                         select new PlotVizDataFileTreeNode()
                         {
                             Key = (int)nodeElement.Element("key"),
                             ClusterKey = (int)nodeElement.Element("clusterkey"),
                             Label = nodeElement.Element("label").Value,
                             MIN = new PlotVizDataFileCoordinate(
                                 (float)nodeElement.Element("min").Attribute("x"),
                                 (float)nodeElement.Element("min").Attribute("y"),
                                 (float)nodeElement.Element("min").Attribute("z")),
                             MAX = new PlotVizDataFileCoordinate(
                                 (float)nodeElement.Element("max").Attribute("x"),
                                 (float)nodeElement.Element("max").Attribute("y"),
                                 (float)nodeElement.Element("max").Attribute("z"))
                         });

            return new PlotVizDataFile(clusters, points, edges, nodes, treeclusters);
        }

        public static PlotVizDataFile Build(string coordinatesFile)
        {
            Dictionary<int, PlotVizDataFileCoordinate> coordinatesMap = BuildCoordianteMap(coordinatesFile);

            Dictionary<int, string> indexMap = new Dictionary<int, string>(coordinatesMap.Count);
            Dictionary<int, ClusterLine> clusterMap = new Dictionary<int, ClusterLine>(coordinatesMap.Count);
            for (int i = 0; i < indexMap.Count; i++)
            {
                indexMap.Add(i, string.Format("Sequence{0}", i));
                clusterMap.Add(i, new ClusterLine(1));
            }

            return Build(indexMap, clusterMap, coordinatesMap);
        }

        public static PlotVizDataFile Build(string indexFile, string coordinatesFile)
        {
            Dictionary<int, string> indexMap = BuildIndexMap(indexFile);
            Dictionary<int, PlotVizDataFileCoordinate> coordinatesMap = BuildCoordianteMap(coordinatesFile);
            Dictionary<int, ClusterLine> clusterMap = new Dictionary<int, ClusterLine>(indexMap.Count);
            for (int i = 0; i < indexMap.Count; i++)
            {
                clusterMap.Add(i, new ClusterLine(1));
            }

            return Build(indexMap, clusterMap, coordinatesMap);
        }

        public static PlotVizDataFile Build(string indexFile, string clusterFile, string coordinatesFile)
        {
            var indexMap = BuildIndexMap(indexFile);
            var clusterMap = BuildClusterMap(clusterFile);
            var coordinatesMap = BuildCoordianteMap(coordinatesFile);

            return Build(indexMap, clusterMap, coordinatesMap);
        }

        private static PlotVizDataFile Build(Dictionary<int, string> indexMap,
            Dictionary<int, ClusterLine> clusterMap, 
            Dictionary<int, PlotVizDataFileCoordinate> coordinatesMap)
        {
            PlotVizDataFile dataFile = new PlotVizDataFile();

            foreach (var item in clusterMap.Values)
            {
                PlotVizDataFileCluster cluster = new PlotVizDataFileCluster();
                cluster.Key = item.ClusterKey;
                cluster.Label = item.ClusterName;

                if (dataFile.Clusters.Contains(item.ClusterKey) == false)
                {
                    dataFile.Clusters.Add(cluster);
                }
            }

            foreach (var item in coordinatesMap.Keys)
            {
                PlotVizDataFilePoint point = new PlotVizDataFilePoint();
                point.Key = item;
                point.Coordinate = coordinatesMap[item];
                point.ClusterKey = clusterMap[item].ClusterKey;
                if (item < indexMap.Count)
                {
                    point.Label = indexMap[item];
                }

                dataFile.Points.Add(point);
            }

            return dataFile;
 
        }

        private static Dictionary<int, string> BuildIndexMap(string fileName)
        {
            // index, name
            Dictionary<int, string> indexMap = new Dictionary<int, string>();

            using (StreamReader reader = File.OpenText(fileName))
            {
                char[] sep = new char[] { ' ', '\t' };

                string line;
                string[] fields;

                while (reader.EndOfStream == false)
                {
                    line = reader.ReadLine().Trim();

                    if (string.IsNullOrEmpty(line) == false)
                    {
                        fields = line.Split(sep);
                        indexMap.Add(Convert.ToInt32(fields[0]), fields[1]);
                    }
                }
            }



            return indexMap;
        }

        private static Dictionary<int, ClusterLine> BuildClusterMap(string fileName)
        {
            // index, cluster
            Dictionary<int, ClusterLine> clusterMap = new Dictionary<int, ClusterLine>();
            using (StreamReader reader = File.OpenText(fileName))
            {
                while (reader.EndOfStream == false)
                {
                    string line = reader.ReadLine().Trim();

                    if (!string.IsNullOrEmpty(line))
                    {

                        string[] fields = Regex.Split(line, " |,|\t");

                        if (fields.Length < 3)
                        {
                            clusterMap.Add(Convert.ToInt32(fields[0]), new ClusterLine(Convert.ToInt32(fields[1])));
                        }
                        else
                        {
                            clusterMap.Add(Convert.ToInt32(fields[0]), new ClusterLine(Convert.ToInt32(fields[1]), fields[2]));
                        }


                    }
                }
            }

            return clusterMap;
        }

        private static Dictionary<int, PlotVizDataFileCoordinate> BuildCoordianteMap(string fileName)
        {
            // index, coordiate
            Dictionary<int, PlotVizDataFileCoordinate> coordianteMap = new Dictionary<int, PlotVizDataFileCoordinate>();

            char[] sep = new char[] { ' ', '\t' };

            using (StreamReader reader = File.OpenText(fileName))
            {
                int count = 0;
                while (reader.EndOfStream == false)
                {
                    string line = reader.ReadLine().Trim();

                    if (string.IsNullOrEmpty(line) == false)
                    {
                        string[] fields = line.Split(sep);

                        PlotVizDataFileCoordinate coords = new PlotVizDataFileCoordinate(Convert.ToSingle(fields[1]), Convert.ToSingle(fields[2]), Convert.ToSingle(fields[3]));
                        coordianteMap.Add(count++, coords);
                    }
                }
            }

            return coordianteMap;
        }

        private class ClusterLine
        {
            public ClusterLine(int clusterKey)
            {
                ClusterKey = clusterKey;
                ClusterName = string.Empty;
            }
            public ClusterLine(int clusterKey, string clusterName)
            {
                ClusterKey = clusterKey;
                ClusterName = clusterName;
            }
            public int ClusterKey;
            public string ClusterName;
        }

    }

    public class PlotVizDataFileCluster
    {
        private int _key = -1;
        private string _label = string.Empty;
        private PlotVizDataFileColor _color;
        private float _size = 1.0f;

        internal PlotVizDataFileCluster()
        {

        }

        public PlotVizDataFileCluster(int key)
        {
            _key = key;
        }

        public int Key
        {
            get
            {
                return _key;
            }
            internal set
            {
                _key = value;
            }
        }

        public string Label
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
            }
        }

        public PlotVizDataFileColor Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }

        public float Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
            }
        }
    }

    public class PlotVizDataFileClusterCollection : KeyedCollection<int, PlotVizDataFileCluster>
    {
        public PlotVizDataFileClusterCollection()
        {
        }

        public PlotVizDataFileClusterCollection(IEnumerable<PlotVizDataFileCluster> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        protected override int GetKeyForItem(PlotVizDataFileCluster item)
        {
            return item.Key;
        }
    }

    public class PlotVizDataFileTreeCluster
    {
        private int _key = -1;
        private string _label = string.Empty;
        private PlotVizDataFileColor _color;
        private float _size = 1.0f;

        internal PlotVizDataFileTreeCluster()
        {

        }

        public PlotVizDataFileTreeCluster(int key)
        {
            _key = key;
        }

        public int Key
        {
            get
            {
                return _key;
            }
            internal set
            {
                _key = value;
            }
        }

        public string Label
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
            }
        }

        public PlotVizDataFileColor Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }

        public float Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
            }
        }
    }

    public class PlotVizDataFileTreeClusterCollection : KeyedCollection<int, PlotVizDataFileTreeCluster>
    {
        public PlotVizDataFileTreeClusterCollection()
        {
        }

        public PlotVizDataFileTreeClusterCollection(IEnumerable<PlotVizDataFileTreeCluster> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        protected override int GetKeyForItem(PlotVizDataFileTreeCluster item)
        {
            return item.Key;
        }
    }

    public class PlotVizDataFilePoint
    {
        private int _key = -1;
        private int _clusterKey = -1;
        private string _label = string.Empty;
        private PlotVizDataFileCoordinate _coordinate;
        private PlotVizDataFileColor _color;
        private float _size = 1.0f;
        private Dictionary<string, string> _metaData = new Dictionary<string, string>();
        private IEnumerable<XElement> _metaDataElements;

        internal PlotVizDataFilePoint()
        {
        }

        public PlotVizDataFilePoint(int key)
        {
            _key = key;
        }

        public int Key
        {
            get
            {
                return _key;
            }
            internal set
            {
                _key = value;
            }
        }

        public int ClusterKey
        {
            get
            {
                return _clusterKey;
            }
            set
            {
                _clusterKey = value;
            }
        }

        public string Label
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
            }
        }

        public PlotVizDataFileColor Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }

        public float Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
            }
        }

        public PlotVizDataFileCoordinate Coordinate
        {
            get
            {
                return _coordinate;
            }
            set
            {
                _coordinate = value;
            }
        }

        public IEnumerable<XElement> MetaDataElements
        {
            get
            {
                return _metaDataElements;
            }
            set
            {
                foreach (var element in value)
                {
                    _metaData.Add(element.Attribute("name").Value, element.Value);
                }
                _metaDataElements = value;
            }
        }

        public Dictionary<string, string> MetaData
        {
            get
            {
                return _metaData;
            }
            set
            {
                _metaData = value;
            }
        }
    }

    public class PlotVizDataFilePointCollection : KeyedCollection<int, PlotVizDataFilePoint>
    {
        public PlotVizDataFilePointCollection()
        {
        }

        public PlotVizDataFilePointCollection(IEnumerable<PlotVizDataFilePoint> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        protected override int GetKeyForItem(PlotVizDataFilePoint item)
        {
            return item.Key;
        }
    }

    public class PlotVizDataFileTreeNode
    {
        private int _key = -1;
        private int _clusterKey = -1;
        private string _label = string.Empty;
        private PlotVizDataFileCoordinate _min;
        private PlotVizDataFileCoordinate _max;
        private PlotVizDataFileColor _color;
        private float _size = 1.0f;
        private List<string> _metaData;

        internal PlotVizDataFileTreeNode()
        {
        }

        public PlotVizDataFileTreeNode(int key)
        {
            _key = key;
        }

        public int Key
        {
            get
            {
                return _key;
            }
            internal set
            {
                _key = value;
            }
        }

        public int ClusterKey
        {
            get
            {
                return _clusterKey;
            }
            set
            {
                _clusterKey = value;
            }
        }

        public string Label
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
            }
        }
        public PlotVizDataFileColor Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }

        public float Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
            }
        }

        public PlotVizDataFileCoordinate MIN
        {
            get
            {
                return _min;
            }
            set
            {
                _min = value;
            }
        }

        public PlotVizDataFileCoordinate MAX
        {
            get
            {
                return _max;
            }
            set
            {
                _max = value;
            }
        }
    }

    public class PlotVizDataFileTreeNodeCollection : KeyedCollection<int, PlotVizDataFileTreeNode>
    {
        public PlotVizDataFileTreeNodeCollection()
        {
        }

        public PlotVizDataFileTreeNodeCollection(IEnumerable<PlotVizDataFileTreeNode> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        protected override int GetKeyForItem(PlotVizDataFileTreeNode item)
        {
            return item.Key;
        }
    }

    public struct PlotVizDataFileCoordinate
    {
        public float X;
        public float Y;
        public float Z;

        public PlotVizDataFileCoordinate(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    public struct PlotVizDataFileColor
    {
        public int R;
        public int G;
        public int B;
        public int A;

        public PlotVizDataFileColor(int r, int g, int b, int a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
    }

    public struct PlotVizDataFileMetaData
    {
        public string Name;
        public string Value;

        public PlotVizDataFileMetaData(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }

    public class PlotVizDataFileMetaDataCollection : Hashtable
    {
        public PlotVizDataFileMetaDataCollection(IEnumerable<PlotVizDataFileMetaData> items)
        {
            foreach (var item in items)
            {
                Add(item.Name, item.Value);
            }
        }
    }

    public class PlotVizDataFileEdge
    {
        private int _key = -1;
        private List<string> _vertices = new List<string>();
        private IEnumerable<XElement> _vertexElements;

        public int Key
        {
            get
            {
                return _key;
            }
            internal set
            {
                _key = value;
            }
        }

        public IEnumerable<XElement> VertexElements
        {
            get
            {
                return _vertexElements;
            }
            set
            {
                foreach (var element in value)
                {
                    _vertices.Add(element.Attribute("key").Value);
                }
                _vertexElements = value;
            }
        }

        public List<string> Vertices
        {
            get
            {
                return _vertices;
            }
        }
    }

    public class PlotVizDataFileEdgeCollection : KeyedCollection<int, PlotVizDataFileEdge>
    {
        public PlotVizDataFileEdgeCollection()
        {
        }

        public PlotVizDataFileEdgeCollection(IEnumerable<PlotVizDataFileEdge> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        protected override int GetKeyForItem(PlotVizDataFileEdge item)
        {
            return item.Key;
        }
    }
}
