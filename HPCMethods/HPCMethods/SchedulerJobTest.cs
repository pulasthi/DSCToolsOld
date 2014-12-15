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
    class SchedulerJobTest
    {
        private static ManualResetEvent manualEvent = new ManualResetEvent(false);
        internal void Run(string username, SecureString pwd)
        {
            IScheduler scheduler = new Scheduler();
            ISchedulerJob job = null;
            ISchedulerTask task = null;

            try
            {
                scheduler.Connect(Constants.Madrid);
                // Create a job and add a task to the job.
                job = scheduler.CreateJob();
                task = job.CreateTask();
                task.CommandLine = "dir c:";
                task.StdOutFilePath = @"\\beaver\c$\temp.txt";
                job.AddTask(task);

                // Specify the events that you want to receive.
                job.OnJobState += JobStateCallback;
                job.OnTaskState += TaskStateCallback;
                
                
                

                // Start the job.
                scheduler.SubmitJob(job, username, pwd.ConvertToUnsecureString());

                // Blocks so the events get delivered. One of your event
                // handlers need to set this event.
                manualEvent.WaitOne();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void TaskStateCallback(object sender, TaskStateEventArg e)
        {
            Console.WriteLine(">>TS: " + e.NewState);
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
