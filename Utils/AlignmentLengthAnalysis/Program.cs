using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bio;
using Bio.IO.FastA;
using Common;

namespace AlignmentLengthAnalysis
{
    class Program
    {
        private static string _distanceFile;
        private static string _alignmentLengthBinaryFile;
        private static string _outDirectory;
        private static string _sequenceFile;
        private static double _min;
        private static double _max;
        private static bool _maxInclusive;
        private static bool _minInclusive;
        private static int _size;
        private static int _xres;

        static void Main(string[] args)
        {
            if (ParseArguments(args))
            {
                using (MatrixReader dreader = new MatrixReader(_distanceFile, MatrixType.Int16, _size),
                    areader = new MatrixReader(_alignmentLengthBinaryFile, MatrixType.Int16, _size))
                {
                    using (FastAParser parser = new FastAParser(_sequenceFile))
                    {
                        IList<ISequence> seqs = parser.Parse().ToList();
                        using (StreamWriter writer = new StreamWriter(Path.Combine(_outDirectory, "alignment_length_analysis.txt")))
                        {
                            writer.WriteLine("#length1\tcount\tlength2\tcount\tminlength\tcount\tmaxlength\tcount\tdiff\tcount\talignmentlength\tcount\tdistance\tcount");
                            List<double > len1 = new List<double>();
                            List<double > len2 = new List<double>();
                            List<double > minlen = new List<double>();
                            List<double > maxlen = new List<double>();
                            List<double > difflen = new List<double>();
                            List<double > alignlen = new List<double>();
                            List<double > dist = new List<double>();

                            long count = 0;
                            for (int i = 0; i < _size; i++)
                            {
                                for (int j = 0; j < _size; j++)
                                {
                                    double d = ((double) BitConverter.ToInt16(dreader.Read(i, j), 0))/Int16.MaxValue;
                                    if (DistanceInRange(d))
                                    {
                                        ++count;
                                        /*long l1 = seqs[i].Count;
                                        long l2 = seqs[j].Count;
                                        long minl1l2 = Math.Min(l1, l2);
                                        long maxl1l2 = Math.Max(l1, l2);
                                        int al = BitConverter.ToInt16(areader.Read(i, j), 0);
//                                        writer.WriteLine(l1 + "\t" + l2 + "\t" + minl1l2 + "\t" + maxl1l2 + "\t" + (maxl1l2-minl1l2) +"\t" + al +"\t" + d);
                                        len1.Add(l1);
                                        len2.Add(l2);
                                        minlen.Add(minl1l2);
                                        maxlen.Add(maxl1l2);
                                        difflen.Add(maxl1l2-minl1l2);
                                        alignlen.Add(al);
                                        dist.Add(d);*/
                                    }
                                }
                            }

                            Console.WriteLine("Distances in range: " + count);
                            return;

                            Dictionary<double, int> histLen1 = GeneralHistogram.MakeHistogram(len1, len1.Min(),len1.Max(), _xres);
                            Dictionary<double, int> histLen2 = GeneralHistogram.MakeHistogram(len2, len2.Min(),len2.Max(), _xres);
                            Dictionary<double, int> histMinLen = GeneralHistogram.MakeHistogram(minlen, minlen.Min(),minlen.Max(), _xres);
                            Dictionary<double, int> histMaxLen = GeneralHistogram.MakeHistogram(maxlen, maxlen.Min(),maxlen.Max(), _xres);
                            Dictionary<double, int> histDiffLen = GeneralHistogram.MakeHistogram(difflen, difflen.Min(),difflen.Max(), _xres);
                            Dictionary<double, int> histAlignLen = GeneralHistogram.MakeHistogram(alignlen, alignlen.Min(),alignlen.Max(), _xres);
                            Dictionary<double, int> histDist = GeneralHistogram.MakeHistogram(dist, dist.Min(),dist.Max(), _xres);

                            for (int i = 0; i < _xres; i++)
                            {
                                KeyValuePair<double, int> histLen1Kv = histLen1.ElementAt(i);
                                KeyValuePair<double, int> histLen2Kv = histLen2.ElementAt(i);
                                KeyValuePair<double, int> histMinLenKv = histMinLen.ElementAt(i);
                                KeyValuePair<double, int> histMaxLenKv = histMaxLen.ElementAt(i);
                                KeyValuePair<double, int> histDiffLenKv = histDiffLen.ElementAt(i);
                                KeyValuePair<double, int> histAligneLenKv = histAlignLen.ElementAt(i);
                                KeyValuePair<double, int> histDistKv = histDist.ElementAt(i);

                                writer.WriteLine(histLen1Kv.Key+"\t"+histLen1Kv.Value+"\t"+
                                    histLen2Kv.Key+"\t"+histLen2Kv.Value+"\t"+
                                    histMinLenKv.Key+"\t"+histMinLenKv.Value+"\t"+
                                    histMaxLenKv.Key+"\t"+histMaxLenKv.Value+"\t"+
                                    histDiffLenKv.Key+"\t"+histDiffLenKv.Value+"\t"+
                                    histAligneLenKv.Key+"\t"+histAligneLenKv.Value+"\t"+
                                    histDistKv.Key+"\t"+histDistKv.Value);
                            }
                        }
                    }
                }
            }
        }

        private static bool DistanceInRange(double d)
        {
            return ((d > _min || (_minInclusive && d == _min)) && (d < _max || (_maxInclusive && d == _max)));
        }

        private static bool ParseArguments(string[] args)
        {
            if (args.Length != 1 || !File.Exists(args[0]))
            {
                Console.WriteLine("Usage: AlignmentLengthAnalysis.exe <config-file>");
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
                                case "DistanceFile":
                                    _distanceFile = value;
                                    break;
                                case "AlignmentLengthBinaryFile":
                                    _alignmentLengthBinaryFile = value;
                                    break;
                                case "OutDirectory":
                                    _outDirectory = value;
                                    break;
                                case "SequenceFile":
                                    _sequenceFile = value;
                                    break;
                                case "DistanceMin":
                                    _min = double.Parse(value);
                                    break;
                                case "DistanceMax":
                                    _max = double.Parse(value);
                                    break;
                                case "MaxInclusive":
                                    _maxInclusive = bool.Parse(value);
                                    break;
                                case "MinInclusive":
                                    _minInclusive = bool.Parse(value);
                                    break;
                                case "Size":
                                    _size = int.Parse(value);
                                    break;
                                case "xres":
                                    _xres = int.Parse(value);
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
    }
}
