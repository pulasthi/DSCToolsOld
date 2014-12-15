using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FillPnums
{
    class Program
    {
        static void Main(string[] args)
        {
            string partialnumsfile =
                @"G:\SugarSyncSharedByMe\SalsaBio\cog\large\sequences\183362+4872_pnums(overalpping_with_95672)_to_cog_clusters_.txt";
            string outputfile =
                @"G:\SugarSyncSharedByMe\SalsaBio\cog\large\sequences\183362+4872_pnums(all)_to_cog_clusters_.txt";
            int total = (183362 + 4872);
            int consensusTotal = 4872;
            string consensudefault = @"ConsensusCOG";
            string unknownclusdefault = @"UnknownCOG";

            Hashtable ht = new Hashtable();
            using (StreamReader reader = new StreamReader(partialnumsfile))
            {
                char[] sep = new[] {' ', '\t'};
                string[] splits;
                
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line) && !line.StartsWith("#"))
                    {
                        splits = line.Split(sep);
                        ht.Add(int.Parse(splits[0]), splits[1]);
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(outputfile))
            {
                for (int i = 0; i < consensusTotal; i++)
                {
                    writer.WriteLine(i + "\t" + consensudefault);
                    
                }
                for (int i = consensusTotal; i < total; i++)
                {
                    writer.WriteLine(i + "\t" + (ht.Contains(i) ? ht[i] : unknownclusdefault));
                }
            }
        }
    }
}
