using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bio;
using Bio.IO.FastA;

namespace ClusterCenters
{
    class Program
    {
        /* NOTE. Now this is changed to do MinMax instead of MinMean. See commented lines */

        static void Main(string[] args)
        {
            /* Prerequisites */
            /*
             * Sequence file and distance file should be in the same order and have the same number of points
             * Point numbers in the points file are within the range of [0,size]
             */
            if (args.Length == 1 && File.Exists(args[0]))
            {
                /*
                 * SequenceFile     <string> Path to sequence fasta file
                 * DistFile     	<string> Path to distance file which has N x N distances and row, col order is as same as the sequences. N is the total number of sequences
                 * Size         	<int> The number of sequences
                 * ClusterString	<string> Comma separated list of of cluster numbers
                 * DataType         <int> 0=Int16, 1=UInt16, 2=double
                 * Method	<int> 0=MinMean, 1=MinMax
                 * PointsFile	    <string> Path to points file. The points MUST have the cluster numbers associated with them
                 * OutputDir	    <string> Path to output directory
                 * Verify	    <bool> True if you need to verify with existing center results. False otherwise. If true, the following must be given
                 * cnum<tab>original-minmean-seqname<tab>mds-minmean-seqname
                 */
                using (StreamReader reader = new StreamReader(args[0]))
                {
                    try
                    {
                        char[] sep = new[] {'\t'};
                        string sequenceFile = reader.ReadLine().Trim().Split(sep)[1];
                        string distFile = reader.ReadLine().Trim().Split(sep)[1];
                        int size = int.Parse(reader.ReadLine().Trim().Split(sep)[1]);

                        string clusterString = reader.ReadLine().Trim().Split(sep)[1];
                        int dataType = int.Parse(reader.ReadLine().Trim().Split(sep)[1]);
                        int method = int.Parse(reader.ReadLine().Trim().Split(sep)[1]);
                        string pointsFile = reader.ReadLine().Trim().Split(sep)[1];
                        string outputDir = reader.ReadLine().Trim().Split(sep)[1];
                        bool verify = bool.Parse(reader.ReadLine().Trim().Split(sep)[1]);
                        Hashtable verifyCnumToOriginalMinMeanSeqName = new Hashtable();
                        Hashtable verifyCnumToEuclideanMinMeanSeqName = new Hashtable();
                        if (verify)
                        {
                            while (!reader.EndOfStream)
                            {
                                string line = reader.ReadLine();
                                if (!string.IsNullOrEmpty(line))
                                {
                                    string[] splits = line.Trim().Split(sep);
                                    int cnum = int.Parse(splits[0]);
                                    verifyCnumToOriginalMinMeanSeqName.Add(cnum, splits[1]);
                                    verifyCnumToEuclideanMinMeanSeqName.Add(cnum, splits[2]);
                                }
                            }
                        }
                        ComputeClusterCenters(distFile, size, clusterString, dataType, method, pointsFile, outputDir, sequenceFile, verify, verifyCnumToOriginalMinMeanSeqName, verifyCnumToEuclideanMinMeanSeqName);
                        Console.WriteLine("Done.");
                        Console.Read();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("An error occurred while reading parameters file: " + e.Message);
                    }
                }
            }
            else
            {
                Console.WriteLine("Usage:\nClusterCenters <path-to-param-file>\n");
            }
        }

