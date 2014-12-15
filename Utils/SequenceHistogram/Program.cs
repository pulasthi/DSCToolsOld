using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Bio;
using Bio.IO.FastA;
using Salsa.Core;

namespace SequenceHistogram
{
    class Program
    {
        // NOTE. Works well for countFile case, didn't improve fastaFile case.
        static void Main(string[] args)
        {
            //Load the command line args into our helper class which allows us to name arguments
            var pargs = new Arguments(args)
            {
                Usage = "Usage: SequenceHistogram.exe [/fastaFile=<string> | /countFile=<string>] [/outDir=<string>] [/lengthCut=<int>] /binCount=<int>"
            };
            
            if (!(pargs.Contains("fastaFile") || pargs.Contains("countFile")) && pargs.CheckRequired(new[] {"binCount"}))
            {
                Console.WriteLine(pargs.Usage);
                Console.Read();
                return;
            }

            if (pargs.Contains("fastaFile"))
            {
                HistogramFasta(pargs);
            }
            else if (pargs.Contains("countFile"))
            {
                HistogramCounts(pargs);
            }
            Console.WriteLine("Done.");
//            Console.Read();
        }

        private static void HistogramCounts(Arguments pargs)
        {
            var isLengthCut = pargs.Contains("lengthCut");
            var lengthCut = isLengthCut ? pargs.GetValue<int>("lengthCut") : int.MaxValue;
            var countFile = pargs.GetValue<string>("countFile");
            var binCount = pargs.GetValue<int>("binCount");

            var max = int.MinValue;
            var min = int.MaxValue;

            var lengths = new List<int>();
            
            long lengthSum = 0;
            using (var reader = new StreamReader(countFile))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line)) continue;
                   
                    var splits = line.Split('\t');
                    
                    if (splits.Length != 3) // Not in index file format
                    {
                        Console.WriteLine("Bad Line: " + line);
                        return;
                    }

                    int length;
                    if (!Int32.TryParse(splits[2], out length))
                    {
                        Console.WriteLine("Unable to parse: " + line);
                        return;
                    }

                    if (length > lengthCut) continue; // Not within length cut

                    lengths.Add(length);
                    lengthSum += length;
                    max = length > max ? length : max;
                    min = length < min ? length : min;
                }
                Console.WriteLine("Average length: " + ((double)lengthSum)/lengths.Count);
                Console.Read();
                var binSize = (max - min)/(double)binCount;
//                var ht = new Dictionary<double, int>();
                var ht = new SortedList<double, int>();
                foreach (var length in lengths)
                {
                    var steps = Math.Floor((length-min)/binSize);
                    var bin = min + steps*binSize;
                    if (ht.ContainsKey(bin))
                    {
                        ++ht[bin];
                    }
                    else
                    {
                        ht.Add(bin,1);
                    }
                }

                string outDir = pargs.Contains("outDir") ?  pargs.GetValue<string>("outDir") : (Path.GetDirectoryName(countFile) ?? string.Empty);
                string outFile = Path.Combine(outDir,
                                              countFile + (isLengthCut ? "_length_lte" + lengthCut : "") +
                                              "_histogram.txt");
