using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace MDSTryout
{
    class Utils
    {
        private static string[] _binaries = new string[] { "ManxcatMDS.exe", "HPC.Utilities.dll", "MathNet.Iridium.dll", "Salsa.CoreTPL.dll" };
        public static bool ValidateBaseDirectory(string projectDir)
        {
            if (Directory.Exists(projectDir))
            {
                return true;
            }
            MessageBox.Show("Project directory does not exist.\n\nPlease specify an existing directory.", "Directory Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        public static bool ValidateMDSAppDir(string mdsAppDir, bool strict)
        {
            
            if (Directory.Exists(mdsAppDir))
            {
                if (!_binaries.All<string>(x => File.Exists(Path.Combine(mdsAppDir, x))))
                {
                    string msg = "One or more of the following binaries could not be found in the specified directory.\n\n{0}\n\n{1}";

                    if (strict)
                    {
                        msg = string.Format(msg, string.Join("\n", _binaries), "Please specify a valid directory.");
                        MessageBox.Show(msg, "Missing Binaries", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    else
                    {
                        msg = string.Format(msg, string.Join("\n", _binaries), "Continue to use this folder?");
                        return MessageBox.Show(msg, "Missing Binaries", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes;
                    }
                }
                return true;
            }
            if (strict)
            {
                string msg = "MDS application directory does not exist.\n\nPlease specify an existing directory with following binaries.\n\n{0}";
                msg = string.Format(msg, string.Join("\n", _binaries));
                MessageBox.Show(msg, "Directory Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("MDS application directory does not exist.\n\nPlease specify an existing directory.", "Directory Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }
    }
}
