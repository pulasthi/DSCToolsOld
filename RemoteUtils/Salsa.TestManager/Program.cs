using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Salsa.TestManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            dlgPickHeadNode dlg = new dlgPickHeadNode();
            Application.Run(dlg);

            if (dlg.DialogResult == DialogResult.OK)
            {
                Application.Run(new frmMain(dlg.JobType, dlg.HeadNode));
            }
        }
    }
}
