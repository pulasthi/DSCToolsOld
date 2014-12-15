using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Salsa.TestManager
{
    public partial class dlgPickHeadNode : Form
    {
        private string _headNode;

        public dlgPickHeadNode()
        {
            InitializeComponent();
            cboHeadNode.DataSource  = HPCHelpers.LoadHeadNodes();
            comboBox1.Items.Add(JobType.PairwiseClustering);
            comboBox1.Items.Add(JobType.SmithWaterman);
            comboBox1.Items.Add(JobType.MDS);
            comboBox1.SelectedIndex = 0;
        }

        public string HeadNode
        {
            get
            {
                return _headNode;
            }
        }

        public JobType JobType
        {
            get
            {
                return (JobType)(comboBox1.SelectedItem);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            _headNode = cboHeadNode.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _headNode = cboHeadNode.Text;
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
