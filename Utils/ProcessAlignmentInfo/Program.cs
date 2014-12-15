using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessAlignmentInfo
{
    class Program
    {
        // Note. Assumes full NxN computation 

        //Example
        /*
        #Row:0	Col:1
        #FL:2872	SL:278	AL277
        #PID:0.72563	Score:665.00000	PIDNG:0.75000	FSLS:1345	SSLS:1380	FSGS:14360	SSGS:1390
        >FR750178_R_[org=Gigaspora_rosea]_DAOM194757_W2856/Att150920_0
        TAGGTGCACTTCTTCGCTTGGCAGGTTAGTGTCGATTTTGGATGTCATAAAATGACTGGGGGAAGGTA-----G--CTCCTTTGGGAGTGTTATAGCCCTTGGTAGATGTGATGTCTGGAATCGAGGATTGCAACGGATACCCTTTAGGGCTAGTCACCTGATCTCTAATTAGTGCTAGATTACGGACAGCATGCTGACCGTGATTTAA-TTATTGGTTAAAAGGTTAGAGTGCACATAAATTCGTTAAGGACACTGACGTAATGGCTTTAAACGAC
        >G4P2R5E01EQVSW_length=278_xy=1829_1262_region=1_run=R_2011_06_22_18_25_07__FoxRegion=0_FoxCluster=14_FoxMethod=SmallestDistanceMeans_1
        TGGGTGTACTTGCCCGTGTGGTTGGTTAACATCAATTTTGGTGGTCATAAAATGACTGGAGGAATGTAGCTTTGATCTCGTATTGAAGTGTTATAGCCTTCGGTAGATGTGATGATCGAGATTGAGGATTGCAACGGATACCCTTCGGGGCTACCTGTCTGGTCTCTGATCGTTGCTCTGGTGCCGAAAGCTTGCTTACGGTTATCAAAGTCGATGGTCAATAGGTTAGAACG-GGTTAAATTCGTTAAGGATGTTGACGTAATGGCTTTAAACGAC
         */

        static void Main(string[] args)
        {
            string alignmentsDir = args[0];
            string outDir = args[1];
            int size = int.Parse(args[2]); // number of rows or columns
            int blocks = int.Parse(args[3]); // number of row or column blocks
            string filePrefix = args[4]; // e.g. alignments_
            string name = args[5]; // name of the project

            string alignmentLengthMatrix = Path.Combine(outDir, name + "_alignmentLength.bin");
            using (BinaryWriter alwriter = new BinaryWriter(File.OpenWrite(alignmentLengthMatrix)))
            {
                for (int i = 0; i < blocks; ++i)
                {
                    using (StreamReader reader = new StreamReader(Path.Combine(alignmentsDir, filePrefix + i + ".txt")))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            if (!string.IsNullOrEmpty(line))
                            {
                                // Just create alignment length marix now.
                                if (line.StartsWith("#Row"))
                                {
                                    int idx = line.IndexOf('\t');
                                    int r = int.Parse(line.Substring(5, (idx - 5)));
                                    int c = int.Parse(line.Substring(idx + 5));


                                    /*if (0 > c && c >= size && r != i)
                                    {
                                        throw new Exception("error in row or column: r=" + r + " expected row=" + i +
                                                            " c=" + c);
                                    }
                                    line = reader.ReadLine();
                                    if (!string.IsNullOrEmpty(line))
                                    {
                                        idx = line.IndexOf("AL", System.StringComparison.Ordinal);
                                        // Note. AL should be followed by a colon, which is not there currently. 
                                        // So when it's there just need to take substring from idx+3
                                        Int16 al = Int16.Parse(line.Substring(idx + 2));

                                    }*/
                                }
                            }
                        }
                    }
                }
            }

        }



        /* string alignmentsDir = args[0];
            string outDir = args[1];
            int size = int.Parse(args[2]);
            string filePrefix = args[3];
            string name = args[4];

            string alignmentLengthMatrix = Path.Combine(outDir, name + "_alignmentLength.bin");
            using (BinaryWriter alwriter = new BinaryWriter(File.OpenWrite(alignmentLengthMatrix)))
            {
                int row = 0, count = 0;
                for (int i = 0; i < size; ++i)
                {
                    using (StreamReader reader = new StreamReader(Path.Combine(alignmentsDir, filePrefix + i + ".txt")))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            if (!string.IsNullOrEmpty(line))
                            {
                                // Just create alignment length marix now.
                                if (line.StartsWith("#Row"))
                                {
                                    int idx = line.IndexOf('\t');
                                    int r = int.Parse(line.Substring(5, (idx - 5)));
                                    int c = int.Parse(line.Substring(idx + 5));
                                    if (0 > c && c >= size && r != i)
                                    {
                                        throw new Exception("error in row or column: r=" + r + " expected row=" + i +
                                                            " c=" + c);
                                    }
                                    line = reader.ReadLine();
                                    if (!string.IsNullOrEmpty(line))
                                    {
                                        idx = line.IndexOf("AL", System.StringComparison.Ordinal);
                                        // Note. AL should be followed by a colon, which is not there currently. 
                                        // So when it's there just need to take substring from idx+3
                                        Int16 al = Int16.Parse(line.Substring(idx + 2));

                                    }
                                }
                            }
                        }
                    }
                }
            }*/
    }
}
