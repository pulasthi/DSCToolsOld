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
namespace GUI_test_2
{
    public partial class Form1 : Form
    {
        private ConfigurationMgr _mgr = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _mgr = ConfigurationMgr.LoadConfiguration(@"C:\Saliya-PTI\salsa-svn\trunk\Saliya\GUI-test-2\GUI-test-2\conf.xml");
            propertyGrid1.SelectedObject = _mgr;
            
            //propertyGrid1.SelectedObject = this.propertyGrid1;
            //string[] arr = new string []{ "hello", "how", "are", "you" };
            //propertyGrid1.SelectedObject = arr;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //_mgr.Sections.Remove(ConfigurationMgr.MANXCAT_SECTION);
        }
    }
}
