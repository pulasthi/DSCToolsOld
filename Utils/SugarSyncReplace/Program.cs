using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SugarSyncReplace
{
    class Program
    {
        static void Main(string[] args)
        {
            const string dir = @"C:\wamp\www\millionseq\haixu";
            string outDir = Path.Combine(dir, "fixed");
            string[] files = Directory.GetFiles(dir, "*.html");
            foreach (string file in files)
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    string outFile = Path.Combine(outDir, Path.GetFileName(file));
                    using (StreamWriter writer = new StreamWriter(outFile))
                    {
                        string line, modified;
                        while (!reader.EndOfStream)
                        {
                            line = reader.ReadLine();
                            modified = SearchAndReplace(line);
                            writer.WriteLine(modified);
                        }
                    }
                }
            }
            Console.WriteLine("done.");
            Console.ReadKey();
        }

        private const string Common = @"https://www.sugarsync.com/pf/";
        private const string DirectDownload = @"?directDownload=true";

        private static string SearchAndReplace(string line)
        {
            
            if (line.Contains(Common))
            {
                
                int start = line.IndexOf(Common);
                int end = line.IndexOf("\"", start);
                string prefix = line.Substring(0, start);
                string url = line.Substring(start, (end - start));
                string suffix = line.Substring(end);
                if (!url.EndsWith(DirectDownload))
                {
                    return prefix + url + DirectDownload + suffix;
                }
            }
            return line;
        }
    }
}
