using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common;

namespace DistanceRearrange
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("Invalid number of arguments");
                return;
            }
            string wrongToRightOrderFile = args[0];
            string wrongOrderDistanceFile = args[1];
            string outputDir = args[2];
            int size = int.Parse(args[3]);

            Hashtable rightToWrongOrder = new Hashtable();
            if (File.Exists(wrongToRightOrderFile) && File.Exists(wrongOrderDistanceFile) && Directory.Exists(outputDir))
            {
                using (StreamReader reader = new StreamReader(wrongToRightOrderFile))
                {
                    char[] sep = new[] {' ', '\t'};
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line))
                        {
                            string[] splits = line.Split(sep);
                            rightToWrongOrder[int.Parse(splits[1])] = int.Parse(splits[0]);

                        }
                    }
                }
                using (MatrixReader wrongOrderDistanceReader = new MatrixReader(wrongOrderDistanceFile, MatrixType.Int16, size))
                {
                    using (BinaryWriter rightOrderDistanceWriter = new BinaryWriter(File.OpenWrite(Path.Combine(outputDir, Path.GetFileNameWithoutExtension(wrongOrderDistanceFile) +"_rearranged.bin"))))
                    {
                        for (int i = 0; i < size; i++)
                        {
                            int wrongRow = (int)rightToWrongOrder[i];
                            for (int j = 0; j < size; j++)
                            {
                                int wrongCol = (int) rightToWrongOrder[j];
                                rightOrderDistanceWriter.Write(wrongOrderDistanceReader.Read(wrongRow, wrongCol));
                            }
                        }
                    }
                }
            }
        }
    }
}
