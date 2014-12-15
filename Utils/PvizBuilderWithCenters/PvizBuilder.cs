using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace PvizBuilderWithCenters
{
    public class PvizBuilder
    {
        private readonly string _centerFile;
        private readonly string _pointsFile;

        private readonly Hashtable _centerInfoTable;
        private readonly Hashtable _cnumTopointsTable;
        private int _maxpnum = -1;
        private readonly int _maxcnum = -1;

        public PvizBuilder(string centerFile, string pointsFile)
        {
            _centerFile = centerFile;
            _pointsFile = pointsFile;
            _centerInfoTable = new Hashtable();
            _cnumTopointsTable = new Hashtable();
            ProcessPointsFile(ref _maxpnum, ref _maxcnum);
            ProcessCenterFile();
        }

        public void BuildOverallBestTopN(int n)
        {
            XElement clustersElement = new XElement("clusters");
            XElement pointsElement = new XElement("points");

            int current = 0;
            for (int i = 0; i < _maxcnum; ++i)
            {
                int temp = current;
                List<CenterInfo> cis = ((List<CenterInfo>) ((Hashtable) _centerInfoTable[i])["OverallBest"]);
                Hashtable requiredCenterPoints = new Hashtable();
                Hashtable sourceTable = new Hashtable();
                for (int j = 0; j < n; ++j)
                {
                    CenterInfo ci = cis[j];
                    if (!sourceTable.Contains(ci.Source))
                    {
                        sourceTable.Add(ci.Source, new Cluster("clus." + i + "." + ci.Source + ".center"));
                    }
// ReSharper disable AssignNullToNotNullAttribute
                    if (!requiredCenterPoints.ContainsKey(ci.Pnum)){
                        requiredCenterPoints.Add(ci.Pnum, null);
                    }
// ReSharper restore AssignNullToNotNullAttribute
                }

                
                List<Point> points = (List<Point>) _cnumTopointsTable[i];
                foreach (Point point in points)
                {
                    point.Cluster = temp;
                    point.Label = point.Index.ToString();
                    pointsElement.Add(point.ToPvizPointElement());

                    if (requiredCenterPoints.ContainsKey(point.Index))
                    {
                        Point clone = point.Clone();
                        clone.Index = -1;
                        clone.Cluster = -1;
                        clone.Label = string.Empty;
                        requiredCenterPoints[point.Index] = clone;
                    }
                }

                clustersElement.Add(new Cluster(temp, "clus." + i).ToClusterElement(Matlab50ColorPicker.Pick(i),false, 0.1));

                if (sourceTable.ContainsKey("Seq"))
                {
                    ((Cluster) sourceTable["Seq"]).ClusterNumber = ++current;
                    clustersElement.Add(((Cluster)sourceTable["Seq"]).ToClusterElement(Matlab50ColorPicker.Pick(i),false, 2.0));
                    sourceTable["Seq"] = current;
                }

                if (sourceTable.ContainsKey("Both"))
                {
                    ((Cluster)sourceTable["Both"]).ClusterNumber = ++current;
                    clustersElement.Add(((Cluster)sourceTable["Both"]).ToClusterElement(Matlab50ColorPicker.Pick(i), false, 3.0));
                    sourceTable["Both"] = current;
                }

                if (sourceTable.ContainsKey("CoG"))
                {
                    ((Cluster)sourceTable["CoG"]).ClusterNumber = ++current;
                    clustersElement.Add(((Cluster)sourceTable["CoG"]).ToClusterElement(Matlab50ColorPicker.Pick(i), false, 4.0));
                    sourceTable["CoG"] = current;
                }

                if (sourceTable.ContainsKey("MDS"))
                {
                    ((Cluster)sourceTable["MDS"]).ClusterNumber = ++current;
                    clustersElement.Add(((Cluster)sourceTable["MDS"]).ToClusterElement(Matlab50ColorPicker.Pick(i), false, 5.0));
                    sourceTable["MDS"] = current;
                }

                for (int j = 0; j < n; ++j)
                {
                    CenterInfo ci = cis[j];
                    Point centerPoint = ((Point) requiredCenterPoints[ci.Pnum]).Clone();
                    centerPoint.Index = ++_maxpnum;
                    centerPoint.Cluster = (int) sourceTable[ci.Source];
                    centerPoint.Label = "clus." + i + "." +ci.Source;
                    pointsElement.Add(centerPoint.ToPvizPointElement());
                }

                ++current;
            }


            string dir = Path.GetDirectoryName(_pointsFile);
            string name = Path.GetFileNameWithoutExtension(_pointsFile) + "_overallbest_" + n;
            string plotFile = string.IsNullOrEmpty(dir) ? name : Path.Combine(dir, name + ".pviz");

            XElement plotvizElement = new XElement("plotviz");
            XElement plotElement = CreatePlotElement(name, true);
            plotvizElement.Add(plotElement);
            plotvizElement.Add(clustersElement);
            plotvizElement.Add(pointsElement);
            plotvizElement.Save(plotFile);
        }

        private void ProcessCenterFile()
        {
            using (CenterFileReader reader = new CenterFileReader(_centerFile))
            {
                while (!reader.EndOfStream)
                {
                    CenterInfo ci = reader.ReadCenterLine();
                    AddCenterInfoToTable(ci);
                }
            }
        }

        private void AddCenterInfoToTable(CenterInfo ci)
        {
            if (_centerInfoTable.ContainsKey(ci.Cluster))
            {
                Hashtable methodTable = (Hashtable)_centerInfoTable[ci.Cluster];
                if (methodTable.ContainsKey(ci.Method))
                {
                    // Need a list to maintain the order of points
                    List<CenterInfo> cps = (List<CenterInfo>)methodTable[ci.Method];
                    cps.Add(ci);
                }
                else
                {
                    // Need a list to maintain the order of points
                    List<CenterInfo> cps = new List<CenterInfo> { ci };
                    methodTable[ci.Method] = cps;
                }
            }
            else
            {
                // Need a list to maintain the order of points
                List<CenterInfo> cps = new List<CenterInfo> { ci };
                Hashtable methodTable = new Hashtable();
                methodTable[ci.Method] = cps;
                _centerInfoTable[ci.Cluster] = methodTable;
            }
        }

        private void ProcessPointsFile(ref int maxpnum, ref int maxcnum)
        {
            using (SimplePointsReader reader = new SimplePointsReader(_pointsFile))
            {
                while (!reader.EndOfStream)
                {
                    Point p = reader.ReadPoint();
                    if (maxpnum < p.Index)
                    {
                        maxpnum = p.Index;
                    }

                    if (maxcnum < p.Cluster)
                    {
                        maxcnum = p.Cluster;
                    }
                    if (_cnumTopointsTable.ContainsKey(p.Cluster))
                    {
                        ((List<Point>)_cnumTopointsTable[p.Cluster]).Add(p);
                    }
                    else
                    {
                        _cnumTopointsTable.Add(p.Cluster, new List<Point> {p});
                    }
                }
            }
        }

        private static XElement CreatePlotElement(string name, bool glyphVisible)
        {
            XElement plot =
                new XElement("plot",
                             new XElement("title", name),
                             new XElement("pointsize", 1),
                             new XElement("glyph",
                                          new XElement("visible", glyphVisible ? 1 : 0),
                                          new XElement("scale", 1)));
            return plot;
        }
    }
}
