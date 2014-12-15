using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using Microsoft.Hpc.Scheduler;
using Microsoft.Hpc.Scheduler.Properties;

namespace Test4
{
    class HPCUtil
    {
        public static string[] LoadHeadNodes()
        {
            DirectoryEntry domain;
            DirectorySearcher searcher;

            string path = "LDAP://ads";

            using (domain = new DirectoryEntry(path))
            {
                string domainName = domain.Name.Substring(3);  // DC=name

                // Search for all v2 head nodes in current domain.
                string[] propsToLoad = new string[] { "servicednsname" };
                string filterForAllHeadNodes = "(&(objectClass=ServiceConnectionPoint)(serviceClassName=MicrosoftComputeCluster)(keywords=*Version2*))";

                searcher = new DirectorySearcher(domain, filterForAllHeadNodes, propsToLoad);

                using (SearchResultCollection results = searcher.FindAll())
                {
                    Console.WriteLine("Found the following {0} head nodes:", results.Count);

                    string[] dnsNames = new string[results.Count];
                    for (int i = 0; i < results.Count; i++)
                    {
                        dnsNames[i] = results[i].Properties["servicednsname"][0] as string;
                    }

                    return dnsNames;
                }
            }
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
