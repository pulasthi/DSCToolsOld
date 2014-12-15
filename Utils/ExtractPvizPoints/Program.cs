using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.pviz;

namespace ExtractPvizPoints
{
    class Program
    {
        static void Main(string[] args)
        {
            var pvizFile = args[0];
            var dumpAll = args.Length > 1 && bool.Parse(args[1]);
            var pviz = PvizModel.LoadPviz(pvizFile);
            var points = pviz.Points;
            var ptable = new Hashtable(points.Count);
            foreach (var p in points.Where(p => p != null))
            {
                if (ptable.ContainsKey(p.Key))
                {
                    // can't happen for simple pviz files
                    throw new Exception("duplicate points");
                }
                ptable.Add(p.Key, p);
            }

            var clusters = pviz.Clusters;
            var ctable = new Hashtable(clusters.Count);
            if (dumpAll)
            {
                foreach (var c in clusters.Where(c => !ctable.ContainsKey(c.Key)))
                {
                    ctable.Add(c.Key,c);
                }
            }

            var pointsText = Path.Combine(Path.GetDirectoryName(pvizFile) ?? string.Empty,
                                             Path.GetFileNameWithoutExtension(pvizFile) + "_points_w_orig_pnum.txt");
            var pointsTextWithLabels = Path.Combine(Path.GetDirectoryName(pvizFile) ?? string.Empty,
                                             Path.GetFileNameWithoutExtension(pvizFile) + "_points_w_orig_pnum_and_labels.txt");
            var writerWithLabels = dumpAll ? new StreamWriter(pointsTextWithLabels) : null;
            using (var writer = new StreamWriter(pointsText))
            {
                for (var i = 0; i < points.Count; i++)
                {
                    if (ptable.ContainsKey(i))
                    {
                        var p = (Point) ptable[i];
                        var l = p.Location;
                        writer.WriteLine(p.Key + "\t" + l.X + "\t" + l.Y + "\t" + l.Z + "\t" + p.ClusterKey);
                        if (dumpAll)
                        {
                            var c = (Cluster) ctable[p.ClusterKey];
                            writerWithLabels.WriteLine(p.Key + "\t" + l.X + "\t" + l.Y + "\t" + l.Z + "\t" + p.ClusterKey + "\t" + p.Label + "\t" + c.Label + "\t" + c.Color);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error!! Point " + i  + " was not present");
                    }
                }
            }
            if (dumpAll)
            {
                writerWithLabels.Close();
                writerWithLabels.Dispose();
            }
            Console.WriteLine("Done");
            Console.Read();
        }
    }
}
