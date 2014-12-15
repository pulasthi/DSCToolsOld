using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MDSTryout
{
    public class IOUtil
    {
        public static readonly string RemoteInputDirName = "input";
        public static readonly string RemoteAppsDirName = "apps";
        public static readonly string RemoteConfigFileName = "remote-config.xml";
        public static readonly string LocalConfigName = "local-config.xml";
        public static readonly string LocalOutStatusFileName = "local-out-status.txt";
        public static readonly string LocalErrStatusFileName = "local-err-status.txt";

        // todo: beta2 - remove these and use localstatus files above
        public static readonly string LocalOutFileName = "local-out.txt";
        public static readonly string LocalErrFileName = "local-err.txt";

        public static readonly string LocalPlotTxtFile = "local-plot.txt";

        public static readonly string RunInfoExt = ".infx";
        public static readonly string TextExt = ".txt";

        public static readonly string ManxcatSimplePointsFileNamePrefix = "SIMPLE";
        public static readonly string ManxcatColonPointsFileNameSuffix = "Colon";
        public static readonly string ManxcatGroupPointsFileNameSuffix = "Group";

        public static readonly string ManxcatOutFileName = "manxcat-out.txt";
        public static readonly string ManxcatErrFileName = "manxcat-err.txt";
        public static readonly string ManxcatExeName = "ManxcatMDS.exe";

        public static void CopyFiles(string fromDir, string toDir)
        {
            string[] files = Directory.GetFiles(fromDir);
            foreach (string t in files)
            {
                File.Copy(t, Path.Combine(toDir, Path.GetFileName(t)), true);
            }
        }
    }
}
