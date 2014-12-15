using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Hpc.Scheduler;
using Microsoft.Hpc.Scheduler.Properties;

namespace MonitorJob
{
    internal class Program
    {
        private static ManualResetEvent jobFinishEvt = new ManualResetEvent(false);

        private static void Main(string[] args)
        {
            Scheduler scheduler = new Scheduler();


            scheduler.Connect("madrid-headnode.ads.iu.edu");

            ISchedulerJob job = scheduler.CreateJob();

            job.MinimumNumberOfNodes = 1;
            job.MaximumNumberOfNodes = 1;


            ISchedulerTask task = job.CreateTask();

            task.CommandLine = "ComputeTask 5";
            task.WorkDirectory = @"c:\tests";

            job.AddTask(task);

            job.OnJobState += new EventHandler<JobStateEventArg>(job_OnJobState);

            scheduler.SubmitJob(job, null, null);

            jobFinishEvt.WaitOne();
        }

        private static void job_OnJobState(object sender, JobStateEventArg e)
        {
            Console.WriteLine("Job <{0}> has changed from state <{1}> to <{2}>",
                              e.JobId, e.PreviousState.ToString(), e.NewState.ToString());
            if (e.NewState.Equals(JobState.Finished))
            {
                Console.WriteLine("Job <{0}> has finished", e.JobId);
                jobFinishEvt.Set();
            }
        }
    }
}
