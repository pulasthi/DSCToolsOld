using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Salsa.Core;

namespace AllInOneClusters
{
    class Program
    {
        static void Main(string[] args)
        {
            //Load the command line args into our helper class which allows us to name arguments
            var pargs = new Arguments(args);
            // dataType {0=int16, 1=uint16, 2=double}
            pargs.Usage = "Usage: AllInOneClusters.exe /indexFile=<string> /pointsFile=<string> /clusterFile=<string>" +
                          "/reclusDir=<string> /outDir=<string> /consensus=<int>" +
                          "Note. Consensus indicates the number of points starting from the first point," +
                          "that need to be extracted as consensus points for each such cluster.";

            if (pargs.CheckRequired(new string[] { "indexFile", "pointsFile", "clusterFile", "reclusDir", "outDir", "consensus"}) == false)
            {
                Console.WriteLine(pargs.Usage);
                Console.Read();
                return;
            }


            string indexFile = pargs.GetValue<string>("indexFile");
            string pointsFile = pargs.GetValue<string>("pointsFile");
            string clusterFile = pargs.GetValue<string>("clusterFile");
            string reclusDir = pargs.GetValue<string>("reclusDir");
            string outDir = pargs.GetValue<string>("outDir");
            int consensus = pargs.GetValue<int>("consensus");

            int count;
            char[] sep = new char[] { ' ', '\t' };
            string[] arr;
            List<int> clusters =  new List<int>();
            using (StreamReader reader = new StreamReader(clusterFile))
            {
                count = 0;
                while (!reader.EndOfStream)
                {
                    if (count < consensus)
                    {
                        reader.ReadLine();
                    }
                    else
                    {
                        arr = reader.ReadLine().Split(sep);
                        clusters.Add(int.Parse(arr[1]));
                    }
                    count++;
                }
            }

            int originalClus, totalClusters;
            
            List<int> reclusters;
            string outFile;
            string[] reclusFiles = Directory.GetFiles(reclusDir);
            foreach (var reclusFile in reclusFiles)
            {
                arr = Path.GetFileNameWithoutExtension(reclusFile).Split('-');
                originalClus = int.Parse(arr[0]);
                totalClusters = int.Parse(arr[2]);

                reclusters = new List<int>();
                count = 0;
                using (StreamReader reader = new StreamReader(reclusFile)) 
                {
                    while (!reader.EndOfStream)
                    {
                        if (count < consensus)
                        {
                            reader.ReadLine();
                        }
                        else
                        {
                            reclusters.Add(int.Parse(reader.ReadLine().Split(sep)[1]));
                        }
                        count++;
                    }
                }

                outFile = Path.Combine(outDir,
                                       (clusters.Count + consensus) + "-allinone-reclus-of-" + originalClus + "-to-" +
                                       (totalClusters - 1) + ".txt");
                using (StreamWriter writer = new StreamWriter(outFile))
                {
                    int total = clusters.Count + consensus;
                    for (int i = 0, j=0; i < total; i++)
                    {
                        if (i < consensus)
                        {
                            writer.WriteLine(i + "\t" + totalClusters);
                        }
                        else
                        {
                            if (clusters[i - consensus] != originalClus)
                            {
                                writer.WriteLine(i + "\t" + (totalClusters + 1));
                            }
                            else
                            {
                                writer.WriteLine(i + "\t" + reclusters[j++]);
                            }
                        }
                    }
                }
                PlotVizDataFile.Build(indexFile, outFile, pointsFile).Save(Path.Combine(Path.GetDirectoryName(outFile),
                                                                                        Path.GetFileNameWithoutExtension
                                                                                            (outFile) + ".pviz"));
            }
        }
    }
}
