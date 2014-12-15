using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Common.pviz;

namespace PvizMovePointsToClusters
{
    class Program
    {
        static void Main(string[] args)
        {
            var plotFile =
                @"G:\Box Sync\SalsaBio\millions\phy\updated_10.2.14\MDS\WDA_SMACOF\updated_10.2.14_wdasmacof.pviz";
            var updatedPlotFile =
                @"G:\Box Sync\SalsaBio\millions\phy\updated_10.2.14\MDS\WDA_SMACOF\updated_10.2.14_wdasmacof_by_species.pviz";
            var paramsFile = @"C:\Sali\pti\sub\salsa\Saliya\c#\Utils\PvizMovePointsToClusters\params3.txt";

            var plot = PvizModel.LoadPviz(plotFile);
            var clusters = plot.Clusters;
            var updatedClusterNumbers = new int[clusters.Count];
            InitializeClusterNumbers(updatedClusterNumbers);

            var clustersToAlter = new Hashtable();
            ReadParamsFile(paramsFile, clustersToAlter, updatedClusterNumbers);

            var toAddClusters = new List<Cluster>();
            foreach (var cluster in clusters)
            {
                var cnum = cluster.Key;
                if (cnum == 5)
                {
                    Console.WriteLine("debug");
                }
                cluster.Key = updatedClusterNumbers[cnum];
                cluster.Label = cnum.ToString(CultureInfo.InvariantCulture);
                if (!clustersToAlter.Contains(cnum)) continue;
                var species = (IList<string[]>)clustersToAlter[cnum];
                for (var speciesCount = 0; speciesCount < species.Count; ++speciesCount)
                {
                    var s = species[speciesCount];
                    var c = cluster.Clone();
                    c.Key += speciesCount+1;
                    c.Label = cnum + "_" + s[0] + (s.Length > 2 ? "_" + s[1] : string.Empty);
                    toAddClusters.Add(c);
                    s[s.Length - 1] = c.Key.ToString(CultureInfo.InvariantCulture);
                }
            }
            clusters.AddRange(toAddClusters);

            var points = plot.Points;
            foreach (var point in points)
            {
                if (point.Key == 1596)
                {
                    Console.WriteLine("Debug");
                }
                var cnum = point.ClusterKey;
                point.ClusterKey = updatedClusterNumbers[cnum];
                if (!clustersToAlter.Contains(cnum)) continue;
                var label = point.Label.ToUpper();
                var species = (IList<string[]>)clustersToAlter[cnum];
                foreach (var s in species)
                {
                    var speciesName = s[0].ToUpper();
                    if (!label.Contains(speciesName) && !label.Contains(speciesName.Replace('_','.'))) continue;
                    if (s.Length > 2 && !label.Contains("("+s[1].ToUpper()+")")) continue;
                    var moveToCluster = int.Parse(s[s.Length - 1]);
                    point.ClusterKey = moveToCluster;
                    break;
                }
            }

            plot.SaveAs(updatedPlotFile);
        }

        private static void ReadParamsFile(string paramsFile, IDictionary clustersToAlter, IList<int> updatedClusterNumbers)
        {
            using (var reader = new StreamReader(paramsFile))
            {
                // This loop will fail if there are empty lines inside a group.
                // A group is cluster number followed by species list
                var sep = new [] {'\t'};
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var cnum = int.Parse(line);
                    var numSpecies = 0;
                    line = reader.ReadLine();
                    while (!string.IsNullOrWhiteSpace(line))
                    {
                        var splits = line.Split(sep,StringSplitOptions.RemoveEmptyEntries);
                        string[] array;
                        if (splits.Length > 1)
                        {
                            numSpecies += splits.Length - 1;
                            for (var i = 1; i < splits.Length; ++i)
                            {
                                array = new[] { splits[0], splits[i],string.Empty };
                                AddToHash(clustersToAlter, cnum, array);
                            }
                        }
                        else
                        {
                            ++numSpecies;
                            array = new[]{splits[0], string.Empty};
                            AddToHash(clustersToAlter, cnum, array);
                        }
                        line = reader.ReadLine();
                    }
                    UpdateClusterNumbers(updatedClusterNumbers, cnum, numSpecies);
                }
            }
        }

        private static void AddToHash(IDictionary dictionary, int key, string[] array)
        {
            if (dictionary.Contains(key))
            {
                ((IList<string[]>) dictionary[key]).Add(array);
            }
            else
            {
                dictionary[key] = new List<String[]> {array};
            }
        }

        private static void UpdateClusterNumbers(IList<int> updatedClusterNumbers, int cnum, int numSpecies)
        {
            for (var i = cnum+1; i < updatedClusterNumbers.Count; i++)
            {
                updatedClusterNumbers[i]+=numSpecies;
            }
        }

        private static void InitializeClusterNumbers(IList<int> updatedClusterNumbers)
        {
            for (var i = 0; i < updatedClusterNumbers.Count; i++)
            {
                updatedClusterNumbers[i] = i;
            }
        }
    }
}
