using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Salsa.Core;
using System.IO;

namespace IndexExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            //Load the command line args into our helper class which allows us to name arguments
            Arguments pargs = new Arguments(args);
            // dataType {0=int16, 1=uint16, 2=double}
            pargs.Usage = "Usage: IndexExtractor.exe /indexFile=<string> /clusterFile=<string> /outDir=<string>" +
                "/clusters=<string> /resetIndices=<boolean>\n  Note. clusters is a comma separated list of cluster numbers";

            if (pargs.CheckRequired(new string[] { "indexFile", "clusterFile", "outDir", "clusters", "resetIndices" }) == false)
            {
                Console.WriteLine(pargs.Usage);
                return;
            }

            string clusterFile = pargs.GetValue<string>("clusterFile");
            string indexFile = pargs.GetValue<string>("indexFile");
            string outDir = pargs.GetValue<string>("outDir");
            string baseName = Path.GetFileNameWithoutExtension(indexFile);
            bool resetIndices = pargs.GetValue<bool>("resetIndices");

            int[] clusters = pargs.GetValue<string>("clusters").Split(',').Select<string, int>(x => int.Parse(x)).ToArray<int>();
            Dictionary<int, StreamWriter> writers = new Dictionary<int, StreamWriter>(clusters.Length);
            Dictionary<int, int> newIndices = new Dictionary<int, int>(clusters.Length);
            foreach (int cluster in clusters)
            {
                writers[cluster] = new StreamWriter(Path.Combine(outDir, baseName + "_" + cluster + ".txt"));
                newIndices[cluster] = 0;
            }

            using (StreamReader indexReader = new StreamReader(indexFile))
            {
                using (StreamReader clusterReader = new StreamReader(clusterFile))
                {
                    
                    string indexLine, clusterLine;
                    int cluster;
                    char[] sep = new[] {' ', '\t'};
                    // Both should have the same number of lines
                    while (!indexReader.EndOfStream && !clusterReader.EndOfStream)
                    {
                        indexLine = indexReader.ReadLine();
                        clusterLine = clusterReader.ReadLine();
                        cluster = int.Parse(clusterLine.Split(sep)[1]);
                        if (writers.ContainsKey(cluster))
                        {
                            if (resetIndices)
                            {
                                writers[cluster].WriteLine(newIndices[cluster] + "\t" + indexLine.Substring(indexLine.IndexOf('\t') + 1));
                                newIndices[cluster]++;
                            }
                            else
                            {
                                writers[cluster].WriteLine(indexLine); 
                            }
                        }
                    }
                }
            }
            foreach (StreamWriter w in writers.Values)
            {
                w.Flush();
                w.Close();
                w.Dispose();
            }
           
        }
    }
}
