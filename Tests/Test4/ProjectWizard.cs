using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Hpc.Scheduler;
using Microsoft.Hpc.Scheduler.Properties;
using System.IO;
using Salsa.Core.Configuration;
using Salsa.Core.Configuration.Sections;

namespace Test4
{
    public partial class ProjectWizard : UserControl
    {
        public static bool CREATE = true;
        public static bool OPEN = false;

        private int _pageNum = 0;
        private Project _project;
        private NavigatorControl _nc;
        private SubmitControl _sc; 
        private IScheduler _scheduler;

        #region Constructors
        /// <summary>
        /// Creates a new ProjectWizard based on a new
        /// Project
        /// </summary>
        /// <param name="folderPath">The path to project</param>
        /// <param name="projectName">The name of the project</param>
        public ProjectWizard(string folderPath, string projectName) 
            : this (new Project(folderPath, projectName))
        {
            _project.Save(); // saves the project after creation
        }

        /// <summary>
        /// Creates a new ProjectWizard based on a given
        /// Project.
        /// </summary>
        /// <param name="project">The exisiting project</param>
        public ProjectWizard(Project project)
        {
            InitializeComponent();
            _project = project;
            _nc = new NavigatorControl();
            _sc = new SubmitControl();
            _nc.nextBtn.Click += new System.EventHandler(this.nextBtn_Click);
            _nc.backBtn.Click += new System.EventHandler(this.backBtn_Click);
            _sc.backBtn.Click += new System.EventHandler(this.backBtn_Click);
            _sc.submitBtn.Click += new System.EventHandler(this.submitBtn_Click);
            _scheduler = new Scheduler();
            PopulateWzd();
        }

       
        #endregion

        #region Properties

        public bool Dirty
        {
            get
            {
                // todo: later handle Dirty in wzd itself (v2)
                return _project.Dirty;
            }
        }

        public string ProjectName
        {
            get
            {
                return _project.ProjectName;
            }
        }
        
        #endregion

        #region Members

        /// <summary>
        /// Populates the wizard based on the values of the _project.
        /// This is invoked when the user opens a project.
        /// </summary>
        private void PopulateWzd()
        {
            headNodeCombo.DataSource = HPCUtil.LoadHeadNodes();

            if (_project.PageNum == tabControl.TabCount - 1)
            {
                btnPanel.Controls.Add(_sc);
            }
            else
            {
                btnPanel.Controls.Add(_nc);
                if (_project.PageNum == 0)
                {
                    _nc.backBtn.Enabled = false;
                }
            }

            tabControl.SelectedIndex = _pageNum = _project.PageNum;
            appCombo.SelectedIndex = _project.AppIndex;
            headNodeCombo.SelectedIndex = _project.HeadNodeIndex;
            // todo: handle errors 
            ChangeDataSource(); // change data source based on the headnode

            nodeBox.ClearSelected();
            // todo: load nodes from collection(see PopulateProject())
            if (_project.Nodes != null)
            {
                nodeBox.BeginUpdate();
                foreach (var i in _project.Nodes)
                {
                    nodeBox.SelectedIndex = i;
                }
                nodeBox.EndUpdate();
            }

            // todo: handle file synchronization for these two (v2)
            inFileBox.Text = _project.InputFile;
            ListBoxUtil.UpdateList(binBox, FileIO.GetFiles(_project.AppDir));

            // todo: this may be removed after intergrating config editor
            configBox.Text = _project.ConfigFile;

            sectionCombo.BeginUpdate();
            sectionCombo.DataSource = _project.Manager.GetSectionNames();
            sectionCombo.EndUpdate();
            sectionCombo.SelectedIndex = _project.SectionIndex;

            _project.Dirty = false; // we just populated the wzd.
        }


        /// <summary>
        /// Populates the project object based on the values in the
        /// wizard.
        /// </summary>
        /// <returns>The populated project object</returns>
        private Project PopulateProject()
        {
            _project.AppIndex = appCombo.SelectedIndex;
            _project.HeadNodeIndex = headNodeCombo.SelectedIndex;
            _project.PageNum = tabControl.SelectedIndex;
            // todo: store a collection itself
            int[] dst = new int[nodeBox.SelectedIndices.Count];
            nodeBox.SelectedIndices.CopyTo(dst, 0);
            _project.Nodes = dst;
            _project.InputFile = inFileBox.Text;

            // todo: this may be removed after intergrating config editor
            _project.ConfigFile = configBox.Text;

            return _project;
        }

        public void Save()
        {
            PopulateProject();
            _project.Save();
        }

        private void submitBtn_Click(object sender, EventArgs e)
        {
            //todo: submit goes here
        }

        #endregion

        #region Navigation

        private void nextBtn_Click(object sender, EventArgs e)
        {
            tabControl.SelectedIndex = ++_pageNum;
            if (_pageNum == tabControl.TabCount - 1)
            {
                btnPanel.Controls.Remove(_nc);
                btnPanel.Controls.Add(_sc);
            }
            else
            {
                _nc.backBtn.Enabled = true;
            }
            _project.Dirty = _project.PageNum == tabControl.SelectedIndex ? _project.Dirty : true;
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            tabControl.SelectedIndex = --_pageNum;
            if (_pageNum == 0)
            {
                _nc.backBtn.Enabled = false;
            }
            else if (_pageNum < tabControl.TabCount - 1)
            {
                btnPanel.Controls.Remove(_sc);
                btnPanel.Controls.Add(_nc);
            }
            _project.Dirty = _project.PageNum == tabControl.SelectedIndex ? _project.Dirty : true;
        }

