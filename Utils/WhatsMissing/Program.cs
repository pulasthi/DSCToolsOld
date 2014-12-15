using System;
using System.Collections;
using System.IO;

namespace WhatsMissing
{
    class Program
    {

        static void Main(string [] args)
        {
            if (args[0] == "missing")
            {
                WhatsMissing();
            }
            else
            {
                SequenceRangesAndBlockNumbers(int.Parse(args[0]), int.Parse(args[1]), args[2]);
            }
        }

        private static void WhatsMissing()
        {
            const string donelist = @"C:\sali\pti\sub\salsa\Saliya\c#\Utils\WhatsMissing\donelist.txt";
            const int start = 0;
            const int end = 767;

            var ht = new Hashtable();
            using (var reader = new StreamReader(donelist))
            {
                while (!reader.EndOfStream)
                {
                    var readLine = reader.ReadLine();
                    if (readLine != null) ht.Add(int.Parse(readLine.Trim()), string.Empty);
                }
            }

            for (int i = start; i <= end; i++)
            {
                if (!ht.ContainsKey(i))
                {
                    Console.WriteLine("Missing: " + i + " in node " + Math.Ceiling(i/24.0));
                }
            }

            Console.WriteLine("Done.");
            Console.Read();
        }

        static void SequenceRangesAndBlockNumbers(int seqCount, int worldSize, string file)
        {
            var processBlocks = BlockPartitioner.Partition(seqCount, seqCount, worldSize, worldSize);
            var processRowRanges = RangePartitioner.Partition(seqCount, worldSize);

            using (var writer = new StreamWriter(new BufferedStream(new FileStream(file,FileMode.Create,FileAccess.Write))))
            {

                writer.WriteLine("Row Ranges");
                for (int i = 0; i < worldSize; i++)
                {
                    var processRowRange = processRowRanges[i];
                    writer.WriteLine("Rank " + i + " [" + processRowRange.StartIndex + "," + processRowRange.EndIndex + "]");
                }
                writer.Close();
            }
        }
    }
}
