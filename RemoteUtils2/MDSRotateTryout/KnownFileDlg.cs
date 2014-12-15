using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Hpc.Scheduler;
using Microsoft.Hpc.Scheduler.Properties;

namespace MDSTryout
{
    public partial class KnownFileDlg : Form
    {

        private static ManualResetEvent _manualEvent = new ManualResetEvent(false);
        private static bool _isJobSuccessful = false;
        private Hashtable _knownFiles;
        private string[] _cnodes;


        public KnownFileDlg(string [] headnodes, int selectedHnIndex, string [] cnodes, string targetDir, Hashtable knownFiles)
        {
            InitializeComponent();
            headNodeCombo.DataSource = headnodes;
            headNodeCombo.SelectedIndex = selectedHnIndex;
            targetDirTxt.Text = targetDir;
            _knownFiles = knownFiles;
            _cnodes = cnodes;
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            if (ParametersAreValid())
            {
                string knownFileInHn = GetKnownFile();
                string targetDir = targetDirTxt.Text;
                string localKnownFile = Path.Combine(targetDir, Path.GetFileName(relativePathTxt.Text)??string.Empty);
                string cmd = @"xcopy /y " + knownFileInHn + " " + targetDir;

                addBtn.Enabled = false;
                statusLabel.Text = LogMessages.WaitingTillAdded;
                SubmitCommand(headNodeCombo.SelectedItem as string, cmd);

                if (_isJobSuccessful)
                {
                    string refKey = refKeyTxt.Text.Trim();
                    _knownFiles.Add(refKey, localKnownFile);
                    using (var writer = new StreamWriter(IOUtil.KnownMatricesFileName,true))
                    {
                        writer.WriteLine("\n" + refKey + "\t" + localKnownFile + "\t" + descriptionTxt.Text);
                    }
                    MessageBox.Show(string.Format(LogMessages.ResourceAdded, refKey), LogCategories.Information,
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(LogMessages.ResourceNotAdded, LogCategories.Error,
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                DialogResult = DialogResult.OK;
            }
            
        }

        private void SubmitCommand(string headnode, string cmd)
        {
            IScheduler scheduler = new Scheduler();

            try
            {
                scheduler.Connect(headnode);

                if (UserPrivilege.Admin == scheduler.GetUserPrivilege())
                {
                    // Specify the nodes that command runs on
                    IStringCollection nodes = scheduler.CreateStringCollection();
                    foreach (var cnode in _cnodes)
                    {
                        nodes.Add(cnode);
                    }

                    // Create the command.
                    IRemoteCommand command = scheduler.CreateCommand(cmd, null, nodes);

                    // Subscribe to one or more events before starting the command.
                    command.OnCommandJobState += JobStateCallback;
                    command.OnCommandTaskState += TaskStateCallback;
                    command.OnCommandOutput += OutputCallback;
                    command.OnCommandRawOutput += RawOutputCallback;

                    // Run the command.
                    command.Start();

                    // Blocks so that the events get delivered.
                    _manualEvent.WaitOne();
                }
                else
                {
                    Console.WriteLine(LogMessages.NeedAdminPrivileges);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void RawOutputCallback(object sender, CommandRawOutputEventArg e)
        {
            // let's keep it empty
        }

        private void OutputCallback(object sender, CommandOutputEventArg e)
        {
            // let's keep it empty
        }

        private void TaskStateCallback(object sender, CommandTaskStateEventArg e)
        {
            // let's keep it empty
        }

        private void JobStateCallback(object sender, JobStateEventArg e)
        {
            if (e.NewState == JobState.Finished || e.NewState == JobState.Canceled || e.NewState == JobState.Failed)
            {
                _manualEvent.Set();
                _isJobSuccessful = (e.NewState == JobState.Finished);
            }
             
        }

        private bool ParametersAreValid()
        {
            if (string.IsNullOrEmpty(relativePathTxt.Text.Trim()))
            {
                MessageBox.Show(LogMessages.EmptyRelativePath, LogCategories.Error, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
            
            if (string.IsNullOrEmpty(refKeyTxt.Text.Trim()))
            {
                MessageBox.Show(LogMessages.EmptyRefKey, LogCategories.Error, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            } 
            
            if (_knownFiles.ContainsKey(refKeyTxt.Text.Trim()))
            {
                MessageBox.Show(LogMessages.DuplicateRefKey, LogCategories.Error, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }

            if (!File.Exists(GetKnownFile()))
            {
                MessageBox.Show(string.Format(LogMessages.NonExistingRelativePath, headNodeCombo.SelectedItem), LogCategories.Error, MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private string GetKnownFile()
        {
            return @"\\" + headNodeCombo.SelectedItem + Path.DirectorySeparatorChar +
                Path.Combine(targetDirTxt.Text.Trim(), relativePathTxt.Text.Trim()).Replace(':', '$');
        }
    }
}
