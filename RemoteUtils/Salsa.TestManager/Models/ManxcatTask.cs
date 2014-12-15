using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Hpc.Scheduler;

namespace Salsa.TestManager.Models
{
    public class ManxcatTask : BaseTask
    {
        public string CreateCommandLine(ManxcatJob job)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("mpiexec");
            sb.AppendFormat(" -hosts {0}", RequiredNodes.Count.ToString());

            foreach (ISchedulerNode node in RequiredNodes)
            {
                sb.AppendFormat(" {0} {1}", node.Name, ProcessesPerNode.ToString());
            }

            sb.AppendFormat(" {0}", job.ExecutableFilePath);
            sb.AppendFormat(" {0}", job.ControlFilePath);
            sb.AppendFormat(" {0}", RequiredNodes.Count);
            sb.AppendFormat(" {0}", ThreadsPerProcess);

            return sb.ToString();
        }
    }
}
