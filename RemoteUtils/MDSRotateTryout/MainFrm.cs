using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Microsoft.Hpc.Scheduler;
using Microsoft.Hpc.Scheduler.Properties;
using Salsa.Core.Configuration;
using Salsa.Core.Configuration.Sections;


namespace MDSTryout
{
    public enum RunType
    {
        Local,
        Remote
    }

    public partial class MainFrm : Form
    {
        private readonly ManualResetEvent _jobTrackerStopEvt = new ManualResetEvent(false);
        private readonly ManualResetEvent _jobTrackerBeginEvt = new ManualResetEvent(false);
        //private readonly ManualResetEvent _jobTrackerEndEvt = new ManualResetEvent(false);
        private readonly ManualResetEvent _monitorEndEvt = new ManualResetEvent(false);
        private readonly ManualResetEvent _runStableEvt = new ManualResetEvent(true);


        private bool _workInProgress = false;
        private readonly object _workInProgressLock = new object();
        private bool _jobTrackerStopInformed = false;
        private readonly object _jobtrackerMonitorStopLock = new object();
        private bool _fireOnState = true;
        private readonly object _fireOnStateLock = new object();
        private bool _uiSessionEnding = false;

        private static readonly string AutoNamePrefix = "MDS_Run_";
        private static readonly Regex AutoNamePrefixRegex = new Regex(@"^MDS_Run_\d+$");

        private Settings _settings;
        private ConfigurationMgr _confMgr;
        private ManxcatSection _referenceMDSConfig;

        private RunInfo _runInfo;

        private bool _isRunSuccess = false; // usage in local run only
        private string _runDir;

        // Todo: Replace with a background worker if possible for local runs
        private MDSRunnable _mdsRunnable;

        private BackgroundWorker _monitor;
        private BackgroundWorker _jobTracker;

        private RunType _runType;
        private string[] _headNodes;

        public MainFrm()
        {
            InitializeComponent();
        }

        private void MainFrmLoad(object sender, EventArgs e)
        {
            LoadSettings();
            PrepareNewRun();
        }

        #region NewRun

        private void PrepareNewRun()
        {
            EndUISession(); // If any
            PrepareNewEnvironment();
            PrepareFreshUI();
            GenerateAutoName();
            InitializeConfig();
        }

        private void PrepareNewEnvironment()
        {
            _confMgr = new ConfigurationMgr();
            _referenceMDSConfig = new ManxcatSection();
            _runInfo = null;
            _isRunSuccess = false; // usage in local run only
            _runDir = string.Empty;
            _mdsRunnable = null; // usage in local run only
            _runType = RunType.Remote;

            _jobTrackerStopEvt.Reset();
            _jobTrackerBeginEvt.Reset();
            _monitorEndEvt.Reset();
            _runStableEvt.Set();

            _workInProgress = false;
            _jobTrackerStopInformed = false;
            _fireOnState = true;
            _uiSessionEnding = false;

            _monitor = null;
            _jobTracker = null;
        }

        private void PrepareFreshUI()
        {
            rootSplit.Panel1.Enabled = true;
            mainSplit.Panel1.Enabled = true;
            mainSplit.Panel2.Enabled = true;
            outTxt.Clear();
            outTxt.Refresh();
            errTxt.Clear();
            errTxt.Refresh();

            /* Run type UI changes*/
            remoteRunRadio.Checked = true;
            // todo: enable back local run in beta2
            localRunRadio.Enabled = false;
            localRunRadio.Text = "Local (disabled in current version)";
          

            cnListBx.ClearSelected();

            /* TargetDir value */
            targetDirTxt.Text = _settings.DefTargetDir;

            /* Run config UI changes */
            mdsAppDirTxt.Text = _settings.DefaultMDSAppDirectory;
            baseDirTxt.Text = _settings.DefaultBaseDirectory;
            processBar.Value = 1;
            processTxt.Text = processBar.Value.ToString();
            threadBar.Value = 1;
            threadTxt.Text = threadBar.Value.ToString();

            // todo: beta2 - visible autoIncr in beta2
            autoIncrChkBx.Enabled = autoIncrChkBx.Visible = false;
//            autoIncrChkBx.Checked = true;
            killBtn.Enabled = killBtn.Visible = false;

            pGrid.SelectedObject = _confMgr.ManxcatSection;
            resetConfigBtn.Enabled = false;
            runIncBtn.Visible = false;
            showInPvizBtn.Enabled = showInPvizBtn.Visible = false;
            rebuildBtn.Enabled = rebuildBtn.Visible = false;
            showRefPlotBtn.Enabled = showRefPlotBtn.Visible = false;
            mainSplit.Panel2.Enabled = false;
        }

        #endregion

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

            #region Lcoal Run Open
            /****** Local Run Open ***********************************************************************************/
            /*
             * resetConfigBtn.Enabled = false;            
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Run Info Files (*.infx)|*.infx";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    _runInfo = RunInfo.LoadRunInfo(dlg.FileName);
                    mdsAppDirTxt.Text = _runInfo.AppDir;
                    baseDirTxt.Text = _runInfo.BaseDir;
                    nameTxt.Text = _runInfo.Name;
                    clusTxt.Text = _runInfo.ClusterFile;
                    processBar.Value = _runInfo.ProcessCount;
                    processTxt.Text = _runInfo.ProcessCount.ToString();
                    autoIncrChkBx.Checked = _runInfo.AutoIncr;

                    _confMgr =
                        ConfigurationMgr.LoadConfiguration(Path.Combine(_runInfo.BaseDir, _runInfo.Name, "config.xml"),
                                                           false);
                    _referenceMDSConfig = new ManxcatSection();
                    CopyMDSConfiguration(_confMgr.ManxcatSection, _referenceMDSConfig, true);
                    pGrid.SelectedObject = _confMgr.ManxcatSection;

                    string summaryFile = _referenceMDSConfig.SummaryOutputFileName;
                    if (File.Exists(summaryFile))
                    {
                        LoadSummary(summaryFile);
                    }

                    mainSplit.Panel2.Enabled = true;
                    outSplit.Panel1.Enabled = true;
                    outSplit.Panel2.Enabled = true;
                    showInPvizBtn.Enabled = _runInfo.IsRunSuccess;
                    showRefPlotBtn.Enabled = _runInfo.IsRunSuccess;
                    rebuildBtn.Enabled = _runInfo.IsRunSuccess;
                }
                pGrid.Refresh();
            }
             ****** End Local Run Open ************************************************************************************/
            #endregion

        }

