using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Salsa.Core.Configuration;

namespace Salsa.ConfigurationEditor
{
    public partial class Form1 : Form
    {
        private ConfigurationMgr _configurationManager;

        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Xml Files (*.xml)|*.xml|All Files (*.*)|*.*";
                dlg.Title = "Select the configuration file to open";

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    _configurationManager = ConfigurationMgr.LoadConfiguration(dlg.FileName,false);
                    propertyGrid1.SelectedObject = _configurationManager.ManxcatSection;

                }




            }
        }
    }
}
