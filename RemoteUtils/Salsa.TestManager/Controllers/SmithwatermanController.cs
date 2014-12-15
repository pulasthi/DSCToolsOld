using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Salsa.TestManager.Views;
using System.Windows.Forms;
using Salsa.TestManager.Models;
using System.ComponentModel;
using Microsoft.Hpc.Scheduler;
using Microsoft.Hpc.Scheduler.Properties;
namespace Salsa.TestManager.Controllers
{
    public class SmithwatermanController : BaseController
    {
        private SmithwatermanView _jobView;
        private TaskView _taskView;
        private SmithwatermanJob _job;
        private BindingList<SmithwatermanTask> _tasks;
        private IScheduler _scheduler;

        public SmithwatermanController(string headNode, IScheduler scheduler)
        {
            _scheduler = scheduler;
            _job = new SmithwatermanJob();
            _tasks = new BindingList<SmithwatermanTask>();

            _jobView = new SmithwatermanView();
            _jobView.Dock = DockStyle.Fill;
            _jobView.DataSource = _job;

            _taskView = new TaskView();
            _taskView.Dock = DockStyle.Fill;
            _taskView.DataSource = _tasks;

            _job.Name = "SmithWaterman MPI Blocked Alignment";
            _job.ExecutableFilePath = @"C:\Salsa\Salsa.SmithWatermanMPI\Salsa.SmithWatermanMPI.exe";
            _job.TimingFilePath = string.Format(@"\\{0}\c$\salsa\Evaluations\Alu35339\Results\SmithwatermanMPITimings.txt", headNode);
            _job.FastaFilePath = @"C:\Salsa\Evaluations\Data\AluY_35339_human_chimp.txt";
            _job.OutputFolderPath = string.Format(@"\\{0}\c$\salsa\Evaluations\Alu35339\Merge", headNode);
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
            SmithwatermanTask task = new SmithwatermanTask();

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

            foreach (SmithwatermanTask task in _tasks)
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

                foreach (SmithwatermanTask task in _tasks)
                {
                    max = Math.Max(max, task.MaxProcessesRequired);
                }

                return max;
            }
        }
    }
}
