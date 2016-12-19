﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Salsa.Core;
using System.IO;

namespace ClusterExtractor
{
    class Program
    {
        /**********************************************************************
         * GOOD VERSION 
         * -- Cluster file should be sorted by point number
         * -- Extracts matrix in the order of point numbers for each clusters
         **********************************************************************/
        private static string configTemplateFile = "C:\\Users\\Pulasthi\\Documents\\GitHub\\DSCToolsOld\\Utils\\ClusterExtractor\\config.properties.template";
        private static string runLine;
        static void Main(string[] args)
        {
            string clusterFile;
            string distFile;
            string outDir;
            int bigc;
            int dataType;
            bool isJava;
            int consensus;
            string clustersLine;
            string newClustersLine;
            using (StreamReader partitionReader = new StreamReader(args[0]))
            {
                char[] sep = new[] {' ','\t'};
                clusterFile = partitionReader.ReadLine().Trim().Split(sep)[1];
                distFile = partitionReader.ReadLine().Trim().Split(sep)[1];
                outDir = partitionReader.ReadLine().Trim().Split(sep)[1];
                bigc = Int32.Parse(partitionReader.ReadLine().Trim().Split(sep)[1]);
                dataType = Int32.Parse(partitionReader.ReadLine().Trim().Split(sep)[1]);
                clustersLine = partitionReader.ReadLine().Trim().Split(sep)[1];
                newClustersLine = partitionReader.ReadLine().Trim().Split(sep)[1];
                consensus = Int32.Parse(partitionReader.ReadLine().Trim().Split(sep)[1]);
                isJava = Convert.ToBoolean(partitionReader.ReadLine().Trim().Split(sep)[1]);
                runLine = partitionReader.ReadLine().Trim().Split('\t')[1];
            }
            //Load the command line args into our helper class which allows us to name arguments
           /* Arguments pargs = new Arguments(args);
            // dataType {0=int16, 1=uint16, 2=double}
            pargs.Usage = "Usage: ClusterExtractor.exe /clusterFile=<string> /distFile=<string> /outDir=<string>" +
                          "/bigc=<int> /dataType=<int> /clusters=<string> /consensus=<int>\n /isJava=<bool>" +
                          "Note. clusters is a comma separated lists of cluster numbers to extract." +
                          "Note. consensus indicates the number of points starting from the first point, that need to be extracted as consensus points for each such cluster.";

            if (!pargs.CheckRequired(new[] { "clusterFile", "distFile", "outDir", "bigc", "dataType", "clusters", "consensus", "isJava" }))
            {
                Console.WriteLine(pargs.Usage);
                Console.Read();
                return;
            }*/

            //var isJava = pargs.GetValue<bool>("isJava");

          //  int consensus = pargs.GetValue<int>("consensus");
            
            //var commaSeperatedClusters = pargs.GetValue<string>("clusters");
            string[] clusters = clustersLine.Split(',');
            string[] newClusters = newClustersLine.Split(',');
            // key<int> is cluster number, value<SortedList<int,int>> is sorted list of points as <pointNumberInList, originalPointNumber>>
            Dictionary<int, SortedList<int, int>> sortedClusterMap =
                new Dictionary<int, SortedList<int, int>>(clusters.Length);
            Dictionary<int, int> newMapCounts =
                new Dictionary<int, int>(newClusters.Length);
            foreach (string cluster in clusters)
            {
                sortedClusterMap[int.Parse(cluster)] = new SortedList<int, int>();
            }

            for (int i = 0; i < newClusters.Length; i++)
            {
                newMapCounts[int.Parse(clusters[i])] = int.Parse(newClusters[i]);
            }
            using (StreamReader reader = new StreamReader(clusterFile))
            {
                char[] sep = { ' ', '\t' };
                int[] data = new int[2];

                if (consensus > 0)
                {
                    for (int i = 0; i < consensus; i++)
                    {
                        // data[0] is the original point number, data[1] is cluster number
                        string readLine = reader.ReadLine();
                        if (!string.IsNullOrEmpty(readLine))
                        {
                            string[] splits = readLine.Split(sep);
                            if (splits.Length == 2) // Usual cluster file with pnum<TAB>cnum
                            {
                                data[0] = int.Parse(splits[0]);
                                data[1] = int.Parse(splits[1]);
                            }
                            else if (splits.Length == 5) // Usual points file with cluster information as pnum<TAB>X<TAB>Y<TAB>Z<TAB>cnum
                            {
                                data[0] = int.Parse(splits[0]);
                                data[1] = int.Parse(splits[4]);
                            }
                            foreach (SortedList<int, int> val in sortedClusterMap.Values)
                            {
                                val.Add(val.Count, data[0]);
                            }
                        }

                    }
                }

                while (!reader.EndOfStream)
                {
                    // data[0] is the original point number, data[1] is cluster number
                    string readLine = reader.ReadLine();
                    if (!string.IsNullOrEmpty(readLine))
                    {
                        string[] splits = readLine.Split(sep);
                        if (splits.Length == 2) // Usual cluster file with pnum<TAB>cnum
                        {
                            data[0] = int.Parse(splits[0]);
                            data[1] = int.Parse(splits[1]);
                        }
                        else if (splits.Length == 5) // Usual points file with cluster information as pnum<TAB>X<TAB>Y<TAB>Z<TAB>cnum
                        {
                            data[0] = int.Parse(splits[0]);
                            data[1] = int.Parse(splits[4]);
                        }
                        if (sortedClusterMap.ContainsKey(data[1]))
                        {
                            // Interested point, i.e. this point belongs to a cluster specified in the clusters list by user
                            SortedList<int, int> sl = sortedClusterMap[data[1]];
                            sl.Add(sl.Count, data[0]);
                        }
                    }
                }
            }


           // var distFile = pargs.GetValue<string>("distFile");
            //var outDir = pargs.GetValue<string>("outDir");


            if (ExtractPoints(distFile, sortedClusterMap, outDir,
                bigc, dataType, isJava, newMapCounts))
            {
                Console.WriteLine("Cluster Extraction Completed");
                Console.WriteLine("  -- Distance Matrix: " + distFile);
                Console.WriteLine("  -- Clusters: " + clustersLine);
                Console.WriteLine("  -- Consensus: " + consensus);
                Console.WriteLine("  -- Output At: " + outDir);
            }
            else
            {
                Console.WriteLine("Cluster Extration Unsuccessful");
            }
            Console.Read();
        }


