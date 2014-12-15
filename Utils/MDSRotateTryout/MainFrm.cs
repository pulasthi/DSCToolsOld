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
using Salsa.Core.Configuration;
using Salsa.Core.Configuration.Sections;



namespace MDSTryout
{
    public partial class MainFrm : Form
    {
        private Settings _settings;
        private ConfigurationMgr _confMgr;
        private ManxcatSection _referenceMDSConfig;

        private RunInfo _runInfo;

        private string _autoNamePrefix = "MDS_Run_";
        private Regex _autoNamePrefixRegex = new Regex(@"^MDS_Run_\d+$");

        private bool _isRunSuccess = false;
        private string _runDir;
        private MDSRunnable _mdsRunnable;

        public MainFrm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            NewRun();     
        }

        private void NewRun()
        {
            _settings = File.Exists(Settings.SettingsFileName) ? Settings.LoadSettings(Settings.SettingsFileName) : new Settings();
            _settings.Save();
            _confMgr = new ConfigurationMgr();
            _referenceMDSConfig = new ManxcatSection();

            pGrid.SelectedObject = _confMgr.ManxcatSection;
            mdsAppDirTxt.Text = _settings.DefaultMDSAppDirectory;
            baseDirTxt.Text = _settings.DefaultBaseDirectory;
            processTxt.Text = processBar.Value.ToString();

            resetConfigBtn.Enabled = false;
            autoIncrChkBx.Checked = true;
            runIncBtn.Visible = false;
            outTxt.Text = string.Empty;
            outTxt.Enabled = true;
            errTxt.Enabled = true;
            showRefPlotBtn.Enabled = false;
            showInPvizBtn.Enabled = false;
            rebuildBtn.Enabled = false;

            GenerateAutoName();
            mainSplit.Panel2.Enabled = true;
            InitializeConfig();


            // Todo: later, probably with http://www.codeproject.com/KB/cs/FilteredPropertyGrid.aspx
            //CustomizePGrid();
        }

        private void OpenRun()
        {
            if (File.Exists(Settings.SettingsFileName))
            {
                Settings.LoadSettings(Settings.SettingsFileName);
            }
            else
            {
                _settings = new Settings();
                _settings.Save();
            }

            resetConfigBtn.Enabled = false;
            
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Run Info Files (*.infx)|*.infx";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    

                    _runInfo = RunInfo.LoadRunInfo(dlg.FileName);
                    mdsAppDirTxt.Text = _runInfo.AppDir;
                    baseDirTxt.Text = _runInfo.BaseDir;
                    nameTxt.Text = _runInfo.Name;
                    clusTx.Text = _runInfo.ClusterFile;
                    processBar.Value = _runInfo.Processes;
                    processTxt.Text = _runInfo.Processes.ToString();
                    autoIncrChkBx.Checked = _runInfo.AutoIncr;

                    _confMgr = ConfigurationMgr.LoadConfiguration(Path.Combine(_runInfo.BaseDir, _runInfo.Name, "config.xml"), false);
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
        }

