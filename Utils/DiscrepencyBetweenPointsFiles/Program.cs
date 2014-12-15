using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace DiscrepancyBetweenPointsFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            var referencePoints = args[0];
            var rotatedPoints = args[1];
            var res = int.Parse(args[2]);

            using (SimplePointsReader refReader = new SimplePointsReader(referencePoints), 
                rotReader = new SimplePointsReader(rotatedPoints))
            {
                var dir = Path.GetDirectoryName(rotatedPoints) ?? string.Empty;
                var runName = Path.GetFileName(dir);
                var histName = runName + "_3d_discrepancy_hist.txt";
                var plotName = runName + "_3d_discrepancy_hist_plot.txt";
                var plotFile = Path.Combine(dir, plotName);
                plotFile = plotFile.Length > 260 ? Path.Combine(dir, "3d_discrepancy_hist_plot.txt") : plotFile;
                var histFile = Path.Combine(dir, histName);
                histFile = histFile.Length > 260 ? Path.Combine(dir, "3d_discrepancy_hist.txt") : histFile;
                using (StreamWriter histogramWriter = new StreamWriter(histFile),
                    plotWriter = new StreamWriter(plotFile))
                {
                    var diffs = new List<double>();
                    double max = double.MinValue;
                    double min = double.MaxValue;
                    while (!refReader.EndOfStream)
                    {
                        var refPoint = refReader.ReadPoint();
                        var rotPoint = rotReader.ReadPoint();
                        var diff = refPoint.DistanceTo(rotPoint);
                        if (diff > max) max = diff;
                        if (diff < min) min = diff;
                        diffs.Add(diff);
                    }
                    double histXMin, histXMax;
                    var histogram = GeneralHistogram.MakeHistogram(diffs, min, max, res, out histXMin, out histXMax);
                    foreach (KeyValuePair<double, int> kv in histogram)
                    {
                        histogramWriter.WriteLine(kv.Key  +"\t" + kv.Value);
                    }

                   plotWriter.WriteLine("set terminal png truecolor nocrop font arial 10 size 800,800\n" +
                                        "set output '" +
                                        Path.Combine(dir, runName + "_discrepancy_hist.png") +
                                        "'\n" +
                                        "set size 1.0, 1.0\n" +
                                        "set multiplot layout 1,1 title \"" + runName + "\"\n" +
                                        "unset key\n" +
                                        "set xtics rotate by -90\n" +
                                        "set logscale y\n" +
                                        "set grid ytics lc rgb \"#bbbbbb\" lw 1 lt 0\n" +
                                        "set grid mytics lc rgb \"#bbbbbb\" lw 1 lt 0\n" +
                                        "set xlabel \"3D Discrepancy\" textcolor rgbcolor \"black\"\n" +
                                        "set ylabel \"Count in log scale\" textcolor rgbcolor \"black\"\n" +
                                        "plot [" + histXMin + ":" + histXMax + "][0.1:] '" + histFile +
                                        "' using 1:2 with filledcurves x1 lt rgb \"black\"\n");
                    histogramWriter.Flush();
                    histogramWriter.Close();
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
                p.WaitForExit();
                Console.WriteLine("Done.");

            }
        }
    }
}
