using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bio;
using Bio.IO.FastA;

namespace ReleaseIdxGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string initFullFasta =
//                @"C:\Users\sekanaya\Desktop\haixureleaseidxgen\mega\init_fasta\allreads_uniques_gt200_446041_random.txt";
                @"C:\Users\sekanaya\Desktop\minareleaseidxgen\mega\init_fasta\hmp16sRNA_uniques.fa";
//            string regionFastaDir = @"C:\Users\sekanaya\Desktop\haixureleaseidxgen\mega\regions_fasta";
            string regionFastaDir = @"C:\Users\sekanaya\Desktop\minareleaseidxgen\mega\regions_fasta";
//            string regionFastaMappingsDir = @"C:\Users\sekanaya\Desktop\haixureleaseidxgen\mega\regions_mappings";
            string regionFastaMappingsDir = @"C:\Users\sekanaya\Desktop\minareleaseidxgen\mega\regions_mappings";
//            string idxFile = @"C:\Users\sekanaya\Desktop\haixureleaseidxgen\mega\idx.txt";
            string idxFile = @"C:\Users\sekanaya\Desktop\minareleaseidxgen\mega\idx.txt";

            Console.WriteLine("Working ...");

            Hashtable allSeqsHt = new Hashtable();
            using (FastAParser parser = new FastAParser(initFullFasta))
            {
                IEnumerable<ISequence> seqs = parser.Parse();
                int count = 0;
                foreach (ISequence seq in seqs)
                {
                    allSeqsHt.Add(seq.ID, count);
                    ++count;
                }
                Console.WriteLine("  Total sequence count: " + count);
            }

            string[] files = Directory.GetFiles(regionFastaDir);
            Hashtable regionFastFiles = new Hashtable(files.Length);
            string fileName;
            foreach (string file in files)
            {
                fileName = Path.GetFileNameWithoutExtension(file);
                regionFastFiles.Add(int.Parse(fileName.Substring(fileName.LastIndexOf('_')+1)), file);
            }

            Console.WriteLine("  Total mega regions: " + files.Length);
            Console.WriteLine();

            files = Directory.GetFiles(regionFastaMappingsDir);
            Hashtable regionFastaMappingFiles = new Hashtable(files.Length);
            foreach (string file in files)
            {
                int startIndex = file.LastIndexOf('_')+1;
                regionFastaMappingFiles.Add(int.Parse(file.Substring(startIndex, file.IndexOf('(') - startIndex)), file);
            }

            // Template index line {sequence#}\t{sequenceName}\t{length}\t{megaregion#}\t{cluster#}\t{FastAName}
            string templateIdxLine = "{0}\t{1}\t{2}\t{3}\t{4}\t{5}";
            SortedList<int, string> sortedIdxList = new SortedList<int, string>();
            Hashtable multiRegionIdxTable = new Hashtable();

            int start = 0;
            int end = regionFastFiles.Count;
            char [] sep = new[]{'\t', ' '};

            int seqNum, megaNum, clusNum;
            IEnumerable<ISequence> regionSeqs;
            for (int i = start; i < end; ++i)
            {
                megaNum = i;
                using (FastAParser parser = new FastAParser(regionFastFiles[i] as string))
                {
                    regionSeqs = parser.Parse();
                    using (StreamReader reader = new StreamReader(regionFastaMappingFiles[i] as string))
                    {
                        Console.Write("  Processing file " + i + " of " + (end - start) + " ... ");
                        foreach (ISequence regionSeq in regionSeqs)
                        {
                            seqNum = (int) allSeqsHt[regionSeq.ID];
                            clusNum = int.Parse(reader.ReadLine().Trim().Split(sep)[4]);
                            if (sortedIdxList.ContainsKey(seqNum))
                            {
                                if(!multiRegionIdxTable.ContainsKey(seqNum))
                                {
                                    multiRegionIdxTable[seqNum] = new List<string>();
                                    ((List<string>) multiRegionIdxTable[seqNum]).Add(sortedIdxList[seqNum]);
                                }
                                ((List<string>) multiRegionIdxTable[seqNum]).Add(string.Format(templateIdxLine, seqNum,
                                                                                               regionSeq.ID,
                                                                                               regionSeq.Count, megaNum,
                                                                                               clusNum,
                                                                                               Path.GetFileName(
                                                                                                   regionFastFiles[i] as
                                                                                                   string)));
                            }
                            else
                            {
                                sortedIdxList.Add(seqNum,
                                                 string.Format(templateIdxLine, seqNum, regionSeq.ID, regionSeq.Count,
                                                               megaNum, clusNum, Path.GetFileName(regionFastFiles[i] as string)));
                            }
                            
                        }
                        Console.WriteLine(" done");
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(idxFile))
            {
                writer.WriteLine("SeqNum\tSeqName\tSeqLen\tMegaRegionNum\tClusNum\tFastaName");
                foreach (string idxLine in sortedIdxList.Values)
                {
                    writer.WriteLine(idxLine);
                }
            }

            if (multiRegionIdxTable.Count > 0)
            {
                using (
                    StreamWriter multiRegionWriter =
                        new StreamWriter(Path.Combine(Path.GetDirectoryName(idxFile),
                                                      Path.GetFileNameWithoutExtension(idxFile) + "multi_regions.txt")))
                {
                    foreach (List<string> ls in multiRegionIdxTable.Values)
                    {
                        foreach (string s in ls)
                        {
                            multiRegionWriter.WriteLine(s);
                        }
                    }
                }
            }

            Console.WriteLine("Done.");
            Console.Read();
        }
    }
}