        private void TrackProgress()
        {
//            if (_runType == RunType.Remote && !_runInfo.IsRunCompleted)
            if (_runType == RunType.Remote)
            {
                if (_runInfo.IsRunCompleted && IncompleteFiles())
                {
                    _runInfo.IsRunCompleted = false;
                    _runInfo.OutLineReadCount = 0;
                    _runInfo.ErrLineReadCount = 0;
                }

                if (!_runInfo.IsRunCompleted)
                {
                    lock (_workInProgressLock)
                    {
                        if (!_workInProgress)
                        {
                            try
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
                                            StopMonitorAsync();
                                                // Request monitor stop (probably redundant, but no harm)
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
                            catch (SchedulerException se)
                            {
                                if (se.Code == -2147220986)
                                {
                                    /* The specified Job ID is not valid.  Check your Job ID and try again. */

                                    // Assume it was not submitted and show the configuration UI.
//                                    BringBackUI(LogMessages.JobNotFoundInCluster);
                                    BringBackUI();
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool IncompleteFiles()
        {
            return (!File.Exists(_runInfo.LocalOutFile) && 
                !File.Exists(_runInfo.LocalErrFile) && 
                !File.Exists(Path.Combine(_runDir, IOUtil.ManxcatOutFileName)) &&
                !File.Exists(Path.Combine(_runDir, IOUtil.ManxcatErrFileName)));
        }

        private void PrepareDirtyEnv(string runInfoFile)
        {
            // Load saved run info :)
            _runInfo = RunInfo.LoadRunInfo(runInfoFile);
            FixRunInfoIfImported(runInfoFile);
            _runDir = Path.Combine(_runInfo.BaseDir, _runInfo.Name);
            _confMgr = ConfigurationMgr.LoadConfiguration(Path.Combine(_runDir, IOUtil.LocalConfigName), false);
            // Fix configuration paths in case if the project was not originally from this location
            ChangeConfigPaths(_runDir, _confMgr.ManxcatSection, false);
            _confMgr.SaveAs(Path.Combine(_runDir, IOUtil.LocalConfigName));
            _referenceMDSConfig = new ManxcatSection();
            CopyMDSConfiguration(_confMgr.ManxcatSection,_referenceMDSConfig, true);
            _isRunSuccess = _runInfo.IsRunSuccess; // Usage in local run only
            _runType = _runInfo.RunType;
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

        private void FixRunInfoIfImported(string runInfoFile)
        {
            if (!Directory.Exists(_runInfo.AppDir))
            {
                // Take the default app directory from settings
                _runInfo.AppDir = _settings.DefaultMDSAppDirectory;
            }

            // Parent directory of the directory where runInfo lives
            string baseDir = Path.GetDirectoryName(Path.GetDirectoryName(runInfoFile));
            if (!_runInfo.BaseDir.Equals(baseDir))
            {
                // Take the parent directory of the directory where runInfo lives
                _runInfo.BaseDir = baseDir;

                // Now fix local out and err file paths
                _runInfo.LocalOutStatusFile = Path.Combine(_runInfo.BaseDir, _runInfo.Name,
                                                           Path.GetFileName(_runInfo.LocalOutStatusFile));
                _runInfo.LocalErrStatusFile = Path.Combine(_runInfo.BaseDir, _runInfo.Name,
                                                           Path.GetFileName(_runInfo.LocalErrStatusFile));
                _runInfo.LocalOutFile = Path.Combine(_runInfo.BaseDir, _runInfo.Name,
                                                           Path.GetFileName(_runInfo.LocalOutFile));
                _runInfo.LocalErrFile = Path.Combine(_runInfo.BaseDir, _runInfo.Name,
                                                           Path.GetFileName(_runInfo.LocalErrFile));
            }

            // Save run info
            _runInfo.SaveAs(runInfoFile);
        }

        private void PrepareDirtyUI()
        {
            remoteRunRadio.Checked = _runInfo.RunType == RunType.Remote;
            hnComboBx.SelectedIndex = _runInfo.HnIndex;
            cnListBx.ClearSelected();
            cnListBx.BeginUpdate();
            foreach (int cnIndex in _runInfo.CnIndices)
            {
                cnListBx.SelectedIndices.Add(cnIndex);
            }
            cnListBx.EndUpdate();
            targetDirTxt.Text = _runInfo.TargetDir;

            mdsAppDirTxt.Text = _runInfo.AppDir;

            // baseDirTxt setting should come before nameTxt setting
            baseDirTxt.Text = _runInfo.BaseDir;
            nameTxt.Text = _runInfo.Name;
            
            processBar.Value = _runInfo.ProcessCount;
            processTxt.Text = processBar.Value.ToString();
            threadBar.Value = _runInfo.ThreadCount;
            threadTxt.Text = threadBar.Value.ToString();

            clusTxt.Text = _runInfo.ClusterFile;

            pGrid.SelectedObject = _confMgr.ManxcatSection;
            pGrid.Refresh();

            runBtn.Enabled = runBtn.Visible = _runInfo.IsRunCompleted;
            killBtn.Enabled = killBtn.Visible = !_runInfo.IsRunCompleted;
            runIncBtn.Enabled = runIncBtn.Visible = false;


            outTxt.Clear();
            if (File.Exists(_runInfo.LocalOutFile))
            {
                outTxt.Text = File.ReadAllText(_runInfo.LocalOutFile);
            }
            else if(File.Exists(Path.Combine(_runDir, IOUtil.ManxcatOutFileName)))
            {
                _runInfo.OutLineReadCount = 0;
                outTxt.Text = File.ReadAllText(Path.Combine(_runDir, IOUtil.ManxcatOutFileName));
                using (StreamWriter outWriter = new StreamWriter(_runInfo.LocalOutFile))
                {
                    using (StreamReader manxcatOutReader = new StreamReader(Path.Combine(_runDir, IOUtil.ManxcatOutFileName)))
                    {
                        while (!manxcatOutReader.EndOfStream)
                        {
                            outWriter.WriteLine(manxcatOutReader.ReadLine());
                            ++_runInfo.OutLineReadCount;
                        }
                    }
                }
            }
            
            errTxt.Clear();
            if (File.Exists(_runInfo.LocalErrFile))
            {
                errTxt.Text = File.ReadAllText(_runInfo.LocalErrFile);
            }
            else if(File.Exists(Path.Combine(_runDir, IOUtil.ManxcatErrFileName)))
            {
                _runInfo.ErrLineReadCount = 0;
                errTxt.Text = File.ReadAllText(Path.Combine(_runDir, IOUtil.ManxcatErrFileName));
                using (StreamWriter errWriter = new StreamWriter(_runInfo.LocalErrFile))
                {
                    using (StreamReader manxcatErrReader = new StreamReader(Path.Combine(_runDir, IOUtil.ManxcatErrFileName)))
                    {
                        while (!manxcatErrReader.EndOfStream)
                        {
                            errWriter.WriteLine(manxcatErrReader.ReadLine());
                            ++_runInfo.ErrLineReadCount;
                        }
                    }
                }
            }

            showInPvizBtn.Enabled =
                showInPvizBtn.Visible = _runInfo.IsRunCompleted && _runInfo.IsRunSuccess && File.Exists(
                    Path.Combine(_runDir, IOUtil.ManxcatSimplePointsFileNamePrefix +
                                          Path.GetFileName(_confMgr.ManxcatSection.ReducedVectorOutputFileName)));
            
            rebuildBtn.Enabled = rebuildBtn.Visible = false; // No need for this, merge it's functionality with showinPviz
            // todo: beta2 - show when a ref coord file exists
            showRefPlotBtn.Enabled = showRefPlotBtn.Visible = false;

            rootSplit.Panel1.Enabled = _runInfo.IsRunCompleted;
            mainSplit.Panel1.Enabled = _runInfo.IsRunCompleted;
            mainSplit.Panel2.Enabled = true;
        }

        private void LoadSummary(string summaryFile)
        {
            using (StreamReader reader = new StreamReader(summaryFile))
            {
                // To skip the XML configuration section
                int skip = 168;
                int count = 0;
                string line;
                while (!reader.EndOfStream && count < skip)
                {
                    reader.ReadLine();
                    count++;
                }
                outTxt.AppendText(reader.ReadToEnd().Replace("\n\n",
                                                             string.Format("{0}{1}", Environment.NewLine,
                                                                           Environment.NewLine)));
            }
            outTxt.Update();
        }

        //private void CustomizePGrid()
        //{
        //    pGrid.BrowsableAttributes = new AttributeCollection(
        //        new Attribute[]
        //        {
        //        });
        //}

        private void GenerateAutoName()
        {
            IEnumerable<string> dirs =
                Directory.GetDirectories(baseDirTxt.Text).Where<string>(
                    x => AutoNamePrefixRegex.IsMatch(x.Substring(x.LastIndexOf(Path.DirectorySeparatorChar) + 1)));
            int maxIdx = dirs.Aggregate<string, int>(-1, (max, x) =>
                                                             {
                                                                 int idx =
                                                                     int.Parse(
                                                                         x.Substring(
                                                                             x.LastIndexOf(Path.DirectorySeparatorChar) +
                                                                             9));
                                                                 return idx >= max ? idx : max;
                                                             });
            nameTxt.Text = AutoNamePrefix + (maxIdx + 1);
        }

        private void ChangeConfigPaths(string projectPath, ManxcatSection config, bool isRemote)
        {
            string fname =
                config.ReducedVectorOutputFileName.Substring(
                    config.ReducedVectorOutputFileName.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            config.ReducedVectorOutputFileName = Path.Combine(projectPath, fname);
            fname =
                config.SummaryOutputFileName.Substring(
                    config.SummaryOutputFileName.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            config.SummaryOutputFileName = Path.Combine(projectPath, fname);
            fname =
                config.TimingOutputFileName.Substring(
                    config.TimingOutputFileName.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            config.TimingOutputFileName = Path.Combine(projectPath, fname);
            config.ControlDirectoryName = projectPath;
            config.BaseResultDirectoryName = projectPath;

            /* If this is a remote run, change local input file paths to headnode's appropriate directory */
            if (isRemote)
            {
                if (!string.IsNullOrEmpty(config.DataLabelsFileName))
                {
                    fname = Path.GetFileName(config.DataLabelsFileName);
                    config.DataLabelsFileName = Path.Combine(projectPath, IOUtil.RemoteInputDirName, fname);
                }

                // Treat distance file path differently as it needs to be in every compute node
                if (!string.IsNullOrEmpty(config.DistanceMatrixFile))
                {
                    fname = Path.GetFileName(config.DistanceMatrixFile);
                    int idxOfDollar = projectPath.IndexOf('$');
                    projectPath = projectPath.Substring(idxOfDollar - 1).Replace('$', ':');
                    config.DistanceMatrixFile = Path.Combine(projectPath, IOUtil.RemoteInputDirName, fname);
                }

                if (!string.IsNullOrEmpty(config.IndexFile))
                {
                    fname = Path.GetFileName(config.IndexFile);
                    config.IndexFile = Path.Combine(projectPath, IOUtil.RemoteInputDirName, fname);
                }

                if (!string.IsNullOrEmpty(config.InitializationFileName))
                {
                    fname = Path.GetFileName(config.InitializationFileName);
                    config.InitializationFileName = Path.Combine(projectPath, IOUtil.RemoteInputDirName, fname);
                }

                if (!string.IsNullOrEmpty(config.RotationLabelsFileName))
                {
                    fname = Path.GetFileName(config.RotationLabelsFileName);
                    config.RotationLabelsFileName = Path.Combine(projectPath, IOUtil.RemoteInputDirName, fname);
                }

                if (!string.IsNullOrEmpty(config.Selectedfixedpointfile))
                {
                    fname = Path.GetFileName(config.Selectedfixedpointfile);
                    config.Selectedfixedpointfile = Path.Combine(projectPath, IOUtil.RemoteInputDirName, fname);
                }

                if (!string.IsNullOrEmpty(config.Selectedvariedpointfile))
                {
                    fname = Path.GetFileName(config.Selectedvariedpointfile);
                    config.Selectedvariedpointfile = Path.Combine(projectPath, IOUtil.RemoteInputDirName, fname);
                }

                if (!string.IsNullOrEmpty(config.WeightingFileName))
                {
                    fname = Path.GetFileName(config.WeightingFileName);
                    config.WeightingFileName = Path.Combine(projectPath, IOUtil.RemoteInputDirName, fname);
                }
            }
        }


        private void ChangeConfigPaths()
        {
            string projectPath = Path.Combine(baseDirTxt.Text, nameTxt.Text);
            ManxcatSection config = _confMgr.ManxcatSection;
            ChangeConfigPaths(projectPath, config, false);
            pGrid.Refresh();
        }

        private void InitializeConfig()
        {
            ManxcatSection manxcat = _confMgr.ManxcatSection;
            manxcat.BaseResultDirectoryName = string.Empty;
            manxcat.ControlDirectoryName = string.Empty;
            manxcat.DistanceMatrixFile = string.Empty;
            manxcat.IndexFile = string.Empty;
            manxcat.ReadPartialMatrix = false;
            ChangeConfigPaths();
            CopyMDSConfiguration(manxcat, _referenceMDSConfig, true);
        }


        private void changeAppDirBtn_Click(object sender, EventArgs e)
        {
            ChangeAppDir();
        }

        private DialogResult ChangeAppDir()
        {
            DialogResult result;
            using (
                ConfigDlg dlg = new ConfigDlg(_settings.DefaultMDSAppDirectory, ConfigDlg.ConfigDlgType.MDSAppDirectory)
                )
            {
                dlg.StricMDSAppDirValidation = true;
                result = dlg.ShowDialog(this);
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    if (!_settings.DefaultMDSAppDirectory.Equals(dlg.DefaultDirectory))
                    {
                        _settings.DefaultMDSAppDirectory = dlg.DefaultDirectory;
                        _settings.Save();
                    }
                    mdsAppDirTxt.Text = dlg.CurrentDir;
                }
            }
            return result;
        }

        private void changeBaseDirBtn_Click(object sender, EventArgs e)
        {
            ChangeBaseDir();
        }

        private DialogResult ChangeBaseDir()
        {
            DialogResult result;
            using (ConfigDlg dlg = new ConfigDlg(_settings.DefaultBaseDirectory, ConfigDlg.ConfigDlgType.BaseDirectory))
            {
                result = dlg.ShowDialog(this);
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    if (!_settings.DefaultBaseDirectory.Equals(dlg.DefaultDirectory))
                    {
                        _settings.DefaultBaseDirectory = dlg.DefaultDirectory;
                        _settings.Save();
                    }
                    baseDirTxt.Text = dlg.CurrentDir;
                }
            }
            if (autoIncrChkBx.Checked)
            {
                GenerateAutoName();
            }

            ChangeConfigPaths();
            return result;
        }

        private void copyMDSAppDirLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(mdsAppDirTxt.Text);
        }

        private void copyDirLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(baseDirTxt.Text);
        }

        private void loadConfigBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Configuration XML (*.xml)|*xml";
                if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    ManxcatSection external = ConfigurationMgr.LoadConfiguration(dlg.FileName, false).ManxcatSection;
                    // Let it load I/O by default, but change output paths correctly
                    //string msg = "Do you want to load I/O configuration as well?";
                    //CopyMDSConfiguration(external, _confMgr.ManxcatSection,
                    //    MessageBox.Show(msg, "Load External I/O Values", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes);
                    CopyMDSConfiguration(external, _confMgr.ManxcatSection, true);
                    ChangeConfigPaths();
                    pGrid.Refresh();
                }
            }
            resetConfigBtn.Enabled = true;
        }

        private void CopyMDSConfiguration(ManxcatSection from, ManxcatSection to, bool loadIO)
        {
            if (loadIO)
            {
                to.BaseResultDirectoryName = from.BaseResultDirectoryName;
                to.ControlDirectoryName = from.ControlDirectoryName;
                to.ClusterDirectory = from.ClusterDirectory;
                to.DataLabelsFileName = from.DataLabelsFileName;
                to.DistanceMatrixFile = from.DistanceMatrixFile;
                to.IndexFile = from.IndexFile;
                to.InitializationFileName = from.InitializationFileName;
                to.ReducedVectorOutputFileName = from.ReducedVectorOutputFileName;
                to.ResultDirectoryExtension = from.ResultDirectoryExtension;
                to.RotationLabelsFileName = from.RotationLabelsFileName;
                to.Selectedfixedpointfile = from.Selectedfixedpointfile;
                to.Selectedvariedpointfile = from.Selectedvariedpointfile;
                to.SummaryOutputFileName = from.SummaryOutputFileName;
                to.TimingOutputFileName = from.TimingOutputFileName;
                to.WeightingFileName = from.WeightingFileName;
            }

            to.AddonforQcomputation = from.AddonforQcomputation;
            to.CGResidualLimit = from.CGResidualLimit;
            to.ChisqChangePerPoint = from.ChisqChangePerPoint;
            to.Chisqnorm = from.Chisqnorm;
            to.ChisqPrintConstant = from.ChisqPrintConstant;
            to.Comment = from.Comment;
            to.ConsoleDebugOutput = from.ConsoleDebugOutput;
            to.ConversionInformation = from.ConversionInformation;
            to.ConversionOption = from.ConversionOption;
            to.DataPoints = from.DataPoints;
            to.DebugPrintOption = from.DebugPrintOption;
            to.Derivtest = from.Derivtest;
            to.DistanceFormula = from.DistanceFormula;
            to.DistanceProcessingOption = from.DistanceProcessingOption;
            to.Eigenvaluechange = from.Eigenvaluechange;
            to.Eigenvectorchange = from.Eigenvectorchange;
            to.Extradata1 = from.Extradata1;
            to.Extradata2 = from.Extradata2;
            to.Extradata3 = from.Extradata3;
            to.Extradata4 = from.Extradata4;
            to.ExtraOption1 = from.ExtraOption1;
            to.Extraprecision = from.Extraprecision;
            to.FixedPointCriterion = from.FixedPointCriterion;
            to.FletcherRho = from.FletcherRho;
            to.FletcherSigma = from.FletcherSigma;
            to.FullSecondDerivativeOption = from.FullSecondDerivativeOption;
            to.FunctionErrorCalcMultiplier = from.FunctionErrorCalcMultiplier;
            to.HistogramBinCount = from.HistogramBinCount;
            to.InitializationLoops = from.InitializationLoops;
            to.InitializationOption = from.InitializationOption;
            to.InitialSteepestDescents = from.InitialSteepestDescents;
            to.LocalVectorDimension = from.LocalVectorDimension;
            to.Maxit = from.Maxit;
            to.MinimumDistance = from.MinimumDistance;
            to.MPIIOStrategy = from.MPIIOStrategy;
            to.MPIperNodeCount = from.MPIperNodeCount;
            to.Nbadgo = from.Nbadgo;
            to.NodeCount = from.NodeCount;
            to.Omega = from.Omega;
            to.OmegaOption = from.OmegaOption;
            to.Pattern = from.Pattern;
            to.PowerIterationLimit = from.PowerIterationLimit;
            to.ProcessingOption = from.ProcessingOption;
            to.QgoodReductionFactor = from.QgoodReductionFactor;
            to.QHighInitialFactor = from.QHighInitialFactor;
            to.QLimitscalculationInterval = from.QLimitscalculationInterval;
            to.RotationOption = from.RotationOption;
            to.RunNumber = from.RunNumber;
            to.RunSetLabel = from.RunSetLabel;
            to.Selectedfixedpoints = from.Selectedfixedpoints;
            to.Selectedvariedpoints = from.Selectedvariedpoints;
            to.ThreadCount = from.ThreadCount;
            to.TimeCutmillisec = from.TimeCutmillisec;
            to.VariedPointCriterion = from.VariedPointCriterion;
            to.WeightingOption = from.WeightingOption;
            to.Write2Das3D = from.Write2Das3D;
        }

        private void resetConfigBtn_Click(object sender, EventArgs e)
        {
            CopyMDSConfiguration(_referenceMDSConfig, _confMgr.ManxcatSection, true);
            pGrid.Refresh();
            resetConfigBtn.Enabled = false;
        }

        private void nameTxt_TextChanged(object sender, EventArgs e)
        {
            ChangeConfigPaths();
            runBtn.Enabled = !(string.IsNullOrEmpty(nameTxt.Text) || string.IsNullOrWhiteSpace(nameTxt.Text));
            outTxt.Clear();
            errTxt.Clear();
            outTxt.Update();
            errTxt.Update();
            mainSplit.Panel2.Enabled = false;
        }

        private void processBar_Scroll(object sender, EventArgs e)
        {
            processTxt.Text = processBar.Value.ToString();
        }

        private void autoIncrChkBx_CheckedChanged(object sender, EventArgs e)
        {
            if (autoIncrChkBx.Checked)
            {
                nameTxt.ReadOnly = true;
                GenerateAutoName();
                ChangeConfigPaths();
                if (runBtn.Text.Equals("Rerun"))
                {
                    runBtn.Text = "Run";
                }
            }
            else
            {
                nameTxt.ReadOnly = false;
                ChangeConfigPaths();
                runIncBtn.Enabled = false;
                runIncBtn.Visible = false;
                // It's possible this may be rerun but since there was an event with name let's say it's run.
                // Otherwise unnecessarily complicated logic to just figure out the name.
                runBtn.Text = "Run";
            }
        }

        private void runBtn_Click(object sender, EventArgs e)
        {
            if (_runType == RunType.Local)
            {
                runJob();
            }
            else
            {
#if GREASY
                GreasyRunJobRemote();
#else
                RunJobRemote();
#endif
            }
        }

        #region GreasyCode

        private void GreasyRunJobRemote()
        {
            lock (_workInProgressLock)
            {
                if (!_workInProgress)
                {
                    PrepareRemoteRunPreEnv();

                    // UI events like exit/new/open with confirm have to wait till _runStable == true
                    PrepareRemoteRunPreUI();
                    AppendToOut(LogMessages.PreparingRun);
                    if (ValidateDirectories())
                    {
                        AppendToOut(LogMessages.DirValidated);
                        PopulateRunInfo();
                        AppendToOut(LogMessages.MetaInfoPopulated);
                        _runDir = Path.Combine(_runInfo.BaseDir, _runInfo.Name);
                        if (CreateRunDir())
                        {
                            AppendToOut(LogMessages.LocalRunDirCreated);
                            ConfigurationMgr remoteMgr = new ConfigurationMgr();
                            ManxcatSection remoteConfig = GenerateRemoteConfig();
                            remoteMgr.ManxcatSection = remoteConfig;
                            // Save remote config
                            remoteMgr.SaveAs(Path.Combine(_runDir, IOUtil.RemoteConfigFileName));
                            // Save local config
                            _confMgr.SaveAs(Path.Combine(_runDir, IOUtil.LocalConfigName));
                            AppendToOut(LogMessages.ConfigSaved);
                            // Save run info
                            _runInfo.SaveAs(Path.Combine(_runDir, _runInfo.Name + IOUtil.RunInfoExt));
                            AppendToOut(LogMessages.MetaInfoSaved);
                            AppendToOut(LogMessages.Done);

                            AppendToOut(LogMessages.MigratingToHn);
                            if (GreasyMigrateToHeadnode())
                            {
                                AppendToOut(LogMessages.Done);
                                IScheduler scheduler = new Scheduler();
                                scheduler.SetInterfaceMode(false, (IntPtr)0);
                                scheduler.Connect(_runInfo.HnName);

                                AppendToOut(LogMessages.CreatingRemoteRun);
                                ISchedulerJob job = CreateJob(scheduler);
                                scheduler.AddJob(job);
                                AppendToOut(LogMessages.Done);

                                // Create a background worker to submit and keep track of job
                                _jobTracker = new BackgroundWorker();
                                _jobTracker.DoWork += JobTrackerDoWork;
                                _jobTracker.RunWorkerAsync(new object[] { scheduler, job, true });
                                _jobTrackerBeginEvt.WaitOne(); // Blocks the UI thread till job tracker begins
                            }

                            // Todo: continue from here - old
                            // -- make directory in hn -- Done
                            // -- copy files to hn directory -- Done
                            // -- clusrun to copy files across cn  -- Note. Handle exisiting cn directories
                            // -- create mds task (w/ notify email) -- Note. add this too
                            // -- Show a cancel button with Note. functionality to add a cancel and sanitize task to job and kill manxcat or other tasks
                            // -- start polling thread              -- Note. continue from here + how to monitor (separate window or blocked main window)
                            //    -- show last refreshed time
                            //    -- if status is done, copy output back to local
                            // -- Remember to save job id and job status in run info
                            //    -- save runinfo again (seems like need to have a job status to check when opening previous runs)
                            // -- handle close
                        }
                        else
                        {
                            AppendToOut(LogMessages.RunAborted);
                            AppendToOut(LogMessages.Seperator);
                        }
                    }
                    else
                    {
                        AppendToOut(LogMessages.RunAborted);
                        AppendToOut(LogMessages.Seperator);
                    }
                }
                else
                {
                    MessageBox.Show(LogMessages.WorkInProgress, LogCategories.Information, MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
            }
        }

        private bool GreasyMigrateToHeadnode()
        {
            try
            {
                string remoteProjectDir = @"\\" + hnComboBx.SelectedValue + @"\" + targetDirTxt.Text.Replace(':', '$') +
                                          @"\" +
                                          nameTxt.Text;
                if (!Directory.Exists(remoteProjectDir))
                {
                    // Create remote project directory
                    Directory.CreateDirectory(remoteProjectDir);
                }
                
                // Copy config file
                File.Copy(Path.Combine(_runDir, IOUtil.RemoteConfigFileName),
                          Path.Combine(remoteProjectDir, IOUtil.RemoteConfigFileName), true);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            return true;
        }

        #endregion 

        #region RunJobRemote

        private void RunJobRemote()
        {
            // todo: continue from here - for rerun and remaining todos
            lock (_workInProgressLock)
            {
                if (!_workInProgress)
                {
                    PrepareRemoteRunPreEnv();
                   
                    // UI events like exit/new/open with confirm have to wait till _runStable == true
                    PrepareRemoteRunPreUI();
                    AppendToOut(LogMessages.PreparingRun);
                    // todo: continue from here - handle aborted cases
                    if (ValidateDirectories())
                    {
                        AppendToOut(LogMessages.DirValidated);
                        PopulateRunInfo();
                        AppendToOut(LogMessages.MetaInfoPopulated);
                        _runDir = Path.Combine(_runInfo.BaseDir, _runInfo.Name);
                        if (CreateRunDir())
                        {
                            AppendToOut(LogMessages.LocalRunDirCreated);
                            ConfigurationMgr remoteMgr = new ConfigurationMgr();
                            ManxcatSection remoteConfig = GenerateRemoteConfig();
                            remoteMgr.ManxcatSection = remoteConfig;
                            // Save remote config
                            remoteMgr.SaveAs(Path.Combine(_runDir, IOUtil.RemoteConfigFileName));
                            // Save local config
                            _confMgr.SaveAs(Path.Combine(_runDir, IOUtil.LocalConfigName));
                            AppendToOut(LogMessages.ConfigSaved);
                            // Save run info
                            _runInfo.SaveAs(Path.Combine(_runDir, _runInfo.Name + IOUtil.RunInfoExt));
                            AppendToOut(LogMessages.MetaInfoSaved);
                            AppendToOut(LogMessages.Done);

                            // Todo: improve here to avoid copying already existing files to existing projects
                            AppendToOut(LogMessages.MigratingToHn);
                            if (MigrateToHeadnode())
                            {
                                AppendToOut(LogMessages.Done);
                                IScheduler scheduler = new Scheduler();
                                scheduler.SetInterfaceMode(false, (IntPtr) 0);
                                scheduler.Connect(_runInfo.HnName);

                                AppendToOut(LogMessages.CreatingRemoteRun);
                                ISchedulerJob job = CreateJob(scheduler);
                                scheduler.AddJob(job);
                                AppendToOut(LogMessages.Done);

                                // Create a background worker to submit and keep track of job
                                _jobTracker = new BackgroundWorker();
                                _jobTracker.DoWork += JobTrackerDoWork;
                                _jobTracker.RunWorkerAsync(new object[] {scheduler, job, true});
                                _jobTrackerBeginEvt.WaitOne(); // Blocks the UI thread till job tracker begins
                            }

                            // Todo: continue from here - old
                            // -- make directory in hn -- Done
                            // -- copy files to hn directory -- Done
                            // -- clusrun to copy files across cn  -- Note. Handle exisiting cn directories
                            // -- create mds task (w/ notify email) -- Note. add this too
                            // -- Show a cancel button with Note. functionality to add a cancel and sanitize task to job and kill manxcat or other tasks
                            // -- start polling thread              -- Note. continue from here + how to monitor (separate window or blocked main window)
                            //    -- show last refreshed time
                            //    -- if status is done, copy output back to local
                            // -- Remember to save job id and job status in run info
                            //    -- save runinfo again (seems like need to have a job status to check when opening previous runs)
                            // -- handle close
                        }
                        else
                        {
                            AppendToOut(LogMessages.RunAborted);
                            AppendToOut(LogMessages.Seperator);
                        }
                    }
                    else
                    {
                        AppendToOut(LogMessages.RunAborted);
                        AppendToOut(LogMessages.Seperator);
                    }
                }
                else
                {
                    MessageBox.Show(LogMessages.WorkInProgress, LogCategories.Information, MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
            }
        }

        private void PrepareRemoteRunPreEnv()
        {
            _workInProgress = true;
            _runStableEvt.Reset();
            _jobTrackerStopEvt.Reset();
            _jobTrackerBeginEvt.Reset();
            _monitorEndEvt.Reset();
            _jobTrackerStopInformed = false;
            _fireOnState = true;
            _uiSessionEnding = false;

            _monitor = null;
            _jobTracker = null;
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

                if ((bool)args[2])
                {
                    AppendToOut(LogMessages.SubmittingRemoteRun);
                    scheduler.SubmitJob(job, null, null);

                    PrepareUIWhenJobRunning();

                    // Update reference manxcat configuration, so that it will have config for the latest run
                    CopyMDSConfiguration(_confMgr.ManxcatSection, _referenceMDSConfig, true);
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

        private delegate void PrepareUIWhenJobRunningDelegate();
        private void PrepareUIWhenJobRunning()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new PrepareUIWhenJobRunningDelegate(PrepareUIWhenJobRunning));
            }
            else
            {
                killBtn.Enabled = killBtn.Visible = true;
                runBtn.Visible = false;
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
                using(StreamWriter plotWriter = new StreamWriter(Path.Combine(_runDir, IOUtil.LocalPlotTxtFile)))
                {
                    char[] sep = new[]{' ','\t'};
                    string[] ptsSplits, clusSplits;
                    while (!clusReader.EndOfStream && !ptsReader.EndOfStream)
                    {
                        ptsSplits = ptsReader.ReadLine().Trim().Split(sep);
                        clusSplits = clusReader.ReadLine().Trim().Split(sep);
                        plotWriter.WriteLine(ptsSplits[0]+'\t'+ptsSplits[1]+'\t'+ptsSplits[2]+'\t'+ptsSplits[3]+'\t'+clusSplits[1]);
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

//        private void BringBackUI()
//        {
//            BringBackUI(string.Empty);
//        }

//        private delegate void BringBackUIDelegate(string message);
        private delegate void BringBackUIDelegate();
//        private void BringBackUI(string message)
        private void BringBackUI()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new BringBackUIDelegate(BringBackUI));
            }
            else
            {
                rootSplit.Panel1.Enabled = true;
                mainSplit.Panel1.Enabled = true;
                runBtn.Enabled = runBtn.Visible = true;
                killBtn.Enabled = killBtn.Visible = false;
                if (File.Exists(
                    Path.Combine(_runDir, IOUtil.ManxcatSimplePointsFileNamePrefix +
                                          Path.GetFileName(_confMgr.ManxcatSection.ReducedVectorOutputFileName))))
                {
                    showInPvizBtn.Enabled = showInPvizBtn.Visible = true;
                }

//                if (!string.IsNullOrEmpty(message))
//                {
//                    outTxt.Text = message;
//                }
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
                _monitor.RunWorkerCompleted += EndMonitor;
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

        private void EndMonitor(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("Monitor ending");
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

            if (File.Exists( copiedErrFile))
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
                for (int i =0; i < readLineCount; ++i)
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

        private void PrepareRemoteRunPreUI()
        {
            rootSplit.Panel1.Enabled = false;
            mainSplit.Panel1.Enabled = false;
            mainSplit.Panel2.Enabled = true;
            mainSplit.Panel2.Refresh();
            runBtn.Enabled = false;
            showRefPlotBtn.Enabled = showRefPlotBtn.Visible = false;
            rebuildBtn.Enabled = rebuildBtn.Visible = false;
            showInPvizBtn.Enabled = showInPvizBtn.Visible = false;
            outTxt.Clear();
            errTxt.Clear();
        }

        private ISchedulerJob CreateJob(IScheduler scheduler)
        {
            string headNode = hnComboBx.SelectedValue as string;
            scheduler.Connect(headNode);
            ISchedulerJob job = scheduler.CreateJob();
            // Todo: delimit the name for max allowable length or use some generated name
            job.Name = _runInfo.Name;
            job.UnitType = JobUnitType.Core;
            job.AutoCalculateMax = job.AutoCalculateMin = false;
            job.MaximumNumberOfCores = _runInfo.MaxCores;
            job.MinimumNumberOfCores = _runInfo.MaxCores;
            job.IsExclusive = true;

            foreach (string node in _runInfo.NodesList.Split(','))
            {
                job.RequestedNodes.Add(node);
            }

#if !GREASY
            ISchedulerTask lastSetupTask = AddSetupTasks(scheduler, job);
            AppendToOut(LogMessages.SetupTasksAdded);
            AddManxcatTask(scheduler, job, lastSetupTask);
#else
            AddManxcatTask(scheduler, job);
#endif
            AppendToOut(LogMessages.ManxcatTaskAdded);

            return job;
        }

#if GREASY
        private void AddManxcatTask(IScheduler scheduler, ISchedulerJob job)
#else
        private void AddManxcatTask(IScheduler scheduler, ISchedulerJob job, ISchedulerTask dependsOnTask)
#endif
        {
            string name = GenerateManxcatName();
            ISchedulerTask task = CreateSimpleEmptyTask(job, name);
            task.Name = name;
            task.WorkDirectory = _runInfo.HnProjectDir;
            task.StdOutFilePath = Path.Combine(_runInfo.HnProjectDir, IOUtil.ManxcatOutFileName);
            task.StdErrFilePath = Path.Combine(_runInfo.HnProjectDir, IOUtil.ManxcatErrFileName);
            task.MaximumNumberOfCores = task.MinimumNumberOfCores = _runInfo.MaxCores;

            // mpiexec /hosts {node-count} {hosts} {path-to-exe} /configFile={path-to-config} /nodeCount={node-count} /threadCount={thread-count}
            const string templateManxcatCmd =
                @"mpiexec /hosts {0} {1} {2} /configFile={3} /nodeCount={4} /threadCount={5}";
            task.CommandLine = string.Format(templateManxcatCmd, _runInfo.NodeCount, _runInfo.HostsList,
#if !GREASY
                                             Path.Combine(_runInfo.CnProjectDir, IOUtil.RemoteAppsDirName,
                                                          IOUtil.ManxcatExeName),
#else
                                            Path.Combine(@"F:\Salsa\saliya\apps\manxcat\11292011",
                                                          IOUtil.ManxcatExeName),
#endif

                                             Path.Combine(_runInfo.HnProjectDir, IOUtil.RemoteConfigFileName),
                                             _runInfo.NodeCount, _runInfo.ThreadCount);
            IStringCollection dependsOnTasks = scheduler.CreateStringCollection();
#if !GREASY
            dependsOnTasks.Add(dependsOnTask.Name);
            task.DependsOn = dependsOnTasks;
#endif
            job.AddTask(task);
        }

        private string GenerateManxcatName()
        {
            // Todo: generate a meaningful name for Manxcat by looking at the config
            return "Manxcat";
        }

        private static ISchedulerTask CreateSimpleEmptyTask(ISchedulerJob job, string name)
        {
            ISchedulerTask task = job.CreateTask();
            task.Name = name;
            task.IsExclusive = true;
            task.MaximumNumberOfCores = 1;
            task.MinimumNumberOfCores = 1;
            task.CommandLine = string.Empty;
            return task;
        }

        private ISchedulerTask AddSetupCmd(IScheduler scheduler, ISchedulerJob job, ISchedulerTask task, string cmd)
        {
            // todo: bug here
            const int maxCmdLength = 480; // Enforced by ISchedulerTask::CommandLine Property
            const string amp = " & ";
            if (string.IsNullOrEmpty(task.CommandLine))
            {
                // Note. Assume cmd.Length <= maxCmdLength. Otherwise have to abort (not implemented)
                task.CommandLine = cmd + amp;
            }
            else
            {
                if (task.CommandLine.Length + cmd.Length + amp.Length <= maxCmdLength)
                    // Good we have enough room to add the command
                {
                    task.CommandLine += cmd + amp;
                }
                else // Not enough room in this task to add the command
                {
                    // Add current task to the job
                    job.AddTask(task);
                    string prevTaskName = task.Name;
                    int idx = task.Name.LastIndexOf(' ');
                    string name = task.Name.Substring(0, idx) + " " + (int.Parse(task.Name.Substring(idx + 1)) + 1);
                    // Create a new task to add the command
                    task = CreateSimpleEmptyTask(job, name);
                    IStringCollection dependsOnTasks = scheduler.CreateStringCollection();
                    dependsOnTasks.Add(prevTaskName);
                    task.DependsOn = dependsOnTasks;
                    task.CommandLine = cmd + amp;
                }
            }
            return task;
        }

        // returns the last setup task so that manxcat task can depend on that
        // Todo: Handle existing folders
        private ISchedulerTask AddSetupTasks(IScheduler scheduler, ISchedulerJob job)
        {
            // Template command, i.e. {cmd} {ops} {args}
            const string templateCommand = @"{0} {1} {2}";
            string cnProjectDirWithDollar = _runInfo.CnProjectDir.Replace(':', '$');
            string[] nodes = _runInfo.NodesList.Split(',');

            string cmd;
            ISchedulerTask task = CreateSimpleEmptyTask(job, "Setup Task 0");
            foreach (string node in nodes)
            {
                string cnProjectDirUnc = @"\\" + node + @"\" + cnProjectDirWithDollar;
                cmd = string.Format(templateCommand, @"mkdir", string.Empty, cnProjectDirUnc);
                task = AddSetupCmd(scheduler, job, task, cmd);
                cmd = string.Format(templateCommand, @"mkdir", string.Empty,
                                    cnProjectDirUnc + @"\" + IOUtil.RemoteAppsDirName);
                task = AddSetupCmd(scheduler, job, task, cmd);
                cmd = string.Format(templateCommand, @"mkdir", string.Empty,
                                    cnProjectDirUnc + @"\" + IOUtil.RemoteInputDirName);
                task = AddSetupCmd(scheduler, job, task, cmd);
                cmd = string.Format(templateCommand, "xcopy", "/E /Y",
                                    Path.Combine(_runInfo.HnProjectDir, IOUtil.RemoteAppsDirName, "*.* ") +
                                    Path.Combine(cnProjectDirUnc, IOUtil.RemoteAppsDirName));
                task = AddSetupCmd(scheduler, job, task, cmd);
                cmd = string.Format(templateCommand, "xcopy", "/E /Y /d",
                                    Path.Combine(_runInfo.HnProjectDir, IOUtil.RemoteInputDirName, "*.* ") +
                                    Path.Combine(cnProjectDirUnc, IOUtil.RemoteInputDirName));
                task = AddSetupCmd(scheduler, job, task, cmd);
            }
            // This may not necessarily be the same task at the beginning
            job.AddTask(task);
            return task;
        }

        // Todo: improve migration when folder exists
        private bool MigrateToHeadnode()
        {
            try
            {
                string remoteProjectDir = @"\\" + hnComboBx.SelectedValue + @"\" + targetDirTxt.Text.Replace(':', '$') +
                                          @"\" +
                                          nameTxt.Text;
                if (Directory.Exists(remoteProjectDir))
                {
                    Directory.Delete(remoteProjectDir, true);
                }
                // Create remote project directory
                Directory.CreateDirectory(remoteProjectDir);
                string remoteAppsDir = Path.Combine(remoteProjectDir, IOUtil.RemoteAppsDirName);
                Directory.CreateDirectory(remoteAppsDir);
                string remoteInputDir = Path.Combine(remoteProjectDir, IOUtil.RemoteInputDirName);
                Directory.CreateDirectory(remoteInputDir);
                // Copy executables
                IOUtil.CopyFiles(_runInfo.AppDir, remoteAppsDir);
                // Copy input files
                CopyInputFilesToHeadNode(remoteInputDir);
                // Copy config file
                File.Copy(Path.Combine(_runDir, IOUtil.RemoteConfigFileName),
                          Path.Combine(remoteProjectDir, IOUtil.RemoteConfigFileName), true);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            return true;
        }

        // Todo: improve copy when files exist
        private void CopyInputFilesToHeadNode(string remoteInputDir)
        {
            ManxcatSection localConfig = _confMgr.ManxcatSection;
            if (File.Exists(localConfig.DataLabelsFileName))
            {
                File.Copy(localConfig.DataLabelsFileName,
                          Path.Combine(remoteInputDir, Path.GetFileName(localConfig.DataLabelsFileName)), true);
            }

            if (File.Exists(localConfig.DistanceMatrixFile))
            {
                File.Copy(localConfig.DistanceMatrixFile,
                          Path.Combine(remoteInputDir, Path.GetFileName(localConfig.DistanceMatrixFile)), true);
            }

            if (File.Exists(localConfig.IndexFile))
            {
                File.Copy(localConfig.IndexFile, Path.Combine(remoteInputDir, Path.GetFileName(localConfig.IndexFile)),
                          true);
            }

            if (File.Exists(localConfig.InitializationFileName))
            {
                File.Copy(localConfig.InitializationFileName,
                          Path.Combine(remoteInputDir, Path.GetFileName(localConfig.InitializationFileName)), true);
            }

            if (File.Exists(localConfig.RotationLabelsFileName))
            {
                File.Copy(localConfig.RotationLabelsFileName,
                          Path.Combine(remoteInputDir, Path.GetFileName(localConfig.RotationLabelsFileName)), true);
            }

            if (File.Exists(localConfig.Selectedfixedpointfile))
            {
                File.Copy(localConfig.Selectedfixedpointfile,
                          Path.Combine(remoteInputDir, Path.GetFileName(localConfig.Selectedfixedpointfile)), true);
            }

            if (File.Exists(localConfig.Selectedvariedpointfile))
            {
                File.Copy(localConfig.Selectedvariedpointfile,
                          Path.Combine(remoteInputDir, Path.GetFileName(localConfig.Selectedvariedpointfile)), true);
            }

            if (File.Exists(localConfig.WeightingFileName))
            {
                File.Copy(localConfig.WeightingFileName,
                          Path.Combine(remoteInputDir, Path.GetFileName(localConfig.WeightingFileName)), true);
            }
        }

        private ManxcatSection GenerateRemoteConfig()
        {
            ManxcatSection remoteConfig = new ManxcatSection();
            CopyMDSConfiguration(_confMgr.ManxcatSection, remoteConfig, true);
            ChangeConfigPaths(_runInfo.HnProjectDir, remoteConfig, true);
            return remoteConfig;
        }

        /* Create run dir */

        private bool CreateRunDir()
        {
            if (Directory.Exists(_runDir))
            {
                string msg = "A previous run by this name already exists.\nDo you want to overwrite its content?";
                if (MessageBox.Show(msg, "Run Exist", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) ==
                    System.Windows.Forms.DialogResult.No)
                {
                    MessageBox.Show(string.Format("{0} aborted", _runInfo.Name), "Run Aborted", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    return false;
                }
                else
                {
                    Directory.Delete(_runDir, true);
                }
            }
            Directory.CreateDirectory(_runDir);
            return true;
        }

        /* Populate run info */

        private void PopulateRunInfo()
        {
            _runInfo = new RunInfo();
            _runInfo.AppDir = mdsAppDirTxt.Text;
            _runInfo.AutoIncr = autoIncrChkBx.Checked;
            _runInfo.BaseDir = baseDirTxt.Text;
            _runInfo.ClusterFile = clusTxt.Text;

            int[] cnIndices = new int[cnListBx.SelectedIndices.Count];
            cnListBx.SelectedIndices.CopyTo(cnIndices, 0);
            _runInfo.CnIndices = cnIndices;
            _runInfo.MaxCores = GetMaximumCoreCount();
            _runInfo.NodeCount = cnListBx.SelectedIndices.Count;
            _runInfo.HnName = hnComboBx.SelectedItem as string;
            _runInfo.HnIndex = hnComboBx.SelectedIndex;
            _runInfo.HnProjectDir = @"\\" + hnComboBx.SelectedValue + @"\" + targetDirTxt.Text.Replace(':', '$') + @"\" +
                                    nameTxt.Text;
            _runInfo.TargetDir = targetDirTxt.Text;
            _runInfo.CnProjectDir = Path.Combine(targetDirTxt.Text, nameTxt.Text);
            _runInfo.NodesList = GenerateNodesList();
            _runInfo.HostsList = GenerateHostsList();

            _runInfo.IsRunSuccess = false;
            _runInfo.Name = nameTxt.Text;
            _runInfo.ProcessCount = processBar.Value;
            _runInfo.RunType = RunType.Remote;
            _runInfo.ThreadCount = threadBar.Value;

            _runInfo.IsRunCompleted = false;
            _runInfo.IsRunSuccess = false;
            _runInfo.OutLineReadCount = 0;
            _runInfo.ErrLineReadCount = 0;

            _runInfo.LocalOutStatusFile = Path.Combine(_runInfo.BaseDir, _runInfo.Name, IOUtil.LocalOutStatusFileName);
            _runInfo.LocalErrStatusFile = Path.Combine(_runInfo.BaseDir, _runInfo.Name, IOUtil.LocalErrStatusFileName);

            // todo: beta2 - remove these and use localstatus files above
            _runInfo.LocalOutFile = Path.Combine(_runInfo.BaseDir, _runInfo.Name, IOUtil.LocalOutFileName);
            _runInfo.LocalErrFile = Path.Combine(_runInfo.BaseDir, _runInfo.Name, IOUtil.LocalErrFileName);
        }

        private string GenerateHostsList()
        {
            string seperator = " " + processTxt.Text + " ";
            return GenerateList(seperator, false);
        }

        private string GenerateNodesList()
        {
            return GenerateList(",", true);
        }

        private string GenerateList(string seperator, bool removeLastSep)
        {
            StringBuilder sb = new StringBuilder();
            foreach (ISchedulerNode node in cnListBx.SelectedItems)
            {
                sb.Append(node.Name);
                sb.Append(seperator);
            }
            if (removeLastSep)
            {
                sb.Remove(sb.Length - seperator.Length, seperator.Length);
            }
            return sb.ToString();
        }

        /* Get maximum core count in selected nodes */

        private int GetMaximumCoreCount()
        {
            int maxCores = 0;
            foreach (ISchedulerNode node in cnListBx.SelectedItems)
            {
                maxCores += node.NumberOfCores;
            }
            return maxCores;
        }

        /* Validate relevent directories */

        private bool ValidateDirectories()
        {
            bool isValid = true;
            if (!Utils.ValidateMDSAppDir(mdsAppDirTxt.Text, true))
            {
                if (ChangeAppDir() == System.Windows.Forms.DialogResult.Cancel)
                {
                    MessageBox.Show(string.Format("{0} aborted", nameTxt.Text), "Run Aborted", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    isValid = false;
                }
            }

            if (!Utils.ValidateBaseDirectory(baseDirTxt.Text))
            {
                if (ChangeBaseDir() == System.Windows.Forms.DialogResult.Cancel)
                {
                    MessageBox.Show(string.Format("{0} aborted", nameTxt.Text), "Run Aborted", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    isValid = false;
                }
            }

            if (!isValid)
            {
                UndoPrepareRemoteRunPreEnv();
            }
            return isValid;
        }

        private void UndoPrepareRemoteRunPreEnv()
        {
            _workInProgress = false;
            _runStableEvt.Set();
            _jobTrackerBeginEvt.Set();
            _jobTrackerStopEvt.Set();
            _monitorEndEvt.Set();
            _jobTrackerStopInformed = true;
            _fireOnState = false;
            _uiSessionEnding = false;

            _monitor = null;
            _jobTracker = null;
        }

        #endregion

        #region RunJobLocal

        /* Local run method */

        private void runJob()
        {
            _isRunSuccess = true;

            if (!Utils.ValidateMDSAppDir(mdsAppDirTxt.Text, true))
            {
                if (ChangeAppDir() == System.Windows.Forms.DialogResult.Cancel)
                {
                    MessageBox.Show(string.Format("{0} aborted", nameTxt.Text), "Run Aborted", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    return;
                }
            }

            if (!Utils.ValidateBaseDirectory(baseDirTxt.Text))
            {
                if (ChangeBaseDir() == System.Windows.Forms.DialogResult.Cancel)
                {
                    MessageBox.Show(string.Format("{0} aborted", nameTxt.Text), "Run Aborted", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    return;
                }
            }

            _runInfo = new RunInfo();
            _runInfo.AppDir = mdsAppDirTxt.Text;
            _runInfo.BaseDir = baseDirTxt.Text;
            _runInfo.Name = nameTxt.Text;
            _runInfo.ClusterFile = clusTxt.Text;
            _runInfo.AutoIncr = autoIncrChkBx.Checked;
            _runInfo.ProcessCount = processBar.Value;

            _runDir = Path.Combine(_runInfo.BaseDir, _runInfo.Name);
            if (Directory.Exists(_runDir))
            {
                string msg = "A previous run by this name already exists.\nDo you want to overwrite its content?";
                if (MessageBox.Show(msg, "Run Exist", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) ==
                    System.Windows.Forms.DialogResult.No)
                {
                    MessageBox.Show(string.Format("{0} aborted", _runInfo.Name), "Run Aborted", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    return;
                }
            }
            else
            {
                Directory.CreateDirectory(_runDir);
            }


            // Todo: saliya - extremely quick hack to set threads
            string cmd = @"mpiexec -np {0} {1}\ManxcatMDS.exe /configFile={2}\config.xml /nodeCount=1 /threadCount={3}";
            cmd = string.Format(cmd, _runInfo.ProcessCount, _runInfo.AppDir, _runDir, threadTxt.Text);

            using (StreamWriter writer = new StreamWriter(Path.Combine(_runDir, "go.bat")))
            {
                writer.WriteLine("@echo off");
                writer.WriteLine(cmd);
                writer.WriteLine("PAUSE");
            }

            // Note. Pre UI effects
            runBtn.Enabled = false;
            mainSplit.Panel1.Enabled = false;
//            mainSplit.Panel2.Enabled = true;
            showInPvizBtn.Enabled = false;
            showRefPlotBtn.Enabled = false;
            rebuildBtn.Enabled = false;
            outTxt.Clear();
            outTxt.Update();
            errTxt.Clear();
            errTxt.Update();

            // Save config
            _confMgr.SaveAs(Path.Combine(_runDir, "config.xml"));
            // Save runinfo
            _runInfo.IsRunSuccess = _isRunSuccess;
            _runInfo.SaveAs(Path.Combine(_runDir, _runInfo.Name + ".infx"));


            outTxt.Text = string.Format("\n{0} is starting ...\n", _runInfo.Name);
            outTxt.Update();

            _mdsRunnable = new MDSRunnable(_runDir, proc_OutputDataReceived, proc_ErrorDataReceived, proc_AfterEffects);
            new Thread(_mdsRunnable.run).Start();
        }


        private delegate void PostProcEffectsDelegate();

        public void proc_AfterEffects()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new PostProcEffectsDelegate(proc_AfterEffects));
            }
            else
            {
                // Update reference manxcat configuration, so that it will have config for the latest run.
                CopyMDSConfiguration(_confMgr.ManxcatSection, _referenceMDSConfig, true);
                if (_isRunSuccess)
                {
                    // Create a plotviz file
                    GeneratePlotViz(_runInfo, false);
                }

                showInPvizBtn.Enabled = _isRunSuccess;
                rebuildBtn.Enabled = _isRunSuccess;
                showRefPlotBtn.Enabled = _isRunSuccess;

                runBtn.Text = "Rerun";
                runBtn.Enabled = true;

                if (autoIncrChkBx.Checked)
                {
                    runIncBtn.Visible = true;
                    runIncBtn.Enabled = true;
                }

                mainSplit.Panel1.Enabled = true;
                mainSplit.Panel2.Enabled = true;

                // Update runinfo
                _runInfo.IsRunSuccess = _isRunSuccess;
                _runInfo.SaveAs(Path.Combine(_runDir, _runInfo.Name + ".infx"));
            }
        }

        // Note. For more information see: http://www.codeproject.com/KB/cs/AOPInvokeRequired.aspx
        // http://stackoverflow.com/questions/142003/cross-thread-operation-not-valid-control-accessed-from-a-thread-other-than-the-t
        private delegate void proc_ErrorDataReceivedDelegate(object sender, DataReceivedEventArgs e);

        private void proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                _isRunSuccess = false;
            }
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new proc_ErrorDataReceivedDelegate(proc_ErrorDataReceived), new object[2] {sender, e});
                return;
            }
            if (!string.IsNullOrEmpty(e.Data))
            {
                errTxt.AppendText(Environment.NewLine + e.Data);
            }
        }

        private delegate void proc_OutputDataReceivedDelegate(object sender, DataReceivedEventArgs e);

        private void proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data) && e.Data.Contains("job aborted"))
            {
                _isRunSuccess = false;
            }

            if (this.outTxt.InvokeRequired)
            {
                this.Invoke(new proc_OutputDataReceivedDelegate(proc_OutputDataReceived), new[] {sender, e});
            }
            else
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    if (e.Data.StartsWith("Loop"))
                    {
                        outTxt.AppendText(Environment.NewLine + Environment.NewLine + e.Data);
                    }
                    else
                    {
                        outTxt.AppendText(Environment.NewLine +
                                          e.Data.Replace("\n", string.Format("{0}", Environment.NewLine)));
                    }
                }
            }
        }

        #endregion LocalRunJob

        private string GeneratePlotViz(RunInfo runInfo, bool refPlot)
        {
            ManxcatSection config = _confMgr.ManxcatSection;
            // todo: this assumes pointsFile is in simply x,y,z format. Generalize this later to colon input as well.
            string pointsFile = refPlot
                                    ? config.InitializationFileName
                                    : Path.Combine(runInfo.BaseDir, runInfo.Name,
                                                   string.Format("SIMPLE{0}",
                                                                 Path.GetFileName(config.ReducedVectorOutputFileName)));
            string plotFile = Path.Combine(_runInfo.BaseDir, _runInfo.Name,
                                           refPlot
                                               ? string.Format("referene-plot-{0}.pviz",
                                                               Path.GetFileNameWithoutExtension(
                                                                   config.InitializationFileName))
                                               : string.Format("{0}-plot.pviz", runInfo.Name));

            PlotVizDataFile pvdf;
            if (File.Exists(pointsFile))
            {
                if (File.Exists(config.IndexFile))
                {
                    pvdf = File.Exists(runInfo.ClusterFile)
                               ? PlotVizDataFile.Build(config.IndexFile, runInfo.ClusterFile, pointsFile)
                               : PlotVizDataFile.Build(config.IndexFile, pointsFile);
                }
                else
                {
                    pvdf = PlotVizDataFile.Build(pointsFile);
                }
                pvdf.Save(plotFile);
                return plotFile;
            }
            return string.Empty;
        }

//        private void GeneratePlotViz(RunInfo runInfo) 
//        { 
//            ManxcatSection config = _confMgr.ManxcatSection;
//            String pointsFile = Path.Combine(runInfo.BaseDir, runInfo.Name,
//                                             string.Format("SIMPLE{0}",
//                                                           Path.GetFileName(config.ReducedVectorOutputFileName)));
//            PlotVizDataFile pvdf;
//            if (File.Exists(pointsFile))
//            {
//                if (File.Exists(config.IndexFile))
//                {
//                    pvdf = File.Exists(runInfo.ClusterFile)
//                               ? PlotVizDataFile.Build(config.IndexFile, runInfo.ClusterFile, pointsFile)
//                               : PlotVizDataFile.Build(config.IndexFile, pointsFile);
//                }
//                else
//                {
//                    pvdf = PlotVizDataFile.Build(pointsFile);
//                }
//                pvdf.Save(Path.Combine(runInfo.BaseDir, runInfo.Name, string.Format("{0}-plot.pviz", runInfo.Name)));
//            }
//        }


        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            PrepareNewRun();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            OpenRun();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrepareNewRun();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenRun();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void clusBrowseBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "(*.txt)|*.txt";
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    clusTxt.Text = dlg.FileName;
                }
            }
        }

        private void showInPvizBtn_Click(object sender, EventArgs e)
        {
            string localPlotFile = Path.Combine(_runDir, IOUtil.LocalPlotTxtFile);
            string ptsFile = Path.Combine(_runDir, IOUtil.ManxcatSimplePointsFileNamePrefix +
                                                   Path.GetFileName(_confMgr.ManxcatSection.ReducedVectorOutputFileName));
            if (File.Exists(ptsFile))
            {
                if (string.IsNullOrEmpty(clusTxt.Text))
                {
                    if (File.Exists(localPlotFile))
                    {
                        File.Delete(localPlotFile);
                    }
                    _runInfo.ClusterFile = string.Empty;
                    _runInfo.SaveAs(Path.Combine(_runDir, _runInfo.Name + IOUtil.RunInfoExt));
                    ShowInPviz(ptsFile);
                }
                else if (File.Exists(clusTxt.Text))
                {
                    if (!_runInfo.ClusterFile.Equals(clusTxt.Text))
                    {
                        if (File.Exists(localPlotFile))
                        {
                            File.Delete(localPlotFile);
                        }
                        _runInfo.ClusterFile = clusTxt.Text;
                        _runInfo.SaveAs(Path.Combine(_runDir, _runInfo.Name + IOUtil.RunInfoExt));
                        CrealteLocalPlotFile(ptsFile, clusTxt.Text);
                    }

                    if (!File.Exists(localPlotFile))
                    {
                        CrealteLocalPlotFile(ptsFile, _runInfo.ClusterFile);
                    }
                    ShowInPviz(localPlotFile);
                }
                else
                {
                    MessageBox.Show(LogMessages.NoClusterFile, LogCategories.Error, MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }

        private void ShowInPviz(string plotFile)
        {
            if (!File.Exists(_settings.DefPlotVizExe))
            {
                // todo: fill here - if plotviz couldn't be found
                //                using (ConfigDlg dlg = new ConfigDlg())
                _settings.Save();
            }

            var start = new ProcessStartInfo(_settings.DefPlotVizExe)
                            {
                                UseShellExecute = true,
                                RedirectStandardOutput = false,
                                CreateNoWindow = false,
                                Arguments = plotFile
                            };
            Process.Start(start);
        }

        private void rebuildBtn_Click(object sender, EventArgs e)
        {
            string coordinatesFile = Path.Combine(
                Path.GetDirectoryName(_referenceMDSConfig.ReducedVectorOutputFileName),
                string.Format("SIMPLE{0}", Path.GetFileName(_referenceMDSConfig.ReducedVectorOutputFileName)));
            using (var dlg = new PlotVizBuilderDlg(_referenceMDSConfig.IndexFile, _runInfo.ClusterFile, coordinatesFile)
                )
            {
                dlg.ShowDialog();
            }
        }

        private void runIncBtn_Click(object sender, EventArgs e)
        {
            GenerateAutoName();
            ChangeConfigPaths();
            runJob();
        }

        private void showRefBtn_Click(object sender, EventArgs e)
        {
            string plotFile = GeneratePlotViz(_runInfo, true);
            if (!string.IsNullOrEmpty(plotFile))
            {
                ShowInPviz(plotFile);
            }
            else
            {
                MessageBox.Show("Couldn't find initialization file to build the reference plot", "No Reference Plot",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void pGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            outTxt.Clear();
            errTxt.Clear();
            outTxt.Update();
            errTxt.Update();
            mainSplit.Panel2.Enabled = false;
        }

        private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
//            _mdsRunnable.kill();
            EndUISession();
        }

        private void EndUISession()
        {
            if (_runType == RunType.Remote)
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
        }

        private void setDefLinkLbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ChangeTargetDir();
        }

        private void ChangeTargetDir()
        {
            if (!_settings.DefTargetDir.Equals(targetDirTxt.Text))
            {
                _settings.DefTargetDir = targetDirTxt.Text;
                _settings.Save();
            }
        }

        private void localRunRadio_CheckedChanged(object sender, EventArgs e)
        {
            ChangeRunType();
        }

        private void remoteRunRadio_CheckedChanged(object sender, EventArgs e)
        {
            ChangeRunType();
        }

        private void hnComboBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            HeadNodeChanged();
        }

        private void HeadNodeChanged()
        {
            string headNode = hnComboBx.SelectedValue as string;
            using (IScheduler scheduler = new Scheduler())
            {
                scheduler.SetInterfaceMode(false, (IntPtr)0);
                scheduler.Connect(headNode);
                ISchedulerNode[] nodes = HPCUtil.GetComputeNodes(scheduler);
                cnBindingSource.DataSource = nodes;
            }
        }

        private void selectAllLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ListBoxUtil.SelectAll(cnListBx);
        }

        private void inverseLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ListBoxUtil.SelectInverse(cnListBx);
        }

        private void clearLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cnListBx.ClearSelected();
        }

        private void threadBar_Scroll(object sender, EventArgs e)
        {
            threadTxt.Text = threadBar.Value.ToString();
        }

        private void ChangeRunType()
        {
            _runType = remoteRunRadio.Checked ? RunType.Remote : RunType.Local;
            hnGroupBx.Enabled = remoteRunRadio.Checked;
            cnGroupBx.Enabled = remoteRunRadio.Checked;
            targetDirGroupBx.Enabled = remoteRunRadio.Checked;
            if (remoteRunRadio.Checked)
            {
                hnComboBx.DataSource = _headNodes;
            }
            ChangeConfigPaths();
        }

        private void LoadSettings()
        {
            if (File.Exists(Settings.SettingsFileName))
            {
                _settings = Settings.LoadSettings(Settings.SettingsFileName);
            }
            else
            {
                _settings = new Settings();
                _settings.Save();
            }

            try
            {
                // Load headnodes once and keep it saved till window is open
                _headNodes = HPCUtil.LoadHeadNodes();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                // Todo: This was necessary when IU VPN was not available. See if it can be fixed somehow.
                // Loading default headnode set.
                _headNodes =
                    new[]
                        {
                            "D2I-HPC-01.ads.iu.edu",
                            "bl-uits-cloud1.ads.iu.edu",
                            "xen73.ads.iu.edu",
                            "i64.ads.iu.edu",
                            "CGLVM1.ads.iu.edu",
                            "CGLVM2.ads.iu.edu",
                            "bl-soic-mugo.ads.iu.edu",
                            "bl-chem-mfj48.ads.iu.edu",
                            "TEMPEST.ads.iu.edu",
                            "IU-CGL-HPC00.ads.iu.edu",
                            "STORM.ads.iu.edu"
                        };
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

        private void processBar_ValueChanged(object sender, EventArgs e)
        {
        }

        private void threadBar_ValueChanged(object sender, EventArgs e)
        {
        }

        private void killBtn_Click(object sender, EventArgs e)
        {
            SendKillSignal();
        }

        private void SendKillSignal()
        {
            using (IScheduler scheduler = new Scheduler())
            {
                scheduler.Connect(_runInfo.HnName);
                scheduler.SetInterfaceMode(false, (IntPtr) 0);

                ISchedulerJob job = scheduler.OpenJob(_runInfo.JobId);
                job.Refresh();

                if (job.State != JobState.Finished &&
                    job.State != JobState.Canceled &&
                    job.State != JobState.Failed)
                {
                    scheduler.CancelJob(_runInfo.JobId, "User Cancelled");
                }
                        
            }
            killBtn.Enabled = killBtn.Visible = false;

        }
    }
}