using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MBF.IO.Fasta;

namespace SequenceAnalyzer
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
        }

        private void inBrowseBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                if (DialogResult.OK == dlg.ShowDialog(this))
                {
                    inPathTxt.Text = dlg.FileName;
                }
            }
        }

        private void analyzeBtn_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                analyzeBtn.Enabled = false;
                Analyze(inPathTxt.Text);
                analyzeBtn.Enabled = true;
            }
        }

        private void Analyze(string inPath)
        {
            FastaParser parser = new FastaParser();
            IList<MBF.ISequence> seqs =  parser.Parse(inPath);
            countLb.Text = seqs.Count.ToString();
            
        }

        private Boolean IsValid()
        {
            if (String.IsNullOrEmpty(inPathTxt.Text))
            {
                MessageBox.Show("Input file cannot be null", "Invalid Data", MessageBoxButtons.OK);
                return false;
            }

            if (!File.Exists(inPathTxt.Text))
            {
                MessageBox.Show("Input file does not exist", "I/O Error", MessageBoxButtons.OK);
                return false;
            }

            return true;
        }
    }
}