        private static byte[] ReadInt16(FileStream fStream)
        {
            var bytes = new byte[2];
            fStream.Read(bytes, 0, 2);
            return bytes;
        }

        private static byte[] ReadUInt16(FileStream fStream)
        {
            var bytes = new byte[2];
            fStream.Read(bytes, 0, 2);
            return bytes;
        }

        private static byte[] ReadDouble(FileStream fStream)
        {
            var bytes = new byte[8];
            fStream.Read(bytes, 0, 8);
            return bytes;
        }

        private static byte[] ReadInt16Java(FileStream fStream)
        {
            var bytes = new byte[2];
            fStream.Read(bytes, 0, 2);
            return Swap(bytes);
        }

        private static byte[] ReadUInt16Java(FileStream fStream)
        {
            var bytes = new byte[2];
            fStream.Read(bytes, 0, 2);
            return Swap(bytes);
        }

        private static byte[] ReadDoubleJava(FileStream fStream)
        {
            var bytes = new byte[8];
            fStream.Read(bytes, 0, 8);
            return Swap(bytes);
        }

        private static byte[] Swap(byte[] arr)
        {
            var b = arr[0];
            arr[0] = arr[1];
            arr[1] = b;

            return arr;
        }

        private static void WriteConfigFile(string distanceFile, int numPoints, int maxncent, int clusterNumber,string outDir)
        {
            string outFile = Path.Combine(new string[] { outDir, "config.properties" + "_" + clusterNumber.ToString()});
            StreamWriter writer = new StreamWriter(outFile);
            using (StreamReader reader = new StreamReader(configTemplateFile))
            {
                char[] sep = { ' ', '\t' };
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string param = line.Trim().Split(sep)[0];
                    if (param.Equals("DistanceMatrixFile"))
                    {
                        writer.WriteLine(line + " " + distanceFile);
                    }
                    else if (param.Equals("MaxNcent"))
                    {
                        writer.WriteLine(line + " " + maxncent.ToString());
                    }
                    else if (param.Equals("NumberDataPoints"))
                    {
                        writer.WriteLine(line + " " + numPoints.ToString());
                    }
                    else
                    {
                        writer.WriteLine(line);
                    }
                }
            }

