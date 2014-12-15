using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common.pviz;

namespace RenamePvizClusters
{
    class Program
    {
        static void Main(string[] args)
        {
            var pvizFile = args[0];
            var subsFile = args[1];
            var pviz = PvizModel.LoadPviz(pvizFile);
            var subs = new Hashtable();
            using (var reader = new StreamReader(subsFile))
            {
                var sep = new [] {' ', '\t'};
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line)) continue;
                    var splits = line.Split(sep);
                    if (!subs.Contains(splits[0]))
                    {
                        subs.Add(splits[0], splits[1]);
                    }
                }
            }
            var clusters = pviz.Clusters;
            foreach (var cluster in clusters)
            {
                if (cluster.Label != null && subs.Contains(cluster.Label))
                {
                    cluster.Label = subs[cluster.Label] as string;
                }
            }
            RemoveGlomus(pviz);
            pviz.SaveAs(args[0]);
            Console.WriteLine("Done.");
        }

        public static void RemoveGlomus(PvizModel pviz)
        {
            var clusters = pviz.Clusters;
            int replaceKey = -1;
            int removeKey = -1;
            Cluster removeCluster = null;
            foreach (var cluster in clusters)
            {
                if (cluster.Label.Equals("Glomus"))
                {
                    replaceKey = cluster.Key;
                }

                if (cluster.Label.Equals("glomus"))
                {
                    removeKey = cluster.Key;
                    removeCluster = cluster;
                }
            }
            clusters.Remove(removeCluster);

            var points = pviz.Points;
            foreach (var point in points)
            {
                if (point.ClusterKey == removeKey)
                {
                    point.ClusterKey = replaceKey;
                }
            }
        }
    }
}
