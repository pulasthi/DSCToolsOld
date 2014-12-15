using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Replacer
{
    class Program
    {
        static void Main(string[] args)
        {
            string description = "Replacer";
            string usage = "\nUsage:\n\n    Replacer.exe <distance file> <output file>";
            Console.WriteLine(description);
            if (args.Length < 2)
            {
                Console.WriteLine(usage);
                Console.Read();
                return;
            }
            TextReader disReader = new StreamReader(args[0]);
            TextWriter outWriter;
            if (!File.Exists(args[1]))
            {
                FileStream fs = File.Create(args[1]);
                fs.Close();                
            }
            
            outWriter = new StreamWriter(args[1], false);

            string line;
            Console.Write("\n    Running ...");
            while ((line = disReader.ReadLine()) != null)
            {
               outWriter.WriteLine(line.Replace(" ", string.Empty));                               
            }
            Console.WriteLine(" Done\n");
            disReader.Close();
            outWriter.Close();            
        }
    }
}
