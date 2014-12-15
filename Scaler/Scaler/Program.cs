using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Scaler
{
    class Program
    {
        //todo: really dirty and clumsy code :D
        static void Main(string[] args)
        {
            string description = "Scales down PlotViz input by largest vector norm";
            string usage = "\nUsage:\n\n    Scaler.exe <plotviz file>";
            Console.WriteLine(description);
            if (args.Length < 1)
            {
                Console.WriteLine(usage);
                Console.Read();
                return;
            }


            TextReader vizReader = new StreamReader(args[0]);
            TextWriter vizWriter;
            
            string viz;
            string[] temp;
            double maxNorm = 0.0;
            double norm, x, y, z;
            Console.Write("\n    Running ...");
            while ((viz = vizReader.ReadLine()) != null)
            {                
                temp = viz.Split('\t');
                x = double.Parse(temp[1]);
                y = double.Parse(temp[2]);
                z = double.Parse(temp[3]);
                norm = x * x + y * y + z * z;
                if (norm > maxNorm)
                {
                    maxNorm = norm;
                }             
            }
            maxNorm = Math.Sqrt(maxNorm);

            vizReader.Close();

            StringBuilder sb = new StringBuilder();
            string str = string.Empty;
            vizReader = new StreamReader(args[0]);
            while ((viz = vizReader.ReadLine()) != null)
            {
                temp = viz.Split('\t');
                x = double.Parse(temp[1]);
                y = double.Parse(temp[2]);
                z = double.Parse(temp[3]);

                x /= maxNorm;
                y /= maxNorm;
                z /= maxNorm;
                
                sb.Append(temp[0]);
                sb.Append("\t");
                sb.Append(x);
                sb.Append("\t");
                sb.Append(y);
                sb.Append("\t");
                sb.Append(z);
                sb.Append("\t");
                sb.AppendLine(temp[4]);
            }
            vizReader.Close();

            vizWriter = new StreamWriter(args[0]);
            vizWriter.WriteLine(sb.ToString());
            vizWriter.Close();

            Console.WriteLine(" Done\n");
        }
    }
}
