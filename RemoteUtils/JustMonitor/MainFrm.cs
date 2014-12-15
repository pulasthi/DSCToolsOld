using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MDSTryout;
using Microsoft.Hpc.Scheduler;
using Microsoft.Hpc.Scheduler.Properties;
using Salsa.Core.Configuration;

namespace JustMonitor
{
    public partial class MainFrm : Form
    {
        private string _defPlotVizExe = @"C:\Program Files (x86)\PVIZ3\bin\pviz3.exe";

        private readonly ManualResetEvent _jobTrackerStopEvt = new ManualResetEvent(false);
        private readonly ManualResetEvent _jobTrackerBeginEvt = new ManualResetEvent(false);
        private readonly ManualResetEvent _monitorEndEvt = new ManualResetEvent(false);
        private readonly ManualResetEvent _runStableEvt = new ManualResetEvent(true);


        private bool _workInProgress = false;
        private readonly object _workInProgressLock = new object();
        private bool _jobTrackerStopInformed = false;
        private readonly object _jobtrackerMonitorStopLock = new object();
        private bool _fireOnState = true;
        private readonly object _fireOnStateLock = new object();
        private bool _uiSessionEnding = false;

        private ConfigurationMgr _confMgr;
        private RunInfo _runInfo;
        private string _runDir;

        private BackgroundWorker _monitor;
        private BackgroundWorker _jobTracker;

        public MainFrm()
        {
            InitializeComponent();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            OpenRun();
        }

