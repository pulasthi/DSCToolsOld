using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common;

namespace MaxDistceOfCenters
{
    class Program
    {
        private static string _pointsFile;
        private static string _clusterFile;
        private static string _outDirectory;

        private static int _clusterCount = 120;
        private static int _centersPerCluster = 3;
        private static int _classifications = 3;

        private static int _xres = 20;

        private static string _name;

        static void Main(string[] args)
        {
            if (ParseArguments(args))
            {
                double[] maxArray = new double[_classifications];
                double[] minArray = new double[_classifications];

                Hashtable[] hist = new Hashtable[_classifications];
                for (int i = 0; i < _classifications; i++)
                {
                    hist[i] = new Hashtable();
                    maxArray[i] = double.MinValue;
                    minArray[i] = double.MaxValue;
                }

                using (StreamReader creader = new StreamReader(_clusterFile))
                {
                    using (SimplePointsReader preader = new SimplePointsReader(_pointsFile))
                    {
                        for (int i = 0; i < _clusterCount; i++)
                        {
                            Point[] points = new Point[_centersPerCluster];
                            string line = string.Empty;
                            for (int j = 0; j < _centersPerCluster; j++)
                            {
                                points[j] = preader.ReadPoint();
                                line = creader.ReadLine();
                            }

                            int clusterClassificaiton = int.Parse(line.Split('\t')[1]);
                            double max = MaxDist(points);
                            Hashtable ht = hist[clusterClassificaiton];
                            ht[max] = ht.Contains(max) ? ((int) ht[max]) + 1 : 1;
                            maxArray[clusterClassificaiton] = max > maxArray[clusterClassificaiton]
                                                                  ? max
                                                                  : maxArray[clusterClassificaiton];
                            minArray[clusterClassificaiton] = max < minArray[clusterClassificaiton]
                                                                  ? max
                                                                  : minArray[clusterClassificaiton];
                        }
                    }
                }

                for (int i = 0; i < _classifications; i++)
                {
                    double min = minArray[i];
                    double max = maxArray[i];
                    Hashtable ht = hist[i];
                    int[] cells = new int[_xres];
                    for (int j = 0; j < _xres; j++)
                    {
                        cells[j] = 0;
                    }
                    double deltax = (max - min)/_xres;
                    foreach (DictionaryEntry kv in ht)
                    {
                        double x = (double) kv.Key;
                        int cell = x == max ? _xres - 1 : (int)Math.Floor((x - min) / deltax);
                        cells[cell] = cells[cell] + (int) kv.Value;
                    }
                    using (StreamWriter writer = new StreamWriter(Path.Combine(_outDirectory, "center-hist-" + i + ".txt")))
                    {
                        double xoffset = min + 0.5 * deltax;
                        for (int j = 0; j < _xres; ++j)
                        {
                            double xcoord = xoffset + j * deltax;
                            writer.WriteLine(xcoord + "\t" + cells[j]);
                        }
                    }
                }

                // Note. not all lines are general for any number of classifications
                using (StreamWriter writer = new StreamWriter(Path.Combine(_outDirectory, "center-hist-plot.txt")))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("set terminal png truecolor nocrop font arial 10 size 1200,400 xffffff");
                    sb.AppendLine("set output 'center-hist-plot.png'");
                    sb.AppendLine("set size 1.0, 1.0");
                    sb.AppendLine("set multiplot layout 1,3 title \"" + _name + "\"");
                    sb.AppendLine("unset key");

                    sb.AppendLine("set xlabel \"Max Distance\" textcolor rgbcolor \"black\"");
                    sb.AppendLine("set ylabel \"Count\" textcolor rgbcolor \"black\"");

                    string[] names = new string[]{"-Good", "-PartOfComplicatedStructure", "-Debris"};
                    for (int i = 0; i < _classifications; i++)
                    {
                        sb.AppendLine("set title \"Histogram of Max 3D Distance for class" + i + names[i] +
                                      "\" textcolor rgbcolor \"black\"");
                        sb.AppendLine("plot [][] 'center-hist-" + i +
                                      ".txt' using 1:2 with filledcurves x1 lt rgb \"black\"");
                    }
                    writer.WriteLine(sb.ToString());
                }
            }
        }

        private static bool ParseArguments(string[] args)
        {
            if (args.Length != 1 || !File.Exists(args[0]))
            {
                Console.WriteLine("Usage: MaxDistceOfCenters.exe <config-file>");
                return false;
            }

            /* Reading parameters file */
            using (StreamReader reader = new StreamReader(args[0]))
            {
                char[] sep = new[] { ' ', '\t' };
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    // Skip null/empty and comment lines
                    if (!string.IsNullOrEmpty(line) && !line.StartsWith("#"))
                    {
                        string[] splits = line.Trim().Split(sep);
                        if (splits.Length >= 2)
                        {
                            string value = splits[1];
                            switch (splits[0])
                            {
                                case "PointsFile":
                                    _pointsFile = value;
                                    break;
                                case "ClusterFile":
                                    _clusterFile = value;
                                    break;
                                case "OutDirectory":
                                    _outDirectory = value;
                                    break;
                                case "ClusterCount":
                                    _clusterCount = int.Parse(value);
                                    break;
                                case "CentersPerCluster":
                                    _centersPerCluster = int.Parse(value);
                                    break;
                                case "Classifications":
                                    _classifications = int.Parse(value);
                                    break;
                                case "xres":
                                    _xres = int.Parse(value);
                                    break;
                                case "Name":
                                    _name = value;
                                    break;
                                default:
                                    throw new Exception("Invalide line configuration file: " + line);
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid line in configuration file: " + line);
                        }

                    }
                }
            }

            return true;
        }

        static double MaxDist(Point[] points)
        {
            double max = 0.0;
            for (int i = 0; i < points.Length; i++)
            {
                for (int j = i + 1; j < points.Length; j++)
                {
                    double d = Dist(points[i], points[j]);
                    if (d > max)
                    {
                        max = d;
                    }
                }
            }
            return max;
        }

        private static double Dist(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2) + Math.Pow(p1.Z - p2.Z, 2));
        }
    }
}
