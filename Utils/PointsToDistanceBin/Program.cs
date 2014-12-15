using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PointsToDistanceBin
{
    class Program
    {
        static void Main(string[] args)
        {
            string pointsFile =
                @"G:\SugarSyncSharedByMe\SalsaBio\cog\100k\correlation\points\manxcat_sammon_cog_unique_blast_trans11_selected_7_wo_cog4608_points.txt";
//                @"G:\SugarSyncSharedByMe\SalsaBio\cog\100k\correlation\points\manxcat_sammon_cog_unique_nw_sqrt4d_selected_7_wo_cog4608_points.txt";
            string outFile =
                @"G:\SugarSyncSharedByMe\SalsaBio\cog\100k\correlation\points\manxcat_sammon_cog_unique_blast_trans11_selected_7_wo_cog4608_distance_double.bin";
//                @"G:\SugarSyncSharedByMe\SalsaBio\cog\100k\correlation\points\manxcat_sammon_cog_unique_nw_sqrt4d_selected_7_wo_cog4608_distance.bin";

            IList<double[]> points = new List<double[]>();
            using (StreamReader reader = new StreamReader(pointsFile))
            {
                char[] sep = new []{' ','\t'};
                while (!reader.EndOfStream)
                {
                    points.Add(reader.ReadLine().Trim().Split(sep).Select(x=>double.Parse(x)).ToArray());
                }
            }

            using (BinaryWriter writer = new BinaryWriter(File.Create(outFile)))
            {
                double xi, xj ,yi, yj, zi, zj, d;
                for (int i = 0; i < points.Count; i++)
                {
                    xi = points[i][2];
                    yi = points[i][3];
                    zi = points[i][4];
                    for (int j = 0; j < points.Count; j++)
                    {
                        xj = points[j][2];
                        yj = points[j][3];
                        zj = points[j][4];
                        d = Math.Sqrt(Math.Pow((xj-xi),2)+ Math.Pow((yj-yi),2) + Math.Pow((zi-zj),2));
                        
                        writer.Write(d);
                    }
                }
            }

            Console.WriteLine("Done.");
            Console.Read();
        }
    }
}
