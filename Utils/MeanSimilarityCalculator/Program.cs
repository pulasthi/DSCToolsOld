using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Salsa.Core;

namespace MeanSimilarityCalculator
{
    public class Program
    {
        private delegate double DecoderDel(FileStream fStream);

        private static void Main(string[] args)
        {
            //Load the command line args into our helper class which allows us to name arguments
            var pargs = new Arguments(args);
            // dataType {0=int16, 1=uint16, 2=double}
            pargs.Usage =
                "Usage: MeanSimilarityCalculator.exe /clusterFile=<string> /distFile=<string> " +
                "/outDir=<string> /bigc=<int> /dataType=<int> /clusters=<string> " +
                "\n  Note. clusters is a comma separated lists of cluster numbers to extract." +
                "\n  Note. dataType = [0-2] where 0=int16, 1=uint16, 2=double";

            if (
                !pargs.CheckRequired(new[] {"clusterFile", "distFile", "outDir", "bigc", "dataType", "clusters"}))
            {
                Console.WriteLine(pargs.Usage);
                return;
            }


            string[] clusters = pargs.GetValue<string>("clusters").Split(',');
            // key<int> is cluster number, value<SortedList<int,int>> is sorted list
            // of points as <pointNumberInList, originalPointNumber>>
            var sortedClusterMap = new Dictionary<int, SortedList<int, int>>(clusters.Length);
            foreach (string cluster in clusters)
            {
                sortedClusterMap[int.Parse(cluster)] = new SortedList<int, int>();
            }

            using (var reader = new StreamReader(pargs.GetValue<string>("clusterFile")))
            {
                int[] data;

                SortedList<int, int> sl;
                char[] sep = {' ', '\t'};
                while (!reader.EndOfStream)
                {
                    // data[0] is the original point number, data[1] is cluster number
                    data = reader.ReadLine().Split(sep).Select(x => int.Parse(x)).ToArray();
                    if (sortedClusterMap.ContainsKey(data[1]))
                    {
                        // Interested point, i.e. this point belongs to a cluster specified in the clusters list by user
                        sl = sortedClusterMap[data[1]];
                        sl.Add(sl.Count, data[0]);
                    }
                }
            }

            int tsize;
            double maxConst;
            DecoderDel decode;
            switch (pargs.GetValue<int>("dataType"))
            {
                case 0:
                    tsize = 2;
                    maxConst = Int16.MaxValue * 1.0;
                    decode = MeanSimilarityCalculator.Program.DecodeInt16;
                    break;
                case 1:
                    tsize = 2;
                    maxConst = UInt16.MaxValue * 1.0;
                    decode = MeanSimilarityCalculator.Program.DecodeUInt16;
                    break;
                case 2:
                    tsize = 8;
                    maxConst = Double.MaxValue;
                    decode = MeanSimilarityCalculator.Program.DecodeDouble;
                    break;
                default:
                    tsize = 2;
                    maxConst = Int16.MaxValue * 1.0;
                    decode = MeanSimilarityCalculator.Program.DecodeInt16;
                    break;
            }

            CalculateMeanSimilarity(pargs.GetValue<string>("distFile"), sortedClusterMap,
                                    pargs.GetValue<string>("outDir"),
                                    pargs.GetValue<int>("bigc"),                                   
                                    decode, tsize, maxConst);
            Console.WriteLine("Done.");
            Console.Read();
        }

        private static double DecodeInt16(FileStream fStream)
        {
            var bytes = new byte[2];
            fStream.Read(bytes, 0, 2);
            return ((Int16)((bytes[1] << 8) | (bytes[0] & 0xff)));
        }

        // Todo: Following needs to be tested
        private static double DecodeUInt16(FileStream fStream)
        {
            var bytes = new byte[2];
            fStream.Read(bytes, 0, 2);
            return ((UInt16) ((bytes[1] << 8) | (bytes[0] & 0xff)));
        }

        private static double DecodeDouble(FileStream fStream)
        {
            var bytes = new byte[8];
            fStream.Read(bytes, 0, 8);
            return ((bytes[7] & 0xff) << 56) |
                   ((bytes[6] & 0xff) << 48) |
                   ((bytes[5] & 0xff) << 40) |
                   ((bytes[4] & 0xff) << 32) |
                   ((bytes[3] & 0xff) << 24) |
                   ((bytes[2] & 0xff) << 16) |
                   ((bytes[1] & 0xff) << 8) |
                   ((bytes[0] & 0xff));
        }