        private void OpenRun()
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Run Info Files (*.infx)|*.infx";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    EndUISession(); // If any
                    PrepareDirtyEnv(dlg.FileName);
                    PrepareDirtyUI();
                    TrackProgress();
                }
            }
        }

        private void TrackProgress()
        {
            if (_runInfo.RunType == RunType.Remote && !_runInfo.IsRunCompleted)
            {
                lock (_workInProgressLock)
                {
                    if (!_workInProgress)
                    {
                        IScheduler scheduler = new Scheduler();
                        scheduler.Connect(_runInfo.HnName);
                        scheduler.SetInterfaceMode(false, (IntPtr) 0);

                        ISchedulerJob job = scheduler.OpenJob(_runInfo.JobId);
                        job.Refresh();
                        if (job.State != JobState.Finished && job.State != JobState.Canceled &&
                            job.State != JobState.Failed)
                        {
                            _workInProgress = true;
                            _runStableEvt.Reset();
                            StartMonitorAsync(); // Keep monitor started (no harm done by this)
                            // Create a background worker to submit and keep track of job
                            _jobTracker = new BackgroundWorker();
                            _jobTracker.DoWork += JobTrackerDoWork;
                            _jobTracker.RunWorkerAsync(new object[] {scheduler, job, false});
                            _jobTrackerBeginEvt.WaitOne(); // Blocks the UI thread till job tracker begins

                            // May be the job finished after our check and before fire on state handlers pick up. So,
                            job.Refresh();
                            if (job.State == JobState.Finished || job.State == JobState.Canceled ||
                                job.State == JobState.Failed)
                            {
                                lock (_jobtrackerMonitorStopLock)
                                {
                                    if (!_jobTrackerStopInformed)
                                    {
                                        _jobTrackerStopInformed = true;
                                        _jobTrackerStopEvt.Set();
                                    }
                                    _fireOnState = false; // No need to fire events
                                    StopMonitorAsync(); // Request monitor stop (probably redundant, but no harm)
                                    _monitorEndEvt.WaitOne(); // Wait till monitor ends
                                    _workInProgress = false;
                                    CompleteRun(job.State == JobState.Finished);
                                }
                            }
                        }
                        else
                        {
                            lock (_jobtrackerMonitorStopLock)
                            {
                                CopyFilesRemoteToLocal();
                                string copiedOutFile = Path.Combine(_runDir, IOUtil.ManxcatOutFileName);
                                string copiedErrFile = Path.Combine(_runDir, IOUtil.ManxcatErrFileName);
                                string outUpdate = string.Empty;
                                string errUpdate = string.Empty;
                                int count;

                                if (File.Exists(copiedOutFile))
                                {
                                    outUpdate = Tail(copiedOutFile, _runInfo.OutLineReadCount, out count);
                                    _runInfo.OutLineReadCount += count;
                                }

                                if (File.Exists(copiedErrFile))
                                {
                                    errUpdate = Tail(copiedErrFile, _runInfo.ErrLineReadCount, out count);
                                    _runInfo.ErrLineReadCount += count;
                                }

                                if (!string.IsNullOrEmpty(outUpdate))
                                {
                                    AppendToOut(outUpdate);
                                }

                                if (!string.IsNullOrEmpty(errUpdate))
                                {
                                    AppendToErr(errUpdate);
                                }
                                CompleteRun(job.State == JobState.Finished);
                            }
                        }
                    }
                }
            }
        }

        private void JobTrackerDoWork(object sender, DoWorkEventArgs e)
        {
            object[] args = (object[]) e.Argument;
            using (IScheduler scheduler = (IScheduler) args[0])
            {
                ISchedulerJob job;
                job = (ISchedulerJob) args[1];

                // Add OnJobState event handler
                job.OnJobState += FireOnJobState;
                // Add OnTaskState event handler
                job.OnTaskState += FireOnTaskState;

                if ((bool) args[2])
                {
                    AppendToOut(LogMessages.SubmittingRemoteRun);
                    scheduler.SubmitJob(job, null, null);

                    // Update job id of the remote run in runinfo
                    _runInfo.JobId = job.Id;
                    // Update the saved copy of run info
                    _runInfo.SaveAs(Path.Combine(_runDir, _runInfo.Name + IOUtil.RunInfoExt));
                }

                _jobTrackerBeginEvt.Set(); // Release UI thread from waiting
                _runStableEvt.Set();

                // Wait till someone signals it's time to stop job tracker
                _jobTrackerStopEvt.WaitOne();


                //_jobTrackerEndEvt.Set(); // Release waiting methods
            }
        }


        private void FireOnTaskState(object sender, TaskStateEventArg e)
        {
            Console.WriteLine(">> 2. fireontask trying to aquire fireonstate lock - state: " + e.NewState);
            lock (_fireOnStateLock)
            {
                Console.WriteLine(">> 2. fireontask got fireonstate lock - fireonstate:" + _fireOnState);
                if (_fireOnState)
                {
                    IScheduler scheduler = (IScheduler) sender;
                    ISchedulerJob job = scheduler.OpenJob(e.JobId);
                    ISchedulerTask task = job.OpenTask(e.TaskId);
                    if (task.Name.StartsWith("Manxcat"))
                    {
                        AppendToOut(string.Format(LogMessages.ManxcatTaskState, TaskStateToString(e.NewState)));
                        if (e.NewState == TaskState.Running)
                        {
                            // Start progress report monitor
                            StartMonitorAsync();
                        }
                        else if (e.NewState == TaskState.Canceled ||
                                 e.NewState == TaskState.Finished ||
                                 e.NewState == TaskState.Failed)
                        {
                            Console.WriteLine(">> 2. fireontask trying to aquire jobtrackermonitor lock");
                            lock (_jobtrackerMonitorStopLock)
                            {
                                Console.WriteLine(">> 2. fireontask got jobtrackermonitor lock");
                                StopMonitorAsync();
                                Console.WriteLine(">> 2. fireontask trying to wait on  monitorend evt");
                                _monitorEndEvt.WaitOne(); // Wait till monitor ends
                                Console.WriteLine(">> 2. fireontask done waiting on  monitorend evt");
                            }
                        }
                    }
                    else
                    {
                        AppendToOut(string.Format(LogMessages.SetupTaskState, task.TaskId.JobTaskId,
                                                  TaskStateToString(e.NewState)));
                        // todo: handle failure/cancel of setup tasks
                        // note. it's not worth trying to proceed. so seems like a good idea to stop the job too.
                    }
                }
            }
        }

        private void FireOnJobState(object sender, JobStateEventArg e)
        {
            Console.WriteLine(">> 1. fireonjob trying to aquire fireonstate lock - state: " + e.NewState);
            lock (_fireOnStateLock)
            {
                Console.WriteLine(">> 1. fireonjob got fireonstate lock - fireonstate: " + _fireOnState);
                if (_fireOnState)
                {
                    AppendToOut(string.Format(LogMessages.JobStatus, e.JobId, JobStateToString(e.NewState)));
                    if (e.NewState == JobState.Finished ||
                        e.NewState == JobState.Canceled ||
                        e.NewState == JobState.Failed)
                    {
                        // UI events like exit/new/open with confirm should also try to acquire this lock before stopping job tracker
                        Console.WriteLine(">> 1. fireonjob trying to aquire jobtrackermonitor lock");
                        lock (_jobtrackerMonitorStopLock)
                        {
                            Console.WriteLine(">> 1. fireonjob got jobtrackermonitor lock");
                            if (!_jobTrackerStopInformed)
                            {
                                _jobTrackerStopInformed = true;
                                _jobTrackerStopEvt.Set();
                            }
                            //Console.WriteLine(">> 1. fireonjob trying to wait on  jobtrackerend evt");
                            //_jobTrackerEndEvt.WaitOne(); // Wait till job tracker ends -- unnecessary since no code after jobtrackerstopevt.waitone()
                            //Console.WriteLine(">> 1. fireonjob done waiting on  jobtrackerend evt");
                            _fireOnState = false; // No need to fire events


                            StopMonitorAsync(); // Request monitor stop (probably redundant, but no harm)
                            Console.WriteLine(">> 1. fireonjob trying to wait on  monitorend evt");
                            _monitorEndEvt.WaitOne(); // Wait till monitor ends
                            Console.WriteLine(">> 1. fireonjob done waiting on  monitorend evt");

                            lock (_workInProgressLock)
                            {
                                _workInProgress = false;
                            }
                            CompleteRun(e.NewState == JobState.Finished);
                        }
                    }
                }
            }
        }

        private delegate void BringBackUIDelegate();

        private void BringBackUI()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new BringBackUIDelegate(BringBackUI));
            }
            else
            {
                if (File.Exists(
                    Path.Combine(_runDir, IOUtil.ManxcatSimplePointsFileNamePrefix +
                                          Path.GetFileName(_confMgr.ManxcatSection.ReducedVectorOutputFileName))))
                {
                    showInPvizBtn.Enabled = showInPvizBtn.Visible = true;
                }
            }
        }

        private void CompleteRun(bool isRunSuccess)
        {
            string ptsFile = Path.Combine(_runDir, IOUtil.ManxcatSimplePointsFileNamePrefix +
                                                   Path.GetFileName(_confMgr.ManxcatSection.ReducedVectorOutputFileName));
            if (File.Exists(ptsFile))
            {
                if (File.Exists(_runInfo.ClusterFile))
                {
                    CrealteLocalPlotFile(ptsFile, _runInfo.ClusterFile);
                }
            }
            _runInfo.IsRunCompleted = true;
            _runInfo.IsRunSuccess = isRunSuccess;
            _runInfo.SaveAs(Path.Combine(_runDir, _runInfo.Name + IOUtil.RunInfoExt));
            BringBackUI();
        }

        private void CrealteLocalPlotFile(string ptsFile, string clusFile)
        {
            using (StreamReader clusReader = new StreamReader(clusFile),
                                ptsReader = new StreamReader(ptsFile))
            {
                using (StreamWriter plotWriter = new StreamWriter(Path.Combine(_runDir, IOUtil.LocalPlotTxtFile)))
                {
                    char[] sep = new[] {' ', '\t'};
                    string[] ptsSplits, clusSplits;
                    while (!clusReader.EndOfStream && !ptsReader.EndOfStream)
                    {
                        ptsSplits = ptsReader.ReadLine().Trim().Split(sep);
                        clusSplits = clusReader.ReadLine().Trim().Split(sep);
                        plotWriter.WriteLine(ptsSplits[0] + '\t' + ptsSplits[1] + '\t' + ptsSplits[2] + '\t' +
                                             ptsSplits[3] + '\t' + clusSplits[1]);
                    }
                    if (!clusReader.EndOfStream)
                    {
                        MessageBox.Show(
                            string.Format(LogMessages.MoreLinesThanOther, "Cluster", "Manxcat points"),
                            LogCategories.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        File.Delete(Path.Combine(_runDir, IOUtil.LocalPlotTxtFile));
                    }

                    if (!ptsReader.EndOfStream)
                    {
                        MessageBox.Show(
                            string.Format(LogMessages.MoreLinesThanOther, "Manxcat points", "cluster"),
                            LogCategories.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        File.Delete(Path.Combine(_runDir, IOUtil.LocalPlotTxtFile));
                    }
                }
            }
        }

        private void PrepareDirtyUI()
        {
            outTxt.Clear();
            if (File.Exists(_runInfo.LocalOutFile))
            {
                outTxt.Text = File.ReadAllText(_runInfo.LocalOutFile);
            }

            errTxt.Clear();
            if (File.Exists(_runInfo.LocalErrFile))
            {
                errTxt.Text = File.ReadAllText(_runInfo.LocalErrFile);
            }

            showInPvizBtn.Enabled =
                showInPvizBtn.Visible = _runInfo.IsRunCompleted && _runInfo.IsRunSuccess && File.Exists(
                    Path.Combine(_runDir, IOUtil.ManxcatSimplePointsFileNamePrefix +
                                          Path.GetFileName(_confMgr.ManxcatSection.ReducedVectorOutputFileName)));
        }

        private void PrepareDirtyEnv(string runInfoFile)
        {
            // Load saved run info :)
            _runInfo = RunInfo.LoadRunInfo(runInfoFile);
            _runDir = Path.Combine(_runInfo.BaseDir, _runInfo.Name);
            _confMgr = ConfigurationMgr.LoadConfiguration(Path.Combine(_runDir, IOUtil.LocalConfigName), false);
            _uiSessionEnding = false;


            _jobTrackerStopEvt.Reset();
            _jobTrackerBeginEvt.Reset();
            _monitorEndEvt.Reset();

            // These two may be changed later if job is still not finished/canceled/failed in cluster
            _runStableEvt.Set();
            _workInProgress = false;

            _jobTrackerStopInformed = false;
            _fireOnState = true;

            _monitor = null;
            _jobTracker = null;
        }

        private void EndUISession()
        {
            Console.WriteLine(">> 3. waiting till runstable");
            _runStableEvt.WaitOne();
            Console.WriteLine(">> 3. Done waiting for runstable");
            // UI events like new/open with confirm should also try to acquire this lock before stopping job tracker
            Console.WriteLine(">> 3. uiexit trying to aquire jobtrackermonitorlock");
            lock (_jobtrackerMonitorStopLock)
            {
                Console.WriteLine(">> 3. uiexit got jobtrackermonitorlock");
                if (!_jobTrackerStopInformed)
                {
                    _jobTrackerStopInformed = true;
                    _jobTrackerStopEvt.Set();
                }
                //Console.WriteLine(">> 3. uiexit trying to wait on  jobtrackerend evt");
                //_jobTrackerEndEvt.WaitOne(); // Wait till job tracker ends
                //Console.WriteLine(">> 3. uiexit done waiting on  jobtrackerend evt");
                _uiSessionEnding = true;
                StopMonitorAsync(); // Request monitor stop (probably redundant, but no harm)
                Console.WriteLine(">> 3. uiexit trying to wait on  monitorend evt");
                _monitorEndEvt.WaitOne(); // Wait till monitor ends
                Console.WriteLine(">> 3. uiexit done waiting on  monitorend evt");
                // todo: beta2 - remove these and use localstatus files 
                // Save local out and err texts

                if (!string.IsNullOrEmpty(outTxt.Text))
                {
                    File.WriteAllText(_runInfo.LocalOutFile, outTxt.Text);
                }
                if (!string.IsNullOrEmpty(errTxt.Text))
                {
                    File.WriteAllText(_runInfo.LocalErrFile, errTxt.Text);
                }

                // Save run info
                if (_runInfo != null)
                {
                    _runInfo.SaveAs(Path.Combine(_runDir, _runInfo.Name + IOUtil.RunInfoExt));
                }
            }
        }

        private void StartMonitorAsync()
        {
            if (_monitor == null)
            {
                _monitor = new BackgroundWorker
                               {
                                   WorkerReportsProgress = true,
                                   WorkerSupportsCancellation = true
                               };
                _monitor.DoWork += MonitorDoWork;
                _monitor.ProgressChanged += UpdateProgress;
                _monitor.RunWorkerAsync();
            }
        }

        private void StopMonitorAsync()
        {
            if (_monitor == null || !_monitor.IsBusy)
            {
                _monitorEndEvt.Set(); // Release waiting methods since there's no monitor or monitor is not working
            }
            else if (_monitor.IsBusy && !_monitor.CancellationPending)
            {
                _monitor.CancelAsync();
            }
        }

        private void UpdateProgress(object sender, ProgressChangedEventArgs e)
        {
            string[] updates = (string[]) e.UserState;
            if (!string.IsNullOrEmpty(updates[0]))
            {
                AppendToOut(updates[0]);
            }

            if (!string.IsNullOrEmpty(updates[1]))
            {
                AppendToErr(updates[1]);
            }
        }

        private void MonitorDoWork(object sender, DoWorkEventArgs e)
        {
            const long refreshInterval = 5*10000000;
            long start = DateTime.Now.Ticks;
            string copiedOutFile = Path.Combine(_runDir, IOUtil.ManxcatOutFileName);
            string copiedErrFile = Path.Combine(_runDir, IOUtil.ManxcatErrFileName);
            while (!_monitor.CancellationPending)
            {
                if ((DateTime.Now.Ticks - start) > refreshInterval)
                {
                    start = DateTime.Now.Ticks;
                    CopyFilesRemoteToLocal();
                    UpdateMonitor(copiedOutFile, copiedErrFile);
                }
            }
            if (_monitor.CancellationPending)
            {
                e.Cancel = true;
                if (!_uiSessionEnding)
                {
                    // Monitor is relieved if it's a ui exit/new/open with confirm)

                    // Give some time to have any files ready in cluster
                    Thread.Sleep(5000);
                    CopyFilesRemoteToLocal();
                    UpdateMonitor(copiedOutFile, copiedErrFile);
                }
                _monitorEndEvt.Set(); // Release waiting methods
                return;
            }
        }

        private void UpdateMonitor(string copiedOutFile, string copiedErrFile)
        {
            string outUpdate = string.Empty;
            string errUpdate = string.Empty;
            int count;

            if (File.Exists(copiedOutFile))
            {
                outUpdate = Tail(copiedOutFile, _runInfo.OutLineReadCount, out count);
                _runInfo.OutLineReadCount += count;
            }

            if (File.Exists(copiedErrFile))
            {
                errUpdate = Tail(copiedErrFile, _runInfo.ErrLineReadCount, out count);
                _runInfo.ErrLineReadCount += count;
            }

            _monitor.ReportProgress(-1, new[] {outUpdate, errUpdate});
        }

        private string Tail(string file, int readLineCount, out int count)
        {
            using (StreamReader reader = new StreamReader(file))
            {
                string update = string.Empty;
                for (int i = 0; i < readLineCount; ++i)
                {
                    // skip already read lines;
                    reader.ReadLine();
                }
                count = 0;
                while (!reader.EndOfStream)
                {
                    update += reader.ReadLine() + LogMessages.nl;
                    ++count;
                }
                return update;
            }
        }

        private void CopyFilesRemoteToLocal()
        {
            string pointsFileNameWoExt =
                Path.GetFileNameWithoutExtension(_confMgr.ManxcatSection.ReducedVectorOutputFileName);
            string fileName = Path.Combine(_runInfo.HnProjectDir,
                                           pointsFileNameWoExt + IOUtil.ManxcatColonPointsFileNameSuffix +
                                           IOUtil.TextExt);
            if (File.Exists(fileName))
            {
                File.Copy(fileName, Path.Combine(_runDir, Path.GetFileName(fileName)), true);
            }

            fileName = Path.Combine(_runInfo.HnProjectDir,
                                    pointsFileNameWoExt + IOUtil.ManxcatGroupPointsFileNameSuffix + IOUtil.TextExt);
            if (File.Exists(fileName))
            {
                File.Copy(fileName, Path.Combine(_runDir, Path.GetFileName(fileName)), true);
            }

            fileName = Path.Combine(_runInfo.HnProjectDir,
                                    IOUtil.ManxcatSimplePointsFileNamePrefix + pointsFileNameWoExt + IOUtil.TextExt);
            Console.WriteLine(">>>>simplepoints exist:  " + File.Exists(fileName));
            if (File.Exists(fileName))
            {
                File.Copy(fileName, Path.Combine(_runDir, Path.GetFileName(fileName)), true);
            }

            fileName = Path.Combine(_runInfo.HnProjectDir, IOUtil.ManxcatOutFileName);
            if (File.Exists(fileName))
            {
                File.Copy(fileName, Path.Combine(_runDir, Path.GetFileName(fileName)), true);
            }

            fileName = Path.Combine(_runInfo.HnProjectDir, IOUtil.ManxcatErrFileName);
            if (File.Exists(fileName))
            {
                File.Copy(fileName, Path.Combine(_runDir, Path.GetFileName(fileName)), true);
            }

            fileName = Path.Combine(_runInfo.HnProjectDir,
                                    Path.GetFileName(_confMgr.ManxcatSection.SummaryOutputFileName));
            if (File.Exists(fileName))
            {
                File.Copy(fileName, _confMgr.ManxcatSection.SummaryOutputFileName, true);
            }

            fileName = Path.Combine(_runInfo.HnProjectDir,
                                    Path.GetFileName(_confMgr.ManxcatSection.TimingOutputFileName));
            if (File.Exists(fileName))
            {
                File.Copy(fileName, _confMgr.ManxcatSection.TimingOutputFileName, true);
            }
        }

        private delegate void AppendToDelegate(TextBox txtBx, string txt);

        private void AppendTo(TextBox txtBx, string txt)
        {
            // todo: bug here
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new AppendToDelegate(AppendTo), new object[] {txtBx, txt});
            }
            else
            {
                txtBx.AppendText(txt);
                txtBx.Refresh();
            }
        }

        private void AppendToErr(string txt)
        {
            AppendTo(errTxt, txt);
        }

        private void AppendToOut(string txt)
        {
            AppendTo(outTxt, txt);
        }

        private string TaskStateToString(TaskState s)
        {
            switch (s)
            {
                case TaskState.Canceled:
                    return "canceled";
                case TaskState.Canceling:
                    return "canceling";
                case TaskState.Configuring:
                    return "configuring";
                case TaskState.Dispatching:
                    return "dispatching";
                case TaskState.Failed:
                    return "failed";
                case TaskState.Finished:
                    return "finished";
                case TaskState.Finishing:
                    return "finishing";
                case TaskState.NA:
                    return "not set";
                case TaskState.Queued:
                    return "queued";
                case TaskState.Running:
                    return "running";
                case TaskState.Submitted:
                    return "submitted";
                case TaskState.Validating:
                    return "validating";
                default:
                    return "undefined";
            }
        }

        private string JobStateToString(JobState s)
        {
            switch (s)
            {
                case JobState.Canceled:
                    return "canceled";
                case JobState.Canceling:
                    return "canceling";
                case JobState.Configuring:
                    return "configuring";
                case JobState.Failed:
                    return "failed";
                case JobState.Finished:
                    return "finished";
                case JobState.Finishing:
                    return "finishing";
                case JobState.Queued:
                    return "queued";
                case JobState.Running:
                    return "running";
                case JobState.Submitted:
                    return "submitted";
                case JobState.Validating:
                    return "validating";
                default:
                    return "undefined";
            }
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
        }

        private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            EndUISession();
        }

        private void showInPvizBtn_Click(object sender, EventArgs e)
        {
            string localPlotFile = Path.Combine(_runDir, IOUtil.LocalPlotTxtFile);
            string ptsFile = Path.Combine(_runDir, IOUtil.ManxcatSimplePointsFileNamePrefix +
                                                   Path.GetFileName(_confMgr.ManxcatSection.ReducedVectorOutputFileName));
            if (File.Exists(ptsFile))
            {
                _runInfo.ClusterFile = string.Empty;
                _runInfo.SaveAs(Path.Combine(_runDir, _runInfo.Name + IOUtil.RunInfoExt));
                ShowInPviz(ptsFile);
            }
        }

        private void ShowInPviz(string plotFile)
        {
            var start = new ProcessStartInfo(_defPlotVizExe)
                            {
                                UseShellExecute = true,
                                RedirectStandardOutput = false,
                                CreateNoWindow = false,
                                Arguments = plotFile
                            };
            Process.Start(start);
        }
    }
}