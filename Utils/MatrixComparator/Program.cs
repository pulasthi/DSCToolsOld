using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Salsa.Core;

namespace MatrixComparator
{
    class Program
    {
        static void Main(string[] args)
        {
            //Load the command line args into our helper class which allows us to name arguments
            var pargs = new Arguments(args)
                            {
                                Usage =
                                    "Usage: MatrixComparator.exe /matA=<string> /matB=<string> /outDir=<string> /bigc=<int>"                                    
                            };

            if (!pargs.CheckRequired(new[] { "matA", "matB", "outDir", "bigc"}))
            {
                Console.WriteLine(pargs.Usage);
                return;
            }

            short tsize = 2;
            int bigc = pargs.GetValue<int>("bigc");

            Int64 length = ((Int64) bigc)*bigc;

            using (StreamWriter outWriter = new StreamWriter(File.OpenWrite(pargs.GetValue<string>("outDir") + Path.DirectorySeparatorChar + "ineq.txt")))
            {
                outWriter.WriteLine("local-pnum\tmatA\tmatB\trow\tcol");
                using (BinaryReader matAs = new BinaryReader(File.OpenRead(pargs.GetValue<string>("matA"))))
                {
                    using (BinaryReader matBs = new BinaryReader(File.OpenRead(pargs.GetValue<string>("matB"))))
                    {
                        bool error = false;
                        Int16 a, b;
                        for (Int64 i = 0; i < length; i++)
                        {
                            a = matAs.ReadInt16();
                            b = matBs.ReadInt16();
                            if (a != b)
                            {
                                outWriter.WriteLine(i + "\t" + a + "\t" + b + "\t" + (i / bigc) + "\t" + (i % bigc));
                                //                            Console.WriteLine("Inequality at i=" + i + " i.e. (" + (i / bigc) + "," + (i % bigc) + ")");
                                //                            Console.WriteLine("  MatA: " + a + " -- MatB: " + b);
                                error = true;
                            }
                        }
                        if (!error)
                        {
                            Console.WriteLine("Good everything went well!");
                        }
                        else
                        {
                            Console.WriteLine("Not good :-/");
                        }
                        Console.Read();
                    }
                }
            }
        }
    }
}