        /// <summary>
        /// Calculates the mean similarity of sub distance matrices for each cluster in the sortedClusterMap.
        /// </summary>
        /// <param name="distFile">Path to binary distance matrix</param>
        /// <param name="sortedClusterMap">Dictionary of <!--<cluster#, SortedList <point#InList, originalPoint#>> </param> --> </param>
        /// <param name="outDir">Path to output directory</param>
        /// <param name="bigc">Total number of columns in the input distance matrix</param>
        /// <param name="decode">Delegate to decode a number from the bytestream. The delegate may 
        ///                      be a decoder for either Int16, UInt16, or double</param>
        /// <param name="tsize">The number of bytes required to represent the number in a particular encoding</param>
        /// <param name="maxConst">The maximum positive value for the stored data type</param>
        private static void CalculateMeanSimilarity(string distFile,
                                                    Dictionary<int, SortedList<int, int>> sortedClusterMap,
                                                    string outDir, int bigc, DecoderDel decode, int tsize, double maxConst)
        {
            Int64 readCount;
            string outFile;
            SortedList<int, int> points;
            Int64 skip;
            Int64 pnum;
            foreach (var kv in sortedClusterMap)
            {
                outFile = Path.Combine(new[] {outDir, string.Format("mean_dissimilarity_for_cluster_{0}.txt", kv.Key)});
                readCount = 0;
                using (FileStream fStream = File.OpenRead(distFile))
                {
                    using (var writer = new StreamWriter(File.OpenWrite(outFile)))
                    {
                        double sum = 0.0;
                        double temp;
                        points = kv.Value;

                        Int64 count = 0;
                        for (int i = 0; i < points.Count; i++)
                        {
                            for (int j = 0; j < points.Count; j++)
                            {                                      
                                pnum = ((Int64)points[i])*bigc + points[j];
                                if (pnum > readCount)
                                {
                                    skip = (pnum - readCount)*tsize;

                                    if (fStream.CanSeek)
                                    {
                                        fStream.Seek(skip, SeekOrigin.Current);
                                    }
                                    readCount += (pnum - readCount);
                                }
                                else if (pnum < readCount)
                                {
                                    Console.WriteLine("weird");
                                }
                                temp = decode(fStream);

                                // consider only top triangle 
                                // (for effiency - we can stop before reading the last row of this sub matrix)
                                if (i < j)
                                {
                                    sum += temp;
                                    count++;
                                }

                                if (i == (points.Count - 2) && j == (points.Count - 1))
                                {
                                    /* time to write output and break the two "for" loops */

                                    // total number of unique pairs, i.e. N(N-1)/2
                                    temp = ((double)points.Count)*(points.Count - 1)/2;
                                    double meanDissimilarity = sum/(temp*maxConst);

                                    Console.WriteLine(new string('-',30));
                                    Console.WriteLine("Cluster: " + kv.Key);
                                    Console.WriteLine("Points: " + points.Count);
                                    Console.WriteLine("Pairs Read: " + count);
                                    Console.WriteLine("Unique Pairs: " + (points.Count * (points.Count - 1) / 2));
                                    Console.WriteLine("Diagonal Pairs: " + points.Count);
                                    Console.WriteLine("Mean Dissimilarity " + meanDissimilarity);
                                    Console.WriteLine(new string('-',30));

                                    // write output
                                    writer.WriteLine(string.Format("Matrix Size: {0} x {1}", points.Count, points.Count));
                                    writer.WriteLine(string.Format("Unique Pairs: {0}", temp));
                                    writer.WriteLine(string.Format("Sum of Dissimilarity: {0}", sum));
                                    writer.WriteLine(string.Format("Mean Dissimilarity: {0}", meanDissimilarity));
                                    writer.WriteLine("{0}\t{1}", points.Count, meanDissimilarity);

                                    // hit the brakes for "for" loops
                                    i = points.Count;
                                    break;
                                }
                                readCount++;
                            }
                        }
                    }
                }
            }
        }
    }
}