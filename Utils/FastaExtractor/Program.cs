using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bio;
using Bio.IO.FastA;
using Salsa.Core;

namespace FastaExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            //Load the command line args into our helper class which allows us to name arguments
            Arguments pargs = new Arguments(args);
            // dataType {0=int16, 1=uint16, 2=double}
            pargs.Usage = "Usage: FastaExtractor.exe /fastaFile=<string> /clusterFile=<string> /outDir=<string>" +
                "/clusters=<string> \n  Note. clusters is a comma separated list of cluster numbers";

            if (pargs.CheckRequired(new string[] { "fastaFile", "clusterFile", "outDir", "clusters" }) == false)
            {
                Console.WriteLine(pargs.Usage);
                return;
            }

            string clusterFile = pargs.GetValue<string>("clusterFile");
            string fastaFile = pargs.GetValue<string>("fastaFile");
            string outDir = pargs.GetValue<string>("outDir");
            string baseName = Path.GetFileNameWithoutExtension(fastaFile);

            int[] clusters = pargs.GetValue<string>("clusters").Split(',').Select<string, int>(x => int.Parse(x)).ToArray<int>();
            Dictionary<int, FastAFormatter> formatters = new Dictionary<int, FastAFormatter>(clusters.Length);
//            Dictionary<int, FastaFormatter> formatters = new Dictionary<int, FastaFormatter>(clusters.Length);
            foreach (int cluster in clusters)
            {
                formatters[cluster] = new FastAFormatter(Path.Combine(outDir, baseName + "_" + cluster + ".fa"));
//                formatters[cluster] = new FastaFormatter();
            }

            IList<ISequence> seqs;
            using (FastAParser parser = new FastAParser(fastaFile))
            {
                seqs = parser.Parse().ToList();
            }
            using (StreamReader clusterReader = new StreamReader(clusterFile))
            {
                string indexLine, clusterLine;
                int cluster;
                int count = 0;
                // Both should have the same number of lines
                char[] sep = {' ', '\t'};
                while (count < seqs.Count && !clusterReader.EndOfStream)
                {
                    clusterLine = clusterReader.ReadLine();
                    string[] splits = clusterLine.Split(sep);
                    // if splits.length == 2 then it's a cluster file otherwise it has to be a points file
                    cluster = (splits.Length == 2) ? int.Parse(splits[1]) : int.Parse(splits[4]);
                    if (formatters.ContainsKey(cluster))
                    {
                        formatters[cluster].Write(seqs[int.Parse(splits[0])]);
                    }
                    count++;
                }
            }
            foreach (FastAFormatter formatter in formatters.Values)
            {
                formatter.Flush();
                formatter.Close();
                formatter.Dispose();
            }
        }
    }
}
