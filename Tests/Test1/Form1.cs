using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Test1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string rootDir = @"C:\Users\sekanaya\Desktop";
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
    }
}
