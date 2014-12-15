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
    public partial class RemovePointsFrm : Form
    {
        public RemovePointsFrm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainFrm.BrowseFile(plotTxt);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainFrm.BrowseFile(removePointsTxt);
        }

        private void plotTxt_DragEnter(object sender, DragEventArgs e)
        {
            MainFrm.HandleDragEnter(e);
        }

        private void plotTxt_DragDrop(object sender, DragEventArgs e)
        {
            MainFrm.HandleDragDrop(e, plotTxt);
        }

        private void removePointsTxt_DragEnter(object sender, DragEventArgs e)
        {
            MainFrm.HandleDragEnter(e);
        }

        private void removePointsTxt_DragDrop(object sender, DragEventArgs e)
        {
            MainFrm.HandleDragDrop(e,removePointsTxt);
        }

        private void applyBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(plotTxt.Text) && File.Exists(plotTxt.Text))
            {
                if (!string.IsNullOrEmpty(removePointsTxt.Text) && File.Exists(removePointsTxt.Text))
                {
                    var ht = new Hashtable();
                    using (var reader = new StreamReader(removePointsTxt.Text))
                    {
                        var sep = new[] {'\t'};
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            if (string.IsNullOrEmpty(line)) continue;
                            var splits = line.Split(sep);
                            if (splits.Length != 2) continue;
                            ht.Add(int.Parse(splits[0]), splits[1].ToUpper());
                        }
                    }

                    var pviz = PvizModel.LoadPviz(plotTxt.Text);
                    var points = pviz.Points;

                    for (var i = points.Count-1; i > 0; --i)
                    {
                        var point = points[i];
                        var pid = point.Key;
                        var label = point.Label.ToUpper();
                        if (ht.Contains(pid) && ht[pid].Equals(label))
                        {
                            points.Remove(point);
                        }
                    }

                    pviz.SaveAs(Path.Combine(Path.GetDirectoryName(plotTxt.Text)??string.Empty, nameTxt.Text+".pviz"));
                    MessageBox.Show(@"Done.");
                }
            }

        }

        private void nameTxt_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
                nameTxt.Text = Path.GetFileNameWithoutExtension(fileNames[0]);
            }
        }

        private void nameTxt_DragEnter(object sender, DragEventArgs e)
        {
            MainFrm.HandleDragEnter(e);
        }
    }
}
