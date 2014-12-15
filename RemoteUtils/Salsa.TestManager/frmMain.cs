using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Salsa.TestManager.Controllers;
using Microsoft.Hpc.Scheduler;

namespace Salsa.TestManager
{
    public partial class frmMain : Form
    {
        private BaseController _controller;
        private IScheduler _scheduler;

        public frmMain()
        {
            InitializeComponent();
            _scheduler = new Scheduler();
            _controller = new PairwiseClusteringController(string.Empty, _scheduler);
            panel1.Height = _controller.JobView.Height + 8;
            panel1.Controls.Add(_controller.JobView);
            panel2.Controls.Add(_controller.TaskView);
        }

        public frmMain(JobType jobType, string headNode)
        {
            InitializeComponent();
            _scheduler = new Scheduler();
            _scheduler.Connect(headNode);
            //_scheduler.Connect("tempest.uits.iu.edu");
            switch (jobType)
            {
                case JobType.PairwiseClustering:
                    _controller = new PairwiseClusteringController(headNode, _scheduler);
                    break;
                case JobType.SmithWaterman:
                    _controller = new SmithwatermanController(headNode, _scheduler);
                    break;
                case JobType.MDS:
                    _controller = new ManxcatController(headNode, _scheduler);
                    break;
            }
            
            panel1.Height = _controller.JobView.Height + 4;
            panel1.Controls.Add(_controller.JobView);
            panel2.Controls.Add(_controller.TaskView);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            _scheduler.SetInterfaceMode(false, this.Handle);
        }

        private void btnAddTask_Click(object sender, EventArgs e)
        {
            _controller.AddTask();
        }

        private void btnSubmitJob_Click(object sender, EventArgs e)
        {
            _controller.SubmitJob();
        }
    }
}
