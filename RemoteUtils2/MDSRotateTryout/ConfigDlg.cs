using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MDSTryout
{
    public partial class ConfigDlg : Form
    {
        public enum ConfigDlgType
        {
            ManxcatAppDirectory,
            PairwiseAppDirectory,
            SpongeAppDirectory,
            BaseDirectory
        }

        #region Members
        private string _defDir, _backupDir, _currentDir;
        private ConfigDlgType _type;
        private bool _strictAppDirValidation;
        #endregion

        #region Constructors
        public ConfigDlg(string defDir, ConfigDlgType type)
        {
            _defDir = defDir;
            _backupDir = _defDir;
            _type = type;
            InitializeComponent();
        }
        #endregion

        #region Properties
        public string DefaultDirectory
        {
            get
            {
                return _defDir;
            }
        }

        public string CurrentDir
        {
            get
            {
                return _currentDir;
            }
        }
        #endregion


        #region Methods
        private void dirBrowseBtn_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    dirTxt.Text = dlg.SelectedPath;
                    setDefBtn.Enabled = !_defDir.Equals(dirTxt.Text);
                }
            }
        }

        private void setDefBtn_Click(object sender, EventArgs e)
        {
            if ((_type == ConfigDlgType.ManxcatAppDirectory && Utils.ValidateManxcatAppDir(dirTxt.Text, _strictAppDirValidation)) ||
                (_type == ConfigDlgType.PairwiseAppDirectory && Utils.ValidatePairwiseAppDir(dirTxt.Text, _strictAppDirValidation)) || 
                (_type == ConfigDlgType.SpongeAppDirectory && Utils.ValidateSpongeAppDir(dirTxt.Text, _strictAppDirValidation)) || 
                (_type == ConfigDlgType.BaseDirectory && Utils.ValidateBaseDirectory(dirTxt.Text)))
            {
                _defDir = dirTxt.Text;
                setDefBtn.Enabled = false;
            }
            
        }

        private void dirTxt_TextChanged(object sender, EventArgs e)
        {
            okBtn.Enabled = setDefBtn.Enabled = !(string.IsNullOrEmpty(dirTxt.Text) || string.IsNullOrWhiteSpace(dirTxt.Text));            
        }

        private void ConfigDlg_Load(object sender, EventArgs e)
        {
            setDefBtn.Enabled = false;
            okBtn.Enabled = false;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            _defDir = _backupDir;
        }
        

        private void okBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(dirTxt.Text) && !string.IsNullOrWhiteSpace(dirTxt.Text) && !setDefBtn.Enabled)
            {
                // Implies that the path specified in the mdsAppDirTxt is saved already as the default MDS app directory.
                // Thus no need to check for validity again even though it may be a directory with missing binaries, because
                // user has accepted that location earlier as OK.
                _currentDir = dirTxt.Text;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                if ((_type == ConfigDlgType.ManxcatAppDirectory && Utils.ValidateManxcatAppDir(dirTxt.Text,_strictAppDirValidation)) ||
                    (_type == ConfigDlgType.PairwiseAppDirectory && Utils.ValidatePairwiseAppDir(dirTxt.Text, _strictAppDirValidation)) ||
                    (_type == ConfigDlgType.SpongeAppDirectory && Utils.ValidateSpongeAppDir(dirTxt.Text, _strictAppDirValidation)) ||
                    (_type == ConfigDlgType.BaseDirectory && Utils.ValidateBaseDirectory(dirTxt.Text)))
                {
                    _currentDir = dirTxt.Text;
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
        }
        #endregion

        public bool StrictAppDirValidation { set { _strictAppDirValidation = value; } }

    }
}
