using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.pviz;

namespace ExtractPvizClusterNames
{
    class Program
    {
        static void Main(string[] args)
        {
            string pvizFile = args[0];

            if (args.Length == 1)
            {
                ExtractPvizClusterNames(pvizFile);
                
            }

            if (args.Length == 2)
            {
                string dir = args[0];
                string nexml = args[1];
                GenDendroscopeCommandFile(args[0], nexml);
            }

            if (args.Length > 2)
            {
                string file = args[1];
                bool removeExistingPrefix = bool.Parse(args[2]);
                bool isTree = bool.Parse(args[3]);
                if (isTree)
                {
                    ModifyTreeNeXml(pvizFile, file, removeExistingPrefix);
                }
                else
                {
                    ModifyMSA(pvizFile, file, removeExistingPrefix);
                }
            }

            Console.WriteLine("Done.");
            Console.Read();
        }

        private static void ModifyMSA(string pvizFile, string msaFile, bool removeExistingPrefix)
        {
            string dir = Path.GetDirectoryName(msaFile) ?? string.Empty;
            string prefixedMSAFile = Path.Combine(dir, Path.GetFileNameWithoutExtension(msaFile) + "_prefixed.fasta");

            PvizModel pviz = PvizModel.LoadPviz(pvizFile);
            IList<Cluster> clusters = pviz.Clusters;
            Hashtable clusterNamesToPrefix = new Hashtable();

            foreach (var cluster in clusters)
            {
                string label = cluster.Label;

                int idx = label.IndexOf('_');
                if (idx > -1)
                {
                    idx = label.IndexOf("_", idx + 1, StringComparison.Ordinal);
                    string prefix = label.Substring(0, idx);
                    string name = label.Substring(idx + 1);
                    if ("InternalNodes".Equals(name, StringComparison.OrdinalIgnoreCase)
                        || "ConnectorNodes".Equals(name, StringComparison.OrdinalIgnoreCase)
                        || !(prefix.StartsWith("C") || prefix.StartsWith("UC"))) continue;
                    clusterNamesToPrefix.Add(name, prefix);
                }
            }

            using (StreamWriter writer = new StreamWriter(prefixedMSAFile))
            {
                using (StreamReader reader = new StreamReader(msaFile))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line))
                        {
                            if (line.StartsWith(">"))
                            {
                                string label = line.Substring(1);
                                if (removeExistingPrefix)
                                {
                                    int idx = label.IndexOf('_');
                                    idx = label.IndexOf('_', idx + 1);
                                    label = label.Substring(idx + 1);
                                }
                                double d;
                                if (!double.TryParse(label, out d))
                                {
                                    string name = (label.StartsWith("fox") || label.StartsWith("fungi"))
                                                      ? label.Substring(0,
                                                                        label.LastIndexOf("_",
                                                                                          StringComparison.Ordinal))
                                                      : label;


                                    if (clusterNamesToPrefix.Contains(name))
                                    {
                                        line = ">" + clusterNamesToPrefix[name] + "_" + label;
                                    }
                                    else
                                    {
                                        if (label.StartsWith("kruger"))
                                        {
                                            int idx = label.LastIndexOf("sp_", StringComparison.Ordinal);
                                            idx = idx < 0 ? label.LastIndexOf("cf_", StringComparison.Ordinal) : idx;
                                            name = label.Substring(0, idx + 2);

                                            if (clusterNamesToPrefix.Contains(name))
                                            {
                                                line = ">" + clusterNamesToPrefix[name] + "_" + label;
                                            }
                                            else
                                            {
                                                // Should not happen
                                                Console.WriteLine("Bad Kruger label " + label);
                                            }
                                        }
                                        else
                                        {
                                            // Should not happen
                                            Console.WriteLine("Bad label " + label);
                                        }
                                    }
                                }
                            }
                            writer.WriteLine(line);
                        }
                    }
                }
            }
        }

        private static void GenDendroscopeCommandFile(string dir, string nexml)
        {
            string[] clusterFiles = Directory.GetFiles(dir, "*.txt");
            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "dendrocmd.txt")))
            {
                writer.WriteLine(@"open file=" + nexml + ";select all;set edgewidth=2;set color=0 0 0;set labelcolor=0 0 0;deselect all;");
                
                foreach (var clusterFile in clusterFiles)
                {
                    string fname = Path.GetFileNameWithoutExtension(clusterFile);
                    if (!string.IsNullOrEmpty(fname) && (fname.StartsWith("C") || fname.StartsWith("UC")))
                    {
                        string rgb = GetRgb(fname);
                        using (StreamReader reader = new StreamReader(clusterFile))
                        {
                            while (!reader.EndOfStream)
                            {
                                string line = reader.ReadLine();
                                if (!string.IsNullOrEmpty(line))
                                {
                                    writer.Write(@"find searchtext=" + line + ";");
                                }
                            }
                            writer.WriteLine();
                            if (fname.StartsWith("C") && !"255 255 255".Equals(rgb))
                            {
                                writer.Write(@"select LSA induced network;");
                            }

                            if (!"255 255 255".Equals(rgb))
                            {
                                writer.Write(@"set color=" + rgb + ";set labelcolor=" + rgb +";" );
                            }
                            else
                            {
                                writer.Write(@"set color=0 0 0;set labelcolor=255 255 255;set labelfillcolor=0 0 0;");
                            }
                            writer.WriteLine("deselect all;");
                        }
                    }
                }
            }
        }

        private static string GetRgb(string fname)
        {
            int start = fname.IndexOf('_') ;
            return fname.Substring(start + 1, fname.LastIndexOf('_') - (start+1)).Replace('.',' ');
        }

        private static void ExtractPvizClusterNames(string pvizFile)
        {
            string outDirectory = Path.GetDirectoryName(pvizFile);

            PvizModel pviz = PvizModel.LoadPviz(pvizFile);
            IList<Cluster> clusters = pviz.Clusters;
            foreach (var cluster in clusters)
            {
                string label = cluster.Label;
                int idx = label.IndexOf('_');
                if (idx > -1)
                {
                    string prefix = label.Substring(0, idx);
                    if (prefix.StartsWith("C") || prefix.StartsWith("UC"))
                    {
                        using (
                            StreamWriter writer =
                                new StreamWriter(
                                    Path.Combine(outDirectory,
                                                 prefix + "_" + cluster.Color.ToStringWithoutAlpha() + "_" +
                                                 cluster.Color.ToColorCode() + ".txt"), true))
                        {
                            // These two are just to get phy tree pviz cluster labels to work
                            writer.WriteLine(cluster.Label.Substring(cluster.Label.IndexOf('_', ++idx) + 1));
                            //                    writer.WriteLine(cluster.Label);
                        }
                    }
                }
            }
        }

        static void ModifyTreeNeXml(string pvizFile, string treeFile, bool removeExistingPrefix)
        {
            string tf = Path.Combine(Path.GetDirectoryName(treeFile),
                                     Path.GetFileNameWithoutExtension(treeFile) + "_modified.nexml");

            PvizModel pviz = PvizModel.LoadPviz(pvizFile);
            IList<Cluster> clusters = pviz.Clusters;
            Hashtable clusterNamesToPrefix = new Hashtable();

            foreach (var cluster in clusters)
            {
                string label = cluster.Label;

                int idx = label.IndexOf('_');
                if (idx > -1)
                {
                    idx = label.IndexOf("_", idx + 1, StringComparison.Ordinal);
                    string prefix = label.Substring(0, idx);
                    string name = label.Substring(idx + 1);
                    if ("InternalNodes".Equals(name, StringComparison.OrdinalIgnoreCase) 
                        || "ConnectorNodes".Equals(name, StringComparison.OrdinalIgnoreCase)
                        || !(prefix.StartsWith("C") || prefix.StartsWith("UC"))) continue;
                    clusterNamesToPrefix.Add(name, prefix);
                }
            }

            using (StreamWriter writer = new StreamWriter(tf))
            {
                using (StreamReader reader = new StreamReader(treeFile))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line))
                        {
                            if (line.Contains("<node"))
                            {
                                int idx = line.IndexOf("label=", StringComparison.Ordinal);
                                if (idx > -1)
                                {
                                    int start = line.IndexOf("\"", idx, StringComparison.Ordinal);
                                    int end = line.IndexOf("\"", start + 1, StringComparison.Ordinal);
                                    
                                    string head = line.Substring(0, start + 1);
                                    string tail = line.Substring(end);

                                    string label = line.Substring(start + 1, end - (start + 1));
                                    if (removeExistingPrefix)
                                    {
                                        idx = label.IndexOf('_');
                                        idx = label.IndexOf('_', idx + 1);
                                        label = label.Substring(idx + 1);
                                    }
                                    double d;
                                    if (!double.TryParse(label, out d))
                                    {


                                        string name = (label.StartsWith("fox") || label.StartsWith("fungi"))
                                                          ? label.Substring(0,
                                                                            label.LastIndexOf("_",
                                                                                              StringComparison.Ordinal))
                                                          : label;



                                        if (clusterNamesToPrefix.Contains(name))
                                        {
                                            line = head + clusterNamesToPrefix[name] + "_" + label + tail;
                                        }
                                        else
                                        {
                                            if (label.StartsWith("kruger"))
                                            {
                                                idx = label.LastIndexOf("sp_", System.StringComparison.Ordinal);
                                                idx = idx < 0 ? label.LastIndexOf("cf_", StringComparison.Ordinal) : idx;
                                                name = label.Substring(0, idx + 2);

                                                if (clusterNamesToPrefix.Contains(name))
                                                {
                                                    line = head + clusterNamesToPrefix[name] + "_" + label + tail;
                                                }
                                                else
                                                {
                                                    // Should not happen
                                                    Console.WriteLine("Bad Kruger label " + label);
                                                }
                                            }
                                            else
                                            {
                                                // Should not happen
                                                Console.WriteLine("Bad label " + label);
                                            }


                                        }
                                    }
                                }
                            }
                            writer.WriteLine(line);
                        }
                    }
                }
            }

        }

        private static void PvizTestCode()
        {
            Test t = Test.Who;
            Console.WriteLine((int) t);

            PvizModel pviz = new PvizModel()
                                 {
                                     Plot = new Plot()
                                                {
                                                    Title = "TestPlot",
                                                    PointSize = 1,
                                                    Glyph = new Glyph(1, 1.0f),
                                                    Camera = new Camera(FocusModes.Custom, new Focus(0.0f, 1.0f, 0.0f))
                                                },
                                     Clusters = new List<Cluster>(new[]
                                                                      {
                                                                          new Cluster()
                                                                              {
                                                                                  Key = 1,
                                                                                  Label = "clusterOne",
                                                                                  Visible = 1,
                                                                                  Default = 0,
                                                                                  Color =
                                                                                      new Color("255", "255", "0", "255"),
                                                                                  Shape = 3,
                                                                                  Size = 1
                                                                              }
                                                                      }),
                                     Points = new List<Point>(new[]
                                                                  {
                                                                      new Point()
                                                                          {
                                                                              Key = 1,
                                                                              ClusterKey = 0,
                                                                              Label = "PointOne",
                                                                              Location = new Location(0.0, 1.0, 0.5)
                                                                          },
                                                                      new Point()
                                                                          {
                                                                              Key = 2,
                                                                              ClusterKey = 0,
                                                                              Label = "PointTwo",
                                                                              Location = new Location(1.0, 1.0, 0.5)
                                                                          }
                                                                  })
                                 };
            pviz.SaveAs(@"C:\users\sekanaya\Desktop\test.pviz");
        }

        enum Test
        {
            Who=2,
            Wha=1
        }
    }
}