//                string logOutFile = Path.Combine(outDir, countFile + (isLengthCut ? "_length_lte" + lengthCut : "") + "_log_histogram.txt");
//                PrintHt(ht, outFile, logOutFile);
                PrintHt(ht,outFile);
                GeneratePlotPlot(outFile, min, max);
            }
        }

        private static void PrintHt(SortedList<double, int> ht, string outFile)
        {
            using (var writer = new StreamWriter(outFile))
            {
                foreach (var key in ht.Keys)
                {
                    writer.WriteLine(key + "\t" + ht[key]);
                }
            }
        }

        private static void PrintHt(Dictionary<double,int> ht, string file, int min, int max, double binSize)
        {
            using (var writer = new StreamWriter(file))
            {
                double i = min;
                while (i <= max)
                {
                    if (!ht.ContainsKey(i)) continue;
                    writer.WriteLine(i + "\t" + ht[i]);
                    i += binSize;
                }
            }
        }

        private static void GeneratePlotPlot(string file, double xmin, double xmax)
        {
            // NOTE. Hack for Larry's sequence data
            string prefix = Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(file)));

            string dir = Path.GetDirectoryName(file) ?? string.Empty;
            var orignalName = Path.GetFileNameWithoutExtension(file);
            string name = "plot_" + orignalName + ".txt";
            string plotFile = Path.Combine(dir, name);

            using (var writer = new StreamWriter(plotFile))
            {
                writer.WriteLine("set terminal png truecolor nocrop font arial 10 size 800,800\n" +
                                 "set output '" + Path.Combine(dir, Path.GetFileNameWithoutExtension(plotFile) + ".png") +
                                 "'\n" +
                                 // NOTE. Hack for Larry's sequence data
                                 //                                 "set output '" + Path.Combine(dir,  prefix +"-" +Path.GetFileNameWithoutExtension(plotFile) +".png") + "'\n" +
                                 "set size 1.0, 1.0\n" +
                                 "set multiplot layout 1,1 title \"" + orignalName + "\"\n" +
                                 // NOTE. Hack for Larry's sequence data
                                 //                                 "set multiplot layout 1,1 title \""+ prefix + "-" + orignalName  +"\"\n" +
                                 "unset key\n" +
                                 "set xtics rotate by -90\n" +
                                 "set logscale y\n" +
                                 "set grid ytics lc rgb \"#bbbbbb\" lw 1 lt 0\n" +
                                 "set grid mytics lc rgb \"#bbbbbb\" lw 1 lt 0\n" +
                                 "set xlabel \"Sequence Length\" textcolor rgbcolor \"black\"\n" +
                                 "set ylabel \"Count in log scale\" textcolor rgbcolor \"black\"\n" +
                                 "plot [:" + xmax + "][] '" + file + "' using 1:2 with filledcurves x1 lt rgb \"black\"\n");
            }
            var start = new ProcessStartInfo(@"C:\Program Files (x86)\gnuplot\bin\gnuplot.exe")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                Arguments = "\"" + plotFile + "\""
            };
            Process p = Process.Start(start);
            string output = p.StandardOutput.ReadToEnd();
            string err = p.StandardError.ReadToEnd();
            Console.WriteLine(output);
            Console.WriteLine(err);

        }

        private static void HistogramFasta(Arguments pargs)
        {
            bool isLengthCut = pargs.Contains("lengthCut");
            int lengthCut = isLengthCut ? pargs.GetValue<int>("lengthCut") : int.MaxValue;
            string fastaFile = pargs.GetValue<string>("fastaFile");
            string fastaName = Path.GetFileNameWithoutExtension(fastaFile);

            IList<ISequence> seqs;

            using (FastAParser parser = new FastAParser(fastaFile))
            {
                seqs = parser.Parse().ToList();
            } 

            string outDir = pargs.Contains("outDir")
                                ? pargs.GetValue<string>("outDir")
                                : (Path.GetDirectoryName(fastaFile) ?? string.Empty);
            string outFile = Path.Combine(outDir, fastaName + (isLengthCut ? "_length_lte" + lengthCut : "") + "_histogram.txt");
            string logOutFile = Path.Combine(outDir, fastaName + (isLengthCut ? "_length_lte" + lengthCut : "") + "_log_histogram.txt");

            SortedList<long,long> ht = new SortedList<long, long>();
            long seqLen;
            foreach (ISequence seq in seqs)
            {
                seqLen = seq.Count;
                if (seqLen <= lengthCut)
                {
                    if (ht.ContainsKey(seqLen))
                    {
                        ht[seqLen] = (ht[seqLen]) + 1;
                    }
                    else
                    {
                        ht.Add(seqLen, 1);
                    }
                }
            }

            PrintHt(ht,outFile,logOutFile);
//            GeneratePlotPlot(outFile);
        }

        private static void PrintHt(SortedList<long,long> ht, string outFile, string logOutFile)
        {
            using (StreamWriter writer = new StreamWriter(outFile), logWriter = new StreamWriter(logOutFile))
            {
                foreach (var key in ht.Keys)
                {
                    long count = ht[key];
                    writer.WriteLine(string.Format("{0}\t{1}", key, count));
                    logWriter.WriteLine(string.Format("{0}\t{1}", key, Math.Log(count,10.0)));
                }
            }
        }
    }
}
