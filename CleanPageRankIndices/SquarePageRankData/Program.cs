using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquarePageRankData
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = @"C:\Users\sekanaya\Downloads\pagerankdata\en0000-01and02_reset_idx.am";
            string sqfile = @"C:\Users\sekanaya\Downloads\pagerankdata\en0000-01and02_reset_idx_and_square.am";
            using (var reader = new StreamReader(file))
            {
                using (var writer = new StreamWriter(sqfile))
                {
                    int totalUrls = int.Parse(reader.ReadLine());
                    writer.WriteLine(totalUrls);

                    var fromUrlToLine = new Dictionary<int, string>();
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var idx = line.IndexOf(' ');
                        var fromUrl = int.Parse(line.Substring(0, idx));
                        fromUrlToLine.Add(fromUrl, line);
                    }

                    for (int i = 0; i < totalUrls; i++)
                    {
                        if (fromUrlToLine.ContainsKey(i))
                        {
                            writer.WriteLine(fromUrlToLine[i]);
                        }
                        else
                        {
                            writer.WriteLine(i);
                        }
                    }
                }
            }
        }
    }
}
