using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Editor_tests
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UC1 uc1 = new UC1();
            uc1.Dock = DockStyle.Fill;
            panel1.Controls.Add(uc1);
          
        }

        private void t1NextBtn_Click(object sender, EventArgs e)
        {
            tabs.SelectedTab = tab2;
        }

        private void t2BackBtn_Click(object sender, EventArgs e)
        {
            tabs.SelectedTab = tab1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    textBox1.Text = dlg.SelectedPath;
                }
            }
        }
    }
}
