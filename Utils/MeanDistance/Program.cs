using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MeanDistance
{
    class Program
    {
        static void Main(string[] args)
        {
            string distanceFile = args[0];
            string pointsFile = args[1];
            string outFolder = args[2];
            int size = int.Parse(args[3]);
            bool combined = bool.Parse(args[5]); // specify if the clusters should be thought of as combined into a single one
            int defCombinedCnum = int.MaxValue;

            Hashtable selectedClusterTable = new Hashtable();
            foreach (int i in args[4].Split(new[] { ',' }).Select(x => int.Parse(x)))
            {
                selectedClusterTable.Add(i,i);
                if (i < defCombinedCnum)
                {
                    defCombinedCnum = i;
                }
            }   


            Hashtable pointsTable = new Hashtable();
            Hashtable pnumsPerClusterTable = new Hashtable();

            using (StreamReader pointsReader = new StreamReader(pointsFile))
            {
                char [] sep = new[]{' ', '\t'};
                int pnum, cnum;
                double x, y, z;

                string[] splits;
                
                while (!pointsReader.EndOfStream)
                {
                    splits = pointsReader.ReadLine().Trim().Split(sep);
                    pnum = int.Parse(splits[0]);
                    x = double.Parse(splits[1]);
                    y = double.Parse(splits[2]);
                    z = double.Parse(splits[3]);
                    cnum = int.Parse(splits[4]);

                    if (selectedClusterTable.ContainsKey(cnum))
                    {
                        pointsTable.Add(pnum, new[] {x, y, z});  
                        if (!pnumsPerClusterTable.ContainsKey(combined ? defCombinedCnum : cnum))
                        {
                            List<int> pointsList = new List<int>();
                            pointsList.Add(pnum);
                            pnumsPerClusterTable.Add(combined? defCombinedCnum : cnum, pointsList);
                        }
                        else
                        {
                            ((List<int>) pnumsPerClusterTable[combined ? defCombinedCnum : cnum]).Add(pnum);
                        }
                    }
                }
            }

//            ComputeMeanDistanceOfClusters(pointsTable, clusterPointsTable, outFolder, distanceFile, size);
            OutputDistancesPerCluster(pointsTable, pnumsPerClusterTable, outFolder, distanceFile, size, combined);


        }

        private static void ComputeMeanDistanceOfClusters(Hashtable pointsTable, Hashtable clusterPointsTable, string outFolder, string distanceFile, int size)
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(outFolder, "meandistance.txt")))
            {
                writer.WriteLine("#ClusterNumber\tOriginalMean\tEuclideanMean");
                using (MatrixReader reader = new MatrixReader(distanceFile, MatrixType.Int16, size))
                {
                    List<int> pnums;
                    int cnum;
                    double euclideanMean, originalMean;
                    Int64 totalPairCount;
                    foreach (DictionaryEntry entry in clusterPointsTable)
                    {
                        cnum = (int) entry.Key;
                        pnums = (List<int>)entry.Value;
                        euclideanMean = 0;
                        originalMean = 0;
                        totalPairCount = ((Int64) pnums.Count)*pnums.Count;
                        for (int i = 0; i < pnums.Count; ++i)
                        {
                            for (int j = 0; j < pnums.Count; ++j)
                            {
                                originalMean += ((double) BitConverter.ToInt16(reader.Read(pnums[i], pnums[j]), 0))/
                                                Int16.MaxValue;
                                euclideanMean += GetEuclideanDistance((double[]) pointsTable[pnums[i]],
                                                                      (double[]) pointsTable[pnums[j]]);

                            }
                        }

                        originalMean /= totalPairCount;
                        euclideanMean /= totalPairCount;

                        writer.WriteLine(cnum + "\t" + originalMean + "\t" + euclideanMean);
                    }
                }
            }
        }

        private static void OutputDistancesPerCluster(Hashtable pointsTable, Hashtable clusterPointsTable, string outFolder, string distanceFile, int size, bool combined)
        {
            using (MatrixReader reader = new MatrixReader(distanceFile, MatrixType.Int16, size))
            {
                List<int> pnums;
                int cnum;
                double originalD, euclideanD, originalMean;
                Int64 totalPairCount;
                foreach (DictionaryEntry entry in clusterPointsTable)
                {
                    cnum = (int) entry.Key;
                    pnums = (List<int>) entry.Value;
                    if (pnums.Count > 1)
                    {
                        using (
                            StreamWriter writer =
                                new StreamWriter(Path.Combine(outFolder, "distances_for_cluster_" + cnum + (combined ? "combined" : "") + ".txt")))
                        {
                            writer.WriteLine("#Pair\tOriginalDistance\tEuclideanDistance");
                            originalMean = 0;
                            totalPairCount = ((Int64) pnums.Count)*pnums.Count;
                            for (int i = 0; i < pnums.Count; ++i)
                            {
                                for (int j = i+1; j < pnums.Count; ++j)
                                {
                                    originalD = ((double) BitConverter.ToInt16(reader.Read(pnums[i], pnums[j]), 0))/ Int16.MaxValue;
                                    euclideanD = GetEuclideanDistance((double[])pointsTable[pnums[i]],
                                                                      (double[])pointsTable[pnums[j]]);
                                    writer.WriteLine("(" + pnums[i] + "," + pnums[j] + ")" + "\t" + originalD + "\t" + euclideanD);
                                }
                            }

                        }
                    }
                }
            }
        }

        private static double GetEuclideanDistance(double [] v1, double [] v2)
        {
            double d = 0.0;
            for (int i = 0; i < v1.Length; ++i)
            {
                d += Math.Pow((v2[i] - v1[i]), 2);
            }
            return Math.Sqrt(d);
        }

        private static void TestDummyMatrix()
        {
            using (MatrixReader reader = new MatrixReader(@"C:\Users\sekanaya\Desktop\junkmatrix\mat8.bin", MatrixType.Int16, 8))
            {
                Console.WriteLine(BitConverter.ToInt16(reader.Read(0,1),0));
                Console.WriteLine(BitConverter.ToInt16(reader.Read(0,5),0));
                Console.WriteLine(BitConverter.ToInt16(reader.Read(0,0),0));
                Console.WriteLine(BitConverter.ToInt16(reader.Read(0,5),0));
                Console.WriteLine(BitConverter.ToInt16(reader.Read(1,5),0));
                Console.WriteLine(BitConverter.ToInt16(reader.Read(1,1),0));
                Console.WriteLine(BitConverter.ToInt16(reader.Read(1,0),0));
                Console.WriteLine(BitConverter.ToInt16(reader.Read(0,0),0));
                Console.WriteLine(BitConverter.ToInt16(reader.Read(0,1),0));

                Console.Read();
            }
        }

        private static void GenerateDummyMatrix(int size)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Create(@"C:\Users\sekanaya\Desktop\junkmatrix\mat" + size +".bin")))
            {
                Int16 v;
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        v = ((Int16) ((((i*size) + j)/100.0) * Int16.MaxValue));
                        writer.Write(v);
                    }
                }
            }

        }
    }
}
