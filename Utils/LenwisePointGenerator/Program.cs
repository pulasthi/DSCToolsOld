using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LenwisePointGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string ptsDir = args[0];
            string idxDir = args[1];
            string outDir = args[2];

            string ptsSuffix = "*_pts.txt";
            string[] ptsFiles = Directory.GetFiles(ptsDir, ptsSuffix);
            Hashtable ht = new Hashtable();
            foreach (string ptsFile in ptsFiles)
            {
                ht.Add(Path.GetFileNameWithoutExtension(ptsFile), ptsFile);
            }

            string idxSuffix = "*_seq_index.txt";
            string[] idxFiles = Directory.GetFiles(idxDir, idxSuffix);
            string ptsFileName;
            string outFile;
            string[] ptsSplits, idxSplits;
            char[] sep = new[] {' ', '\t'};
            string template = "{0}\t{1}\t{2}\t{3}\t{4}";
            foreach (string idxFile in idxFiles)
            {
                ptsFileName = Path.GetFileNameWithoutExtension(idxFile);
                ptsFileName = ptsFileName.Substring(0, ptsFileName.LastIndexOf("_seq_index")) + "_pts";
                outFile = Path.Combine(outDir, Path.GetFileNameWithoutExtension(ptsFileName) + "_lenwise_zeroidx.txt");
                if (ht.ContainsKey(ptsFileName))
                {
                    using (StreamReader idxReader = new StreamReader(idxFile),
                        ptsReader = new StreamReader(ht[ptsFileName] as string))
                    {
                        using (StreamWriter writer = new StreamWriter(outFile))
                        {
                            while (!idxReader.EndOfStream && !ptsReader.EndOfStream)
                            {
                                ptsSplits = ptsReader.ReadLine().Split(sep);
                                idxSplits = idxReader.ReadLine().Split(sep);
                                string s = string.Format(template, ptsSplits[0], ptsSplits[1], ptsSplits[2],
                                                              ptsSplits[3],
                                                              GetLengthwiseClusterNumber(int.Parse(idxSplits[2])));

                                writer.WriteLine(s);
                            }
                        }
                        if (!idxReader.EndOfStream || ! ptsReader.EndOfStream)
                        {
                            Console.WriteLine("Error: points file " + ptsFileName + " and idx file " +
                                              Path.GetFileName(ht[ptsFileName] as string) +
                                              " have different number of lines");
                            if (File.Exists(outFile))
                            {
                                File.Delete(outFile);
                                continue;
                            }
                        }
                    }
                }
            }
        }

        static int GetLengthwiseClusterNumber(int len)
        {
            if (len >= 500) return 0;
            if (475 <= len && len < 500) return 1;
            if (450 <= len && len < 475) return 2;
            if (425 <= len && len < 450) return 3;
            if (400 <= len && len < 425) return 4;
            if (375 <= len && len < 400) return 5;
            if (350 <= len && len < 375) return 6;
            if (325 <= len && len < 350) return 7;
            if (300 <= len && len < 325) return 8;
            if (len < 300) return 9;
            throw new Exception("Error: Invalid length " + len);
        }
    }
}
