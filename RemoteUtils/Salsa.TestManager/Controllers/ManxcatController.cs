using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Salsa.TestManager.Models;
using Salsa.TestManager.Views;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Hpc.Scheduler;
using Microsoft.Hpc.Scheduler.Properties;

namespace Salsa.TestManager.Controllers
{
    public class ManxcatController : BaseController
    {
        private ManxcatView _jobView;
        private TaskView _taskView;
        private ManxcatJob _job;
        private BindingList<ManxcatTask> _tasks;
        private IScheduler _scheduler;

        public ManxcatController(string headnNode, IScheduler scheduler)
        {
            _scheduler = scheduler;
            _job = new ManxcatJob();
            _tasks = new BindingList<ManxcatTask>();

            _jobView = new ManxcatView();
            _jobView.Dock = DockStyle.Fill;
            _jobView.DataSource = _job;

            _taskView = new TaskView();
            _taskView.Dock = DockStyle.Fill;
            _taskView.DataSource = _tasks;

            _job.Name = "Manxcat MDS";
            _job.ExecutableFilePath = @"C:\salsa\MDSasCHISQ\ManxcatMDS.exe";
            _job.ControlFilePath = string.Format(@"\\{0}\c$\salsa\Evaluations\Alu35339\ManxcatControl.txt", headnNode);
        }

        public override Control JobView
        {
            get
            {
                return _jobView;
            }
        }

        public override Control TaskView
        {
            get
            {
                return _taskView;
            }
        }

        public override void AddTask()
        {
            ManxcatTask task = new ManxcatTask();

            using (dlgAddTask dlg = new dlgAddTask(task, _scheduler))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    _tasks.Add(task);
                }
            }

        }

        public override void SubmitJob()
        {
            ISchedulerJob schedulerJob = _scheduler.CreateJob();
            schedulerJob.Name = _job.Name;
            schedulerJob.UnitType = JobUnitType.Core;
            schedulerJob.AutoCalculateMax = false;
            schedulerJob.AutoCalculateMin = false;
            schedulerJob.MaximumNumberOfCores = MaxCoresRequired;
            schedulerJob.MinimumNumberOfCores = MaxCoresRequired;
            schedulerJob.IsExclusive = true;

            foreach (ManxcatTask task in _tasks)
            {
                ISchedulerTask schedularTask = schedulerJob.CreateTask();
                schedularTask.Name = task.Name;
                schedularTask.CommandLine = task.CreateCommandLine(_job);
                schedularTask.IsExclusive = true;
                schedularTask.IsRerunnable = true;
                schedularTask.MaximumNumberOfCores = task.MaxProcessesRequired;
                schedularTask.MinimumNumberOfCores = task.MaxProcessesRequired;
                schedularTask.WorkDirectory = System.IO.Path.GetDirectoryName(_job.ExecutableFilePath);
                schedularTask.StdErrFilePath = task.Name + "_err.txt";
                schedularTask.StdOutFilePath = task.Name + "_out.txt";

                foreach (ISchedulerNode node in task.RequiredNodes)
                {
                    schedularTask.RequiredNodes.Add(node.Name);
                }

                schedulerJob.AddTask(schedularTask);
            }

            _scheduler.SubmitJob(schedulerJob, null, null);
        }

        internal int MaxCoresRequired
        {
            get
            {
                int max = int.MinValue;

                foreach (ManxcatTask task in _tasks)
                {
                    max = Math.Max(max, task.MaxProcessesRequired);
                }

                return max;
            }
        }
    }
}
