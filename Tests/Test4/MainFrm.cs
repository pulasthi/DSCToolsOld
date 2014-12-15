using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Test4
{
    public partial class MainFrm : Form
    {
        private ProjectWizard _wzd;
        private bool _loaded;
        
        private void createNewProject()
        {
            using (NewProjectDlg dlg = new NewProjectDlg())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    if (FileIO.CreateProjectStructure(dlg.FolderPath, dlg.ProjectName))
                    {
                        _wzd = new ProjectWizard(dlg.FolderPath, dlg.ProjectName);
                        wzdPanel.Controls.Add(_wzd);
                        _loaded = true;
                        MessageBox.Show("Project created successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Project creation failed!");
                    }

                }
            }
        }

        private void openProject()
        {
            closeProject();
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Project Files (*.pet)|*.pet";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    _wzd = new ProjectWizard(Project.Load(dlg.FileName));
                    wzdPanel.Controls.Add(_wzd);
                    _loaded = true;
                }
            }
 
        }

        private void saveProject()
        {
            if (_wzd.Dirty)
            {
                _wzd.Save();
            }
        }

        private bool closeProject()
        {
            if (_loaded)
            {
                if (_wzd.Dirty)
                {
                    string msg = "Do you want to save the changes to " + _wzd.ProjectName + " ?";
                    DialogResult result = MessageBox.Show(msg, "Confirm Close", MessageBoxButtons.YesNoCancel);
                    switch (result)
                    {
                        case DialogResult.Yes:
                            _wzd.Save();
                            wzdPanel.Controls.Clear();
                            break;
                        case DialogResult.No:
                            wzdPanel.Controls.Clear();
                            break;
                        case DialogResult.Cancel:
                            return false;
                    }
                }
                else
                {
                    wzdPanel.Controls.Clear();
                } 
            }
            _loaded = false;
            return true;
        }

        public MainFrm()
        {
            InitializeComponent();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            createNewProject();
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            createNewProject();
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveProject();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            saveProject();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            openProject();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openProject();
        }

        private void MainFrm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            e.Cancel = !closeProject();
        }
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeProject();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }


    }
}
