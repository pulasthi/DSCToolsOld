using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Hpc.Scheduler;
using Microsoft.Hpc.Scheduler.Properties;

namespace HPCSDKTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (IScheduler scheduler = new Scheduler())
            {
                scheduler.Connect(@"madrid-headnode.ads.iu.edu");
                Console.WriteLine("Connected with headnode");

                ISchedulerJob job = scheduler.CreateJob();
                job.Name = "Dummy dir test";
                job.AutoCalculateMax = job.AutoCalculateMin = false;
                job.UnitType = JobUnitType.Core;
                job.MinimumNumberOfCores = 16;
                job.MaximumNumberOfCores = 16;
                job.IsExclusive = true;
                job.RunUntilCanceled = false;
                job.RequestedNodes.Add("madrid-101");

                ISchedulerTask task = job.CreateTask();
                task.Name = "dir task";
                task.MinimumNumberOfCores = task.MinimumNumberOfCores = 1;
                task.CommandLine =
                    @"dir \\madrid-headnode\c$\salsa\saliya\remotetest > \\madrid-101\c$\salsa\saliya\remotetest\dir.txt";
                job.AddTask(task);
                scheduler.SubmitJob(job, null, null);
                Console.WriteLine("Submission done.");
                
                Console.Read();

            }
        }
    }
}
