using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UtilsManager
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
        }

        private void dupRemBtn_Click(object sender, EventArgs e)
        {
            DuplicateRemover.MainFrm frm = new DuplicateRemover.MainFrm();
            frm.Show();
        }

        private void randSamplerBtn_Click(object sender, EventArgs e)
        {
            RandomSampler.MainFrm frm = new RandomSampler.MainFrm();
            frm.Show();
        }
    }
}
