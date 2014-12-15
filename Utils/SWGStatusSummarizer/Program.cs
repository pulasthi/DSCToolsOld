using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SWGStatusSummarizer
{
    class Program
    {
        static void Main(string[] args)
        {
            string status = @"G:\swgms_mbf2_real_out_wo_11,31.txt";
            using (StreamReader reader = new StreamReader(status))
            {
                SortedList<int, int> ranks = new SortedList<int, int>();
                int rank;
                string line;
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    if (line.Contains("Finished Blocks Compute"))
                    {
                        rank = Int32.Parse(line.Split(' ')[1]);
                        ranks.Add(rank, rank);
                    }
                }

                Console.WriteLine("Total Ranks: " + ranks.Count);
                for (int i = 0; i < 720; i++ )
                {
                    if (!ranks.ContainsKey(i))
                    {
                        Console.WriteLine("Unfinished Rank: " + i);
                    }
                }
                    Console.Read();
            }
        }
    }
}