        private static void ComputeClusterCenters(string distFile, int size, string clusterString, int dataType, int method, string pointsFile, string outputDir, string sequenceFile, bool verify, Hashtable verifyCnumToOriginalMinMeanSeqName, Hashtable verifyCnumToEuclideanMinMeanSeqName)
        {
            string[] temp = clusterString.Split(',');
            HashSet<int> clusters = new HashSet<int>();
            foreach (string t in temp)
            {
                clusters.Add(int.Parse(t));
            }

            Hashtable pnumToXyzTable = new Hashtable();
            Hashtable cnumToPnumsTable = new Hashtable();
            Hashtable pnumToCnumTable = new Hashtable();

            int maxCnum = 0;
            using (StreamReader reader = new StreamReader(pointsFile))
            {
                char[] sep = new[] { ' ', '\t' };
                while (!reader.EndOfStream)
                {
                    string[] splits = reader.ReadLine().Trim().Split(sep);
                    int cnum = int.Parse(splits[4]);
                    maxCnum = cnum > maxCnum ? cnum : maxCnum;

                    // Use this point only if its cnum is in the desired set of clusters
                    if (clusters.Contains(cnum))
                    {
                        int pnum = int.Parse(splits[0]);
                        double x = double.Parse(splits[1]);
                        double y = double.Parse(splits[2]);
                        double z = double.Parse(splits[3]);

                        pnumToXyzTable[pnum] = new[] { x, y, z };
                        pnumToCnumTable[pnum] = cnum;
                        if (cnumToPnumsTable.ContainsKey(cnum))
                        {
                            ((IList<int>)cnumToPnumsTable[cnum]).Add(pnum);
                        }
                        else
                        {
                            IList<int> pnums = new List<int> { pnum };
                            cnumToPnumsTable[cnum] = pnums;
                        }
                    }
                }
            }
            Hashtable[] arr = ComputeClusterCentersAsMinMean(distFile, size, pnumToXyzTable, pnumToCnumTable, cnumToPnumsTable);
            Hashtable cnumToOriginalMinMeanPnumTable = arr[0];
            Hashtable cnumToEuclideanMinMeanPnumTable = arr[1];

           using (StreamReader reader = new StreamReader(pointsFile))
            {
                string outputPointsFile = Path.Combine(outputDir,
                                                       Path.GetFileNameWithoutExtension(pointsFile) +
                                                       "_with_MinMean_Centers.txt");
                string metaFile = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(pointsFile) +
                                                          "_meta.txt");

                using (StreamWriter writer = new StreamWriter(outputPointsFile), metaWriter = new StreamWriter(metaFile))
                {
                    using (FastAParser parser = new FastAParser(sequenceFile))
                    {
                        IList<ISequence> seqs = parser.Parse().ToList();
                        Hashtable seqIdToPnum = new Hashtable();
                        for (int i = 0; i < seqs.Count; i++)
                        {
                            ISequence seq = seqs[i];
                            seqIdToPnum.Add(seq.ID.ToUpper(), i);
                        }
                        while (!reader.EndOfStream)
                        {
                            writer.WriteLine(reader.ReadLine());
                        }

                        int count = 0;
                        if (verify)
                        {
                            metaWriter.WriteLine(
                                "Cnum\tOriginalMinMeanSeqId\tPnum\tVerifyWith\tPnum\tEuclideanMinMeanSeqId\tPnum\tVerifyWith\tPnum");
                        }
                        else
                        {
                            metaWriter.WriteLine(
                                "cnum\toriginal-center-seq-name\toriginal-center-pnum\toriginal-center-pnum-in-final-output\teuclidean-center-seq-name\teuclidean-center-pnum\teuclidean-center-pnum-in-final-output");
                        }
                        foreach (DictionaryEntry kv in cnumToOriginalMinMeanPnumTable)
                        {
                            int cnum = (int)kv.Key;
                            int originalCenterPnum = (int)kv.Value;
                            int euclideanCenterPnum = (int) cnumToEuclideanMinMeanPnumTable[cnum];
                            double[] originalCenterXyz = (double[])pnumToXyzTable[originalCenterPnum];
                            double[] euclideanCenterXyz = (double[]) pnumToXyzTable[euclideanCenterPnum];

                            string originalMinMeanSeqId = seqs[originalCenterPnum].ID;
                            string euclideanMinMeanSeqId = seqs[euclideanCenterPnum].ID;

                            if (verify)
                            {
                                string verifyOriginalMinMeanSeqId = (string)verifyCnumToOriginalMinMeanSeqName[cnum];
                                string verifyEuclideanMinMeanSeqId = (string)verifyCnumToEuclideanMinMeanSeqName[cnum];
                                metaWriter.WriteLine(cnum + "\t" + originalMinMeanSeqId +"\t" + originalCenterPnum + "\t" + (verifyOriginalMinMeanSeqId.Equals(originalMinMeanSeqId,StringComparison.OrdinalIgnoreCase) ? "OK" : verifyOriginalMinMeanSeqId) +"\t" + seqIdToPnum[verifyOriginalMinMeanSeqId.ToUpper()] +"\t"
                                    + euclideanMinMeanSeqId + "\t" + euclideanCenterPnum + "\t" + (verifyEuclideanMinMeanSeqId.Equals(euclideanMinMeanSeqId, StringComparison.OrdinalIgnoreCase) ? "OK" : verifyEuclideanMinMeanSeqId) + "\t" + seqIdToPnum[verifyEuclideanMinMeanSeqId.ToUpper()]);
                            }
                            else
                            {
                                metaWriter.WriteLine(cnum + "\t" + originalMinMeanSeqId + "\t" + originalCenterPnum +
                                                     "\t" + (size + count) + "\t" + euclideanMinMeanSeqId + "\t" +
                                                     euclideanCenterPnum + "\t" + (size + count + 1));
                            }

                            writer.WriteLine((size + count++) + "\t" + originalCenterXyz[0] + "\t" + originalCenterXyz[1] + "\t" + originalCenterXyz[2] +
                                             // To put all original min mean centers in one cluster
                                             // "\t" + (maxCnum + 1));
                                             // To put all original min mean centers in a separate cluster
                                             "\t" + cnum +".OriginalMin");
                            
                            writer.WriteLine((size + count) + "\t" + euclideanCenterXyz[0] + "\t" + euclideanCenterXyz[1] + "\t" + euclideanCenterXyz[2] +
                                             // To put all euclidean min mean centers in one cluster
                                             //"\t" + (maxCnum + 2));
                                             // To put all euclidean min mean centers in a separate cluster
                                             "\t" + cnum +".EuclideanMin");
                            ++count;
                        }
                    }
                }
            }
        }

