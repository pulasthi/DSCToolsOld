using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Windows.Forms;

namespace MDSTryout
{
    public partial class PlotVizBuilderDlg : Form
    {

        public PlotVizBuilderDlg()
        {
            InitializeComponent();
        }

        public PlotVizBuilderDlg(string indexFile, string clusterFile, string coordinatesFile)
        {
            InitializeComponent();
            indexFileTxt.Text = indexFile;
            clusterFileTxt.Text = clusterFile;
            coordinatesFileTxt.Text = coordinatesFile;
        }



        private void btnBrowseIndexFile_Click(object sender, EventArgs e)
        {
            Browse(indexFileTxt);
        }

        private void Browse(TextBox textBox)
        {
            using (var dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    textBox.Text = dlg.FileName;
                }
            }
        }

        private void btnBrowseClusterFile_Click(object sender, EventArgs e)
        {
            Browse(clusterFileTxt);
        }

        private void btnBrowseCoordiateFile_Click(object sender, EventArgs e)
        {
            Browse(coordinatesFileTxt);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                PlotVizDataFile pvdf;
                if (File.Exists(coordinatesFileTxt.Text))
                {
                    if (File.Exists(indexFileTxt.Text))
                    {
                        if (File.Exists(clusterFileTxt.Text))
                        {
                            pvdf = PlotVizDataFile.Build(indexFileTxt.Text, clusterFileTxt.Text, coordinatesFileTxt.Text);
                        }
                        else
                        {
                            pvdf = PlotVizDataFile.Build(indexFileTxt.Text, coordinatesFileTxt.Text);
                        }
                    }
                    else
                    {
                        pvdf = PlotVizDataFile.Build(coordinatesFileTxt.Text);
                    }

                    pvdf.Save(Path.Combine(Path.GetDirectoryName(coordinatesFileTxt.Text), Path.GetFileNameWithoutExtension(coordinatesFileTxt.Text) + ".pviz"));
                    MessageBox.Show("PlotViz file rebuild is complete.", "Rebuild Complete", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
                else
                {
                    var msg = "Could not find a coordinates file to build the PltoViz file.";
                    MessageBox.Show(msg, "Error Rebuilding PlotViz File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception exception)
            {
                var msg =
                    "Could not rebuild PlotViz file. If the plot is open already in PlotViz please close it and try again.";
                MessageBox.Show(msg, "Error Rebuilding PlotViz File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}
