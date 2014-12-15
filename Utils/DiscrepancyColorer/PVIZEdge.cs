using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PhylogeneticTreeToPlotViz
{
    public class PVIZEdge
    {
        private int _key;
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
}
