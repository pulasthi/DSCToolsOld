using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanPageRankIndices
{
    class Program
    {
        static void Main(string[] args)
        {
            string inFile = @"C:\Users\sekanaya\Downloads\en0000-01and02.am";
            string outFile = @"C:\Users\sekanaya\Downloads\en0000-01and02_reset_idx.am";
            string mappingFile = @"C:\Users\sekanaya\Downloads\en0000-01and02_mapping_idx.am";
            using (var reader = new StreamReader(inFile))
            {
                using (StreamWriter writer = new StreamWriter(outFile),  mappingWriter = new StreamWriter(mappingFile))
                {
                    writer.WriteLine(reader.ReadLine());
                    var sep = new[] {' '};
                    var ht = new Hashtable();
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var splits = line.Split(sep, StringSplitOptions.RemoveEmptyEntries);

                        for (int i = 0; i < splits.Length; ++i)
                        {
                            int url = int.Parse(splits[i]);
                            if (!ht.Contains(url))
                            {
                                ht.Add(url, ht.Count);
                            }
                            writer.Write(ht[url] + " ");
                        }
                        writer.WriteLine();
                    }

                    foreach (var key in ht.Keys)
                    {
                        mappingWriter.WriteLine(key +"\t" + ht[key]);
                    }
                }
            }
        }
    }
}
