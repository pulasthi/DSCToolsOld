using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PvizBuilderSimple
{
    public partial class MainFrm : Form
    {
        private List<Color> _matlab50Colors;
        public MainFrm()
        {
            InitializeComponent();
        }

        private void BrowseFile(TextBox textBox)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    textBox.Text = dlg.FileName;
                }
            }
        }

        private void ptsBrwsBtn_Click(object sender, EventArgs e)
        {
            BrowseFile(ptsTxt);
        }

        private void clusterBrwsBtn_Click(object sender, EventArgs e)
        {
            BrowseFile(clusterTxt);
        }

        private void ptsTxt_TextChanged(object sender, EventArgs e)
        {
            if (autoCheckBx.Checked && File.Exists(ptsTxt.Text))
            {
                nameTxt.Text = Path.GetFileNameWithoutExtension(ptsTxt.Text);
            }
        }

        private void buildBtn_Click(object sender, EventArgs e)
        {
            SaveAndShow(true);
        }

        private void SaveAndShow(bool show)
        {
            if (ValidParameters())
            {
                XElement plotviz = new XElement("plotviz");
                XElement plot = CreatePlotElement(nameTxt.Text);
                XElement clusters = new XElement("clusters");
                XElement points = new XElement("points");

                plotviz.Add(plot);
                plotviz.Add(clusters);
                plotviz.Add(points);

                using (StreamReader clusReader = new StreamReader(clusterTxt.Text),
                                    ptsReader = new StreamReader(ptsTxt.Text))
                {
                    string pointsTxtFile = Path.Combine(Path.GetDirectoryName(ptsTxt.Text),
                                                        nameTxt.Text + "plot_as_text.txt");
                    using (StreamWriter pointsTxtWriter = new StreamWriter(pointsTxtFile))
                    {
                        Hashtable clustersTable = new Hashtable();
                        char[] sep = new[] { ' ', '\t' };
                        string[] splits;
                        string label;
                        int pnum;
                        while (!clusReader.EndOfStream)
                        {
                            splits = clusReader.ReadLine().Trim().Split(sep);
                            label = splits[1];
                            if (!clustersTable.ContainsKey(label))
                            {
                                clustersTable.Add(label, int.Parse(label));
                                clusters.Add(CreateClusterElement(int.Parse(label), label,
                                                                  _matlab50Colors[int.Parse(label)%_matlab50Colors.Count], false));
                            }
                            splits = ptsReader.ReadLine().Trim().Split(sep);
                            pnum = int.Parse(splits[0]);
                            points.Add(CreatePointElement(pnum, ((int) clustersTable[label]), splits[0],
                                                          double.Parse(splits[1]), double.Parse(splits[2]),
                                                          double.Parse(splits[3])));
                            pointsTxtWriter.WriteLine(pnum + "\t" + splits[1] + "\t" + splits[2] + "\t" + splits[3] +
                                                      "\t" + clustersTable[label]);
                        }
                    }
                }

                string pvizFile = Path.Combine(Path.GetDirectoryName(ptsTxt.Text), nameTxt.Text + (autoCheckBx.Checked ? "_plot.pviz" : ".pviz"));
                plotviz.Save(pvizFile);
                if (show)
                {
                    ShowInPviz(new []{pvizFile});
                }
            }
            else
            {
                MessageBox.Show(@"Please check if either points file exist or cluster file exist or name is not empty");

            }
        }

        private XElement CreatePlotElement(string name)
        {
            XElement plot =
                new XElement("plot",
                             new XElement("title", name),
                             new XElement("pointsize", 1),
                             new XElement("glyph",
                                          new XElement("visible", 0),
                                          new XElement("scale", 1)));
            return plot;
        }

        private XElement CreateClusterElement(int key, string label, Color color, bool isDefault)
        {
            XElement cluster =
                new XElement("cluster",
                             new XElement("key", key),
                             new XElement("label", label),
                             new XElement("visible", 1),
                             new XElement("default", isDefault ? 1 : 0),
                             new XElement("color",
                                          new XAttribute("r", color.R),
                                          new XAttribute("g", color.G),
                                          new XAttribute("b", color.B),
                                          new XAttribute("a", color.A)),
                             new XElement("size", 1));
            return cluster;
        }

        private XElement CreatePointElement(int key, int clusterKey, string label, double x, double y, double z)
        {
            XElement point =
                new XElement("point",
                             new XElement("key", key),
                             new XElement("clusterkey", clusterKey),
                             new XElement("label", label),
                             new XElement("location",
                                          new XAttribute("x", x),
                                          new XAttribute("y", y),
                                          new XAttribute("z", z)));
            return point;
        }

        private bool ValidParameters()
        {
            if (!File.Exists(ptsTxt.Text) || !File.Exists(clusterTxt.Text) || string.IsNullOrEmpty(nameTxt.Text))
            {
                return false;
            }
            return true;
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            plotDoneLabel.Visible = false;
            if (File.Exists("Matlab50.txt"))
            {
                using (StreamReader reader = new StreamReader("Matlab50.txt"))
                {
                    _matlab50Colors = new List<Color>();
                    char[] sep = new[] { ' ', '\t' };
                    string[] splits;
                    string split;
                    int startIdx = 3, endIdx;
                    int r, g, b, a;
                    while (!reader.EndOfStream)
                    {
                        splits = reader.ReadLine().Trim().Split(sep);

                        split = splits[0];
                        r = int.Parse(split.Substring(startIdx, (split.Length - (startIdx + 1))));

                        split = splits[1];
                        g = int.Parse(split.Substring(startIdx, (split.Length - (startIdx + 1))));

                        split = splits[2];
                        b = int.Parse(split.Substring(startIdx, (split.Length - (startIdx + 1))));

                        split = splits[3];
                        a = int.Parse(split.Substring(startIdx, (split.Length - (startIdx + 1))));

                        _matlab50Colors.Add(Color.FromArgb(a, r, g, b));
                    }
                }
            }
        }

        private void ShowInPviz(string[] plotFiles)
        {
            string pvizExePath = @"C:\Program Files (x86)\PVIZ3\bin\pviz3.exe";
            var start = new ProcessStartInfo(pvizExePath)
            {
                UseShellExecute = true,
                RedirectStandardOutput = false,
                CreateNoWindow = false,
                Arguments = string.Join(" ", plotFiles)
            };
            Process.Start(start);
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            SaveAndShow(false);
            plotDoneLabel.Visible = true;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            plotDoneLabel.Visible = false;
            timer1.Stop();
        }


    }
}
