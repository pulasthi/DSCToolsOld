using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace BlockMerger
{
    class Program
    {
        public static void MergeBlocks(string blockDir, string nameFormat, int blockCount, string outFile, HashSet<int> skipRanks)
        {
            var timer = new Stopwatch();
            timer.Start();
            // No metadata version
            using (var writer = new BinaryWriter(File.OpenWrite(outFile)))
            {
                for (var i = 0; i < blockCount; i++)
                {
                    if (skipRanks.Contains(i))
                    {
                        Console.WriteLine("Skipping row block " + i);
                        continue;
                    }
                    Console.WriteLine("Reading column blocks for row: " + i + " ... ");
                    var columnBlocks = new Block<Int16>[blockCount];
                    for (var j = 0;  j < blockCount; j++)
                    {
                        if (skipRanks.Contains(j))
                        {
                            Console.WriteLine("\tSkipping columns block " + j);
                            continue;
                        }
                        var blockFile = Path.Combine(blockDir, string.Format(nameFormat, i, j));
                        if (File.Exists(blockFile))
                        {
                            columnBlocks[j] = Block<Int16>.LoadFromFile(blockFile, false, Block<Int16>.ReadfInt16);
                        }
                        else
                        {
                            // If direct block (i,j) does not exist then transpose (j,i) must exist.
                            blockFile = Path.Combine(blockDir, string.Format(nameFormat, j, i));
                            if (!File.Exists(blockFile))
                            {
                                throw new Exception("Error: Unable to find block (" + i + "," + j + ") or its transpose");
                            }
                            columnBlocks[j] = Block<Int16>.LoadFromFile(blockFile, true, Block<Int16>.ReadfInt16);
                        }
                    }
                    Console.WriteLine("Done.");

                    // Going to write row blocks to disk
                    Console.WriteLine("Writing column blocks for row: " + i + " ... ");
                    UInt32 rows = columnBlocks[0].Rows;
                    for (UInt32 m = 0; m < rows; m++)
                    {
                        for (UInt32 j = 0; j < blockCount; j++)
                        {
                            if (skipRanks.Contains((int)j)) continue;

                            Block<Int16> block = columnBlocks[j];
                            for (UInt32 n = 0; n < block.Cols; n++)
                            {
                                writer.Write(block.DistanceMatrix[m][n]);
                            }
                        }
                    }
                    Console.WriteLine("Done.");
                }
            }
            timer.Stop();
            Console.WriteLine("Block merge completed at {0} with elapsed {1}", DateTime.Now,
                              TimeSpan.FromMilliseconds(timer.ElapsedMilliseconds));
        }

        static void Main(string[] args)
        {
//            Block<Int16> b = Block<Int16>.LoadFromFile(@"C:\Users\sekanaya\Desktop\swgmstester\block\1-2.bin", false, Block<Int16>.ReadfInt16);
//            b.PrintBlock(true);
//            Int16[][] direct = b.DistanceMatrix;
//            b = Block<Int16>.LoadFromFile(@"C:\Users\sekanaya\Desktop\debug\partialfiles\24-127.bin", true, Block<Int16>.ReadfInt16);
//            Console.WriteLine();
//            b.PrintBlock(false);
//            Int16[][] trans = b.DistanceMatrix;
//
//            for (int i = 0; i < direct.Length; i++ )
//            {
//                for (int j = 0; j < direct[i].Length; j++)
//                {
//                    if (direct[i][j] != trans[j][i])
//                    {
//                        Console.WriteLine("Bad !! at direct " + i + "," + j);
//                    }
//                }
//            }
//
//            Console.WriteLine("Done.");

/************************************************/
            string blockDir = args[0];
            string nameFormat = args[1];
            int blockCount = int.Parse(args[2].Trim());
            string outFile = args[3];
            var skipRanks = new HashSet<int>();
            if (args.Length == 5)
            {
                var skips = args[4].Split(new[] {','});

                foreach (var skip in skips)
                {
                    skipRanks.Add(int.Parse(skip));
                }
            }
            MergeBlocks(blockDir, nameFormat, blockCount, outFile, skipRanks);
            Console.Read();
        }
    }
}
