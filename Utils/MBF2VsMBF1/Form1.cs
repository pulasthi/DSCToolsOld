using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Bio;
using Bio.IO.FastA;

namespace MBF2VsMBF1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path =
                @"C:\Users\sekanaya\Documents\Magic Briefcase\SALSA2\Millions\OriginalSequences\~400K\allreads_uniques_gt200.txt";
            FastAParser parser = new FastAParser(path);
            IEnumerable<ISequence> seqs = parser.Parse();
            IList<ISequence> seqList = seqs.ToList<ISequence>();
            MessageBox.Show(seqList.Count.ToString());
        }
    }
}
