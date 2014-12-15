using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Salsa.Core;

namespace GlobalPnumGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            //Load the command line args into our helper class which allows us to name arguments
            var pargs = new Arguments(args)
            {
                Usage =
                    "Usage: GlobalPnumGenerator.exe /badPnumsLocal=<string> /clusPointsGlobal=<string> /clusc=<int> /bigc=<int> /outDir"
            };

            if (!pargs.CheckRequired(new[] { "badPnumsLocal", "clusPointsGlobal", "clusc", "bigc", "outDir" }))
            {
                Console.WriteLine(pargs.Usage);
                return;
            }

            // Dictionary of <localpoint, globalpoint> for cluster. Also localpoint, globalpoint < int.MaxValue
            Dictionary<int,int> map = new Dictionary<int, int>();
            // number of points (x,y,z) per cluster is < int.MaxValue
            int count = 0;
            using (StreamReader reader = new StreamReader(File.OpenRead(pargs.GetValue<string>("clusPointsGlobal"))))
            {
                while(!reader.EndOfStream)
                {
                    map.Add(count, Int32.Parse(reader.ReadLine()));
                    count++;
                }
            }

            int clusc = pargs.GetValue<int>("clusc");
            int bigc = pargs.GetValue<int>("bigc");
            Int64 badPnumLocal;
            Int64 badPnumGlobal;
            using (StreamWriter outWriter = new StreamWriter(pargs.GetValue<string>("outDir")+Path.DirectorySeparatorChar+"badPnumsGlobal.txt"))
            {
                using (StreamReader badPnumsLocal = new StreamReader(File.OpenRead(pargs.GetValue<string>("badPnumsLocal"))))
                {
                    while (!badPnumsLocal.EndOfStream)
                    {
                        badPnumLocal = Int64.Parse(badPnumsLocal.ReadLine());
                        badPnumGlobal = ((Int64) map[((int) (badPnumLocal/clusc))])*bigc + map[((int) (badPnumLocal%clusc))];
                        outWriter.WriteLine(badPnumGlobal);
                    }
                }
            }
            Console.WriteLine("Done.");
            Console.Read();
        }
    }
}
