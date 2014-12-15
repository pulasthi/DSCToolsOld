using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Salsa.Core.Configuration;
using Salsa.Core.Configuration.Sections;

namespace PetqLight
{
    public partial class ConfDlg : Form
    {
        private Section _original;
        private Section _current;
        private ApplicationType _appType;

        public ConfDlg(Section sec, string name, ApplicationType appType)
        {
            InitializeComponent();
            _original = ConfUtil.CloneSection(sec, appType);
            _current = sec;
            _appType = appType;
            pGrid.SelectedObject = sec;
            this.Text += name;
            
        }

        public Section Original { get { return _original; } }

        private void loadBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Configuration XML (*.xml)|*xml";
                if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    Section external = ConfigurationMgr.LoadConfiguration(dlg.FileName, false).GetSection(_appType.ToString());
                    ConfUtil.CopySection(external, _current, _appType, false);
                    pGrid.Refresh();
                }
            }
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            ConfUtil.CopySection(_original, _current, _appType, true);
            pGrid.Refresh();
        }
    }
}
