using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace Editor
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
            //_fileIO = new FileIO();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // a wizard would be nice
            createNewProject();         
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openProject();
        }

        private void loadProjectTree(string rootDir)
        {
            DirectoryInfo rootInfo = new DirectoryInfo(rootDir);

            string rootName = rootInfo.Name + " (" + rootInfo.Parent.FullName + ")";
            TreeNode rootNode = new TreeNode(rootName, 0, 0);
            rootNode.Tag = rootInfo;
            List<TreeNode> nodes = new List<TreeNode>();
            nodes.Add(rootNode);

            //MessageBox.Show(rootNode.Text);
            while (nodes.Count > 0)
            {
                addChildren(nodes);
            }

            treeView.Nodes.Add(rootNode);                     
        }

        private void addChildren(List<TreeNode> nodes)
        {
            TreeNode parentNode = nodes.First();
            nodes.Remove(parentNode);
            DirectoryInfo parentDir = parentNode.Tag as DirectoryInfo;

            DirectoryInfo[] subDirs = parentDir.GetDirectories();
            TreeNode childNode;
            foreach (DirectoryInfo dir in subDirs)
            {
                childNode = parentNode.Nodes.Add(dir.Name);
                childNode.ImageIndex = 0;
                childNode.SelectedImageIndex = 0;
                childNode.Tag = dir;
                nodes.Add(childNode);
            }

            FileInfo[] files = parentDir.GetFiles();
            foreach (FileInfo file in files)
            {
                childNode = parentNode.Nodes.Add(file.Name);
                childNode.ImageIndex = 1;
                childNode.SelectedImageIndex = 1;
                childNode.Tag = file;
            }

        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            openProject();
        }

        private void openProject()
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    treeView.Nodes.Clear();
                    if (FileIO.CheckProjectStructure(dlg.SelectedPath))
                    {
                        loadProjectTree(dlg.SelectedPath);
                    }
                    else
                    {
                        MessageBox.Show("The path " + dlg.SelectedPath +
                            " does not refer to a valid project");
                    }

                }
            }
        }

        private void createNewProject()
        {
            using (NewProjectDlg dlg = new NewProjectDlg())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    // todo: saliya - refinements here
                    if (FileIO.CreateProjectStructure(dlg.FolderPath, dlg.ProjectName))
                    {
                        MessageBox.Show("Project created successfully!");
                        treeView.Nodes.Clear();
                        loadProjectTree(Path.Combine(dlg.FolderPath, dlg.ProjectName));
                    }
                    else
                    {
                        MessageBox.Show("Project creation failed!");
                    }

                }
            }
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            createNewProject();
        }

    }
}
