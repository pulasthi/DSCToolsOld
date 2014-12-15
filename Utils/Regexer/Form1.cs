using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Regexer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void regexBx_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBx_TextChanged(object sender, EventArgs e)
        {
            TestMatch();
        }

        private void TestMatch()
        {
            Match m = Regex.Match(textBx.Text, regexBx.Text);
            if (m.Success)
            {
                label.Text = string.Format("Match: >>{0}<<", m.ToString());
                label.ForeColor = Color.Green;
            }
            else
            {
                label.Text = "No Match";
                label.ForeColor = Color.Red;

            }

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            TestMatch();
        }
    }
}
