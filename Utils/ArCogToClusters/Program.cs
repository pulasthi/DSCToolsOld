using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bio;
using Bio.IO.FastA;

namespace ArCogToClusters
{
    class Program
    {
        static void Main(string[] args)
        {
            const string arCogToCogFile = @"G:\SugarSyncSharedByMe\SalsaBio\arcog\sequences\ar120_to_COG_clusters.txt";
            const string uniqueArCogFile = @"G:\SugarSyncSharedByMe\SalsaBio\arcog\sequences\unique_262206_ar120.fa";
            string outFile = Path.Combine(Path.GetDirectoryName(uniqueArCogFile)??string.Empty,
                                          Path.GetFileNameWithoutExtension(uniqueArCogFile) + "_to_COG_clusters.txt");
            Hashtable arCogToCogTable = ParseClusters(arCogToCogFile);
            using (FastAParser parser = new FastAParser(uniqueArCogFile))
            {
                using (StreamWriter writer = new StreamWriter(outFile))
                {
                    IList<ISequence> uniqueArCogSeqs = parser.Parse().ToList();
                    char[] sep = new[] {'|'};
                    foreach (ISequence uniqueArCogSeq in uniqueArCogSeqs)
                    {
                        string id = uniqueArCogSeq.ID;
                        string[] splits = id.Split(sep);
                        string key = splits[1].Trim();
                        var value = arCogToCogTable[key];
                        writer.WriteLine(key +"\t" + (value??"NULL"));
                    }
                }
            }
            Console.WriteLine("Done.");
            Console.Read();
        }

        private static Hashtable ParseClusters(string arCogToCogFile)
        {
            Hashtable ht = new Hashtable();
            using (StreamReader reader = new StreamReader(arCogToCogFile))
            {
                using (StreamWriter dupWriter = new StreamWriter(Path.Combine(Path.GetDirectoryName(arCogToCogFile) ?? string.Empty, Path.GetFileNameWithoutExtension(arCogToCogFile) + "_mapping_conflicts.txt")))
                {
                    char[] sep = new[] {'\t'};
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line))
                        {
                            string[] splits = line.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                            string key = splits[0].Trim();
                            string value = splits.Length > 1 ? splits[1].Trim() : "NULL";
                            if (!ht.Contains(key))
                            {
                                ht.Add(key, value);
                            }
                            else
                            {
                                string originalValue = (string) ht[key];
                                if (!originalValue.Equals(value) && !value.Equals("NULL"))
                                {
                                    if (originalValue.Equals("NULL"))
                                    {
                                        ht[key] = value;
                                    }
                                    else if (!originalValue.Contains(value))
                                    {

                                        Console.WriteLine("Duplicate key found: " + key +
                                                          " with different value. Original: " +
                                                          ht[key] + " duplicate: " + value);
                                        dupWriter.WriteLine(key+"\t"+originalValue+"\t"+value);
                                        ht[key] = originalValue + "_" + value;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return ht;
        }
    }
}
