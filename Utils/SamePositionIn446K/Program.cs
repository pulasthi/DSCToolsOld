using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace SamePositionIn446K
{
    class Program
    {
        static void Main(string[] args)
        {
            string initialFile =
                @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\haixu_release\plots\initial\haixu_446041_mega_regions_zeroidx.txt";
            string outFile =
                @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\haixu_release\plots\initial\haixu_446041_mega_regions_position_duplicates.txt";

            using (SimplePointsReader reader = new SimplePointsReader(initialFile))
            {
                using (StreamWriter writer = new StreamWriter(outFile))
                {
                    writer.WriteLine("region\tx\ty\tz\tcount\tpnums");
                    Dictionary<int,Dictionary<string, IList<int>>> regionDic = new Dictionary<int, Dictionary<string, IList<int>>>();
                    while (!reader.EndOfStream)
                    {
                        Point p = reader.ReadPoint();
                        if (!regionDic.ContainsKey(p.Cluster))
                        {
                            regionDic.Add(p.Cluster, new Dictionary<string, IList<int>>());
                        }

                        Dictionary<string, IList<int>> positionDic = regionDic[p.Cluster];
                        string position = p.PositionString();
                        if (!positionDic.ContainsKey(position))
                        {
                            positionDic.Add(position, new List<int>());
                        }

                        IList<int> pnums = positionDic[position];
                        pnums.Add(p.Index);
                    }

                    int numRegions = regionDic.Count;

                    for (int region = 0; region < numRegions; ++region )
                    {
                        Dictionary<string, IList<int>> positionDic = regionDic[region];
                        foreach (KeyValuePair<string, IList<int>> positionKv in positionDic)
                        {
                            string position = positionKv.Key;
                            IList<int> pnums = positionKv.Value;
                            if (pnums.Count > 1)
                            {
                                string pnumsString = string.Join(",", pnums.ToArray());
                                writer.WriteLine(region + "\t" + position + "\t" + pnums.Count + "\t" + pnumsString);
                            }
                        }
                    }

                    Console.WriteLine("Done.");
                    Console.Read();
                }
            }
        }
    }
}
