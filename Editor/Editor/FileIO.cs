using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace Editor
{
    public class FileIO
    {
        public const string APPS = "apps";
        public const string CONFIG = "config";
        public const string INPUT = "input";
        public const string OUTPUT = "output";

        public static bool CreateProjectStructure(string folderPath, string projectName)
        {           
            string projectPath = Path.Combine(folderPath, projectName);
            try
            {
                Directory.CreateDirectory(Path.Combine(projectPath, APPS));
                Directory.CreateDirectory(Path.Combine(projectPath, CONFIG));
                Directory.CreateDirectory(Path.Combine(projectPath, INPUT));
                Directory.CreateDirectory(Path.Combine(projectPath, OUTPUT));
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
                    if (name != APPS && name != CONFIG && name != INPUT &&
                        name != OUTPUT)
                    {
                        return false;
                    }
                }
                return true;
            }

            return false;
        }
    }
}
