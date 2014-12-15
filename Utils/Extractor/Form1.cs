using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace Extractor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void inBrowseBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    inFileTxt.Text = dlg.FileName;
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) 
                {
                    outPathTxt.Text = dlg.SelectedPath;
                }
            }
        }


        private void extractBtn_Click(object sender, EventArgs e)
        {
            using (StreamReader reader = new StreamReader(inFileTxt.Text))
            {
                using (StreamWriter writer = new StreamWriter(outPathTxt.Text + Path.DirectorySeparatorChar + "out.txt"))
                {
                    string str, a, b;
                    string[] arr1, arr2;
                    while (!reader.EndOfStream)
                    {
                        str = reader.ReadLine();
                        arr1 = str.Split('|');
                        str = arr1[1];
                        arr2 = str.Split('<');
                        writer.Write(arr2[0].Trim());
                        writer.Write(",");
                        writer.Write(arr2[1].Substring(0, arr2[1].IndexOf('>')));
                        writer.Write(",");
                        writer.WriteLine(arr1[2]);
                    }
                    writer.Close();
                }
                reader.Close();
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            outPathTxt.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }
    }


}
