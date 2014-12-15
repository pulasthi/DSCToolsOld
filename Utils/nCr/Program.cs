using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace nCr
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader reader = new StreamReader(args[0]))
            {
                char[] sep = new[] {' ', '\t'};
                string[] headers = reader.ReadLine().Trim().Split(sep);

                string outPath = args[1];

                Hashtable writers = new Hashtable();
                using (StreamWriter scriptWriter = new StreamWriter(Path.Combine(outPath, "2Dscatterscript.txt")))
                {
                    scriptWriter.WriteLine(string.Format("set terminal png transparent nocrop enhanced font arial 8 size {0},{1}", args[2], args[3]));
                    scriptWriter.WriteLine("set dummy u,v");

                    for (int i = 0; i < headers.Length; i++)
                    {
                        for (int j = i + 1; j < headers.Length; j++)
                        {
                            string scatterDataFile = Path.Combine(outPath, headers[j] + "_vs_" + headers[i] + ".txt");
                            StreamWriter writer = new StreamWriter(scatterDataFile);
                            writer.WriteLine("# x:" + headers[i] + "\ty:" + headers[j]);
                            writers.Add(i.ToString() + j, writer);
                            
                            scriptWriter.WriteLine("set output '" + headers[j] + "_vs_" + headers[i] + ".png'");
                            scriptWriter.WriteLine("set xlabel '" + headers[i] + "'");
                            scriptWriter.WriteLine("set ylabel '" + headers[j] + "'");
                            scriptWriter.WriteLine("set title '" + headers[j] + " Vs " + headers[i] + "'");
                            scriptWriter.WriteLine("plot '" + Path.GetFileName(scatterDataFile) + "'");
                        }
                    }
                }

                double[] values;
                while (!reader.EndOfStream)
                {
                    values = reader.ReadLine().Trim().Split(sep).Select(x => double.Parse(x)).ToArray();
                    string istr;
                    for (int i = 0; i < values.Length; i++)
                    {
                        istr = i.ToString();
                        for (int j = i+1; j < values.Length; j++)
                        {
                            ((StreamWriter)writers[istr + j]).WriteLine(values[i] + "\t" + values[j]);
                        }
                    }
                }

                foreach (StreamWriter writer in writers.Values)
                {
                    writer.Flush();
                    writer.Close();
                    writer.Dispose();
                }
            }
        }
    }
}
