using System;
using System.Collections;
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

    public enum ApplicationType
    {
        Manxcat,
        PairwiseClustering,
        SequentialSponge
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

        private static string _autoNamePrefix = ConstantNames.DefaultManxcatRunNamePrefix;
        private static Regex _autoNamePrefixRegex = new Regex(@"^" + ConstantNames.DefaultManxcatRunNamePrefix + @"\d+$");

        private Settings _settings;
        private ConfigurationMgr _confMgr;
        private ManxcatSection _referenceManxcatConfig;
        private PairwiseSection _referencePairwiseConfig;
        private DAVectorSpongeSection _referenceSpongeSection;

        private Hashtable _knownFiles;

        private RunInfo _runInfo;

        private bool _isRunSuccess = false; // usage in local run only
        private string _runDir;

        // Todo: Replace with a background worker if possible for local runs
        private MDSRunnable _mdsRunnable;

        private BackgroundWorker _monitor;
        private BackgroundWorker _jobTracker;

        private RunType _runType;
        private ApplicationType _appType;
        private string[] _headNodes;
        private Dictionary<int, string> _oldHeadNodes;
        

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
            _referenceManxcatConfig = new ManxcatSection();
            _referencePairwiseConfig = new PairwiseSection();
            _referenceSpongeSection = new DAVectorSpongeSection();
            _runInfo = null;
            _isRunSuccess = false; // usage in local run only
            _runDir = string.Empty;
            _mdsRunnable = null; // usage in local run only
            _runType = RunType.Remote;
            _appType = ApplicationType.Manxcat;

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
            targetDirTxt.Text = _settings.DefaultTargetDirectory;

            /* Run config UI changes */
            manxcatRadio.Checked = true;
            manxcatRadio.Enabled = pairwiseRadio.Enabled =  spongeRadio.Enabled = true;
            baseDirTxt.Text = _settings.DefaultBaseDirectory;
            processBar.Value = 1;
            processTxt.Text = processBar.Value.ToString();
            threadBar.Value = 1;
            threadTxt.Text = threadBar.Value.ToString();

            // todo: beta2 - visible autoIncr in beta2
            autoIncrChkBx.Enabled = autoIncrChkBx.Visible = false;
//            autoIncrChkBx.Checked = true;
            killBtn.Enabled = killBtn.Visible = false;
            runBtn.Enabled = runBtn.Visible = true;

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
                    // For compatibility reasons
                    RunInfo tmp = RunInfo.LoadRunInfo(dlg.FileName);
                    if (!tmp.HnName.Contains("TEMPEST"))
                    {
                        Console.WriteLine("This run cannot be opened because the cluster is no longer available.");
                        return;
                    }
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
                    clusTxt.Text = _runInfo.ComplementryTextFile;
                    processBar.Value = _runInfo.ProcessCount;
                    processTxt.Text = _runInfo.ProcessCount.ToString();
                    autoIncrChkBx.Checked = _runInfo.AutoIncr;

                    _confMgr =
                        ConfigurationMgr.LoadConfiguration(Path.Combine(_runInfo.BaseDir, _runInfo.Name, "config.xml"),
                                                           false);
                    _referenceManxcatConfig = new ManxcatSection();
                    CopyManxcatConfiguration(_confMgr.ManxcatSection, _referenceManxcatConfig, true);
                    pGrid.SelectedObject = _confMgr.ManxcatSection;

                    string summaryFile = _referenceManxcatConfig.SummaryOutputFileName;
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
                                        string copiedOutFile = string.Empty;
                                        string copiedErrFile  = string.Empty;

                                        if (_appType == ApplicationType.Manxcat)
                                        {
                                            copiedOutFile = Path.Combine(_runDir, IOUtil.ManxcatOutFileName);
                                            copiedErrFile = Path.Combine(_runDir, IOUtil.ManxcatErrFileName);
                                        }
                                        else if (_appType == ApplicationType.PairwiseClustering)
                                        {
                                            copiedOutFile = Path.Combine(_runDir, IOUtil.PairwiseOutFileName);
                                            copiedErrFile = Path.Combine(_runDir, IOUtil.PairwiseErrFileName);
                                        }
                                        else if (_appType == ApplicationType.SequentialSponge)
                                        {
                                            copiedOutFile = Path.Combine(_runDir, IOUtil.SpongeOutFileName);
                                            copiedErrFile = Path.Combine(_runDir, IOUtil.SpongeErrFileName);
                                        }

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
            if (_runInfo.AppType == ApplicationType.Manxcat)
            {
                ChangeManxcatConfigPaths(_runDir, _confMgr.ManxcatSection, false);
                _referenceManxcatConfig = new ManxcatSection();
                CopyManxcatConfiguration(_confMgr.ManxcatSection, _referenceManxcatConfig, true);
            }
            else if (_runInfo.AppType == ApplicationType.PairwiseClustering)
            {
                ChangePairwiseConfigPaths(_runDir, _confMgr.PairwiseSection, false);
                _referencePairwiseConfig = new PairwiseSection();
                CopyPairwiseConfiguration(_confMgr.PairwiseSection, _referencePairwiseConfig, true);
            }
            else if (_runInfo.AppType == ApplicationType.SequentialSponge)
            {
                ChangeSpongeConfigPaths(_runDir, _confMgr.DAVectorSpongeSection, false);
                _referenceSpongeSection = new DAVectorSpongeSection();
                CopySpongeConfiguration(_confMgr.DAVectorSpongeSection, _referenceSpongeSection, true);
            }
            _confMgr.SaveAs(Path.Combine(_runDir, IOUtil.LocalConfigName));
            _isRunSuccess = _runInfo.IsRunSuccess; // Usage in local run only
            _appType = _runInfo.AppType;
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
                if (_runInfo.AppType == ApplicationType.Manxcat)
                {
                    _runInfo.AppDir = _settings.DefaultManxcatAppDirectory;
                }
                else if (_runInfo.AppType == ApplicationType.PairwiseClustering)
                {
                    _runInfo.AppDir = _settings.DefaultPairwiseAppDirectory;
                }
                else if (_runInfo.AppType == ApplicationType.SequentialSponge)
                {
                    _runInfo.AppDir = _settings.DefaultSpongeAppDirectory;
                }
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
            
            hnComboBx.SelectedIndex = 0; // Only tempest is present as a cluster
            cnListBx.ClearSelected();
            cnListBx.BeginUpdate();
            foreach (int cnIndex in _runInfo.CnIndices)
            {
                cnListBx.SelectedIndices.Add(cnIndex);
            }
            cnListBx.EndUpdate();
            targetDirTxt.Text = _runInfo.TargetDir;

            switch (_appType)
            {
                case ApplicationType.Manxcat:
                    manxcatRadio.Enabled = manxcatRadio.Checked = true;
                    break;
                case ApplicationType.PairwiseClustering:
                    pairwiseRadio.Enabled = pairwiseRadio.Checked = true;
                    break;
                    case ApplicationType.SequentialSponge:
                    spongeRadio.Enabled = spongeRadio.Checked = true;
                    break;
                default:
                    throw new Exception("Undefined application type: " + _appType);

            }

            appDirTxt.Text = _runInfo.AppDir;

            // baseDirTxt setting should come before nameTxt setting
            baseDirTxt.Text = _runInfo.BaseDir;
            nameTxt.Text = _runInfo.Name;
            
            processBar.Value = _runInfo.ProcessCount;
            processTxt.Text = processBar.Value.ToString();
            threadBar.Value = _runInfo.ThreadCount;
            threadTxt.Text = threadBar.Value.ToString();

            complementryTextFileTxt.Text = _runInfo.ComplementryTextFile;

            if (_appType == ApplicationType.Manxcat)
            {
                pGrid.SelectedObject = _confMgr.ManxcatSection;
            }
            else if (_appType == ApplicationType.PairwiseClustering)
            {
                pGrid.SelectedObject = _confMgr.PairwiseSection;
            }
            else if (_appType == ApplicationType.SequentialSponge)
            {
                pGrid.SelectedObject = _confMgr.DAVectorSpongeSection;
            }
            pGrid.Refresh();

            runBtn.Enabled = runBtn.Visible = _runInfo.IsRunCompleted;
            killBtn.Enabled = killBtn.Visible = !_runInfo.IsRunCompleted;
            runIncBtn.Enabled = runIncBtn.Visible = false;


            string alternativeOutFileName = string.Empty;
            string alternativeErrFileName = string.Empty;

            if (_appType == ApplicationType.Manxcat)
            {
                alternativeOutFileName = IOUtil.ManxcatOutFileName;
                alternativeErrFileName = IOUtil.ManxcatErrFileName;
            }
            else if (_appType == ApplicationType.PairwiseClustering)
            {
                alternativeOutFileName = IOUtil.PairwiseOutFileName;
                alternativeErrFileName = IOUtil.PairwiseErrFileName;
            }
            else if (_appType == ApplicationType.SequentialSponge)
            {
                alternativeOutFileName = IOUtil.SpongeOutFileName;
                alternativeErrFileName = IOUtil.SpongeErrFileName;
            }

            outTxt.Clear();
            string fileToRead = string.Empty;
            if (File.Exists(_runInfo.LocalOutFile))
            {
                fileToRead = _runInfo.LocalOutFile;
                
            }
            else if (File.Exists(Path.Combine(_runDir, alternativeOutFileName)))
            {
                fileToRead = alternativeOutFileName;
                _runInfo.OutLineReadCount = 0;
                using (StreamWriter outWriter = new StreamWriter(_runInfo.LocalOutFile))
                {
                    using (StreamReader alternativeOutReader = new StreamReader(Path.Combine(_runDir, alternativeOutFileName)))
                    {
                        while (!alternativeOutReader.EndOfStream)
                        {
                            outWriter.WriteLine(alternativeOutReader.ReadLine());
                            ++_runInfo.OutLineReadCount;
                        }
                    }
                }
            }
            if (File.Exists(fileToRead))
            {
                outTxt.Text = @"Loading content ...";
                outTxt.Refresh();
                using (StreamReader reader = new StreamReader(fileToRead))
                {
                    StringBuilder sb = new StringBuilder();
                    while (!reader.EndOfStream)
                    {
                        sb.AppendLine(reader.ReadLine());
                    }
                    outTxt.Text = sb.ToString();
                }
            }
            outTxt.Refresh();

            fileToRead = string.Empty;
            errTxt.Clear();
            
            if (File.Exists(_runInfo.LocalErrFile))
            {
                fileToRead = _runInfo.LocalErrFile;
            }
            else if(File.Exists(Path.Combine(_runDir, alternativeErrFileName)))
            {
                fileToRead = alternativeErrFileName;
                _runInfo.ErrLineReadCount = 0;
                using (StreamWriter errWriter = new StreamWriter(_runInfo.LocalErrFile))
                {
                    using (StreamReader alternativeErrReader = new StreamReader(Path.Combine(_runDir, alternativeErrFileName)))
                    {
                        while (!alternativeErrReader.EndOfStream)
                        {
                            errWriter.WriteLine(alternativeErrReader.ReadLine());
                            ++_runInfo.ErrLineReadCount;
                        }
                    }
                }
            }

            if (File.Exists(fileToRead))
            {
                errTxt.Text =  @"Loading content ...";
                using (StreamReader reader = new StreamReader(fileToRead))
                {
                    StringBuilder sb = new StringBuilder();
                    while (!reader.EndOfStream)
                    {
                        sb.AppendLine(reader.ReadLine());
                    }
                    errTxt.Text = sb.ToString();
                }
            }
            errTxt.Refresh();

            showInPvizBtn.Enabled =
                showInPvizBtn.Visible = (_appType == ApplicationType.Manxcat && _runInfo.IsRunCompleted && _runInfo.IsRunSuccess && File.Exists(
                    Path.Combine(_runDir, IOUtil.ManxcatSimplePointsFileNamePrefix +
                                          Path.GetFileName(_confMgr.ManxcatSection.ReducedVectorOutputFileName)))) ||
                                          (_appType == ApplicationType.PairwiseClustering && Directory.GetFiles(_runDir,
                                                               Path.GetFileNameWithoutExtension(
                                                                   _confMgr.PairwiseSection.ClusterFile) + "*.txt").Length > 0);
            
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
            if (Directory.Exists(baseDirTxt.Text))
            {
                IEnumerable<string> dirs =
                    Directory.GetDirectories(baseDirTxt.Text).Where<string>(
                        x => _autoNamePrefixRegex.IsMatch(x.Substring(x.LastIndexOf(Path.DirectorySeparatorChar) + 1)));
                int maxIdx = dirs.Aggregate<string, int>(-1, (max, x) =>
                                                                 {
                                                                     int idx =
                                                                         int.Parse(
                                                                             x.Substring(
                                                                                 x.LastIndexOf(
                                                                                     Path.DirectorySeparatorChar) +
                                                                                 _autoNamePrefix.Length + 1));
                                                                     return idx >= max ? idx : max;
                                                                 });
                nameTxt.Text = _autoNamePrefix + (maxIdx + 1);
            }
        }

        private void ChangeManxcatConfigPaths(string projectPath, ManxcatSection config, bool isRemote)
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
                    fname = IOUtil.ManxcatDataLabelsFilePrefix + Path.GetFileName(config.DataLabelsFileName);
                    config.DataLabelsFileName = Path.Combine(projectPath, IOUtil.RemoteInputDirName, fname);
                }

                // Treat distance file path differently as it needs to be in every compute node
                if (!string.IsNullOrEmpty(config.DistanceMatrixFile))
                {
                    if (!config.DistanceMatrixFile.StartsWith("$"))
                    {
                        fname = IOUtil.ManxcatDistanceFilePrefix + Path.GetFileName(config.DistanceMatrixFile);
                        int idxOfDollar = projectPath.IndexOf('$');
                        projectPath = projectPath.Substring(idxOfDollar - 1).Replace('$', ':');
                        config.DistanceMatrixFile = Path.Combine(projectPath, IOUtil.RemoteInputDirName, fname);
                    }
                    else
                    {
                        // Indicates a known matrix already in cluster
                        fname = config.DistanceMatrixFile.Substring(1); // excludes the $ symbol
                        if (_knownFiles.ContainsKey(fname))
                        {
                            config.DistanceMatrixFile = (string) _knownFiles[fname];
                        }
                    }
                }

                if (!string.IsNullOrEmpty(config.ClusterFile))
                {
                    fname = IOUtil.ManxcatClusterFilePrefix + Path.GetFileName(config.ClusterFile);
                    config.ClusterFile = Path.Combine(projectPath, IOUtil.RemoteInputDirName, fname);
                }

                if (!string.IsNullOrEmpty(config.IndexFile))
                {
                    fname = IOUtil.ManxcatIndexFilePrefix + Path.GetFileName(config.IndexFile);
                    config.IndexFile = Path.Combine(projectPath, IOUtil.RemoteInputDirName, fname);
                }

                if (!string.IsNullOrEmpty(config.InitializationFileName))
                {
                    fname = IOUtil.ManxcatInitializationFilePrefix + Path.GetFileName(config.InitializationFileName);
                    config.InitializationFileName = Path.Combine(projectPath, IOUtil.RemoteInputDirName, fname);
                }

                if (!string.IsNullOrEmpty(config.RotationLabelsFileName))
                {
                    fname = IOUtil.ManxcatRotationLabelsFilePrefix + Path.GetFileName(config.RotationLabelsFileName);
                    config.RotationLabelsFileName = Path.Combine(projectPath, IOUtil.RemoteInputDirName, fname);
                }

                if (!string.IsNullOrEmpty(config.Selectedfixedpointfile))
                {
                    fname = IOUtil.ManxcatSelectedFixedPointsFilePrefix + Path.GetFileName(config.Selectedfixedpointfile);
                    config.Selectedfixedpointfile = Path.Combine(projectPath, IOUtil.RemoteInputDirName, fname);
                }

                if (!string.IsNullOrEmpty(config.Selectedvariedpointfile))
                {
                    fname = IOUtil.ManxcatSelectedVariedPointsFilePrefix + Path.GetFileName(config.Selectedvariedpointfile);
                    config.Selectedvariedpointfile = Path.Combine(projectPath, IOUtil.RemoteInputDirName, fname);
                }

                if (!string.IsNullOrEmpty(config.WeightingFileName))
                {
                    fname = IOUtil.ManxcatWeightingFilePrefix + Path.GetFileName(config.WeightingFileName);
                    config.WeightingFileName = Path.Combine(projectPath, IOUtil.RemoteInputDirName, fname);
                }

                if (!string.IsNullOrEmpty(config.ScalingFileName))
                {
                    fname = IOUtil.ManxcatScalingFilePrefix + Path.GetFileName(config.ScalingFileName);
                    config.ScalingFileName = Path.Combine(projectPath, IOUtil.RemoteInputDirName, fname);
                }
            }
        }

        private void ChangePairwiseConfigPaths(string projectPath, PairwiseSection config, bool isRemote)
        {
            string fname;

            if (!string.IsNullOrEmpty(config.ClusterFile))
            {
                fname = config.ClusterFile.Contains(Path.DirectorySeparatorChar)
                            ? Path.GetFileName(config.ClusterFile)
                            : config.ClusterFile;
                config.ClusterFile = Path.Combine(projectPath, !string.IsNullOrEmpty(fname) ? fname : "cluster.txt");
            }

            if (!string.IsNullOrEmpty(config.TimingFile))
            {
                fname = config.TimingFile.Contains(Path.DirectorySeparatorChar)
                            ? Path.GetFileName(config.TimingFile)
                            : config.TimingFile;
                config.TimingFile = Path.Combine(projectPath, !string.IsNullOrEmpty(fname) ? fname : "pwc-timing.txt");
            }

            if (!string.IsNullOrEmpty(config.SummaryFile))
            {
                fname = config.SummaryFile.Contains(Path.DirectorySeparatorChar)
                            ? Path.GetFileName(config.SummaryFile)
                            : config.SummaryFile;
                config.SummaryFile = Path.Combine(projectPath, !string.IsNullOrEmpty(fname) ? fname : "pwc-summary.txt");
            }

            if (!string.IsNullOrEmpty(config.CenterPlotFile))
            {
                fname = config.CenterPlotFile.Contains(Path.DirectorySeparatorChar)
                            ? Path.GetFileName(config.CenterPlotFile)
                            : config.CenterPlotFile;
                config.CenterPlotFile = Path.Combine(projectPath, !string.IsNullOrEmpty(fname) ? fname : "pwc-plot.pviz");
            }

            /* If this is a remote run, change local input file paths to headnode's appropriate directory */
            if (isRemote)
            {
                if (!string.IsNullOrEmpty(config.IndexFile))
                {
                    if (!config.IndexFile.StartsWith("$"))
                    {
                        fname = IOUtil.PairwiseIndexFilePrefix + (config.IndexFile.Contains(Path.DirectorySeparatorChar)
                                                                      ? Path.GetFileName(config.IndexFile)
                                                                      : config.IndexFile);
                        config.IndexFile = !string.IsNullOrEmpty(fname)
                                               ? Path.Combine(projectPath, IOUtil.RemoteInputDirName, fname)
                                               : string.Empty;
                    }
                    else
                    {
                        // Indicates a known file is already setup in machines
                        fname = config.IndexFile.Substring(1); // excludes the $ symbol
                        if (_knownFiles.ContainsKey(fname))
                        {
                            config.IndexFile = (string)_knownFiles[fname];
                        }
                    }
                }

                if (!string.IsNullOrEmpty(config.AddMdsFile))
                {
                    if (!config.AddMdsFile.StartsWith("$"))
                    {
                        fname = IOUtil.PairwiseMdsFilePrefix + (config.AddMdsFile.Contains(Path.DirectorySeparatorChar)
                                                                    ? Path.GetFileName(config.AddMdsFile)
                                                                    : config.AddMdsFile);
                        config.AddMdsFile = !string.IsNullOrEmpty(fname)
                                                ? Path.Combine(projectPath, IOUtil.RemoteInputDirName, fname)
                                                : string.Empty;
                    }
                    else
                    {
                        // Indicates a known file is already setup in machines
                        fname = config.AddMdsFile.Substring(1); // excludes the $ symbol
                        if (_knownFiles.ContainsKey(fname))
                        {
                            config.AddMdsFile = (string)_knownFiles[fname];
                        }
                    }
                }

                if (!string.IsNullOrEmpty(config.LabelFile))
                {
                    if (!config.LabelFile.StartsWith("$"))
                    {
                        fname = IOUtil.PairwiseLabelFilePrefix + (config.LabelFile.Contains(Path.DirectorySeparatorChar)
                                                                      ? Path.GetFileName(config.LabelFile)
                                                                      : config.LabelFile);
                        config.LabelFile = !string.IsNullOrEmpty(fname)
                                               ? Path.Combine(projectPath, IOUtil.RemoteInputDirName, fname)
                                               : string.Empty;
                    }
                    else
                    {
                        // Indicates a known file is already setup in machines
                        fname = config.LabelFile.Substring(1); // excludes the $ symbol
                        if (_knownFiles.ContainsKey(fname))
                        {
                            config.LabelFile = (string)_knownFiles[fname];
                        }
                    }

                }

                if (!string.IsNullOrEmpty(config.ClusterNumberFile))
                {
                    if (!config.ClusterNumberFile.StartsWith("$"))
                    {
                        fname = IOUtil.PairwiseClusterNumberFilePrefix +
                                (config.ClusterNumberFile.Contains(Path.DirectorySeparatorChar)
                                     ? Path.GetFileName(config.ClusterNumberFile)
                                     : config.ClusterNumberFile);
                        config.ClusterNumberFile = !string.IsNullOrEmpty(fname)
                                                       ? Path.Combine(projectPath, IOUtil.RemoteInputDirName, fname)
                                                       : string.Empty;
                    }
                    else
                    {
                        // Indicates a known file is already setup in machines
                        fname = config.ClusterNumberFile.Substring(1); // excludes the $ symbol
                        if (_knownFiles.ContainsKey(fname))
                        {
                            config.ClusterNumberFile = (string)_knownFiles[fname];
                        }
                    }
                }

                // Treat distance file path differently as it needs to be in every compute node
                if (!string.IsNullOrEmpty(config.DistanceMatrixFile))
                {
                    if (!config.DistanceMatrixFile.StartsWith("$"))
                    {
                        fname = IOUtil.PairwiseDistanceFilePrefix + (config.DistanceMatrixFile.Contains(Path.DirectorySeparatorChar)
                                    ? Path.GetFileName(config.DistanceMatrixFile)
                                    : config.DistanceMatrixFile);
                        int idxOfDollar = projectPath.IndexOf('$');
                        projectPath = projectPath.Substring(idxOfDollar - 1).Replace('$', ':');
                        config.DistanceMatrixFile = !string.IsNullOrEmpty(fname)
                                                        ? Path.Combine(projectPath, IOUtil.RemoteInputDirName, fname)
                                                        : string.Empty;
                    }
                    else
                    {
                        // Indicates a known matrix is already setup in machines
                        fname = config.DistanceMatrixFile.Substring(1); // excludes the $ symbol
                        if (_knownFiles.ContainsKey(fname))
                        {
                            config.DistanceMatrixFile = (string)_knownFiles[fname];
                        }
                    }
                }
            }
        }

        private void ChangeSpongeConfigPaths(string projectPath, DAVectorSpongeSection config, bool isRemote)
        {
            string fname;
            if (!string.IsNullOrEmpty(config.ClusterFile))
            {
                
                fname = config.ClusterFile.Contains(Path.DirectorySeparatorChar)
                            ? Path.GetFileName(config.ClusterFile)
                            : config.ClusterFile;
                config.ClusterFile = Path.Combine(projectPath, !string.IsNullOrEmpty(fname) ? fname : IOUtil.SpongeDefaultClustFileName);
            }
            if (!string.IsNullOrEmpty(config.TimingFile))
            {
                fname = config.TimingFile.Contains(Path.DirectorySeparatorChar)
                            ? Path.GetFileName(config.TimingFile)
                            : config.TimingFile;
                config.TimingFile = Path.Combine(projectPath, !string.IsNullOrEmpty(fname) ? fname : IOUtil.SpongeDefaultTimingFileName);
            }

            if (!string.IsNullOrEmpty(config.SummaryFile))
            {
                fname = config.SummaryFile.Contains(Path.DirectorySeparatorChar)
                            ? Path.GetFileName(config.SummaryFile)
                            : config.SummaryFile;
                config.SummaryFile = Path.Combine(projectPath, !string.IsNullOrEmpty(fname) ? fname : IOUtil.SpongeDefaultSummaryFilename);
            }
            
            if (isRemote)
            {
                if (!string.IsNullOrEmpty(config.LabelFile))
                {
                    if (!config.LabelFile.StartsWith("$"))
                    {
                        fname = IOUtil.SpongeLabelsFilePrefix + (config.LabelFile.Contains(Path.DirectorySeparatorChar)
                                    ? Path.GetFileName(config.LabelFile)
                                    : config.LabelFile);
                        config.LabelFile = !string.IsNullOrEmpty(fname)
                                               ? Path.Combine(projectPath, IOUtil.RemoteInputDirName, fname)
                                               : string.Empty;
                    }
                    else
                    {
                        fname = config.LabelFile.Substring(1); // excludes the $ symbol
                        if (_knownFiles.ContainsKey(fname))
                        {
                            config.LabelFile = (string)_knownFiles[fname];
                        }
                    }
                }

                // Treat distance file path differently as it needs to be in every compute node
                if (!string.IsNullOrEmpty(config.DistanceMatrixFile))
                {
                    if (!config.DistanceMatrixFile.StartsWith("$"))
                    {
                        fname = IOUtil.SpongeDistanceFilePrefix + (config.DistanceMatrixFile.Contains(Path.DirectorySeparatorChar)
                                    ? Path.GetFileName(config.DistanceMatrixFile)
                                    : config.DistanceMatrixFile);
                        int idxOfDollar = projectPath.IndexOf('$');
                        projectPath = projectPath.Substring(idxOfDollar - 1).Replace('$', ':');
                        config.DistanceMatrixFile = !string.IsNullOrEmpty(fname)
                                                        ? Path.Combine(projectPath, IOUtil.RemoteInputDirName, fname)
                                                        : string.Empty;
                    }
                    else
                    {
                        // Indicates a known matrix already in cluster
                        fname = config.DistanceMatrixFile.Substring(1); // excludes the $ symbol
                        if (_knownFiles.ContainsKey(fname))
                        {
                            config.DistanceMatrixFile = (string)_knownFiles[fname];
                        }
                    }
                }
            }
        }


        private void ChangeConfigPaths()
        {
            // Note. may be it'll be good to avoid changing configs for both cases. This is necessary only for initializing case

            string projectPath = Path.Combine(baseDirTxt.Text, nameTxt.Text);
            
            ManxcatSection manxcatSection = _confMgr.ManxcatSection;
            manxcatSection.ManxcatRunName = nameTxt.Text;
            manxcatSection.ManxcatRunDescription = manxcatSection.ManxcatRunName;
            ChangeManxcatConfigPaths(projectPath, manxcatSection, false);
            
            PairwiseSection pairwiseSection = _confMgr.PairwiseSection;
            ChangePairwiseConfigPaths(projectPath, pairwiseSection, false);

            DAVectorSpongeSection spongeSection = _confMgr.DAVectorSpongeSection;
            ChangeSpongeConfigPaths(projectPath, spongeSection, false);
            
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

            PairwiseSection pairwise = _confMgr.PairwiseSection;
            pairwise.DistanceMatrixFile = string.Empty;
            pairwise.IndexFile = string.Empty;
            pairwise.LabelFile = string.Empty;
            pairwise.AddMdsFile = string.Empty;
            pairwise.ClusterNumberFile = string.Empty;
            pairwise.ClusterFile = IOUtil.PairwiseDefaultClustFileName;
            pairwise.TimingFile = IOUtil.PairwiseDefaultTimingFileName;
            pairwise.SummaryFile = IOUtil.PairwiseDefaultSummaryFilename;
            pairwise.CenterPlotFile = IOUtil.PairwiseDefaultCenterPlotFileName;
            pairwise.ReadPartialMatrix = false;
            pairwise.ToosmalltoSplit = 5;
            pairwise.ContinuousClustering = true;
            pairwise.ConvergenceLoopLimit = 50;
            pairwise.FineCoolingFactor = 0.995;
            pairwise.InitialCoolingFactor = 0.95;
            pairwise.Iterationatend = 1000;
            pairwise.CenterPointsPerCenterTypeInOuput = 3;

            DAVectorSpongeSection sponge = _confMgr.DAVectorSpongeSection;
            sponge.DistanceMatrixFile = string.Empty;
            sponge.LabelFile = string.Empty;
            sponge.ClusterFile = IOUtil.SpongeDefaultClustFileName;
            sponge.TimingFile = IOUtil.SpongeDefaultTimingFileName;
            sponge.SummaryFile = IOUtil.SpongeDefaultSummaryFilename;

            ChangeConfigPaths();
            CopyManxcatConfiguration(manxcat, _referenceManxcatConfig, true);
            CopyPairwiseConfiguration(pairwise, _referencePairwiseConfig, true);
            CopySpongeConfiguration(sponge, _referenceSpongeSection, true);
        }

       


        private void changeAppDirBtn_Click(object sender, EventArgs e)
        {
            ChangeAppDir();
        }

        private DialogResult ChangeAppDir()
        {
            DialogResult result;
            ConfigDlg dlg = null;
            if (_appType == ApplicationType.Manxcat)
            {
                dlg = new ConfigDlg(_settings.DefaultManxcatAppDirectory,
                                    ConfigDlg.ConfigDlgType.ManxcatAppDirectory);
            }
            else if (_appType == ApplicationType.PairwiseClustering)
            {
                dlg = new ConfigDlg(_settings.DefaultPairwiseAppDirectory,
                                    ConfigDlg.ConfigDlgType.PairwiseAppDirectory);
            }
            else if (_appType == ApplicationType.SequentialSponge)
            {
                dlg = new ConfigDlg(_settings.DefaultSpongeAppDirectory,
                                    ConfigDlg.ConfigDlgType.SpongeAppDirectory);
            }

            using (dlg)
            {
                dlg.StrictAppDirValidation = true;
                result = dlg.ShowDialog(this);
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    if (_appType == ApplicationType.Manxcat)
                    {
                        if (!_settings.DefaultManxcatAppDirectory.Equals(dlg.DefaultDirectory))
                        {
                            _settings.DefaultManxcatAppDirectory = dlg.DefaultDirectory;
                            
                        }
                    }
                    else if (_appType == ApplicationType.PairwiseClustering)
                    {
                        if (!_settings.DefaultPairwiseAppDirectory.Equals(dlg.DefaultDirectory))
                        {
                            _settings.DefaultPairwiseAppDirectory = dlg.DefaultDirectory;

                        }
                    }
                    else if (_appType == ApplicationType.SequentialSponge)
                    {
                        if (!_settings.DefaultSpongeAppDirectory.Equals(dlg.DefaultDirectory))
                        {
                            _settings.DefaultSpongeAppDirectory = dlg.DefaultDirectory;

                        }
                    }
                    _settings.Save();
                    appDirTxt.Text = dlg.CurrentDir;
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
            Clipboard.SetText(appDirTxt.Text);
        }

        private void copyDirLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(baseDirTxt.Text);
        }

        // todo: fix load and reset config for pairwise as well
        private void loadConfigBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Configuration XML (*.xml)|*xml";
                if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    ConfigurationMgr external = ConfigurationMgr.LoadConfiguration(dlg.FileName, false);
                    ManxcatSection externalManxcat = external.ManxcatSection;
                    PairwiseSection externalPairwise = external.PairwiseSection;
                    DAVectorSpongeSection externalSponge = external.DAVectorSpongeSection;
                    // Let it load I/O by default, but change output paths correctly

                    if (_appType == ApplicationType.Manxcat)
                    {
                        CopyManxcatConfiguration(externalManxcat, _confMgr.ManxcatSection, true);
                    }
                    else if (_appType == ApplicationType.PairwiseClustering)
                    {
                        CopyPairwiseConfiguration(externalPairwise, _confMgr.PairwiseSection, true);
                    }
                    else if (_appType == ApplicationType.SequentialSponge)
                    {
                        CopySpongeConfiguration(externalSponge, _confMgr.DAVectorSpongeSection, true);
                    }
                    ChangeConfigPaths();
                    pGrid.Refresh();
                }
            }
            resetConfigBtn.Enabled = true;
        }

        private void CopyManxcatConfiguration(ManxcatSection from, ManxcatSection to, bool loadIO)
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
                to.ScalingFileName = from.ScalingFileName;
                to.ClusterFile = from.ClusterFile;
            }

            to.AddonforQcomputation = from.AddonforQcomputation;
            to.CalcFixedCrossFixed = from.CalcFixedCrossFixed;
            to.CGResidualLimit = from.CGResidualLimit;
            to.ChisqChangePerPoint = from.ChisqChangePerPoint;
            to.Chisqnorm = from.Chisqnorm;
            to.ChisqPrintConstant = from.ChisqPrintConstant;
            to.Comment = from.Comment;
            to.ConsoleDebugOutput = from.ConsoleDebugOutput;
            to.ConversionInformation = from.ConversionInformation;
            to.ConversionOption = from.ConversionOption;
            to.CoordinateWriteFrequency = from.CoordinateWriteFrequency;
            to.DataPoints = from.DataPoints;
            to.DebugPrintOption = from.DebugPrintOption;
            to.Derivtest = from.Derivtest;
            to.DiskDistanceOption = from.DiskDistanceOption;
            to.DistanceCut = from.DistanceCut;
            to.DistanceFormula = from.DistanceFormula;
            to.DistanceProcessingOption = from.DistanceProcessingOption;
            to.DistanceWeightsCuts = from.DistanceWeightsCuts;
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
            to.LinkCut = from.LinkCut;
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
            to.StoredDistanceOption = from.StoredDistanceOption;
            to.ThreadCount = from.ThreadCount;
            to.TimeCutmillisec = from.TimeCutmillisec;
            to.TransformMethod = from.TransformMethod;
            to.TransformParameter = from.TransformParameter;
            to.UndefinedDistanceValue = from.UndefinedDistanceValue;
            to.VariedPointCriterion = from.VariedPointCriterion;
            to.WeightingOption = from.WeightingOption;
            to.Write2Das3D = from.Write2Das3D;

            /* Density and Html */
            to.Alpha = from.Alpha;
            to.Pcutf = from.Pcutf;
            to.SelectedClusters = from.SelectedClusters;
            to.XmaxBound = from.XmaxBound;
            to.Xres = from.Xres;
            to.YmaxBound = from.YmaxBound;
            to.Yres = from.Yres;
            to.Normalize = from.Normalize;
            to.ManxcatRunName = from.ManxcatRunName;
            to.ManxcatRunDescription = from.ManxcatRunDescription;

        }

        private void CopyPairwiseConfiguration(PairwiseSection from, PairwiseSection to, bool  loadIO)
        {
            if (loadIO)
            {
                to.IndexFile = from.IndexFile;
                to.DistanceMatrixFile = from.DistanceMatrixFile;
                to.ClusterFile = from.ClusterFile;
                to.TimingFile = from.TimingFile;
                to.SummaryFile = from.SummaryFile;
                to.AddMdsFile = from.AddMdsFile;
                to.LabelFile = from.LabelFile;
                to.ClusterNumberFile = from.ClusterNumberFile;
                to.CenterPlotFile = from.CenterPlotFile;
            }

            to.DebugPrintOption = from.DebugPrintOption;
            to.ConsoleDebugOutput = from.ConsoleDebugOutput;

            to.DataPoints = from.DataPoints;
            to.ProcessingOption = from.ProcessingOption;
            to.TransformDimension = from.TransformDimension;
            to.MaxNcent = from.MaxNcent;
            to.Splitorexpandit = from.Splitorexpandit;
            to.Pattern = from.Pattern;
            to.ThreadCount = from.ThreadCount;
            to.NodeCount = from.NodeCount;
            to.MPIperNodeCount = from.MPIperNodeCount;
            to.MPIIOStrategy = from.MPIIOStrategy;
            to.ToosmalltoSplit = from.ToosmalltoSplit;
            to.MinEigtest = from.MinEigtest;
            to.ConvergeIntermediateClusters = from.ConvergeIntermediateClusters;
            to.Waititerations = from.Waititerations;
            to.Epsi_max_change = from.Epsi_max_change;
            to.InitialCoolingFactor = from.InitialCoolingFactor;
            to.FineCoolingFactor = from.FineCoolingFactor;
            to.Eigenvaluechange = from.Eigenvaluechange;
            to.Eigenvectorchange = from.Eigenvectorchange;
            to.Iterationatend = from.Iterationatend;
            to.ConvergenceLoopLimit = from.ConvergenceLoopLimit;
            to.FreezingLimit = from.FreezingLimit;
            to.PowerIterationLimit = from.PowerIterationLimit;
            to.DebugPrintOption = from.DebugPrintOption;
            to.ConsoleDebugOutput = from.ConsoleDebugOutput;
            to.ReadPartialMatrix = from.ReadPartialMatrix;
            to.ContinuousClustering = from.ContinuousClustering;
            to.AddMds = from.AddMds;
            to.BucketFractions = from.BucketFractions;
            to.NumberOfCenters = from.NumberOfCenters;
            to.CenterPointsPerCenterTypeInOuput = from.CenterPointsPerCenterTypeInOuput;
        }

        private void CopySpongeConfiguration(DAVectorSpongeSection from, DAVectorSpongeSection to, bool loadIO)
        {
            if (loadIO)
            {
                to.ClusterFile = from.ClusterFile;
                to.DistanceMatrixFile = from.DistanceMatrixFile;
                to.LabelFile = from.LabelFile;
                to.TimingFile = from.TimingFile;
                to.SummaryFile = from.SummaryFile;
            }

            to.UseSponge = from.UseSponge;
            to.SpongeFactor1 = from.SpongeFactor1;
            to.SpongeFactor2 = from.SpongeFactor2;
            to.SpongePOption = from.SpongePOption;
            to.SpongePWeight = from.SpongePWeight;
            to.CreateSpongeScaledSquaredWidth = from.CreateSpongeScaledSquaredWidth;
            to.ContinuousClustering = from.ContinuousClustering;
            to.ParameterVectorDimension = from.ParameterVectorDimension;

            to.SpongeTemperature1 = from.SpongeTemperature1;
            to.SpongeTemperature2 = from.SpongeTemperature2;
            to.RestartTemperature = from.RestartTemperature;

            to.NumberDataPoints = from.NumberDataPoints;
            to.SelectedInputLabel = from.SelectedInputLabel;
            to.OutputFileType = from.OutputFileType;
            to.Replicate = from.Replicate;

            to.SigmaMethod = from.SigmaMethod;
            to.FinalTargetTemperature = from.FinalTargetTemperature;
            to.FinalTargetSigma0 = from.FinalTargetSigma0;
            to.InitialSigma0 = from.InitialSigma0;

            to.ClusterCountOutput = from.ClusterCountOutput;
            to.NumberNearbyClusters = from.NumberNearbyClusters;
            to.NearbySpongePointLimit = from.NearbySpongePointLimit;

            to.ProcessingOption = from.ProcessingOption;

            to.CacheLineSize = from.CacheLineSize;
            to.ClusterPrintNumber = from.ClusterPrintNumber;
            to.PrintInterval = from.PrintInterval;
            to.RemovalDiagnosticPrint = from.RemovalDiagnosticPrint;
            to.MagicTemperatures = from.MagicTemperatures;
            to.MagicIndex = from.MagicIndex;

            to.MaxNcentPerNode = from.MaxNcentPerNode;
            to.MaxNcentTotal = from.MaxNcentTotal;
            to.TargetNcentPerPoint = from.TargetNcentPerPoint;
            to.TargetMinimumNcentPerPoint = from.TargetMinimumNcentPerPoint;
            to.MaxNcentPerPoint = from.MaxNcentPerPoint;

            to.MaxIntegerComponents = from.MaxIntegerComponents;
            to.MaxDoubleComponents = from.MaxDoubleComponents;
            to.MaxMPITransportBuffer = from.MaxMPITransportBuffer;
            to.MaxNumberAccumulationsPerNode = from.MaxNumberAccumulationsPerNode;
            to.MaxTransportedClusterStorage = from.MaxTransportedClusterStorage;

            to.ExpArgumentCut1 = from.ExpArgumentCut1;
            to.ExpArgumentCut2 = from.ExpArgumentCut2;
            to.ExpArgumentCut3 = from.ExpArgumentCut3;
            to.Tminimum = from.Tminimum;

            to.InitalNcent = from.InitalNcent;
            to.MinimumCountForClusterCk = from.MinimumCountForClusterCk;
            to.MinimumCountForClusterCkWithSponge = from.MinimumCountForClusterCkWithSponge;
            to.MinimuCountForClusterPoints = from.MinimuCountForClusterPoints;
            to.CountForClusterCkToBeZero = from.CountForClusterCkToBeZero;
            to.AddSpongeScaledWidthSquared = from.AddSpongeScaledWidthSquared;

            to.InitialCoolingFactor = from.InitialCoolingFactor;
            to.FineCoolingFactor = from.FineCoolingFactor;
            to.WaitIterations = from.WaitIterations;

            to.IterationAtEnd = from.IterationAtEnd;
            to.ConvergenceLoopLimit = from.ConvergenceLoopLimit;

            to.FreezingLimit = from.FreezingLimit;
            to.MalphaMaxChange = from.MalphaMaxChange;
            to.MaxNumberSplitClusters = from.MaxNumberSplitClusters;
            to.ConvergeIntermediateClusters = from.ConvergeIntermediateClusters;
            to.TooSmallToSplit = from.TooSmallToSplit;
            to.ScaledWidthSquaredToSplit = from.ScaledWidthSquaredToSplit;

            to.ClusterLimitForDistribution = from.ClusterLimitForDistribution;
            to.TemperatureLimitForDistribution = from.TemperatureLimitForDistribution;

            to.Pattern = from.Pattern;
            to.NodeCount = from.NodeCount;
            to.ThreadCount = from.ThreadCount;
            to.MPIPerNodeCount = from.MPIPerNodeCount;



        }

        private void resetConfigBtn_Click(object sender, EventArgs e)
        {
            if (_appType == ApplicationType.Manxcat)
            {
                CopyManxcatConfiguration(_referenceManxcatConfig, _confMgr.ManxcatSection, true);
            }
            else if (_appType == ApplicationType.PairwiseClustering)
            {
                CopyPairwiseConfiguration(_referencePairwiseConfig, _confMgr.PairwiseSection, true);
            }
            else if (_appType == ApplicationType.SequentialSponge)
            {
                CopySpongeConfiguration(_referenceSpongeSection, _confMgr.DAVectorSpongeSection, true);
            }
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
                            ManxcatSection remoteConfig = GenerateRemoteManxcatConfig();
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
                            if (_appType == ApplicationType.Manxcat)
                            {
                                ManxcatSection remoteManxcatConfig = GenerateRemoteManxcatConfig();
                                remoteMgr.ManxcatSection = remoteManxcatConfig;
                            }
                            else if (_appType == ApplicationType.PairwiseClustering)
                            {
                                PairwiseSection remotePairwiseConfig = GenerateRemotePairwiseConfig();
                                remoteMgr.PairwiseSection = remotePairwiseConfig;
                            }
                            else if (_appType == ApplicationType.SequentialSponge)
                            {
                                DAVectorSpongeSection remoteSpongeConfig = GenerateRemoteSpongeConfig();
                                remoteMgr.DAVectorSpongeSection = remoteSpongeConfig;
                            }
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

                    if (_appType == ApplicationType.Manxcat)
                    {
                        // Update reference manxcat configuration, so that it will have config for the latest run
                        CopyManxcatConfiguration(_confMgr.ManxcatSection, _referenceManxcatConfig, true);
                    }
                    else if (_appType == ApplicationType.PairwiseClustering)
                    {
                        // Update reference pairwise configuration, so that it will have config for the latest run
                        CopyPairwiseConfiguration(_confMgr.PairwiseSection, _referencePairwiseConfig, true);
                    }
                    else if (_appType == ApplicationType.SequentialSponge)
                    {
                        // Update reference sponge configuration, so that it will have config for the latest run
                        CopySpongeConfiguration(_confMgr.DAVectorSpongeSection, _referenceSpongeSection, true);
                    }
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
            lock (_fireOnStateLock)
            {
                if (_fireOnState)
                {
                    IScheduler scheduler = (IScheduler) sender;
                    ISchedulerJob job = scheduler.OpenJob(e.JobId);
                    ISchedulerTask task = job.OpenTask(e.TaskId);
                    if (task.Name.StartsWith(ConstantNames.DefaultManxcatTaskName) || 
                        task.Name.StartsWith(ConstantNames.DefaultPairwiseTaskName)||
                        task.Name.StartsWith(ConstantNames.DefaultSpongeTaskName))
                    {
                        AppendToOut(string.Format(LogMessages.ApplicationTaskState, task.Name, TaskStateToString(e.NewState)));
                        if (e.NewState == TaskState.Running)
                        {
                            // Start progress report monitor
                            StartMonitorAsync();
                        }
                        else if (e.NewState == TaskState.Canceled ||
                                 e.NewState == TaskState.Finished ||
                                 e.NewState == TaskState.Failed)
                        {
                            lock (_jobtrackerMonitorStopLock)
                            {
                                StopMonitorAsync();
                                _monitorEndEvt.WaitOne(); // Wait till monitor ends
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
            lock (_fireOnStateLock)
            {
                if (_fireOnState)
                {
                    AppendToOut(string.Format(LogMessages.JobStatus, e.JobId, JobStateToString(e.NewState)));
                    if (e.NewState == JobState.Finished ||
                        e.NewState == JobState.Canceled ||
                        e.NewState == JobState.Failed)
                    {
                        // UI events like exit/new/open with confirm should also try to acquire this lock before stopping job tracker
                        lock (_jobtrackerMonitorStopLock)
                        {
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
                            _monitorEndEvt.WaitOne(); // Wait till monitor ends
                           
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
            if (_appType == ApplicationType.Manxcat)
            {
                string ptsFile = Path.Combine(_runDir, IOUtil.ManxcatSimplePointsFileNamePrefix +
                                                       Path.GetFileName(
                                                           _confMgr.ManxcatSection.ReducedVectorOutputFileName));
                if (File.Exists(ptsFile))
                {
                    if (File.Exists(_runInfo.ComplementryTextFile))
                    {
                        CreateLocalPlotFiles(ptsFile, new[] {_runInfo.ComplementryTextFile});
                    }
                }
            }
            else if (_appType == ApplicationType.PairwiseClustering)
            {
                if (File.Exists(_runInfo.ComplementryTextFile))
                {
                    string[] clusterFiles = Directory.GetFiles(_runDir, Path.GetFileNameWithoutExtension(_confMgr.PairwiseSection.ClusterFile) + "*.txt");
                    CreateLocalPlotFiles(_runInfo.ComplementryTextFile, clusterFiles);
                }
            }
            else if (_appType == ApplicationType.SequentialSponge)
            {
                // Todo (sponge) - any final sponge work?
            }
            _runInfo.IsRunCompleted = true;
            _runInfo.IsRunSuccess = isRunSuccess;
            _runInfo.SaveAs(Path.Combine(_runDir, _runInfo.Name + IOUtil.RunInfoExt));
            BringBackUI();
        }


        private void CreateLocalPlotFiles(string ptsFile, string [] clusFiles)
        {
            IEnumerable<string> existingClusFiles = clusFiles.Where(File.Exists);
            if (existingClusFiles.Count() > 0)
            {
                StreamReader[] clusReaders = existingClusFiles.Select(x => new StreamReader(x)).ToArray();
                using (StreamReader ptsReader = new StreamReader(ptsFile))
                {

                    StreamWriter[] plotWriters =
                        existingClusFiles.Select(x => new StreamWriter(Path.Combine(Path.GetDirectoryName(x),
                                                                                    IOUtil.LocalPlotTxtFileName + "_" +
                                                                                    Path.GetFileName(x)))).ToArray();

                    char[] sep = new[] {' ', '\t'};
                    string[] ptsSplits, clusSplits;
                    while (!ptsReader.EndOfStream)
                    {
                        ptsSplits = ptsReader.ReadLine().Trim().Split(sep);
                        for (int i = 0; i < clusReaders.Length; ++i)
                        {
                            clusSplits = clusReaders[i].ReadLine().Trim().Split(sep);
                            plotWriters[i].WriteLine(ptsSplits[0] + '\t' + ptsSplits[1] + '\t' + ptsSplits[2] + '\t' +
                                                 ptsSplits[3] +
                                                 '\t' + clusSplits[1]);
                        }
                    }

                    foreach (var streamWriter in plotWriters)
                    {
                        streamWriter.Close();
                        streamWriter.Dispose();
                    }


                    foreach (var streamReader in clusReaders)
                    {
                        streamReader.Close();
                        streamReader.Dispose();
                    }
                    // Note. Assume all clusterfiles and pts file have the same  number of lines
                    /*if (!clusReader.EndOfStream)
                    {
                        MessageBox.Show(
                            string.Format(LogMessages.MoreLinesThanOther, "Cluster", "Manxcat points"),
                            LogCategories.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        File.Delete(Path.Combine(_runDir, IOUtil.LocalPlotTxtFileName));
                    }

                    if (!ptsReader.EndOfStream)
                    {
                        MessageBox.Show(
                            string.Format(LogMessages.MoreLinesThanOther, "Manxcat points", "cluster"),
                            LogCategories.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        File.Delete(Path.Combine(_runDir, IOUtil.LocalPlotTxtFileName));
                    }*/
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
                rootSplit.Panel1.Enabled = true;
                mainSplit.Panel1.Enabled = true;
                runBtn.Enabled = runBtn.Visible = true;
                killBtn.Enabled = killBtn.Visible = false;

                if (_appType == ApplicationType.Manxcat)
                {
                    if (File.Exists(
                        Path.Combine(_runDir, IOUtil.ManxcatSimplePointsFileNamePrefix +
                                              Path.GetFileName(_confMgr.ManxcatSection.ReducedVectorOutputFileName))))
                    {
                        showInPvizBtn.Enabled = showInPvizBtn.Visible = true;
                    }
                }
                else if (_appType == ApplicationType.PairwiseClustering)
                {
                    string[] clusterFiles = Directory.GetFiles(_runDir,
                                                               Path.GetFileNameWithoutExtension(
                                                                   _confMgr.PairwiseSection.ClusterFile) + "*.txt");

                    if (clusterFiles != null && clusterFiles.Length > 0)
                    {
                        showInPvizBtn.Enabled = showInPvizBtn.Visible = true;
                    }
                }
                else if (_appType == ApplicationType.SequentialSponge)
                {
                    // Todo (sponge) - bring back UI for sponge?
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

            string copiedOutFile = string.Empty;
            string copiedErrFile = string.Empty;
            
            if (_appType == ApplicationType.Manxcat)
            {
                copiedOutFile = Path.Combine(_runDir, IOUtil.ManxcatOutFileName);
                copiedErrFile = Path.Combine(_runDir, IOUtil.ManxcatErrFileName);
            }
            else if (_appType == ApplicationType.PairwiseClustering)
            {
                copiedOutFile = Path.Combine(_runDir, IOUtil.PairwiseOutFileName);
                copiedErrFile = Path.Combine(_runDir, IOUtil.PairwiseErrFileName);
            }
            else if (_appType == ApplicationType.SequentialSponge)
            {
                copiedOutFile = Path.Combine(_runDir, IOUtil.SpongeOutFileName);
                copiedErrFile = Path.Combine(_runDir, IOUtil.SpongeErrFileName);
            }

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
            if (_appType == ApplicationType.Manxcat)
            {
                CopyManxcatFilesRemoteToLocal();
            }
            else if (_appType == ApplicationType.PairwiseClustering)
            {
                CopyPairwiseFileRemoteToLocal();
            }
            else if (_appType == ApplicationType.SequentialSponge)
            {
                CopySpongeFileRemoteToLocal();
            }
        }

        private void CopySpongeFileRemoteToLocal()
        {
            string hnProjectDir = _runInfo.HnProjectDir;
            string fileName = Path.Combine(_runDir, IOUtil.SpongeOutFileName);
            CopyFromRemoteIfNotNullAndExists(fileName, hnProjectDir);

            fileName = Path.Combine(_runDir, IOUtil.SpongeErrFileName);
            CopyFromRemoteIfNotNullAndExists(fileName,hnProjectDir);


            DAVectorSpongeSection daVectorSpongeSection = _confMgr.DAVectorSpongeSection;
            CopyFromRemoteIfNotNullAndExists(daVectorSpongeSection.SummaryFile, hnProjectDir);
            CopyFromRemoteIfNotNullAndExists(daVectorSpongeSection.TimingFile, hnProjectDir);

            // Todo (sponge) - sponge configuration section files (if any other than the ones above)?
        }


        private void CopyPairwiseFileRemoteToLocal()
        {
            string hnProjectDir = _runInfo.HnProjectDir;
            string file = Path.Combine(hnProjectDir, IOUtil.PairwiseOutFileName);
            if (File.Exists(file))
            {
                string fileName = Path.GetFileName(file);
                if (!string.IsNullOrEmpty(fileName))
                {
                    File.Copy(file, Path.Combine(_runDir, fileName), true);
                }
            }

            file = Path.Combine(hnProjectDir, IOUtil.PairwiseErrFileName);
            if (File.Exists(file))
            {
                string fileName = Path.GetFileName(file);
                if (!string.IsNullOrEmpty(fileName))
                {
                    File.Copy(file, Path.Combine(_runDir, fileName), true);
                }
            }

            PairwiseSection pairwiseSection = _confMgr.PairwiseSection;
            CopyFromRemoteIfNotNullAndExists(pairwiseSection.SummaryFile, hnProjectDir);
            CopyFromRemoteIfNotNullAndExists(pairwiseSection.TimingFile, hnProjectDir);
            CopyFromRemoteIfNotNullAndExists(pairwiseSection.CenterPlotFile, hnProjectDir);
            
            // Note. starting from 1 as our PWC is based on 1 based indices
            for (int i = 1; i <= _confMgr.PairwiseSection.MaxNcent; ++i)
            {
                if (!string.IsNullOrEmpty(pairwiseSection.ClusterFile))
                {
                    string fileName = string.Format(ConstantNames.PairwiseClusterFileNameFormat,
                                                    Path.GetFileNameWithoutExtension(pairwiseSection.ClusterFile),
                                                    pairwiseSection.MaxNcent, i);

                    file = Path.Combine(_runDir, fileName);
                    CopyFromRemoteIfNotNullAndExists(file, hnProjectDir);
                }

                file = Path.Combine(_runDir,
                                        string.Format(ConstantNames.PairwiseCenterFileNameFormat,
                                                      _confMgr.PairwiseSection.MaxNcent, i));
                CopyFromRemoteIfNotNullAndExists(file, hnProjectDir);
            }

        }

        private void CopyFromRemoteIfNotNullAndExists(string localPath, string remoteDir)
        {
            if (!string.IsNullOrEmpty(localPath))
            {
                string fileName = Path.GetFileName(localPath);
                if(!string.IsNullOrEmpty(fileName))
                {
                    string file = Path.Combine(remoteDir, fileName);
                    if (File.Exists(file))
                    {
                        File.Copy(file, localPath, true);
                    }
                }
            }
        }

        private void CopyManxcatFilesRemoteToLocal()
        {
            ManxcatSection manxcatSection = _confMgr.ManxcatSection;
            string hnProjectDir = _runInfo.HnProjectDir;
            
            CopyFromRemoteIfNotNullAndExists(manxcatSection.SummaryOutputFileName, hnProjectDir);
            CopyFromRemoteIfNotNullAndExists(manxcatSection.TimingOutputFileName, hnProjectDir);

            if (!string.IsNullOrEmpty(manxcatSection.ReducedVectorOutputFileName))
            {
                string pointsFileNameWoExt = Path.GetFileNameWithoutExtension(manxcatSection.ReducedVectorOutputFileName);

                string file = Path.Combine(_runDir,
                                           pointsFileNameWoExt + IOUtil.ManxcatColonPointsFileNameSuffix +
                                           IOUtil.TextExt);
                CopyFromRemoteIfNotNullAndExists(file, hnProjectDir);

                file = Path.Combine(_runDir,
                                    pointsFileNameWoExt + IOUtil.ManxcatGroupPointsFileNameSuffix + IOUtil.TextExt);
                CopyFromRemoteIfNotNullAndExists(file, hnProjectDir);

                file = Path.Combine(_runDir,
                                    IOUtil.ManxcatSimplePointsFileNamePrefix + pointsFileNameWoExt + IOUtil.TextExt);
                CopyFromRemoteIfNotNullAndExists(file, hnProjectDir);

                file = Path.Combine(_runDir, IOUtil.ManxcatOutFileName);
                CopyFromRemoteIfNotNullAndExists(file, hnProjectDir);

                file = Path.Combine(_runDir, IOUtil.ManxcatErrFileName);
                CopyFromRemoteIfNotNullAndExists(file, hnProjectDir);

                string dir = Path.GetDirectoryName(manxcatSection.ReducedVectorOutputFileName);
                if (!string.IsNullOrEmpty(dir))
                {
                    file = Path.Combine(dir, IOUtil.ManxcatWholeDensityFile);
                    CopyFromRemoteIfNotNullAndExists(file, hnProjectDir);

                    file = Path.Combine(dir, IOUtil.ManxcatWholeXHistFile);
                    CopyFromRemoteIfNotNullAndExists(file,hnProjectDir);

                    file = Path.Combine(dir, IOUtil.ManxcatWholeYHistFile);
                    CopyFromRemoteIfNotNullAndExists(file, hnProjectDir);

                    file = Path.Combine(dir, IOUtil.ManxcatWholePlotFile);
                    CopyFromRemoteIfNotNullAndExists(file, hnProjectDir);

                    file = Path.Combine(dir, IOUtil.ManxcatSelectedDensityFile);
                    CopyFromRemoteIfNotNullAndExists(file,hnProjectDir);

                    file = Path.Combine(dir, IOUtil.ManxcatSelectedInterDensityFile);
                    CopyFromRemoteIfNotNullAndExists(file, hnProjectDir);

                    file = Path.Combine(dir, IOUtil.ManxcatSelectedXHistFile);
                    CopyFromRemoteIfNotNullAndExists(file, hnProjectDir);

                    file = Path.Combine(dir, IOUtil.ManxcatSelectedInterXHistFile);
                    CopyFromRemoteIfNotNullAndExists(file,hnProjectDir);

                    file = Path.Combine(dir, IOUtil.ManxcatSelectedYHistFile);
                    CopyFromRemoteIfNotNullAndExists(file,hnProjectDir);

                    file = Path.Combine(dir, IOUtil.ManxcatSelectedInterYHistFile);
                    CopyFromRemoteIfNotNullAndExists(file,hnProjectDir);

                    file = Path.Combine(dir, IOUtil.ManxcatSelectedPlotFile);
                    CopyFromRemoteIfNotNullAndExists(file,hnProjectDir);

                    file = Path.Combine(dir, IOUtil.ManxcatSelectedInterPlotFile);
                    CopyFromRemoteIfNotNullAndExists(file,hnProjectDir);

                    file = Path.Combine(dir, IOUtil.ManxcatHtmlFile);
                    CopyFromRemoteIfNotNullAndExists(file,hnProjectDir);

                    file = Path.Combine(dir, IOUtil.ManxcatCSSFile);
                    CopyFromRemoteIfNotNullAndExists(file,hnProjectDir);
                }
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
            AddApplicationTask(scheduler, job, lastSetupTask);
#else
            AddManxcatTask(scheduler, job);
#endif
            AppendToOut(string.Format(LogMessages.ApplicationTaskAdded, _appType));

            return job;
        }

#if GREASY
        private void AddManxcatTask(IScheduler scheduler, ISchedulerJob job)
#else
        private void AddApplicationTask(IScheduler scheduler, ISchedulerJob job, ISchedulerTask dependsOnTask)
#endif
        {
            string name = GenerateApplicationName();
            ISchedulerTask task = CreateSimpleEmptyTask(job, name);
            task.Name = name;
            task.WorkDirectory = _runInfo.HnProjectDir;
            task.MaximumNumberOfCores = task.MinimumNumberOfCores = _runInfo.MaxCores;

            if (_appType == ApplicationType.Manxcat)
            {

                task.StdOutFilePath = Path.Combine(_runInfo.HnProjectDir, IOUtil.ManxcatOutFileName);
                task.StdErrFilePath = Path.Combine(_runInfo.HnProjectDir, IOUtil.ManxcatErrFileName);

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
            }
            else if (_appType == ApplicationType.PairwiseClustering)
            {
                task.StdOutFilePath = Path.Combine(_runInfo.HnProjectDir, IOUtil.PairwiseOutFileName);
                task.StdErrFilePath = Path.Combine(_runInfo.HnProjectDir, IOUtil.PairwiseErrFileName);

                // mpiexec /hosts {node-count} {hosts} {path-to-exe} /configFile={path-to-config} /nodeCount={node-count} /threadCount={thread-count}
                const string templatePairwiseCmd =
                    @"mpiexec /hosts {0} {1} {2} /configFile={3} /nodeCount={4} /threadCount={5}";
                task.CommandLine = string.Format(templatePairwiseCmd, _runInfo.NodeCount, _runInfo.HostsList,
                                                 Path.Combine(_runInfo.CnProjectDir, IOUtil.RemoteAppsDirName,
                                                              IOUtil.PairwiseExeName),
                                                 Path.Combine(_runInfo.HnProjectDir, IOUtil.RemoteConfigFileName),
                                                 _runInfo.NodeCount, _runInfo.ThreadCount);
            }

            else if (_appType == ApplicationType.SequentialSponge)
            {
                task.StdOutFilePath = Path.Combine(_runInfo.HnProjectDir, IOUtil.SpongeOutFileName);
                task.StdErrFilePath = Path.Combine(_runInfo.HnProjectDir, IOUtil.SpongeErrFileName);

                const string templateSpongeCmd =
                    @"mpiexec /hosts {0} {1} {2} /configFile={3} /nodeCount={4} /threadCount={5}";
                task.CommandLine = string.Format(templateSpongeCmd, _runInfo.NodeCount, _runInfo.HostsList,
                                                 Path.Combine(_runInfo.CnProjectDir, IOUtil.RemoteAppsDirName,
                                                              IOUtil.SpongeExeName),
                                                 Path.Combine(_runInfo.HnProjectDir, IOUtil.RemoteConfigFileName),
                                                 _runInfo.NodeCount, _runInfo.ThreadCount);
            }

            IStringCollection dependsOnTasks = scheduler.CreateStringCollection();
#if !GREASY
            dependsOnTasks.Add(dependsOnTask.Name);
            task.DependsOn = dependsOnTasks;
#endif
            job.AddTask(task);
        }

        private string GenerateApplicationName()
        {
            if (_appType == ApplicationType.Manxcat)
            {
                // Todo: generate a meaningful name for Manxcat by looking at the config
                return ConstantNames.DefaultManxcatTaskName;
            }
            if (_appType == ApplicationType.PairwiseClustering)
            {
                return ConstantNames.DefaultPairwiseTaskName;
            }
            if (_appType == ApplicationType.SequentialSponge)
            {
                return ConstantNames.DefaultSpongeTaskName;
            }

            return string.Empty;
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

        // returns the last setup task so that manxcat or pairwise clustering task can depend on that
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
            if (_appType == ApplicationType.Manxcat)
            {
                ManxcatSection localManxcatConfig = _confMgr.ManxcatSection;
                if (File.Exists(localManxcatConfig.DataLabelsFileName))
                {
                    File.Copy(localManxcatConfig.DataLabelsFileName,
                              Path.Combine(remoteInputDir,
                                           IOUtil.ManxcatDataLabelsFilePrefix +
                                           Path.GetFileName(localManxcatConfig.DataLabelsFileName)), true);
                }

                if (localManxcatConfig.DistanceMatrixFile != null && !localManxcatConfig.DistanceMatrixFile.StartsWith("$") && File.Exists(localManxcatConfig.DistanceMatrixFile))
                {
                    File.Copy(localManxcatConfig.DistanceMatrixFile,
                              Path.Combine(remoteInputDir, IOUtil.ManxcatDistanceFilePrefix + Path.GetFileName(localManxcatConfig.DistanceMatrixFile)), true);
                }

                if (File.Exists(localManxcatConfig.ClusterFile))
                {
                    File.Copy(localManxcatConfig.ClusterFile,
                              Path.Combine(remoteInputDir, IOUtil.ManxcatClusterFilePrefix + Path.GetFileName(localManxcatConfig.ClusterFile)),
                              true);
                }

                if (File.Exists(localManxcatConfig.IndexFile))
                {
                    File.Copy(localManxcatConfig.IndexFile,
                              Path.Combine(remoteInputDir, IOUtil.ManxcatIndexFilePrefix + Path.GetFileName(localManxcatConfig.IndexFile)),
                              true);
                }

                if (File.Exists(localManxcatConfig.InitializationFileName))
                {
                    File.Copy(localManxcatConfig.InitializationFileName,
                              Path.Combine(remoteInputDir, IOUtil.ManxcatInitializationFilePrefix + Path.GetFileName(localManxcatConfig.InitializationFileName)), true);
                }

                if (File.Exists(localManxcatConfig.RotationLabelsFileName))
                {
                    File.Copy(localManxcatConfig.RotationLabelsFileName,
                              Path.Combine(remoteInputDir, IOUtil.ManxcatRotationLabelsFilePrefix + Path.GetFileName(localManxcatConfig.RotationLabelsFileName)), true);
                }

                if (File.Exists(localManxcatConfig.Selectedfixedpointfile))
                {
                    File.Copy(localManxcatConfig.Selectedfixedpointfile,
                              Path.Combine(remoteInputDir, IOUtil.ManxcatSelectedFixedPointsFilePrefix + Path.GetFileName(localManxcatConfig.Selectedfixedpointfile)), true);
                }

                if (File.Exists(localManxcatConfig.Selectedvariedpointfile))
                {
                    File.Copy(localManxcatConfig.Selectedvariedpointfile,
                              Path.Combine(remoteInputDir, IOUtil.ManxcatSelectedVariedPointsFilePrefix + Path.GetFileName(localManxcatConfig.Selectedvariedpointfile)), true);
                }

                if (File.Exists(localManxcatConfig.WeightingFileName))
                {
                    File.Copy(localManxcatConfig.WeightingFileName,
                              Path.Combine(remoteInputDir, IOUtil.ManxcatWeightingFilePrefix + Path.GetFileName(localManxcatConfig.WeightingFileName)), true);
                }

                if (File.Exists(localManxcatConfig.ScalingFileName))
                {
                    File.Copy(localManxcatConfig.ScalingFileName,
                              Path.Combine(remoteInputDir, IOUtil.ManxcatScalingFilePrefix + Path.GetFileName(localManxcatConfig.ScalingFileName)), true);
                }
            }
            else if (_appType == ApplicationType.PairwiseClustering)
            {
                PairwiseSection localPairwiseConfig = _confMgr.PairwiseSection;
                if (localPairwiseConfig.DistanceMatrixFile !=null && !localPairwiseConfig.DistanceMatrixFile.StartsWith("$") && File.Exists(localPairwiseConfig.DistanceMatrixFile))
                {
                    File.Copy(localPairwiseConfig.DistanceMatrixFile,
                              Path.Combine(remoteInputDir,
                                           IOUtil.PairwiseDistanceFilePrefix +
                                           Path.GetFileName(localPairwiseConfig.DistanceMatrixFile)), true);
                }

                if (localPairwiseConfig.IndexFile != null && !localPairwiseConfig.IndexFile.StartsWith("$") && File.Exists(localPairwiseConfig.IndexFile))
                {
                    File.Copy(localPairwiseConfig.IndexFile,
                              Path.Combine(remoteInputDir,
                                           IOUtil.PairwiseIndexFilePrefix +
                                           Path.GetFileName(localPairwiseConfig.IndexFile)), true);
                }

                if (localPairwiseConfig.LabelFile != null && !localPairwiseConfig.LabelFile.StartsWith("$") && File.Exists(localPairwiseConfig.LabelFile))
                {
                    File.Copy(localPairwiseConfig.LabelFile,
                              Path.Combine(remoteInputDir, IOUtil.PairwiseLabelFilePrefix + Path.GetFileName(localPairwiseConfig.LabelFile)),
                              true);
                }

                if (localPairwiseConfig.AddMdsFile != null && !localPairwiseConfig.AddMdsFile.StartsWith("$") && File.Exists(localPairwiseConfig.AddMdsFile))
                {
                    File.Copy(localPairwiseConfig.AddMdsFile,
                              Path.Combine(remoteInputDir, IOUtil.PairwiseMdsFilePrefix + Path.GetFileName(localPairwiseConfig.AddMdsFile)),
                              true);
                }

                if (localPairwiseConfig.ClusterNumberFile != null && !localPairwiseConfig.ClusterNumberFile.StartsWith("$") && File.Exists(localPairwiseConfig.ClusterNumberFile))
                {
                    File.Copy(localPairwiseConfig.ClusterNumberFile,
                              Path.Combine(remoteInputDir, IOUtil.PairwiseClusterNumberFilePrefix + Path.GetFileName(localPairwiseConfig.ClusterNumberFile)),
                              true);
                }
            }
            else if (_appType == ApplicationType.SequentialSponge)
            {
                
                DAVectorSpongeSection localSpongeConfig = _confMgr.DAVectorSpongeSection;
                if (!localSpongeConfig.DistanceMatrixFile.StartsWith("$") && File.Exists(localSpongeConfig.DistanceMatrixFile))
                {
                    File.Copy(localSpongeConfig.DistanceMatrixFile,
                              Path.Combine(remoteInputDir, IOUtil.SpongeDistanceFilePrefix + Path.GetFileName(localSpongeConfig.DistanceMatrixFile)), true);
                }

                if (!localSpongeConfig.LabelFile.StartsWith("$") && File.Exists(localSpongeConfig.LabelFile))
                {
                    File.Copy(localSpongeConfig.LabelFile,
                              Path.Combine(remoteInputDir, IOUtil.SpongeLabelsFilePrefix + Path.GetFileName(localSpongeConfig.LabelFile)),
                              true);
                }

                // Todo (sponge) - files to copy to headnode (any other than the ones above)?
            }
        }

        private ManxcatSection GenerateRemoteManxcatConfig()
        {
            ManxcatSection remoteConfig = new ManxcatSection();
            CopyManxcatConfiguration(_confMgr.ManxcatSection, remoteConfig, true);
            ChangeManxcatConfigPaths(_runInfo.HnProjectDir, remoteConfig, true);
            return remoteConfig;
        }

        private PairwiseSection GenerateRemotePairwiseConfig()
        {
            PairwiseSection remoteConfig = new PairwiseSection();
            CopyPairwiseConfiguration(_confMgr.PairwiseSection, remoteConfig, true);
            ChangePairwiseConfigPaths(_runInfo.HnProjectDir, remoteConfig, true);
            return remoteConfig;
        }

        private DAVectorSpongeSection GenerateRemoteSpongeConfig()
        {
            DAVectorSpongeSection remoteConfig = new DAVectorSpongeSection();
            CopySpongeConfiguration(_confMgr.DAVectorSpongeSection, remoteConfig, true);
            ChangeSpongeConfigPaths(_runInfo.HnProjectDir, remoteConfig, true);
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

                    UndoPopulateRunInfo();
                    UndoPrepareRemoteRunPreUI();
                    UndoPrepareRemoteRunPreEnv();
                    AppendToOut(LogMessages.RunAborted);

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

        private void UndoPopulateRunInfo()
        {
            _runInfo = null;
        }

        /* Populate run info */

        private void PopulateRunInfo()
        {
            _runInfo = new RunInfo();
            _runInfo.AppType = _appType;

            _runInfo.AppDir = appDirTxt.Text;
            _runInfo.AutoIncr = autoIncrChkBx.Checked;
            _runInfo.BaseDir = baseDirTxt.Text;
            _runInfo.ComplementryTextFile = complementryTextFileTxt.Text;

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

            if (_appType == ApplicationType.Manxcat)
            {
                if (!Utils.ValidateManxcatAppDir(appDirTxt.Text, true))
                {
                    if (ChangeAppDir() == System.Windows.Forms.DialogResult.Cancel)
                    {
                        MessageBox.Show(string.Format("{0} aborted", nameTxt.Text), "Run Aborted", MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                        isValid = false;
                    }
                }
            }
            else if (_appType == ApplicationType.PairwiseClustering)
            {
                if (!Utils.ValidatePairwiseAppDir(appDirTxt.Text, true))
                {
                    if (ChangeAppDir() == System.Windows.Forms.DialogResult.Cancel)
                    {
                        MessageBox.Show(string.Format("{0} aborted", nameTxt.Text), "Run Aborted", MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                        isValid = false;
                    }
                }
            }
            else if (_appType == ApplicationType.SequentialSponge)
            {
                if (!Utils.ValidateSpongeAppDir(appDirTxt.Text, true))
                {
                    if (ChangeAppDir() == System.Windows.Forms.DialogResult.Cancel)
                    {
                        MessageBox.Show(string.Format("{0} aborted", nameTxt.Text), "Run Aborted", MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                        isValid = false;
                    }
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
                UndoPrepareRemoteRunPreUI();
                UndoPrepareRemoteRunPreEnv();
                AppendToOut(LogMessages.RunAborted);
            }
            return isValid;
        }

        private void UndoPrepareRemoteRunPreUI()
        {
            rootSplit.Panel1.Enabled = true;
            mainSplit.Panel1.Enabled = true;
            mainSplit.Panel2.Enabled = true;
            runBtn.Enabled = true;
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

            if (!Utils.ValidateManxcatAppDir(appDirTxt.Text, true))
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
            _runInfo.AppDir = appDirTxt.Text;
            _runInfo.BaseDir = baseDirTxt.Text;
            _runInfo.Name = nameTxt.Text;
            _runInfo.ComplementryTextFile = complementryTextFileTxt.Text;
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
                CopyManxcatConfiguration(_confMgr.ManxcatSection, _referenceManxcatConfig, true);
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
                    pvdf = File.Exists(runInfo.ComplementryTextFile)
                               ? PlotVizDataFile.Build(config.IndexFile, runInfo.ComplementryTextFile, pointsFile)
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
//                    pvdf = File.Exists(runInfo.ComplementryTextFile)
//                               ? PlotVizDataFile.Build(config.IndexFile, runInfo.ComplementryTextFile, pointsFile)
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
                    complementryTextFileTxt.Text = dlg.FileName;
                }
            }
        }

        private void showInPvizBtn_Click(object sender, EventArgs e)
        {
            if (_appType == ApplicationType.Manxcat)
            {
                string localPlotFile = string.Empty;
                string ptsFile = Path.Combine(_runDir, IOUtil.ManxcatSimplePointsFileNamePrefix +
                                                       Path.GetFileName(
                                                           _confMgr.ManxcatSection.ReducedVectorOutputFileName));
                if (File.Exists(ptsFile))
                {
                    string[] files = Directory.GetFiles(_runDir, IOUtil.LocalPlotTxtFileName + "*.txt");
                    localPlotFile = files != null && files.Length > 0 ? files[0] : string.Empty;
                    if (string.IsNullOrEmpty(complementryTextFileTxt.Text))
                    {
                        if (File.Exists(localPlotFile))
                        {
                            File.Delete(localPlotFile);
                        }
                        _runInfo.ComplementryTextFile = string.Empty;
                        _runInfo.SaveAs(Path.Combine(_runDir, _runInfo.Name + IOUtil.RunInfoExt));
                        ShowInPviz(new[] {ptsFile});        
                    }
                    else if (File.Exists(complementryTextFileTxt.Text))
                    {
                        if (!_runInfo.ComplementryTextFile.Equals(complementryTextFileTxt.Text))
                        {
                            if (File.Exists(localPlotFile))
                            {
                                File.Delete(localPlotFile);
                            }
                            _runInfo.ComplementryTextFile = complementryTextFileTxt.Text;
                            _runInfo.SaveAs(Path.Combine(_runDir, _runInfo.Name + IOUtil.RunInfoExt));
                            CreateLocalPlotFiles(ptsFile, new[] {complementryTextFileTxt.Text});
                        }

                        if (!File.Exists(localPlotFile))
                        {
                            CreateLocalPlotFiles(ptsFile, new []{_runInfo.ComplementryTextFile});
                        }
                        ShowInPviz(new []{localPlotFile});
                    }
                    else
                    {
                        MessageBox.Show(LogMessages.NoClusterFile, LogCategories.Error, MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                    }
                }
            }
            else if (_appType == ApplicationType.PairwiseClustering)
            {
                // todo: PWC - naive plot creation everytime
                if (File.Exists(complementryTextFileTxt.Text))
                {
                    _runInfo.ComplementryTextFile = complementryTextFileTxt.Text;
                    _runInfo.SaveAs(Path.Combine(_runDir, _runInfo.Name + IOUtil.RunInfoExt));

                    string[] clusterFiles = Directory.GetFiles(_runDir,
                                                               Path.GetFileNameWithoutExtension(
                                                                   _confMgr.PairwiseSection.ClusterFile) + "*.txt");
                    if (clusterFiles != null && clusterFiles.Length > 0)
                    {
                        CreateLocalPlotFiles(complementryTextFileTxt.Text, clusterFiles);
                        int N = _confMgr.PairwiseSection.MaxNcent;

                        int MAX = 4;
                        int X = N >= MAX ? MAX: N;
                        // Just take the last X plots since plotviz will crash otherwise

                        string[] plotFiles = new string[X];
                        int count = 0;
                        for (int i = N; i > 0; --i)
                        {
                            string plotFile = Path.Combine(_runDir, IOUtil.LocalPlotTxtFileName + "_" + string.Format(ConstantNames.PairwiseClusterFileNameFormat, Path.GetFileNameWithoutExtension(_confMgr.PairwiseSection.ClusterFile), N, i));
                            if (count < X && File.Exists(plotFile))
                            {
                                plotFiles[count] = plotFile;
                                ++count;
                            }
                        }
                        ShowInPviz(plotFiles);
                    }
                }
                else
                {
                    MessageBox.Show(LogMessages.NoCoordinatesFile, LogCategories.Error, MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
            else if (_appType == ApplicationType.SequentialSponge)
            {
                // Todo (sponge) - show in pviz for sponge?
            }
        }

        private void ShowInPviz(string[] plotFiles)
        {
            if (!File.Exists(_settings.DefaultPlotVizExe))
            {
                // todo: fill here - if plotviz couldn't be found
                //                using (ConfigDlg dlg = new ConfigDlg())
                _settings.Save();
            }

            var start = new ProcessStartInfo(_settings.DefaultPlotVizExe)
                            {
                                UseShellExecute = true,
                                RedirectStandardOutput = false,
                                CreateNoWindow = false,
                                Arguments = string.Join(" ", plotFiles)
                            };
            Process.Start(start);
        }

        private void rebuildBtn_Click(object sender, EventArgs e)
        {
            string coordinatesFile = Path.Combine(
                Path.GetDirectoryName(_referenceManxcatConfig.ReducedVectorOutputFileName),
                string.Format("SIMPLE{0}", Path.GetFileName(_referenceManxcatConfig.ReducedVectorOutputFileName)));
            using (var dlg = new PlotVizBuilderDlg(_referenceManxcatConfig.IndexFile, _runInfo.ComplementryTextFile, coordinatesFile)
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
                ShowInPviz(new []{plotFile});
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
                _runStableEvt.WaitOne();
                // UI events like new/open with confirm should also try to acquire this lock before stopping job tracker
                lock (_jobtrackerMonitorStopLock)
                {
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
                    _monitorEndEvt.WaitOne(); // Wait till monitor ends
                    // todo: beta2 - remove these and use localstatus files 
                    // Save local out and err texts

                    try
                    {
                        if (outTxt.Text.Length > 0)
                        {
                            string text = outTxt.Text;
                            outTxt.Text = @"Please wait while content is written to disk";
                            outTxt.Refresh();
                            using (StreamWriter writer = new StreamWriter(_runInfo.LocalOutFile))
                            {
                                writer.Write(text);
                            }
                            outTxt.AppendText(Environment.NewLine + "  Done.");
                        }
                    }
                    catch(Exception e)
                    {
                        
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
            if (!_settings.DefaultTargetDirectory.Equals(targetDirTxt.Text))
            {
                _settings.DefaultTargetDirectory = targetDirTxt.Text;
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
            if (File.Exists(IOUtil.KnownMatricesFileName))
            {
                _knownFiles = new Hashtable();
                using (StreamReader reader = new StreamReader(IOUtil.KnownMatricesFileName))
                {
                    char[] sep = new[] {' ', '\t'};
                    string[] splits;
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line) && !line.StartsWith("#"))
                        {
                            splits = line.Trim().Split(sep);
                            _knownFiles.Add(splits[0], splits[1]);
                        }
                    }
                }
            }

            if (File.Exists(Settings.SettingsFileName))
            {
                _settings = Settings.LoadSettings(Settings.SettingsFileName);
            }
            else
            {
                _settings = new Settings();
                _settings.Save();
            }

            // Load headnodes once and keep it saved till window is open
            /*_headNodes = HPCUtil.LoadHeadNodes();*/ // Not necessary as we have only Tempest now.
            _headNodes = new[] {"TEMPEST.ads.iu.edu"};

            // For compatibilty reasons 
            _oldHeadNodes = new Dictionary<int, string>
                                {
                                    {0, "MADRID-HEADNODE.ads.iu.edu"},
                                    {1, "D2I-HPC-01.ads.iu.edu"},
                                    {2, "bl-uits-cloud1.ads.iu.edu"},
                                    {3, "xen73.ads.iu.edu"},
                                    {4, "i64.ads.iu.edu"},
                                    {5, "CGLVM1.ads.iu.edu"},
                                    {6, "CGLVM2.ads.iu.edu"},
                                    {7, "bl-soic-mugo.ads.iu.edu"},
                                    {8, "bl-chem-mfj48.ads.iu.edu"},
                                    {9, "TEMPEST.ads.iu.edu"},
                                    {10, "IU-CGL-HPC00.ads.iu.edu"},
                                    {11, "STORM.ads.iu.edu"}
                                };
            
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

        private void manxcatRadio_CheckedChanged(object sender, EventArgs e)
        {
            ChangeAppType();
        }

        private void pairwiseRadio_CheckedChanged(object sender, EventArgs e)
        {
            ChangeAppType();
        }

        private void spongeRadio_CheckedChanged(object sender, EventArgs e)
        {
            ChangeAppType();
        }

        private void ChangeAppType()
        {
            _appType = manxcatRadio.Checked
                           ? ApplicationType.Manxcat
                           : pairwiseRadio.Checked
                                 ? ApplicationType.PairwiseClustering
                                 : ApplicationType.SequentialSponge;

            /* UI and value changes */
            if (_appType == ApplicationType.Manxcat)
            {
                appDirGroupBx.Text = ConstantNames.ManxcatAppDirGroupBxText;
                runInfoGroupBx.Text = ConstantNames.ManxcatRunInfoGroupBxText;
                fileGroupBx.Text = ConstantNames.ManxcatFileGroupBxText;
                fileLabel.Text = ConstantNames.ManxcatFileLabelText;
                configGroupBx.Text = ConstantNames.ManxcatConfigurationGroupBxText;
                appDirTxt.Text = _settings.DefaultManxcatAppDirectory;
                if (_confMgr != null)
                {
                    pGrid.SelectedObject = _confMgr.ManxcatSection;
                }

                _autoNamePrefix = ConstantNames.DefaultManxcatRunNamePrefix;
                _autoNamePrefixRegex = new Regex(@"^" + ConstantNames.DefaultManxcatRunNamePrefix + @"\d+$");
                GenerateAutoName();

            }
            else if (_appType == ApplicationType.PairwiseClustering)
            {
                appDirGroupBx.Text = ConstantNames.PairwiseAppDirGroupBxText;
                runInfoGroupBx.Text = ConstantNames.PairwiseRunInfoGroupBxText;
                fileGroupBx.Text = ConstantNames.PairwiseFileGroupBxText;
                fileLabel.Text = ConstantNames.PairwiseFileLabelText;
                configGroupBx.Text = ConstantNames.PairwiseConfigurationGroupBxText;
                appDirTxt.Text = _settings.DefaultPairwiseAppDirectory;
                if (_confMgr != null)
                {
                    pGrid.SelectedObject = _confMgr.PairwiseSection;
                }

                _autoNamePrefix = ConstantNames.DefaultPairwiseRunNamePrefix;
                _autoNamePrefixRegex = new Regex(@"^" + ConstantNames.DefaultPairwiseRunNamePrefix + @"\d+$");
                GenerateAutoName();
            }
            else if (_appType == ApplicationType.SequentialSponge)
            {
                appDirGroupBx.Text = ConstantNames.SpongeAppDirGroupBxText;
                runInfoGroupBx.Text = ConstantNames.SpongeRunInfoGroupBxText;

                // Todo (sponge) - fileGroupBx and fileLabel text values?
                fileGroupBx.Text = ConstantNames.PairwiseFileGroupBxText;
                fileLabel.Text = ConstantNames.PairwiseFileLabelText;

                configGroupBx.Text = ConstantNames.SpongConfigurationGroupBxText;

                appDirTxt.Text = _settings.DefaultSpongeAppDirectory;

                if (_confMgr != null)
                {
                    pGrid.SelectedObject = _confMgr.DAVectorSpongeSection;
                }

                _autoNamePrefix = ConstantNames.DefaultSpongeRunNamePrefix;
                _autoNamePrefixRegex = new Regex(@"^" + ConstantNames.DefaultPairwiseRunNamePrefix + @"\d+$");
                GenerateAutoName();
            }
        }

       

        private void addKnownFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddKnownFile();
        }

        private void AddKnownFile()
        {
            using (
                var dlg = new KnownFileDlg(_headNodes, hnComboBx.SelectedIndex,
                                           ((ISchedulerNode[]) cnBindingSource.DataSource).Select(x => x.Name).ToArray(),
                                           targetDirTxt.Text, _knownFiles))
            {
                dlg.ShowDialog();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            AddKnownFile();
        }

        
    }
}