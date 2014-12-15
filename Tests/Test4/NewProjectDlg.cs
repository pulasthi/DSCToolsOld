using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Test4
{
    public partial class NewProjectDlg : Form
    {
        public string FolderPath { get; set; }
        public string ProjectName { get; set; }

        public NewProjectDlg()
        {
            InitializeComponent();
        }

        private void browseBtn_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                if (DialogResult.OK == dlg.ShowDialog())
                {
                    pathTx.Text = dlg.SelectedPath;

                    // todo: saliya - seems bit dirty
                    if (nameTx.Text != string.Empty)
                    {
                        okBtn.Enabled = true;
                    }
                    else
                    {
                        okBtn.Enabled = false;
                    }

                }
            }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            ProjectName = nameTx.Text;
            FolderPath = pathTx.Text;
            if (Directory.Exists(Path.Combine(FolderPath, ProjectName)))
            {
                if (MessageBox.Show("A project exist with the given name. Do you want to overwrite it?",
                    "Warning - Project Exists", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    okBtn.DialogResult = DialogResult.None;
                }
                else
                {
                    Close();
                }

            }
            else
            {
                Close();
            }

        }

        private void nameTx_TextChanged(object sender, EventArgs e)
        {
            // todo: saliya - some regex to check if starts with invalid character
            if (nameTx.Text != string.Empty && pathTx.Text != string.Empty)
            {
                okBtn.Enabled = true;
            }
            else
            {
                okBtn.Enabled = false;
            }

        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pathTx_TextChanged(object sender, EventArgs e)
        {
            // todo: saliya - some regex to check if starts with invalid character
            if (nameTx.Text != string.Empty && pathTx.Text != string.Empty)
            {
                okBtn.Enabled = true;
            }
            else
            {
                okBtn.Enabled = false;
            }

        }



     
    }
}
