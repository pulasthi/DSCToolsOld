using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Hpc.Scheduler;

namespace Salsa.TestManager.Models
{
    public class SmithwatermanTask : BaseTask
    {
        public string CreateCommandLine(SmithwatermanJob job)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("mpiexec");
            sb.AppendFormat(" -hosts {0}", RequiredNodes.Count.ToString());

            foreach (ISchedulerNode node in RequiredNodes)
            {
                sb.AppendFormat(" {0} {1}", node.Name, ProcessesPerNode.ToString());
            }

            sb.AppendFormat(" {0}", job.ExecutableFilePath);
            sb.AppendFormat(@" /inputFile={0}", job.FastaFilePath);
            sb.AppendFormat(@" /outputFolder={0}", job.OutputFolderPath);
            sb.AppendFormat(@" /timingFile={0}", job.TimingFilePath);
            sb.AppendFormat(@" /nodeCount={0}", RequiredNodes.Count);
            sb.AppendFormat(@" /writeFullMatrix={0}", "true");

            if (job.UseRange == true)
            {
                sb.AppendFormat(" /startIndex={0}", job.StartIndex);
                sb.AppendFormat(" /endIndex={0}", job.EndIndex);
            }

            sb.AppendFormat(" /gapOpen={0}", job.GapOpenPenalty);
            sb.AppendFormat(" /gapExtension={0}", job.GapExtensionPenalty);
            sb.AppendFormat(" /jobid=%CCP_JOBID% /taskid=%CCP_TASKID%");
            return sb.ToString();
        }
    }
}
