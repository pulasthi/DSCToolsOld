using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace Splitter
{
    class Program
    {
        static void Main(string[] args)
        {
            string description = "Splitter";
            string usage = "\nUsage:\n\n    Replacer.exe <distance file>";
            Console.WriteLine(description);
            if (args.Length < 1)
            {
                Console.WriteLine(usage);
                Console.Read();
                return;
            }
            TextReader disReader = new StreamReader(args[0]);
            //TextWriter outWriter;
            //if (!File.Exists(args[1]))
            //{
            //    FileStream fs = File.Create(args[1]);
            //    fs.Close();
            //}

            //outWriter = new StreamWriter(args[1], false);

            string line;
            Console.WriteLine("\n    Running ...");
            int count = 0;
            while ((line = disReader.ReadLine()) != null)
            {
                //if (line.Split(new char[] { ',' }).Length != 35339)
                //{
                //    Console.WriteLine("\tLine number: " + count);                     
                //}
                count++;
            }
            Console.WriteLine("\tTotal lines - (0 to " + (count - 1) + "): " + count); ;
            Console.WriteLine(" Done\n");
            disReader.Close();
        }
    }
}
