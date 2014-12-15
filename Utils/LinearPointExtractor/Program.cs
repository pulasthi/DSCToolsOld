using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Salsa.Core;

namespace LinearPointExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            //Load the command line args into our helper class which allows us to name arguments
            var pargs = new Arguments(args)
            {
                Usage =
                    "Usage: LinearPointExtractor.exe /mat=<string> /pnums=<string> /outDir=<string> /bigc=<int>"
            };

            if (!pargs.CheckRequired(new[] { "mat", "pnums", "outDir", "bigc" }))
            {
                Console.WriteLine(pargs.Usage);
                return;
            }
            Func<FileStream, Int16> decodeInt16 = delegate(FileStream fStream)
                                                      {
                                                          var bytes = new byte[2];
                                                          fStream.Read(bytes, 0, 2);
                                                          return ((Int16) ((bytes[1] << 8) | (bytes[0] & 0xff)));
                                                      };
            int tsize = 2;
            Int16 val;

            using (StreamWriter outWriter = new StreamWriter(File.OpenWrite(pargs.GetValue<string>("outDir") + Path.DirectorySeparatorChar + "originalvals.txt")))
            {
                outWriter.WriteLine("pnum\torignal");
                using (StreamReader pnums = new StreamReader(File.OpenRead(pargs.GetValue<string>("pnums"))))
                {
                    Int64 pnum;
                    Int64 readCount = 0;
                    Int64 skip;
                    using (FileStream fStream = File.OpenRead(pargs.GetValue<string>("mat")))
                    {
                        while (!pnums.EndOfStream)
                        {
                            pnum = Int64.Parse(pnums.ReadLine());
                            if (pnum > readCount)
                            {
                                skip = (pnum - readCount) * tsize;
                                fStream.Seek(skip, SeekOrigin.Current);
                                readCount += (pnum - readCount);
                            }
                            else if (pnum < readCount)
                            {
                                Console.WriteLine("weird");
                            }
                            val = decodeInt16(fStream);
                            readCount++;
                            outWriter.WriteLine(pnum + "\t" + val);
                        }
                    }
                }
            }
            Console.WriteLine("Done.");
            Console.Read();
        }
    }
}
