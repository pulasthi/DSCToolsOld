using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Salsa.Core;

namespace DistanceConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            //Load the command line args into our helper class which allows us to name arguments
            var pargs = new Arguments(args)
            {
                Usage =
                    "Usage: DistanceConverter.exe /file=<string> /from=<string> /colsize<int>"
            };

            if (!pargs.CheckRequired(new[] { "file", "from", "colsize"}))
            {
                Console.WriteLine(pargs.Usage);
                return;
            }

            long totalBytes = ((long) pargs.GetValue<int>("colsize"))*pargs.GetValue<int>("colsize")*2;
            string file = pargs.GetValue<string>("file");
            string outFile = Path.Combine(Path.GetDirectoryName(file),(Path.GetFileNameWithoutExtension(file) +
                             ("JAVA".Equals(pargs.GetValue<string>("from").ToUpper()) ? "_java2c#_" : "_c#2java_") +
                             Path.GetExtension(file)));
            using (FileStream inStream = new FileStream(file,FileMode.Open))
            {
                using (FileStream outStream = new FileStream(outFile, FileMode.CreateNew))
                {
                    byte head, tail;
                    int count = 0;
                    while (count < totalBytes)
                    {
                        head = (byte) inStream.ReadByte();
                        tail = (byte) inStream.ReadByte();
                        outStream.WriteByte(tail);
                        outStream.WriteByte(head);
                        count+= 2;
                    }
                    Console.WriteLine("Done writing");
                }
            }
            Console.Read();


        }
    }
}
