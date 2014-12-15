using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ToFasta
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles(@"C:\Users\sekanaya\Desktop\junk");
            string output = @"C:\Users\sekanaya\Desktop\junk\allInOneFasta.txt";

            using (StreamWriter writer = new StreamWriter(output))
            {
                foreach (string file in files)
                {
                    using (StreamReader reader = new StreamReader(file))
                    {
                        string seq = string.Empty;
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            if (!string.IsNullOrEmpty(line))
                            {
                                if (line.StartsWith(">"))
                                {
                                    if (!string.IsNullOrEmpty(seq))
                                    {
                                        writer.WriteLine(seq);
                                    }
                                    writer.WriteLine(line);
                                }
                                else
                                {
                                    string[] splits = line.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);

                                    for (int i = 1; i < splits.Length; ++i)
                                    {
                                        seq += splits[i];
                                    }
                                }
                            }
                        }
                        writer.WriteLine(seq);
                    }
                }
            }
        }
    }
}
