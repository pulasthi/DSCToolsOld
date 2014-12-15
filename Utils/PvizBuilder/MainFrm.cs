using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PvizBuilder
{
    public partial class MainFrm : Form
    {
        private List<Color> _matlab50Colors;
        public MainFrm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BrowseFile(ptsTxt);
        }

        public static void BrowseFile(TextBox textBox)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    textBox.Text = dlg.FileName;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BrowseFile(clusterTxt);
        }

        private void buildBtn_Click(object sender, EventArgs e)
        {
            if (ValidParameters())
            {
                bool allNonDef = allNonDefChkBx.Checked;
                bool incOnlyNonDef = incOnlyNonDefChkBx.Checked;

                XElement plotviz = new XElement("plotviz");
                XElement plot = CreatePlotElement(nameTxt.Text);
                XElement clusters = new XElement("clusters");
                XElement points = new XElement("points");

                plotviz.Add(plot);
                plotviz.Add(clusters);
                plotviz.Add(points);

                string[] lines = nonDefTxtBx.Lines;
                Hashtable nonDefClusterLabels = new Hashtable(lines.Length);
                for (int i = 0; i < lines.Length; i++)
                {
                    string nonDefClusterLabel = lines[i];
                    nonDefClusterLabel = oneToZeroChkBx.Checked
                                             ? (int.Parse(nonDefClusterLabel) - 1).ToString(CultureInfo.InvariantCulture)
                                             : nonDefClusterLabel;

                    if (!nonDefClusterLabels.Contains(nonDefClusterLabel))
                    {
                        nonDefClusterLabels.Add(nonDefClusterLabel, i);
                    }
                }

                Hashtable deletedPointsTable = new Hashtable();
                if (!string.IsNullOrEmpty(deletedPointsTxt.Text))
                {
                    IEnumerable<int> deletedPoints = deletedPointsTxt.Text.Split(new[] {','}).Select(x => int.Parse(x.Trim()));
                    foreach (int deletedPoint in deletedPoints)
                    {
                        deletedPointsTable.Add(deletedPoint, deletedPoint);
                    }
                }

                Dictionary<int,string> pnumToPointLabelTable = new Dictionary<int, string>();
                if (!string.IsNullOrEmpty(pointsLabelTxt.Text))
                {
                    using (StreamReader reader = new StreamReader(pointsLabelTxt.Text))
                    {
                        var sep = new[] {'\t'};
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            if (string.IsNullOrEmpty(line)) continue;
                            var splits = line.Split(sep);
                            if (splits.Length == 2 || splits.Length == 5 || splits.Length == 3) // 2 for pnum and label style file, 5 for simple points style file, 3 for index style file
                            {
                                pnumToPointLabelTable[int.Parse(splits[0])] = splits.Length==2 ? splits[1] : splits.Length == 5 ? splits[4] : splits[1];
                            }
                        }
                    }
                }

                using (StreamReader clusReader = new StreamReader(clusterTxt.Text),
                    ptsReader = new StreamReader(ptsTxt.Text))
                {
                    string pointsTxtFile = Path.Combine(Path.GetDirectoryName(ptsTxt.Text),
                                                        nameTxt.Text +
                                                        (deletedPointsTable.Count > 0
                                                             ? ("_deleted_" + deletedPointsTable.Count +
                                                                "_points_w_orig_pnum.txt")
                                                             : "_points_w_orig_pnum.txt"));
                    string labelsFile = Path.Combine(Path.GetDirectoryName(ptsTxt.Text), nameTxt.Text + "_labels.txt");

                    StreamWriter pointsTxtWriter=null;
                    StreamWriter labelsWriter = null;
                    if (writeSimplePointsChkBx.Checked)
                    {
                        pointsTxtWriter = new StreamWriter(pointsTxtFile);
                    }

                    if (writeLabelsChkBx.Checked)
                    {
                        labelsWriter = new StreamWriter(labelsFile);
                    }

                    Hashtable clustersTable = new Hashtable();
                    Hashtable clusterColorTable = new Hashtable();
                    char[] sep = new[] {' ', '\t'};
                    var dotsep = new[] {'.'};
                    string[] splits;
                    string label;
                    int count = 0;
                    int pnum;
                    while (!clusReader.EndOfStream)
                    {
                        splits = clusReader.ReadLine().Trim().Split(sep);
                        label = splits.Length == 2 ? splits[1] : splits.Length== 5 ? splits[4] : splits.Length == 3 ? splits[1] : "0";
                        label = oneToZeroChkBx.Checked
                                    ? (int.Parse(label) - 1).ToString(CultureInfo.InvariantCulture)
                                    : label;
                        var color = splits.Length == 3 && Regex.IsMatch(splits[2], @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")
                                        ? splits[2]
                                        : string.Empty;
                        if (!clustersTable.ContainsKey(label))
                        {
                            if (!string.IsNullOrEmpty(color))
                            {
                                clusterColorTable.Add(label,color);
                            }
                            clustersTable.Add(label, 
                                labelsAsClusterNumbersCheckBx.Checked ? int.Parse(label) : count);
                            string outputClusterLabel = string.Empty;
                            int key = -1;
                            if (allNonDef || nonDefClusterLabels.ContainsKey(label))
                            {
                                key = (labelsAsClusterNumbersCheckBx.Checked ? int.Parse(label) : count);
                                outputClusterLabel = clusterPrefixTxt.Text +
                                                            (genNumericLabelsChkBx.Checked
                                                                 ? count.ToString(CultureInfo.InvariantCulture)
                                                                 : (outputLabelsEqualToNumbersChkBx.Checked
                                                                        ? key.ToString(CultureInfo.InvariantCulture)
                                                                        : label));
                                Color c;
                                if (clusterColorTable.Contains(label))
                                {
                                    var colorString = (string) clusterColorTable[label];
                                    var comps = colorString.Split(dotsep);
                                    c = Color.FromArgb(int.Parse(comps[3]), int.Parse(comps[0]),
                                                             int.Parse(comps[1]), int.Parse(comps[2]));

                                }
                                else
                                {
                                    c = _matlab50Colors[
                                        (allNonDef
                                             ? (labelsAsClusterNumbersCheckBx.Checked ? int.Parse(label) : count)
                                             : (int) nonDefClusterLabels[label])%_matlab50Colors.Count
                                        ];
                                }
                                clusters.Add(CreateClusterElement(
                                    key,
                                    outputClusterLabel,
                                    c,
                                    false));
                            }
                            else if (!incOnlyNonDef)
                            {
                                key = (labelsAsClusterNumbersCheckBx.Checked ? int.Parse(label) : count);
                                outputClusterLabel = clusterPrefixTxt.Text + (genNumericLabelsChkBx.Checked
                                                                ? count.ToString(CultureInfo.InvariantCulture)
                                                                : (outputLabelsEqualToNumbersChkBx.Checked
                                                                       ? key.ToString(CultureInfo.InvariantCulture)
                                                                       : label));
                                clusters.Add(CreateClusterElement(
                                    key, outputClusterLabel,
                                    pickColorBtn.BackColor, true));
                            }

                            if (writeLabelsChkBx.Checked)
                            {
                                labelsWriter.WriteLine(key +"\t" +outputClusterLabel);
                            }
                            ++count;
                        }

                        string ptsLine = ptsReader.ReadLine();
                        splits = ptsLine.Trim().Split(sep);
                        pnum = int.Parse(splits[0]);

                        if (!deletedPointsTable.ContainsKey(pnum) && (!incOnlyNonDef || allNonDef || nonDefClusterLabels.ContainsKey(label)))
                        {
                            points.Add(CreatePointElement(pnum, ((int) clustersTable[label]), pnumToPointLabelTable.ContainsKey(pnum) ? pnumToPointLabelTable[pnum] : splits[0],
                                                            double.Parse(splits[1]), double.Parse(splits[2]),
                                                            double.Parse(splits[3])));
                            if (pointsTxtWriter != null)
                            {
                                pointsTxtWriter.WriteLine(pnum + "\t" + splits[1] + "\t" + splits[2] + "\t" +
                                                            splits[3] +
                                                            "\t" + clustersTable[label]);
                            }
                        }
                    }
                    if (pointsTxtWriter != null)
                    {
                        pointsTxtWriter.Flush();
                        pointsTxtWriter.Close();
                        pointsTxtWriter.Dispose();
                    }

                    if (labelsWriter != null)
                    {
                        labelsWriter.Flush();
                        labelsWriter.Close();
                        labelsWriter.Dispose();
                    }
                }
                
                
                string pvizFile = Path.Combine(Path.GetDirectoryName(ptsTxt.Text), nameTxt.Text + ".pviz");
                plotviz.Save(pvizFile);
                MessageBox.Show(@"Done.");
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
                             new XElement("size", 1),
                             new XElement("shape", 3));
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
            if (!File.Exists(ptsTxt.Text) || !File.Exists(clusterTxt.Text) || string.IsNullOrEmpty(nameTxt.Text) || (!string.IsNullOrEmpty(pointsLabelTxt.Text) && !File.Exists(pointsLabelTxt.Text)))
            {
                return false;
            }
            return true;
        }

        private void pickColorBtn_Click(object sender, EventArgs e)
        {
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                pickColorBtn.BackColor = colorDlg.Color;
            }
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            pickColorBtn.BackColor = colorDlg.Color;
            groupBox3.Enabled = !allNonDefChkBx.Checked;
            if (File.Exists("Matlab50.txt"))
            {
                using (StreamReader reader = new StreamReader("Matlab50.txt"))
                {
                    _matlab50Colors = new List<Color>();
                    char[] sep = new[] {' ', '\t'};
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

                        _matlab50Colors.Add(Color.FromArgb(a,r,g,b));
                    }
                }
            }

        }

        private void allNonDefChkBx_CheckedChanged(object sender, EventArgs e)
        {
            groupBox3.Enabled = !allNonDefChkBx.Checked;
        }

        private void ptsTxt_DragDrop(object sender, DragEventArgs e)
        {
           HandleDragDrop(e,ptsTxt);
        }

        private void ptsTxt_DragEnter(object sender, DragEventArgs e)
        {
           HandleDragEnter(e);
        }

        private void clusterTxt_DragEnter(object sender, DragEventArgs e)
        {
            HandleDragEnter(e);
        }

        public static void HandleDragDrop(DragEventArgs e, TextBox t)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
                t.Text = fileNames[0];
            }
        }

        public static void HandleDragEnter(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void clusterTxt_DragDrop(object sender, DragEventArgs e)
        {
            HandleDragDrop(e,clusterTxt);
        }

        private void nameTxt_DragEnter(object sender, DragEventArgs e)
        {
            HandleDragEnter(e);
        }

        private void nameTxt_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
                nameTxt.Text = Path.GetFileNameWithoutExtension(fileNames[0]);
            }
        }

        private void labelsAsClusterNumbersCheckBx_CheckedChanged(object sender, EventArgs e)
        {
            outputLabelsEqualToNumbersChkBx.Checked = labelsAsClusterNumbersCheckBx.Checked;
            outputLabelsEqualToNumbersChkBx.Enabled = !labelsAsClusterNumbersCheckBx.Checked;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BrowseFile(pointsLabelTxt);
        }

        private void pointsLabelTxt_DragDrop(object sender, DragEventArgs e)
        {
            HandleDragDrop(e, pointsLabelTxt);
        }

        private void pointsLabelTxt_DragEnter(object sender, DragEventArgs e)
        {
            HandleDragEnter(e);
        }

        private void applyClusterLabelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ApplyPointsLabelsFrm().Show();
        }

        private void ptsTxt_TextChanged(object sender, EventArgs e)
        {

        }

        private void removePointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new RemovePointsFrm().Show();
        }

       
    }
}
