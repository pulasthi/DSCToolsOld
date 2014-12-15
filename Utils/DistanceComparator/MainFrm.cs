using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HPC.Utilities;

namespace DistanceComparator
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            dataTypeCombo.SelectedIndex = 0;
        }

        private void showPointsBtn_Click(object sender, EventArgs e)
        {
            if (cmpCheck.Checked)
            {
                string pointsFile = pointsTxt.Text;
                string distFile = distTxt.Text;
                Int32 x;
                Int32[] range = rangeTxt.Text.Split('-').Select(s => Int32.TryParse(s, out x) ? x : -1).ToArray();
                if (File.Exists(distFile) && File.Exists(pointsFile) && range[0] > -1 && range[0] < range[1])
                {
                    Dictionary<string, RowData> rowMap = ExtractGeometricDistances(pointsFile, range);
                    FillOriginalDistances(distFile, range, rowMap, dataTypeCombo.SelectedIndex, Int32.Parse(colCountTxt.Text));
                    long pointCount = (range[1] - range[0]) + 1;
                    string txt = "Selected Points Count:\t" + pointCount;
                    txt += Environment.NewLine + "Total Selected Pairs Count (without diagonal):\t" + (pointCount * (pointCount - 1));
                    long colCount = long.Parse(colCountTxt.Text);
                    txt += Environment.NewLine + "Total Points Count:\t" + colCount;
                    txt += Environment.NewLine + "Total Pairs Count (without diagonal):\t" + (colCount*(colCount-1));
                    txt += Environment.NewLine + Environment.NewLine + "Showing half of the selected pairs...";
                    new ResultsFrm(rowMap.Values.ToArray(),txt).Show();
                }
            }
            else
            {
                // TODO: just show distances from the matrix.
            }

        }

        private static void FillOriginalDistances(string distFile, Int32[] range, Dictionary<string, RowData> rowMap, Int32 type, Int32 bigc)
        {
            string pairTemplate = "({0},{1})";

            Int32 tsize = type == 0 || type == 1 ? 2 : 8;
            double maxConst = type == 0 ? Int16.MaxValue : type == 1 ? UInt16.MaxValue : Double.MaxValue;

            Int32 start = range[0];
            Int32 end = range[1];

            Int64 readCount;
            Int64 skip;
            Int32 skipPerTime;
            Int64 pnum;
            double temp = -1.0;

            readCount = 0;
            using (BinaryReader reader = new BinaryReader(File.OpenRead(distFile)))
            {
                for (Int32 i = start; i <= end; i++)
                {
                    for (Int32 j = start; j <= end; j++)
                    {
                        pnum = i * bigc + j;
                        if (pnum > readCount)
                        {
                            skip = (pnum - readCount) * tsize;

                            while (skip > 0)
                            {
                                skipPerTime = skip > Int32.MaxValue ? Int32.MaxValue : (Int32)skip;
                                reader.ReadBytes(skipPerTime);
                                skip -= skipPerTime;
                            }
                            readCount += (pnum - readCount);
                        }
                        switch (type)
                        {
                            case 0:
                                temp = reader.ReadInt16();
                                break;
                            case 1:
                                temp = reader.ReadUInt16();
                                break;
                            case 2:
                                temp = reader.ReadDouble();
                                break;
                        }

                        // consider only top triangle 
                        // (for effiency - we can stop before reading the last row of this sub matrix)
                        if (i < j)
                        {
                            rowMap[string.Format(pairTemplate, i, j)].OriginalDistance = temp / maxConst;
                            //                            rowMap[string.Format(pairTemplate, i, j)].OriginalDistance = temp;
                        }

                        if (i == (end - 1) && j == end)
                        {
                            /* time to break the two "for" loops */

                            // hit the brakes for "for" loops
                            i = end + 1;
                            break;
                        }
                        readCount++;
                    }
                }
            }
        }


        private static Dictionary<string, RowData> ExtractGeometricDistances(string pointsFile, Int32[] range)
        {
            Int32 start = range[0];
            var points = ReadPoints(pointsFile, range);

            string pairTemplate = "({0},{1})";
            string pair;
            Dictionary<string, RowData> rowMap = new Dictionary<string, RowData>();
            double ix, iy, iz, x, y, z, dsq;
            for (int i = 0; i < points.Count; i++)
            {
                ix = points[i][0];
                iy = points[i][1];
                iz = points[i][2];
                for (int j = i + 1; j < points.Count; j++)
                {
                    x = points[j][0];
                    y = points[j][1];
                    z = points[j][2];
                    dsq = Math.Pow(x - ix, 2) + Math.Pow(y - iy, 2) + Math.Pow(z - iz, 2);
                    pair = string.Format(pairTemplate, i + start, j + start);
                    rowMap.Add(pair, new RowData(pair, Math.Sqrt(dsq)));
                }
            }
            return rowMap;
        }

        private static List<double[]> ReadPoints(string pointsFile, Int32[] range)
        {
            using (var reader = new StreamReader(pointsFile))
            {
                Int32 start = range[0];
                Int32 end = range[1];

                char[] sep = { ' ', '\t' };
                Int32 x;
                var points = new List<double[]>();
                string[] lineData;
                int count = 0;
                while (!reader.EndOfStream)
                {
                    if (count < start)
                    {
                        reader.ReadLine();
                        count++;
                        continue;
                    }

                    if (count > end) break;

                    lineData = reader.ReadLine().Split(sep);
                    points.Add(new[] { double.Parse(lineData[1]), double.Parse(lineData[2]), double.Parse(lineData[3]) });
                    count++;
                }
                return points;
            }
        }

        private void distBrowseBtn_Click(object sender, EventArgs e)
        {
            BrowseFile(distTxt);
        }

        private void BrowseFile(TextBox txt)
        {
            using (var dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txt.Text = dlg.FileName;
                }
            }
        }

        private void pointsBrowseBtn_Click(object sender, EventArgs e)
        {
            BrowseFile(pointsTxt);
        }

        private void validateBtn_Click(object sender, EventArgs e)
        {
            validateBtn.Enabled = false;
            validateBtn.Text = "Validating...";
            // Check for any negative values assuming symmetry
            int colCount;
            if (Int32.TryParse(colCountTxt.Text, out colCount))
            {
                using (BinaryReader reader = new BinaryReader(File.OpenRead(distTxt.Text)))
                {
                    IList<NegativeRowData> negatives = new List<NegativeRowData>();
                    string pairTemplate = "({0},{1})";
                    double temp;
                    switch (dataTypeCombo.SelectedIndex)
                    {
                        case 0: // Read Int16
                            for (int i = 0; i < colCount; i++)
                            {
                                for (int j = i+1; j < colCount; j++) 
                                {
                                    temp = reader.ReadInt16();
                                    if (temp < 0)
                                    {
                                        negatives.Add(new NegativeRowData(string.Format(pairTemplate, i, j), temp));
                                    }
                                }
                            }
                            break;
                        case 1: // Read UInt16
                            for (int i = 0; i < colCount; i++)
                            {
                                for (int j = i + 1; j < colCount; j++) 
                                {
                                    temp = reader.ReadUInt16();
                                    if (temp < 0)
                                    {
                                        negatives.Add(new NegativeRowData(string.Format(pairTemplate, i, j), temp));
                                    }
                                }
                            }
                            break;
                        case 2: // Read Double
                            for (int i = 0; i < colCount; i++)
                            {
                                for (int j = i + 1; j < colCount; j++) 
                                {
                                    temp = reader.ReadDouble();
                                    if (temp < 0)
                                    {
                                        negatives.Add(new NegativeRowData(string.Format(pairTemplate, i, j), temp));
                                    }
                                }
                            }
                            break;
                    }
                    if (negatives.Count > 0)
                    {
                        string txt = "Total Negative Pairs (without diagonal):\t" + negatives.Count * (negatives.Count - 1);
                        txt += Environment.NewLine + "Total Pairs Count (without diagonal):\t" + (((long) colCount)*(colCount -1));
                        txt += Environment.NewLine + Environment.NewLine + "Showing half of the negative pairs...";
                        new ResultsFrm(negatives.ToArray(),txt).Show();
                    }
                    else
                    {
                        MessageBox.Show(@"No negatives found", @"Validated");
                    }
                    validateBtn.Enabled = true;
                    validateBtn.Text = "Validate Matrix";

                }
            }
        }
    }
}