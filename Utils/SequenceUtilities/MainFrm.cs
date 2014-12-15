using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Bio;
using Bio.IO.FastA;

namespace SequenceUtilities
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
        }

        private void browseBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                if (DialogResult.OK == dlg.ShowDialog(this))
                {
                    sequenceTxt.Text = dlg.FileName;
                    DoOnBrowse(sequenceTxt.Text);
                }
            }
        }

        private void DoOnBrowse(string path)
        {
            if (File.Exists(path))
            {
                string nl = Environment.NewLine;
                using (FastAParser parser = new FastAParser(path))
                {
                    IList<ISequence> seqs = parser.Parse().ToList();
                    MessageBox.Show("Count: " + seqs.Count);
                }
                
            }
        }
    }
}
