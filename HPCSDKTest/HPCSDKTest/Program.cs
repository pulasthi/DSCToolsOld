using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Hpc.Scheduler;
using Microsoft.Hpc.Scheduler.Properties;

namespace HPCSDKTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string headNode = @"airavata.cloudapp.net";
            using (IScheduler scheduler = new Scheduler())
            {
                scheduler.SetInterfaceMode(false, (IntPtr)0);
                scheduler.Connect(headNode);
                ISchedulerNode[] nodes = GetComputeNodes(scheduler);
                foreach (var schedulerNode in nodes)
                {
                    Console.WriteLine(schedulerNode.Name);
                }
            }
            Console.Read();
        }

        public static ISchedulerNode[] GetComputeNodes(IScheduler scheduler)
        {
            IFilterCollection filters = scheduler.CreateFilterCollection();
            filters.Add(FilterOperator.Equal, PropId.Node_State, NodeState.Online);

            ISortCollection sorters = scheduler.CreateSortCollection();
            sorters.Add(SortProperty.SortOrder.Ascending, PropId.Node_Name);

            List<ISchedulerNode> computeNodes = new List<ISchedulerNode>();
            foreach (ISchedulerNode node in scheduler.GetNodeList(filters, sorters))
            {
                if (node.NodeGroups.Contains("HeadNodes") == false)
                {
                    computeNodes.Add(node);
                }
            }

            return computeNodes.ToArray();
        }
    }
}
