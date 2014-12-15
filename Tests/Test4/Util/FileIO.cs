using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace Test4
{
    public class FileIO
    {
        public static bool CreateProjectStructure(string folderPath, string projectName)
        {
            string projectPath = Path.Combine(folderPath, projectName);
            try
            {
                Directory.CreateDirectory(Path.Combine(projectPath, Project.APPS));
                Directory.CreateDirectory(Path.Combine(projectPath, Project.CONFIG));
                Directory.CreateDirectory(Path.Combine(projectPath, Project.INPUT));
                Directory.CreateDirectory(Path.Combine(projectPath, Project.OUTPUT));
            }
            catch (Exception e)
            {
                // todo: saliya - more error handling here
                return false;
            }
            return true;
        }

        public static bool CheckProjectStructure(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            DirectoryInfo[] childrenInfo = info.GetDirectories();
            if (childrenInfo.Length == 4)
            {
                string name;
                foreach (DirectoryInfo childInfo in childrenInfo)
                {
                    name = childInfo.Name;
                    if (name != Project.APPS && 
                        name != Project.CONFIG && 
                        name != Project.INPUT &&
                        name != Project.OUTPUT)
                    {
                        return false;
                    }
                }
                return true;
            }

            return false;
        }

        public static string Copy(string dir, string file)
        {
            string name = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) +1);
            name = Path.Combine(dir, name);
            File.Copy(file, name, true);
            return name;
        }

        public static void CopyFiles(string dir, string[] files)
        {
            string name;
            foreach (string file in files)
            {
                name = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) +1);
                File.Copy(file, Path.Combine(dir, name), true); 
            }
        }

        public static void DeleteFiles(ICollection files)
        {
            foreach (string file in files)
            {
                File.Delete(file);
            }
        }

        public static void DeleteFile(string file)
        {
            File.Delete(file);
        }

        public static string[] GetFiles(string dir)
        {
            return Directory.GetFiles(dir);
        }

    }
}
