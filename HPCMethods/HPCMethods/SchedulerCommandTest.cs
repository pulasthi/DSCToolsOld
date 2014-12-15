using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Hpc.Scheduler;
using Microsoft.Hpc.Scheduler.Properties;


namespace HPCMethods
{
    class SchedulerCommandTest
    {
        private static ManualResetEvent manualEvent = new ManualResetEvent(false);
        internal void Run(string username, SecureString pwd)
        {
            IScheduler scheduler = new Scheduler();
            IRemoteCommand command = null;
            IStringCollection nodes = null;

            try
            {
                scheduler.Connect("MADRID-HEADNODE");

                if (UserPrivilege.Admin == scheduler.GetUserPrivilege())
                {
                    // Specify that the command runs on a single node. To specify
                    // more nodes, use the nodes.Add method.
                    nodes = scheduler.CreateStringCollection();
                    nodes.Add("MADRID-101");

                    // Create the command.
                    command = scheduler.CreateCommand(@"dir c:", null, nodes);

                    // Subscribe to one or more events before starting the command.
                    command.OnCommandJobState += JobStateCallback;
                    command.OnCommandTaskState += TaskStateCallback;
                    command.OnCommandOutput += OutputCallback;
                    command.OnCommandRawOutput += RawOutputCallback;

                    // Run the command.
                    command.StartWithCredentials(username, pwd.ConvertToUnsecureString());
//                    command.Start();

                    // Blocks so that the events get delivered. One of your eventa
                    // handlers need to set this event.
                    manualEvent.WaitOne();
                }
                else
                {
                    Console.WriteLine("You must run as an administrator to create commands.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void RawOutputCallback(object sender, CommandRawOutputEventArg e)
        {
            // let's keep it empty
        }

        private void OutputCallback(object sender, CommandOutputEventArg e)
        {
            Console.WriteLine(">>" + e.Message);
        }

        private void TaskStateCallback(object sender, CommandTaskStateEventArg e)
        {
            Console.WriteLine("TaksStateCallback:-jobid" + e.JobId + "-taskid:" + e.TaskId + "-newst:" + e.NewState +
                              "-prevst:" + e.PreviousState);
        }

        private void JobStateCallback(object sender, JobStateEventArg e)
        {
            Console.WriteLine("JobStateCallback-jobid:" + e.JobId + "-newst:" + e.NewState + "-prevst:" + e.PreviousState);
            if (e.NewState == JobState.Finished || e.NewState == JobState.Canceled || e.NewState == JobState.Failed)
            {
                manualEvent.Set();
            }
        }
    }
}
