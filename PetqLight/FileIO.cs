using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using Salsa.Core.Bio.Algorithms;
using Salsa.Core.Configuration;
using Salsa.Core.Configuration.Sections;


namespace PetqLight
{
    class FileIO
    {
        public static bool CreateProjectStructure(string folder, string name)
        {
            string projectPath = Path.Combine(folder, name);
            Directory.CreateDirectory(Path.Combine(projectPath, Constants.AppsDirName));
            Directory.CreateDirectory(Path.Combine(projectPath, Constants.ConfigDirName));
            Directory.CreateDirectory(Path.Combine(projectPath, Constants.InputDirName));
            Directory.CreateDirectory(Path.Combine(projectPath, Constants.OutputDirName));

            return true;
        }

        public static string Copy(string dir, string file)
        {
            string name = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            name = Path.Combine(dir, name);
            File.Copy(file, name, true);
            return name;
        }

        public static void CopyBinaries(string folder, ApplicationType saApp, ApplicationType pwcApp, ApplicationType mdsApp)
        {
            string path;
            // Copy files other than executables
            foreach (string file in Directory.GetFiles(Constants.BinariesDirName).Where<string>(x => !Path.GetExtension(x).Equals(".exe")))
            {
                path = Path.Combine(folder, Constants.AppsDirName, Path.GetFileName(file));
                if (!File.Exists(path))
                {
                    File.Copy(file, path);
                }
            }

            string executableName;
            if (saApp != ApplicationType.UNDEFINED)
            {
                switch (saApp)
                {
                    case ApplicationType.NW:
                        executableName = Constants.NeedlemanWunschExe;
                        break;
                    case ApplicationType.SWG:
                        executableName = Constants.SmithWatermanExe;
                        break;
                    case ApplicationType.SWMS:
                        executableName = Constants.SmithWatermanMSExe;
                        break;
                    default: // This case should be avoided by a front end tool always
                        executableName = Constants.NeedlemanWunschExe;
                        break;
                }
                path = Path.Combine(folder, Constants.AppsDirName, executableName);
                if (!File.Exists(path))
                {
                    File.Copy(Path.Combine(Path.GetFullPath(Constants.BinariesDirName), executableName), path);
                }
            }

            if (pwcApp != ApplicationType.UNDEFINED)
            {
                path = Path.Combine(folder, Constants.AppsDirName, Constants.PairwiseClusteringExe);
                if (!File.Exists(path))
                {
                    File.Copy(Path.Combine(Path.GetFullPath(Constants.BinariesDirName), Constants.PairwiseClusteringExe), path);
                }
 
            }

            if (mdsApp != ApplicationType.UNDEFINED)
            {
                path = Path.Combine(folder, Constants.AppsDirName, Constants.MultiDimensionalScalingExe);
                if (!File.Exists(path))
                {
                    File.Copy(Path.Combine(Path.GetFullPath(Constants.BinariesDirName), Constants.MultiDimensionalScalingExe), path);
                }
            }
        }

        public static int GetSequenceCount(string file)
        {
            int count = 0;
            using (StreamReader reader = new StreamReader(file))
            {
                string str;
                while (reader.EndOfStream == false)
                {
                    str = reader.ReadLine();
                    if (str.StartsWith(">"))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public static void SaveAs(string str, string file)
        {
            using (StreamWriter writer = new StreamWriter(file))
            {
                writer.WriteLine(str);
            }
        }

        public static void CopyProjectToHeadNode(string headNodeName, string targetDir, string projectDir, string projectName)
        {
            
        }
    }
}
