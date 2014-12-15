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
        private static string[] _manxcatBinaries = new string[] { "ManxcatMDS.exe", "HPC.Utilities.dll", "MathNet.Iridium.dll", "Salsa.CoreTPL.dll" };
        private static string[] _pairwiseBinaries = new string[] { "Salsa.SmithWatermanTPL.exe", "HPC.Utilities.dll", "Salsa.CoreTPL.dll" };
        public static bool ValidateBaseDirectory(string projectDir)
        {
            if (Directory.Exists(projectDir))
            {
                return true;
            }
            MessageBox.Show("Project directory does not exist.\n\nPlease specify an existing directory.", "Directory Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        public static bool ValidateManxcatAppDir(string manxcatAppDir, bool strict)
        {
            return ValidateAppDir(ApplicationType.Manxcat, manxcatAppDir, strict);
        }

        public static bool ValidatePairwiseAppDir(string pairwiseAppDir, bool strict)
        {
            return ValidateAppDir(ApplicationType.PairwiseClustering, pairwiseAppDir, strict);
        }

        public static bool ValidateAppDir(ApplicationType appType, string appDir, bool strict)
        {
            if (Directory.Exists(appDir))
            {
                if ((appType == ApplicationType.Manxcat && !_manxcatBinaries.All(x => File.Exists(Path.Combine(appDir, x)))) ||
                    (appType == ApplicationType.PairwiseClustering && !_pairwiseBinaries.All(x=>File.Exists(Path.Combine(appDir, x)))))
                {
                    string msg = "One or more of the following binaries could not be found in the specified directory.\n\n{0}\n\n{1}";

                    if (strict)
                    {
                        if (appType == ApplicationType.Manxcat)
                        {
                            msg = string.Format(msg, string.Join("\n", _manxcatBinaries),
                                                "Please specify a valid directory.");
                        }
                        else if (appType == ApplicationType.PairwiseClustering)
                        {
                            msg = string.Format(msg, string.Join("\n", _pairwiseBinaries),
                                                "Please specify a valid directory.");
                        }
                        MessageBox.Show(msg, "Missing Binaries", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    else
                    {
                        if (appType == ApplicationType.Manxcat)
                        {
                            msg = string.Format(msg, string.Join("\n", _manxcatBinaries), "Continue to use this folder?");
                        }
                        else if (appType == ApplicationType.PairwiseClustering)
                        {
                            msg = string.Format(msg, string.Join("\n", _pairwiseBinaries), "Continue to use this folder?");
                        }
                        return MessageBox.Show(msg, "Missing Binaries", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes;
                    }
                }
                return true;
            }
            if (strict)
            {
                string msg = "Application directory does not exist.\n\nPlease specify an existing directory with following binaries.\n\n{0}";
                if (appType == ApplicationType.Manxcat)
                {
                    msg = string.Format(msg, string.Join("\n", _manxcatBinaries));
                }
                else if (appType == ApplicationType.PairwiseClustering)
                {
                    msg = string.Format(msg, string.Join("\n", _pairwiseBinaries));
                }
                MessageBox.Show(msg, "Directory Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Application directory does not exist.\n\nPlease specify an existing directory.", "Directory Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        public static bool ValidateSpongeAppDir(string text, bool b)
        {
            // Todo (sponge) - sponge directory valid?
            return true;
        }
    }
}
