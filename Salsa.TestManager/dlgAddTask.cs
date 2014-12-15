using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Hpc.Scheduler;
using Salsa.TestManager.Models;

namespace Salsa.TestManager
{
    public partial class dlgAddTask : Form
    {
        private BaseTask _task;
        private ISchedulerNode[] _computeNodes;

        public dlgAddTask()
        {
            InitializeComponent();
            UpdatePattern();
        }

        public dlgAddTask(BaseTask task, IScheduler scheduler)
        {
            InitializeComponent();
            lbxComputeNode.DisplayMember = "Name";
            lbxRequireNodes.DisplayMember = "Name";
            _task = task;
            _computeNodes = HPCHelpers.GetComputeNodes(scheduler);

            lbxComputeNode.BeginUpdate();
            for (int i = 0; i < _computeNodes.Length; i++)
            {
                lbxComputeNode.Items.Add(_computeNodes[i]);
            }
            lbxComputeNode.EndUpdate();
            UpdatePattern();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lbxRequireNodes.Items.Count == 0)
            {
                MessageBox.Show(this, "Please select required nodes.", "Missing required nodes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _task.ProcessesPerNode = Convert.ToInt32(txtProcessPerNode.Text);
            _task.ThreadsPerProcess = Convert.ToInt32(txtThreadsPerProcess.Text);

            foreach (ISchedulerNode computeNode in lbxRequireNodes.Items)
            {
                _task.RequiredNodes.Add(computeNode);
            }

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnAddAllRequired_Click(object sender, EventArgs e)
        {
            foreach (var computeNode in lbxComputeNode.Items)
            {
                lbxRequireNodes.Items.Add(computeNode);
            }

            lbxComputeNode.Items.Clear();
            UpdatePattern();
        }

        private void btnAddRequired_Click(object sender, EventArgs e)
        {
            foreach(var item in lbxComputeNode.SelectedItems)
            {
                lbxRequireNodes.Items.Add(item);
            }

            foreach(var item in lbxRequireNodes.Items)
            {
                if (lbxComputeNode.Items.Contains(item))
                {
                    lbxComputeNode.Items.Remove(item);
                }
            }

            lbxComputeNode.Focus();
            UpdatePattern();
        }

        private void btnRemoveRequired_Click(object sender, EventArgs e)
        {
            foreach (var item in lbxRequireNodes.SelectedItems)
            {
                lbxComputeNode.Items.Add(item);
            }

            foreach (var item in lbxComputeNode.Items)
            {
                if (lbxRequireNodes.Items.Contains(item))
                {
                    lbxRequireNodes.Items.Remove(item);
                }
            }


            lbxRequireNodes.Focus();
            UpdatePattern();
        }
    
        private void btnRemoveAllRequired_Click(object sender, EventArgs e)
        {
            foreach (var computeNode in lbxRequireNodes.Items)
            {
                lbxComputeNode.Items.Add(computeNode);
            }

            lbxRequireNodes.Items.Clear();
            UpdatePattern();
        }

        private void btnNodeInfo_Click(object sender, EventArgs e)
        {
            using(dlgViewComputeNodes dlg = new dlgViewComputeNodes(_computeNodes))
            {
                dlg.ShowDialog(this);
            }
        }
                
        private void txtThreadsPerProcess_TextChanged(object sender, EventArgs e)
        {
            UpdatePattern();
        }

        private void txtProcessPerNode_TextChanged(object sender, EventArgs e)
        {
            UpdatePattern();
        }

        private void UpdatePattern()
        {
            string pattern = string.Format("Pattern: {0}x{1}x{2}", txtThreadsPerProcess.Text, txtProcessPerNode.Text, lbxRequireNodes.Items.Count);
            lblPattern.Text = pattern;
        }
    }
}