        private static Hashtable[] ComputeClusterCentersAsMinMean(string distFile, int size, Hashtable pnumToXyzTable, Hashtable pnumToCnumTable, Hashtable cnumToPnumsTable)
        {
            Hashtable cnumToOriginalMinMeanPnumTable = new Hashtable(cnumToPnumsTable.Count);
            Hashtable cnumToEuclideanMinMeanPnumTable = new Hashtable(cnumToPnumsTable.Count);
            using (MatrixReader reader = new MatrixReader(distFile, MatrixType.Int16, size))
            {
                int cnum;
                IList<int> pnums;
                Hashtable cnumToOriginalMinMeanTable = new Hashtable(cnumToPnumsTable.Count);
                Hashtable cnumToEuclideanMinMeanTable = new Hashtable(cnumToPnumsTable.Count);
                double od, ed; // od = original distance, ed = Euclidean distance
                for (int i = 0; i < size; ++i)
                {
                    // Use this point only if it is a point in one of the desired clusters
                    if (pnumToCnumTable.ContainsKey(i))
                    {
                        od = 0.0;
                        ed = 0.0;
                        cnum = (int)pnumToCnumTable[i];
                        pnums = (IList<int>)cnumToPnumsTable[cnum];
                        foreach (int pnum in pnums)
                        {
                            double odtmp = (((double) BitConverter.ToInt16(reader.Read(i, pnum), 0))/Int16.MaxValue);
                            double edtmp = GetEuclideanDistance((double[]) pnumToXyzTable[i], (double[]) pnumToXyzTable[pnum]);

                            // for MinMax
                            if (od < odtmp) od = odtmp;
                            if (ed < edtmp) ed = edtmp;

                            // for MinMean
//                            od += odtmp;
//                            ed += edtmp;
                        }

                        // for MinMean
//                        od = od/pnums.Count;
//                        ed = ed/pnums.Count;


                        if (!cnumToOriginalMinMeanTable.ContainsKey(cnum) || od < ((double)cnumToOriginalMinMeanTable[cnum]))
                        {
                            cnumToOriginalMinMeanTable[cnum] = od;
                            cnumToOriginalMinMeanPnumTable[cnum] = i;
                        }

                        if (!cnumToEuclideanMinMeanTable.ContainsKey(cnum) || ed < ((double)cnumToEuclideanMinMeanTable[cnum]))
                        {
                            cnumToEuclideanMinMeanTable[cnum] = ed;
                            cnumToEuclideanMinMeanPnumTable[cnum] = i;
                        }
                    }
                }
            }
            return new[]{cnumToOriginalMinMeanPnumTable,cnumToEuclideanMinMeanPnumTable};
        }

        private static double GetEuclideanDistance(double[] v1, double[] v2)
        {
            return Math.Sqrt(Math.Pow((v1[0] - v2[0]), 2) + Math.Pow((v1[1] - v2[1]), 2) + Math.Pow((v1[2] - v2[2]), 2));
        }
    }
}
