using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common.pviz;

namespace PvizMovePointsToAbundantOTUClusters
{
    class Program
    {
        static void Main(string[] args)
        {
            /*var plotFile =
                @"G:\Box Sync\SalsaBio\millions\phy\updated_10.2.14\MDS\WDA_SMACOF\updated_10.2.14_wdasmacof.pviz";
            var updatedPlotFile =
                @"G:\Box Sync\SalsaBio\millions\phy\updated_10.2.14\MDS\WDA_SMACOF\updated_10.2.14_wdasmacof_by_abundant_otu.pviz";
            var paramsFile = @"C:\Sali\pti\sub\salsa\Saliya\c#\Utils\PvizMovePointsToAbundantOTUClusters\Params.txt";*/

            var plotFile =
                @"E:\Sali\InCloud\IUBox\Box Sync\SalsaBio\millions\phy\updated_10.2.14\MDS\WDA_SMACOF\updated_10.2.14_wdasmacof.pviz";
            var paramsFile = @"E:\Sali\git\github\DSCToolsOld\Utils\PvizMovePointsToAbundantOTUClusters\MDS_Cluster_AbundantOTU_Cluster_Map_97Perc_RevisedCluster_Pviz_File.txt";
            var title = @"updated_10.2.14_wdasmacof_by_abundant_otu_new";
            var updatedPlotFile =
                @"E:\Sali\InCloud\IUBox\Box Sync\SalsaBio\millions\phy\updated_10.2.14\MDS\WDA_SMACOF\" + title + ".pviz";
            

            var plot = PvizModel.LoadPviz(plotFile);
            plot.Plot.Title = title;
            var clusters = plot.Clusters;
            var updatedClusterNumbers = new int[clusters.Count];
            InitializeClusterNumbers(updatedClusterNumbers);
            var pointsToAlter = new Hashtable();
            var otuClusters = new Hashtable();
            
            ReadParamsFile(paramsFile, updatedClusterNumbers, pointsToAlter, otuClusters);
            UpdateClusters(clusters, updatedClusterNumbers, otuClusters);

            var points = plot.Points;
            foreach (var point in points)
            {
                point.ClusterKey = updatedClusterNumbers[point.ClusterKey];
                var label = point.Label;
                if (!pointsToAlter.Contains(label)) continue;
                var whereToMove = (int[]) pointsToAlter[label];
                var updatedCnum = updatedClusterNumbers[whereToMove[0]] + whereToMove[1];
                point.ClusterKey = updatedCnum;
            }

            plot.SaveAs(updatedPlotFile);
        }

        private static void UpdateClusters(List<Cluster> clusters, int[] updatedClusterNumbers, Hashtable otuClusters)
        {
            var toAddClusters = new List<Cluster>();
            foreach (var cluster in clusters)
            {
                var cnum = cluster.Key;
                cluster.Key = updatedClusterNumbers[cnum];
                if (!otuClusters.Contains(cnum)) continue;
                var otuCnums = (List<int>) otuClusters[cnum];
                for (var i = 0; i < otuCnums.Count; ++i)
                {
                    var otuCnum = otuCnums[i];
                    var clone = cluster.Clone();
                    clone.Key += (i + 1);
                    clone.Label = clone.Label + "_OTU_cluster_" + otuCnum;
                    toAddClusters.Add(clone);
                }
            }
            clusters.AddRange(toAddClusters);
        }

        private static void ReadParamsFile(string paramsFile, int[] updatedClusterNumbers, Hashtable pointsToAlter, Hashtable otuClusters)
        {
            using (var reader = new StreamReader(paramsFile))
            {
                var sep = new [] {'\t'};
                int cnum = -1, otucnum = -1, numOTUs = -1;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line)) continue;
                    var splits = line.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                    if (splits.Length != 3) continue;
                    var x = int.Parse(splits[0]);
                    var y = int.Parse(splits[1]);
                    if (cnum != x)
                    {
                        if (cnum != -1 && numOTUs != -1)
                        {
                            // not the initial case
                            UpdateClusterNumbers(updatedClusterNumbers, cnum, numOTUs);
                        }
                        cnum = x;
                        otucnum = y;
                        numOTUs = 1;
                        otuClusters.Add(cnum, new List<int>{otucnum});
                    }
                    else if (otucnum != y)
                    {
                        otucnum = y;
                        ++numOTUs;
                        ((List<int>)otuClusters[cnum]).Add(otucnum);
                    }
                    pointsToAlter.Add(splits[2], new []{cnum, numOTUs});
                }
                UpdateClusterNumbers(updatedClusterNumbers, cnum, numOTUs);
            }
        }

        private static void InitializeClusterNumbers(IList<int> updatedClusterNumbers)
        {
            for (var i = 0; i < updatedClusterNumbers.Count; i++)
            {
                updatedClusterNumbers[i] = i;
            }
        }

        private static void UpdateClusterNumbers(IList<int> updatedClusterNumbers, int cnum, int numOtuClusters)
        {
            for (var i = cnum + 1; i < updatedClusterNumbers.Count; i++)
            {
                updatedClusterNumbers[i] += numOtuClusters;
            }
        }
    }
}