        private void tabControl_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPageIndex == 0 && _pageNum != 0)
            {
                _nc.backBtn.Enabled = false;
            }

            if (_pageNum < tabControl.TabCount - 1)
            {

                if (e.TabPageIndex == tabControl.TabCount - 1)
                {
                    btnPanel.Controls.Clear();
                    btnPanel.Controls.Add(_sc);
                }
            }
            else
            {
                btnPanel.Controls.Clear();
                btnPanel.Controls.Add(_nc);
            }
            _pageNum = e.TabPageIndex;
            _project.Dirty = _project.PageNum == tabControl.SelectedIndex ? _project.Dirty : true;
        }

        #endregion

        #region ApplicationSettingsTab

        private void headNodeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeDataSource();
            _project.Dirty =
                _project.HeadNodeIndex == headNodeCombo.SelectedIndex ? _project.Dirty : true;
        }

        private void ChangeDataSource()
        {
            string headNode = headNodeCombo.SelectedValue as string;
            try
            {
                _scheduler.Connect(headNode);
                ISchedulerNode[] nodes = HPCUtil.GetComputeNodes(_scheduler);
                bindingSource.DataSource = nodes;
            }
            catch (Exception e)
            {
                // may be throw it back
                MessageBox.Show(e.Message);
            }
            
        }

        /// <summary>
        /// Will be useful at the submit stage to create the actual job
        /// </summary>
        /// <returns>The total number of cores of the selected nodes</returns>
        private int GetTotalCores()
        {
            int totalCores = 0;
            foreach (ISchedulerNode node in nodeBox.SelectedItems)
            {
                totalCores += node.GetCores().Count;
            }
            return totalCores;
        }

        private void nodeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _project.Dirty = true;
        }

        private void clearLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            nodeBox.ClearSelected();
        }

        private void selectAllLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ListBoxUtil.SelectAll(nodeBox);
        }

        private void inverseLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           ListBoxUtil.SelectInverse(nodeBox);
        }

        private void appCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            _project.Dirty =
                _project.AppIndex == appCombo.SelectedIndex ? _project.Dirty : true;
        }

        #endregion

        #region FilesTab

        private void clearLink2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ListBoxUtil.ClearAll(binBox);
        }

        private void selectAllLink2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ListBoxUtil.SelectAll(binBox);
        }

        private void inverseLink2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ListBoxUtil.SelectInverse(binBox);
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Multiselect = true;
                dlg.Filter = "Binaries (*.exe,*.dll)|*.exe;*.dll" +
                    "|All Files (*.*)|*.*";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    FileIO.CopyFiles(_project.AppDir, dlg.FileNames);
                    ListBoxUtil.UpdateList(binBox, FileIO.GetFiles(_project.AppDir));
                    // make project dirty eventhough I don't record these file names
                    // in project xml at the moment
                    _project.Dirty = true;
                }
            }
        }

        private void remBtn_Click(object sender, EventArgs e)
        {
            string msg = "Selected files will be removed from the project and disk\n\n"+
                "Do you want to continue?";
            DialogResult result = MessageBox.Show(msg, "Confirm Remove", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                FileIO.DeleteFiles(binBox.SelectedItems);
                ListBoxUtil.RemoveSelected(binBox);
            }

        }

        private void changeBtn_Click(object sender, EventArgs e)
        {
            if (inFileBox.Text != string.Empty)
            {
                string msg = "Current input file will be removed from the project and disk\n\n" +
                     "Do you want to continue?";
                DialogResult result = MessageBox.Show(msg, "Confirm Remove", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    FileIO.DeleteFile(_project.InputFile);
                }
                else
                {
                    return;
                }
            }

            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "FASTA or Distance files (*.txt,*.bin)|*.txt;*.bin" +
                    "|All Files (*.*)|*.*";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    if (dlg.FileName.Contains(_project.InputDir))
                    {
                        inFileBox.Text = dlg.FileName;
                    }
                    else
                    {
                        inFileBox.Text = FileIO.Copy(_project.InputDir, dlg.FileName);
                    }
                    _project.Dirty = true;
                }
            }

        }

        #endregion

        #region RuntimeConfigurationTab
        
        //todo: this will be removed when config editor is integrated
        private void changeBtn2_Click(object sender, EventArgs e)
        {
            if (configBox.Text != string.Empty)
            {
                string msg = "Current configuration file will be removed from the project and disk\n\n" +
                     "Do you want to continue?";
                DialogResult result = MessageBox.Show(msg, "Confirm Remove", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    FileIO.DeleteFile(_project.ConfigFile);
                }
                else
                {
                    return;
                }
            }

            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Configuration Files (*.xml)|*.xml" +
                    "|All Files (*.*)|*.*";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    if (dlg.FileName.Contains(_project.ConfigDir))
                    {
                        configBox.Text = dlg.FileName;
                    }
                    else
                    {
                        configBox.Text = FileIO.Copy(_project.ConfigDir, dlg.FileName);
                        _project.UpdateManager(configBox.Text);
                        sectionCombo.BeginUpdate();
                        sectionCombo.DataSource = _project.Manager.GetSectionNames();
                        sectionCombo.EndUpdate();
                    }
                    _project.Dirty = true;
                }
            }
        }

        private void sectionCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            propGrid.SelectedObject = _project.Manager.GetSection(sectionCombo.Text);
            _project.Dirty = true;
        }

        #endregion

        void propGrid_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
        {
            _project.Dirty = true;
        }
    }
}