            writer.Flush();
            writer.Close();
        }


        /// <summary>
        /// Extracts sub distance matrices for each cluster in the sortedClusterMap.
        /// </summary>
        /// <param name="distFile">Path to binary distance matrix</param>
        /// <param name="sortedClusterMap">Dictionary of <cluster#, SortedList <point#InList, originalPoint#>> </param>
        /// <param name="outDir">Path to output directory</param>
        /// <param name="bigc">Total number of columns in the input distance matrix</param>
        /// <param name="bigr">Total number of rows in the input distance matrix</param>
        /// <param name="type">Data type of the binary matrix. 
        ///                    Possible values {0=int16, 1=unit16, 2=double}</param>

        private static bool ExtractPoints(string distFile, Dictionary<int, SortedList<int, int>> sortedClusterMap,
            string outDir, int bigc, int type, bool isJava, Dictionary<int, int> newclusters)
        {
            Func<FileStream, byte[]> read;
            int tsize;
            if (isJava)
            {
                switch (type)
                {
                    case 0:
                        tsize = 2;
                        read = ReadInt16Java;
                        break;
                    case 1:
                        tsize = 2;
                        read = ReadUInt16Java;
                        break;
                    case 2:
                        tsize = 8;
                        read = ReadDoubleJava;
                        break;
                    default:
                        tsize = 2;
                        read = ReadInt16Java;
                        break;
                }
            }
            else
            {
                switch (type)
                {
                    case 0:
                        tsize = 2;
                        read = ReadInt16;
                        break;
                    case 1:
                        tsize = 2;
                        read = ReadUInt16;
                        break;
                    case 2:
                        tsize = 8;
                        read = ReadDouble;
                        break;
                    default:
                        tsize = 2;
                        read = ReadInt16;
                        break;
                }
            }

            string outrunFile = Path.Combine(new string[] { outDir, "run.sh"});
            StreamWriter writerrun = new StreamWriter(outrunFile);
            foreach (KeyValuePair<int, SortedList<int, int>> kv in sortedClusterMap)
            {
                string outFile = Path.Combine(new string[] { outDir, Path.GetFileNameWithoutExtension(distFile) + "_" + kv.Key.ToString() + ".bin" });
                WriteConfigFile(outFile, kv.Value.Count, newclusters[kv.Key], kv.Key, outDir);
                writerrun.WriteLine(string.Format(runLine,kv.Key));
                Int64 readCount = 0;
                using (FileStream inStream = File.OpenRead(distFile))
                {
                    using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(outFile)))
                    {
                        SortedList<int, int> points = kv.Value;
                        for (int i = 0; i < points.Count; i++)
                        {
                            for (int j = 0; j < points.Count; j++)
                            {
                                Int64 pnum = ((Int64)points[i]) * bigc + points[j];
                                if (pnum > readCount)
                                {
                                    Int64 skip = (pnum - readCount) * tsize;
                                    if (inStream.CanSeek)
                                    {
                                        inStream.Seek(skip, SeekOrigin.Current);
                                    }
                                    readCount += (pnum - readCount);
                                }
                                else if (pnum < readCount)
                                {
                                    Console.WriteLine("Error: pnum < readCount");
                                    Console.WriteLine("  -- while extracting cluster " + kv.Key);
                                    Console.WriteLine("  -- global pnum = " + pnum);
                                    Console.WriteLine("  -- readCount = " + readCount);
                                    return false;
                                }
                                writer.Write(read(inStream));
                                readCount++;
                            }
                        }
                    }
                }
            }
            writerrun.Flush();
            writerrun.Close();
            return true;
        }
    }
}
