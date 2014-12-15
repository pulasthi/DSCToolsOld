using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitPageRankData
{
    class Program
    {
        static void Main(string[] args)
        {
            string dataFile = @"C:\Users\sekanaya\Downloads\pagerankdata\en0000-01and02_reset_idx_and_square.am";
            int numOfSplits = 8;

            string splitDir = Path.Combine(Path.GetDirectoryName(dataFile), numOfSplits.ToString());
            if (!Directory.Exists(splitDir))
            {
                Directory.CreateDirectory(splitDir);
            }

            using (var reader = new StreamReader(dataFile))
            {
                int totalUrls = int.Parse(reader.ReadLine());
                int n = totalUrls/numOfSplits;
                int r = totalUrls%numOfSplits;
                for (int i = 0; i < numOfSplits; i++)
                {
                    string splitFile = Path.Combine(splitDir,
                                                    Path.GetFileNameWithoutExtension(dataFile) + "_" + i + ".txt");
                    using (var writer = new StreamWriter(splitFile))
                    {
                        int x = n;
                        if (r != 0)
                        {
                            ++x;
                            --r;
                        }
                        writer.WriteLine(x);
                        for (int j = 0; j < x; j++)
                        {
                            writer.WriteLine(reader.ReadLine());
                        }

                    }
                }
            }


        }
    }
}
