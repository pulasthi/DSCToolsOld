using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Salsa.PlotViz.IO;

namespace PlotVizTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowseIndexFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    txtIndexFile.Text = dlg.FileName;
                }
            }
        }

        private void btnBrowseClusterFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    txtClusterFile.Text = dlg.FileName;
                }
            }
        }

        private void btnBrowseCoordiateFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    txtCoordianteFile.Text = dlg.FileName;
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            PlotVizDataFile dataFile = PlotVizDataFile.Build(IndexFile, ClusterFile, CoordinateFile);

            using (SaveFileDialog savedlg = new SaveFileDialog())
            {

                if (savedlg.ShowDialog(this) == DialogResult.OK)
                {
                    dataFile.Save(savedlg.FileName);
                }
            }
        }

        public string IndexFile
        {
            get
            {
                return txtIndexFile.Text.Trim();
            }
        }

        public string ClusterFile
        {
            get
            {
                return txtClusterFile.Text.Trim();
            }
        }

        public string CoordinateFile
        {
            get
            {
                return txtCoordianteFile.Text.Trim();
            }
        }

    }
}
