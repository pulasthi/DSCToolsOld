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
        public static readonly string NoCoordinatesFile = @"Coordinates file does not exist";



        public static readonly string PreparingRun = @"Preparing run ..." + nl;
        public static readonly string DirValidated = @"  -- Validated local directories." + nl;
        public static readonly string MetaInfoPopulated = @"  -- Populated meta information." + nl;
        public static readonly string LocalRunDirCreated = @"  -- Local run directory created." + nl;
        public static readonly string ConfigSaved = @"  -- Configuration saved to disk." + nl;
        public static readonly string MetaInfoSaved = @"  -- Meta information saved to disk." + nl;
        
        public static readonly string MigratingToHn = @"Migrating to headnode ..." + nl;

        public static readonly string CreatingRemoteRun = @"Creating remote run ..." + nl;
        public static readonly string SetupTasksAdded = @"  -- Setup tasks added." + nl;
        public static readonly string ApplicationTaskAdded = @"  -- {0} task added." + nl;

        public static readonly string SubmittingRemoteRun = @"Submitting remote run ..." + nl;

        // OnJobState message
        public static readonly string JobStatus = @"Job {0} is {1}" + nl;
        // OnTaskState messages
        public static readonly string SetupTaskState = @"Setup task {0} is {1}" + nl;
        public static readonly string ApplicationTaskState = @"{0} task is {1}" + nl;

        public static string JobNotFoundInCluster = @"Job was not in cluster. Probably not submitted.";

        /* Messages on KnowFileDlg */
        public static string EmptyRelativePath = @"Relative path is empty.";
        public static string NonExistingRelativePath = @"Relative path does not exist in cluster {0}.";
        public static string EmptyRefKey = @"Reference key is empty.";
        public static string DuplicateRefKey = @"Reference key already exists. Please select a different one.";
        public static string WaitingTillAdded = @"Waiting for resource to be added to cluster...";
        public static string NeedAdminPrivileges = @"You must run as an administrator to create commands.";
        public static string ResourceAdded = "Resource successfully added to cluster and is avaialble as ${0}";
        public static string ResourceNotAdded = "Resource could not be added to cluster. Please try again.";


    }

    public class LogCategories
    {
        public static readonly string Warning = @"Warning";
        public static readonly string Error = @"Error";
        public static readonly string Information = @"Information";
    }
}