        private void LoadSummary(string summaryFile)
        {
            using (StreamReader reader = new StreamReader(summaryFile))
            {
                int skip = 168;
                int count = 0;
                string line;
                while (!reader.EndOfStream && count < skip)
                {
                    reader.ReadLine();
                    count++;
                }
                outTxt.AppendText(reader.ReadToEnd().Replace("\n\n", string.Format("{0}{1}", Environment.NewLine, Environment.NewLine)));
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
            IEnumerable<string> dirs = Directory.GetDirectories(baseDirTxt.Text).Where<string>(x => _autoNamePrefixRegex.IsMatch(x.Substring(x.LastIndexOf(Path.DirectorySeparatorChar) + 1)));
            int maxIdx = dirs.Aggregate<string, int>(-1, (max, x) => { int idx = int.Parse(x.Substring(x.LastIndexOf(Path.DirectorySeparatorChar) + 9)); return idx >= max ? idx : max; });
            nameTxt.Text = _autoNamePrefix + (maxIdx + 1);
        }

        private void ChangeConfigPaths()
        {
            string projectPath = Path.Combine(baseDirTxt.Text, nameTxt.Text);
            ManxcatSection manxcat = _confMgr.ManxcatSection;
            string fname = manxcat.ReducedVectorOutputFileName.Substring(manxcat.ReducedVectorOutputFileName.LastIndexOf(Path.DirectorySeparatorChar)+1);
            manxcat.ReducedVectorOutputFileName = Path.Combine(projectPath, fname);
            fname = manxcat.SummaryOutputFileName.Substring(manxcat.SummaryOutputFileName.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            manxcat.SummaryOutputFileName = Path.Combine(projectPath, fname);
            fname = manxcat.TimingOutputFileName.Substring(manxcat.TimingOutputFileName.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            manxcat.TimingOutputFileName = Path.Combine(projectPath, fname);
            manxcat.ControlDirectoryName = projectPath;
            manxcat.BaseResultDirectoryName = projectPath;
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
            using (ConfigDlg dlg = new ConfigDlg(_settings.DefaultMDSAppDirectory, ConfigDlg.ConfigDlgType.MDSAppDirectory))
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
                    CopyMDSConfiguration(external, _confMgr.ManxcatSection,true);
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
            runJob();
        }

        private void runJob()
        {
            _isRunSuccess = true; 

            if (!Utils.ValidateMDSAppDir(mdsAppDirTxt.Text, true))
            {
                if (ChangeAppDir() == System.Windows.Forms.DialogResult.Cancel)
                {
                    MessageBox.Show(string.Format("{0} aborted", nameTxt.Text), "Run Aborted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            if (!Utils.ValidateBaseDirectory(baseDirTxt.Text))
            {
                if (ChangeBaseDir() == System.Windows.Forms.DialogResult.Cancel)
                {
                    MessageBox.Show(string.Format("{0} aborted", nameTxt.Text), "Run Aborted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            _runInfo = new RunInfo();
            _runInfo.AppDir = mdsAppDirTxt.Text;
            _runInfo.BaseDir = baseDirTxt.Text;
            _runInfo.Name = nameTxt.Text;
            _runInfo.ClusterFile = clusTx.Text;
            _runInfo.AutoIncr = autoIncrChkBx.Checked;
            _runInfo.Processes = processBar.Value;

            _runDir = Path.Combine(_runInfo.BaseDir, _runInfo.Name);
            if (Directory.Exists(_runDir))
            {
                string msg = "A previous run by this name already exists.\nDo you want to overwrite its content?";
                if (MessageBox.Show(msg, "Run Exist", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                {
                    MessageBox.Show(string.Format("{0} aborted", _runInfo.Name), "Run Aborted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else
            {
                Directory.CreateDirectory(_runDir);
            }


            // Todo: saliya - extremely quick hack to set threads
            string cmd = @"mpiexec -np {0} {1}\ManxcatMDS.exe /configFile={2}\config.xml /nodeCount=1 /threadCount={3}";
            cmd = string.Format(cmd, _runInfo.Processes, _runInfo.AppDir, _runDir, threadTxt.Text);

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
            if(this.InvokeRequired)
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
                this.Invoke(new proc_OutputDataReceivedDelegate(proc_OutputDataReceived), new[] { sender, e });
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
                        outTxt.AppendText(Environment.NewLine + e.Data.Replace("\n", string.Format("{0}", Environment.NewLine)));
                        
                    }
                }
                
            }
        }

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
            NewRun();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            OpenRun();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewRun();
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
                    clusTx.Text = dlg.FileName;
                }
            }
        }

        private void showInPvizBtn_Click(object sender, EventArgs e)
        {
            ShowInPviz(Path.Combine(_runInfo.BaseDir, _runInfo.Name,
                                           string.Format("{0}-plot.pviz", _runInfo.Name)));
        }

        private void ShowInPviz(string plotFile)
        {
            if (!File.Exists(_settings.DefPlotVizExe))
            {
                // todo: fill here
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
            using (Process process = Process.Start(start))
            {
//                process.WaitForExit();
            }
        }

        private void rebuildBtn_Click(object sender, EventArgs e)
        {
            string coordinatesFile = Path.Combine(
                Path.GetDirectoryName(_referenceMDSConfig.ReducedVectorOutputFileName),
                string.Format("SIMPLE{0}", Path.GetFileName(_referenceMDSConfig.ReducedVectorOutputFileName)));
            using (var dlg = new PlotVizBuilderDlg(_referenceMDSConfig.IndexFile, _runInfo.ClusterFile, coordinatesFile))
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
            _mdsRunnable.kill();
        }

       
    }
}
