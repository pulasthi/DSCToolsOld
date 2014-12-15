using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Hpc.Scheduler;
using Microsoft.Hpc.Scheduler.Properties;
using System.IO;
using Salsa.Core.Configuration;
using Salsa.Core.Configuration.Sections;

namespace PetqLight
{
    public enum ExecutionType
    {
        Local,
        Remote
    }

    public enum ApplicationType
    {
        NW,
        SWG,
        SWMS,
        PWC,
        MDS,
        UNDEFINED
    }

    public partial class MainFrm : Form
    {
        private IScheduler _scheduler;
        // todo: saliya - at the moment cached forever untill application is reloaded
        private string[] _cachedHeadNodes;
        private ExecutionType _executionType;

        private ApplicationType _saApp;
        private ApplicationType _pwcApp;
        private ApplicationType _mdsApp;

        private ConfigurationMgr _mgr;
        
        public MainFrm()
        {
            InitializeComponent();
        }

        #region TopController

        private void pathBrowseBtn_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    directoryTxt.Text = dlg.SelectedPath;
                }
            }
        }

        private void fileBrowseBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "FASTA files (*.txt,*.fa)|*.txt;*.fa" +
                    "|All Files (*.*)|*.*";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    inFileTxt.Text = dlg.FileName;
                }
            }
        }

        #endregion

        #region Generator

        private void genBtn_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                string projectDir = directoryTxt.Text;
                string projectName = nameTxt.Text;
                string inputFile = inFileTxt.Text;

                if (FileIO.CreateProjectStructure(projectDir, projectName))
                {
                    FileIO.Copy(Path.Combine(projectDir, projectName, Constants.InputDirName), inputFile);
                    FileIO.CopyBinaries(Path.Combine(projectDir, projectName), _saApp, _pwcApp, _mdsApp);
                    if (_executionType == ExecutionType.Local)
                    {
                        HandleLocalExecution(projectDir, projectName, inputFile);
                    }
                    else if (_executionType == ExecutionType.Remote)
                    {
                        HandleRemoteExecution(projectDir, projectName, inputFile);
                    }
                    MessageBox.Show("Project generated successfully!", "Generation Complete");
                }
                else
                {
                    MessageBox.Show("Unable to create project in " + directoryTxt.Text, "Error Creating Project");
                }
            }
        }

        private bool IsValid()
        {
            if (String.IsNullOrEmpty(directoryTxt.Text))
            {
                MessageBox.Show("Project directory is null", "Invalid Data");
                return false;
            }

            if (String.IsNullOrEmpty(nameTxt.Text))
            {
                MessageBox.Show("Project name is null", "Invalid Data");
                return false;
            }

            if (String.IsNullOrEmpty(inFileTxt.Text))
            {
                MessageBox.Show("Input file is null", "Invalid Data");
                return false;
            }

            if (!Directory.Exists(directoryTxt.Text))
            {
                MessageBox.Show("Project directory does not exist", "Invalid Data");
                return false;
            }

            if (Directory.Exists(Path.Combine(directoryTxt.Text, nameTxt.Text)))
            {
                MessageBox.Show("Project already exists", "Invalid Data");
                return false;
            }

            if (!File.Exists(inFileTxt.Text))
            {
                MessageBox.Show("Input file does not exist", "Invalid Data");
                return false;
            }

            if (localRBtn.Checked && String.IsNullOrEmpty(processBox.Text))
            {
                MessageBox.Show("Number of processes should contain a valid number", "Invalid Data");
                return false;
            }

            try
            {
                int.Parse(processBox.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Number of processes has an invalid format. Should be an integer", "Invalid Data");
                return false;
            }
            catch (OverflowException)
            {
                MessageBox.Show("Number of processes overflows the integer range", "Invalid Data");
                return false;
            }


            return true;
        }

        private void HandleLocalExecution(string projectDir, string projectName, string inputFile)
        {
            int numOfProcesses = int.Parse(processBox.Text);
            int sequenceCount = FileIO.GetSequenceCount(inputFile);
            string inputFileName = Path.GetFileName(inputFile);

            // Default configuration
            ConfUtil.ConfigureLocalExecution(_mgr, projectDir, projectName, inputFileName, sequenceCount)
                .SaveAs(Path.Combine(projectDir, projectName, Constants.ConfigDirName, Constants.ConfigFileName));

            // GoLocal.bat
            FileIO.SaveAs(BatGenerator.GenerateGoLocalBat(
                projectDir, projectName, numOfProcesses, _saApp, _pwcApp, _mdsApp),
                Path.Combine(projectDir, projectName, Constants.GoLocalBatFileName));


        }

        private void HandleRemoteExecution(string projectDir, string projectName, string inputFile)
        {
            int sequenceCount = FileIO.GetSequenceCount(inputFile);
            string inputFileName = Path.GetFileName(inputFile);

            string targetDir = targetTxt.Text;

            string headNode = headNodeCombo.Text;
            IEnumerable<ISchedulerNode> computeNodes = nodeBox.SelectedItems.Cast<ISchedulerNode>();
            IEnumerable<string> nodeNames = computeNodes.Select<ISchedulerNode, string>(x => x.Name);
            
            int nodes = computeNodes.Count();
            int cores = computeNodes.Select<ISchedulerNode, int>(x => x.NumberOfCores).Sum();
            int avg = cores / nodes;

            // Default configuration
            ConfUtil.ConfigureRemoteExecution(_mgr, headNode, targetDir, projectName, inputFileName, sequenceCount)
                .SaveAs(Path.Combine(projectDir, projectName, Constants.ConfigDirName, Constants.ConfigFileName));

            // Setup.bat
            FileIO.SaveAs(BatGenerator.GenerateSetupBat(headNode, targetDir, projectName, nodeNames),
                Path.Combine(projectDir, projectName, Constants.SetupBatFileName));

            // Cleanup.bat
            FileIO.SaveAs(BatGenerator.GenerateCleanupBat(targetDir, projectName, nodeNames),
                Path.Combine(projectDir, projectName, Constants.CleanupBatFileName));

            
            // Go.bat
            FileIO.SaveAs(BatGenerator.GenerateGoBat(
                headNode, targetDir, projectName, nodes, cores, avg, nodeNames, _saApp, _pwcApp, _mdsApp),
                Path.Combine(projectDir, projectName, Constants.GoBatFileName));
        }

        #endregion

        #region FormLoad

        private void Form1_Load(object sender, EventArgs e)
        {
            label8.Location = label5.Location;
            processBox.Location = headNodeCombo.Location;
            localRBtn.Checked = true;
            saChkBx.Checked = true;
            swmsRBtn.Checked = true;
            pwcChkBx.Checked = true;
            mdsChkBx.Checked = true;
            directoryTxt.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            
            _scheduler = new Scheduler();
            _mgr = ConfUtil.DefaultConfiguration;
        }

        #endregion

        #region BottomController

        private void headNodeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string headNode = headNodeCombo.SelectedValue as string;
            try
            {
                _scheduler.Connect(headNode);
                ISchedulerNode[] nodes = HPCUtil.GetComputeNodes(_scheduler);
                nodeBox.DataSource = nodes;
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "Error Loading Compute Nodes");
            }
        }

        private void selectAllLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < nodeBox.Items.Count; i++)
            {
                if (!nodeBox.GetSelected(i))
                {
                    nodeBox.SetSelected(i, true);
                }
            }
        }

        private void inverseLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < nodeBox.Items.Count; i++)
            {
                nodeBox.SetSelected(i, !nodeBox.GetSelected(i));
            }
        }

        private void clearLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            nodeBox.ClearSelected();
        }

        private void remoteRBtn_CheckedChanged(object sender, EventArgs e)
        {
            ShowLocalComponents(false);
            if (_cachedHeadNodes == null)
            {
                _cachedHeadNodes = HPCUtil.LoadHeadNodes();
                headNodeCombo.DataSource = _cachedHeadNodes;
            }
            this.Height = 700;
            ShowRemoteComponents(true);
            _executionType = ExecutionType.Remote;
        }

        private void localRBtn_CheckedChanged(object sender, EventArgs e)
        {
            ShowRemoteComponents(false);
            ShowLocalComponents(true);
            this.Height = 350;
            _executionType = ExecutionType.Local;
        }

        private void ShowRemoteComponents(bool visible)
        {
            label5.Visible = visible;
            headNodeCombo.Visible = visible;
            label6.Visible = visible;
            groupBox3.Visible = visible;
            label4.Visible = visible;
            targetTxt.Visible = visible;
            genSubBtn.Text = "Submit";
        }

        private void ShowLocalComponents(bool visible)
        {
            label8.Visible = visible;
            processBox.Visible = visible;
            genSubBtn.Text = "Generate";
        }

        #endregion

       

        private void saChkBx_CheckedChanged(object sender, EventArgs e)
        {
            saGroup.Enabled = saChkBx.Checked;
            saConfLink.Visible = saChkBx.Checked;
            if (!saChkBx.Checked)
            {
                _saApp = ApplicationType.UNDEFINED;
            }
            else 
            {
                if (nwRBtn.Checked)
                {
                    _saApp = ApplicationType.NW;
                }
                else if (swgRBtn.Checked)
                {
                    _saApp = ApplicationType.SWG;
                }
                else if (swmsRBtn.Checked)
                {
                    _saApp = ApplicationType.SWMS;
                }
            }
        }

        private void nwRBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (nwRBtn.Checked)
            {
                _saApp = ApplicationType.NW;
            }
        }

        private void swgRBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (swgRBtn.Checked)
            {
                _saApp = ApplicationType.SWG;
            }
            
        }

        private void swmsRBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (swmsRBtn.Checked)
            {
                _saApp = ApplicationType.SWMS;
            }
        }

        private void pwcChkBx_CheckedChanged(object sender, EventArgs e)
        {
            pwcConfLink.Visible = pwcChkBx.Checked;
            _pwcApp = pwcChkBx.Checked ? ApplicationType.PWC : ApplicationType.UNDEFINED;
        }

        private void mdsChkBx_CheckedChanged(object sender, EventArgs e)
        {
            mdsConfLink.Visible = mdsChkBx.Checked;
            _mdsApp = mdsChkBx.Checked ? ApplicationType.MDS : ApplicationType.UNDEFINED;
        }

        private void saConfLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowConfigurationDialog(_saApp, _mgr.GetSection(_saApp.ToString()), Constants.GetTitleForApplication(_saApp));
        }

        private void ShowConfigurationDialog(ApplicationType appType, Section sec, string name)
        {
            using (ConfDlg dlg = new ConfDlg(sec, name, appType))
            {
                if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
                {
                    switch (appType) 
                    {
                        case ApplicationType.NW:
                            _mgr.NeedlemanWunschSection = dlg.Original as NeedlemanWunschSection;
                            break;
                        case ApplicationType.SWMS:
                            _mgr.SmithWatermanMS = dlg.Original as SmithWatermanMS;
                            break;
                        case ApplicationType.SWG:
                            _mgr.SmithWatermanSection = dlg.Original as SmithWatermanSection;
                            break;
                        case ApplicationType.PWC:
                            _mgr.PairwiseSection = dlg.Original as PairwiseSection;
                            break;
                        case ApplicationType.MDS:
                            _mgr.ManxcatSection = dlg.Original as ManxcatSection;
                            break;
                    }
 
                }
            }
        }

        private void pwcConfLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowConfigurationDialog(_pwcApp, _mgr.PairwiseSection, Constants.GetTitleForApplication(_pwcApp));
        }

        private void mdsConfLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowConfigurationDialog(_mdsApp, _mgr.ManxcatSection, Constants.GetTitleForApplication(_mdsApp));
        }
    }
}
