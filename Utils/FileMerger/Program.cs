using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Salsa.Core;

namespace FileMerger
{
    class Program
    {
        static void Main(string[] args)
        {
            //Load the command line args into our helper class which allows us to name arguments
            var pargs = new Arguments(args)
            {
                Usage = "Usage: FilerMerger.exe /dir=<string> /pattern=<string>"
            };

            if (!pargs.CheckRequired(new[] { "dir", "pattern" }))
            {
                Console.WriteLine(pargs.Usage);
                Console.Read();
                return;
            }

            string dir = pargs.GetValue<string>("dir");
            string pattern = pargs.GetValue<string>("pattern");

            // Note. doesn't preserve any order here
            string[] files = Directory.GetFiles(dir, pattern);

            string outfile = Path.Combine(dir, "merged.txt");
            string line;
            using (StreamWriter writer = new StreamWriter(outfile))
            {
                for (int i = 0; i < files.Length; i++)
                {
                    using (StreamReader reader = new StreamReader(files[i]))
                    {
                        Console.WriteLine("Using file: " + Path.GetFileNameWithoutExtension(files[i]));
                        while (!reader.EndOfStream)
                        {
                            line = reader.ReadLine();
//                            if (line.Split('\t').Length != 5)
//                            {
//                                Console.WriteLine(Path.GetFileNameWithoutExtension(files[i]) + "\n" + line);
//                            }
                            writer.WriteLine(line);
                        }
                    }

                }
                writer.Flush();                
            }
          

            Console.WriteLine("good");
            Console.Read();
        }
    }
}
