using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace _10d3dDelta
{
    class Program
    {
        static void Main(string[] args)
        {
            string original = args[0];
            string threeD = args[1];
            string tenD = args[2];
            int size = int.Parse(args[3]);

            string dir = args[4];
            int res = int.Parse(args[5]);         
            

            const string deltaTenDThreeDSuffix = "delta.10d.3d.txt";
            const string deltaTenDOriginalSuffix = "delta.10d.original.txt";
            const string deltaThreeDOriginalSuffix = "delta.3d.original.txt";

            using (MatrixReader originalReader = new MatrixReader(original, MatrixType.Int16, size), 
                threeDReader = new MatrixReader(threeD, MatrixType.Int16, size),
                tenDReader = new MatrixReader(tenD, MatrixType.Int16, size))
            {
                using (StreamWriter tenDThreeDHistWriter = new StreamWriter(Path.Combine(dir, "hist." + deltaTenDThreeDSuffix)), 
                    tenDOriginalHistWriter = new StreamWriter(Path.Combine(dir, "hist." + deltaTenDOriginalSuffix)),
                    threeDOriginalHistWriter = new StreamWriter(Path.Combine(dir, "hist." + deltaThreeDOriginalSuffix)),
                    tenDThreeDDeltaWriter = new StreamWriter(Path.Combine(dir, "delta." + deltaTenDThreeDSuffix)),
                    tenDOriginalDeltaWriter = new StreamWriter(Path.Combine(dir, "delta." + deltaTenDOriginalSuffix)),
                    threeDOriginalDeltaWriter = new StreamWriter(Path.Combine(dir, "delta." + deltaThreeDOriginalSuffix)))

                {
                    IList<double> avgDeltaTenDThreeDList = new List<double>(size);
                    IList<double> avgDeltaTenDOriginalList = new List<double>(size);
                    IList<double> avgDeltaThreeDOriginalList = new List<double>(size);
                    for (int i = 0; i < size; i++)
                    {
                        double avgDeltaTenDThreeD = 0.0, avgDeltaTenDOriginal = 0.0, avgDeltaThreeDOriginal = 0.0;
                        for (int j = 0; j < size; j++)
                        {
                            if (i != j)
                            {
                                double originalDist = ((double) BitConverter.ToInt16(originalReader.Read(i, j), 0))/
                                                      Int16.MaxValue;
                                double threeDDist = ((double) BitConverter.ToInt16(threeDReader.Read(i, j), 0))/
                                                    Int16.MaxValue;
                                double tenDDist = ((double) BitConverter.ToInt16(tenDReader.Read(i, j), 0))/
                                                  Int16.MaxValue;

                                avgDeltaTenDThreeD += Math.Abs(tenDDist - threeDDist)/Math.Abs(tenDDist + threeDDist);
                                avgDeltaTenDOriginal += Math.Abs(tenDDist - originalDist)/
                                                        Math.Abs(tenDDist + originalDist);
                                avgDeltaThreeDOriginal += Math.Abs(threeDDist - originalDist)/
                                                          Math.Abs(threeDDist + originalDist);
                            }
                        }

                        avgDeltaTenDThreeD /= (size - 1);
                        avgDeltaTenDOriginal /= (size - 1);
                        avgDeltaThreeDOriginal /= (size - 1);

                        tenDThreeDDeltaWriter.WriteLine(i +"\t"  +avgDeltaTenDThreeD);
                        tenDOriginalDeltaWriter.WriteLine(i +"\t"  +avgDeltaTenDOriginal);
                        threeDOriginalDeltaWriter.WriteLine(i +"\t"  +avgDeltaThreeDOriginal);

                        avgDeltaTenDThreeDList.Add(avgDeltaTenDThreeD);
                        avgDeltaTenDOriginalList.Add(avgDeltaTenDOriginal);
                        avgDeltaThreeDOriginalList.Add(avgDeltaThreeDOriginal);

                        Console.WriteLine("done row " + i);
                    }

                    Dictionary<double,int> histDeltaTenDThreeD = GeneralHistogram.MakeHistogram(avgDeltaTenDThreeDList,
                                                                                   avgDeltaTenDThreeDList.Min(),
                                                                                   avgDeltaTenDThreeDList.Max(), res);

                    Dictionary<double, int> histDeltaTenDOriginal = GeneralHistogram.MakeHistogram(avgDeltaTenDOriginalList,
                                                                                   avgDeltaTenDOriginalList.Min(),
                                                                                   avgDeltaTenDOriginalList.Max(), res);

                    Dictionary<double, int> histDeltaThreeDOriginal = GeneralHistogram.MakeHistogram(avgDeltaThreeDOriginalList,
                                                                                   avgDeltaThreeDOriginalList.Min(),
                                                                                   avgDeltaThreeDOriginalList.Max(), res);
                    WriteHist(histDeltaTenDThreeD, tenDThreeDHistWriter);
                    WriteHist(histDeltaTenDOriginal, tenDOriginalHistWriter);
                    WriteHist(histDeltaThreeDOriginal, threeDOriginalHistWriter);
                }

                using (StreamWriter plotWriter = new StreamWriter(Path.Combine(dir, "delta.hist.plot.txt")))
                {

                    plotWriter.WriteLine(@"set terminal png truecolor nocrop font arial 10 size 1200,400 xffffff");
                    plotWriter.WriteLine(@"set output 'delta-hist-plot.png'");
                    plotWriter.WriteLine(@"set size 1.0, 1.0");
                    plotWriter.WriteLine("set multiplot layout 1,3 title \"Delta Histograms\"");
                    plotWriter.WriteLine(@"unset key");
                    plotWriter.WriteLine("set ylabel \"Count\" textcolor rgbcolor \"black\"");
                    plotWriter.WriteLine("set xlabel \"Delta(10D-3D)\" textcolor rgbcolor \"black\"");
                    plotWriter.WriteLine("set title \"Histogram of Delta(10D-3D)\" textcolor rgbcolor \"black\"");
                    plotWriter.WriteLine("plot [][] 'hist." + deltaTenDThreeDSuffix + "' using 1:2 with filledcurves x1 lt rgb \"black\"");
                    plotWriter.WriteLine("set xlabel \"Delta(10D-PID)\" textcolor rgbcolor \"black\"");
                    plotWriter.WriteLine("set title \"Histogram of Delta(10D-PID)\" textcolor rgbcolor \"black\"");
                    plotWriter.WriteLine("plot [][] 'hist." + deltaTenDOriginalSuffix + "' using 1:2 with filledcurves x1 lt rgb \"black\"");
                    plotWriter.WriteLine("set xlabel \"Delta(3D-PID)\" textcolor rgbcolor \"black\"");
                    plotWriter.WriteLine("set title \"Histogram of Delta(3D-PID)\" textcolor rgbcolor \"black\"");
                    plotWriter.WriteLine("plot [][] 'hist." + deltaThreeDOriginalSuffix + "' using 1:2 with filledcurves x1 lt rgb \"black\"");
                }
                Console.WriteLine("done.");
                Console.Read();
            }
        }

        private static void WriteHist(Dictionary<double, int> hist, StreamWriter writer)
        {
            foreach (KeyValuePair<double, int> kv in hist)
            {
                writer.WriteLine(kv.Key + "\t" + kv.Value);
            }
        }
    }
}
