using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.pviz;
using Point = Common.pviz.Point;

namespace PvizBuilder
{
    public partial class ApplyPointsLabelsFrm : Form
    {
        public ApplyPointsLabelsFrm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainFrm.BrowseFile(plotTxt);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainFrm.BrowseFile(pointsLabelsTxt);
        }

        private void plotTxt_DragEnter(object sender, DragEventArgs e)
        {
            MainFrm.HandleDragEnter(e);
        }

        private void plotTxt_DragDrop(object sender, DragEventArgs e)
        {
            MainFrm.HandleDragDrop(e, plotTxt);
        }

        private void clusterTxt_DragEnter(object sender, DragEventArgs e)
        {
            MainFrm.HandleDragEnter(e);
        }

        private void clusterTxt_DragDrop(object sender, DragEventArgs e)
        {
            MainFrm.HandleDragDrop(e,pointsLabelsTxt);
        }

        private void applyBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(plotTxt.Text) && File.Exists(plotTxt.Text))
            {
                if (!string.IsNullOrEmpty(pointsLabelsTxt.Text) && File.Exists(pointsLabelsTxt.Text))
                {
                    var pviz = PvizModel.LoadPviz(plotTxt.Text);
                    var points = pviz.Points;
                    var ht = new Hashtable();
                    foreach(var point in points)
                    {
                        ht.Add(point.Key, point);
                    }
                    using (var reader = new StreamReader(pointsLabelsTxt.Text))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            if (string.IsNullOrEmpty(line)) continue;
                            var sep = new[] {'\t'};
                            var splits = line.Split(sep);
                            var labelIndex = splits.Length == 3 ? 1 : splits.Length == 5 ? 4 : 1;
                            var pnum = int.Parse(splits[0]);
                            var label = splits[labelIndex];
                            ((Point) ht[pnum]).Label = label;
                        }
                    }
                    pviz.SaveAs(plotTxt.Text);
                    MessageBox.Show(@"Done.");
                }
            }
            
        }
    }
}
