using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Hpc.Scheduler;

namespace Salsa.TestManager
{
    public partial class dlgViewComputeNodes : Form
    {
        public dlgViewComputeNodes()
        {
            InitializeComponent();
        }

        public dlgViewComputeNodes(IEnumerable<ISchedulerNode> nodes)
        {
            InitializeComponent();
            bindingSource1.DataSource = nodes;
        }
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
