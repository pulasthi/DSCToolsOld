using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDSTryout
{
    public class LogMessages
    {
        public static readonly string Seperator = new string('-',75);
        public static readonly string nl = Environment.NewLine;
        public static readonly string Done = @"Done." + nl + nl;
        public static readonly string RunAborted = @"Run aborted." + nl + nl;
        public static readonly string WorkInProgress = @"Another run is already in progress";
        public static readonly string MoreLinesThanOther = @"{0} file has more points than {1} file";
        public static readonly string NoClusterFile = @"Cluster file does not exist";



        public static readonly string PreparingRun = @"Preparing run ..." + nl;
        public static readonly string DirValidated = @"  -- Validated local directories." + nl;
        public static readonly string MetaInfoPopulated = @"  -- Populated meta information." + nl;
        public static readonly string LocalRunDirCreated = @"  -- Local run directory created." + nl;
        public static readonly string ConfigSaved = @"  -- Configuration saved to disk." + nl;
        public static readonly string MetaInfoSaved = @"  -- Meta information saved to disk." + nl;
        
        public static readonly string MigratingToHn = @"Migrating to headnode ..." + nl;

        public static readonly string CreatingRemoteRun = @"Creating remote run ..." + nl;
        public static readonly string SetupTasksAdded = @"  -- Setup tasks added." + nl;
        public static readonly string ManxcatTaskAdded = @"  -- Manxcat task added." + nl;

        public static readonly string SubmittingRemoteRun = @"Submitting remote run ..." + nl;

        // OnJobState message
        public static readonly string JobStatus = @"Job {0} is {1}" + nl;
        // OnTaskState messages
        public static readonly string SetupTaskState = @"Setup task {0} is {1}" + nl;
        public static readonly string ManxcatTaskState = @"Manxcat task is {0}" + nl;

        public static string JobNotFoundInCluster = @"Job was not in cluster. Probably not submitted.";
    }

    public class LogCategories
    {
        public static readonly string Warning = @"Warning";
        public static readonly string Error = @"Error";
        public static readonly string Information = @"Information";
    }
}
