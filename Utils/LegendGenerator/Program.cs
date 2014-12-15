using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LegendGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string infoFile = args[0];
            using (StreamReader infoReader = new StreamReader(infoFile))
            {
                char [] sep = new char[]{' ','\t'};
                string ptsDir = infoReader.ReadLine().Trim().Split(sep)[1];
                string outDir = infoReader.ReadLine().Trim().Split(sep)[1];
                int regions = int.Parse(infoReader.ReadLine().Trim().Split(sep)[1]);

                string outFile;
                string ptsFile;
                Hashtable ptsTable;
                int c;
                for (int i = 0; i < regions; ++i)
                {
                    ptsFile = Directory.GetFiles(Path.Combine(ptsDir, i.ToString()), "*.txt")[0];
                    using (StreamReader ptsReader = new StreamReader(ptsFile))
                    {
                        ptsTable = new Hashtable();
                        while(!ptsReader.EndOfStream)
                        {
                            c = int.Parse(ptsReader.ReadLine().Trim().Split(sep)[4]);
                            ptsTable[c] = ptsTable.ContainsKey(c) ? ((int)ptsTable[c]) + 1 : 1;
                        }
                    }

                    outFile = Path.Combine(outDir, i + "_legend.txt");
                    using (StreamWriter outWriter = new StreamWriter(outFile))
                    {
                        outWriter.WriteLine("Cluster Number\tPoint Count");
                        foreach (DictionaryEntry dictionaryEntry in ptsTable)
                        {
                            outWriter.WriteLine(dictionaryEntry.Key + "\t" + dictionaryEntry.Value);
                        }
                    }
                }
            }
        }
    }
}
