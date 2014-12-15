using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Salsa.Core;

namespace DistanceHistogram
{
    class Program
    {
        private static string _matFile;
        private static string _outdir;
        private static Range[] _rowranges;
        private static Range[] _colranges;
        private static int _xres;
        private static int _rows;
        private static int _cols;
        private static string _title;

        /* Produce distance histograms for each pair of row and column range */
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: mpiexec -np N DistanceHistogram.exe <path-to-params-file>");
                return;
            }

            using (new MPI.Environment(ref args))
            {
                ReadConfiguration(args[0]);
            }
        }

        private static void ReadConfiguration(string configFile)
        {
            /* Reading parameters file */
            using (var reader = new StreamReader(configFile))
            {
                var sep = new[] { '\t' };
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
                            if (string.IsNullOrEmpty(value)) throw new Exception("Empty value for " + splits[0]);
                            switch (splits[0])
                            {
                                case "mat":
                                    _matFile = value;
                                    break;
                                case "outdir":
                                    _outdir = value;
                                    break;
                                case "rowmask":
                                    _rowranges = ParseRanges(value);
                                    break;
                                case "colmask":
                                    _colranges = ParseRanges(value);
                                    break;
                                case "rows":
                                    _rows = int.Parse(value);
                                    break;
                                case "cols":
                                    _cols = int.Parse(value);
                                    break;
                                case "xres":
                                    _xres = int.Parse(value);
                                    break;
                                case "title":
                                    _title = value;
                                    break;
                                default:
                                    throw new Exception("Invalide line in configuration file: " + line);
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid line in configuration file: " + line);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Parse a comma separated list of ranges into an array of <code>Range</code> objects
        /// </summary>
        /// <param name="csranges">Comma separated list of ranges. A range is defined as m-n including indices m and n</param>
        /// <returns>An array <code>Range</code> objects</returns>
        private static Range[] ParseRanges(string csranges)
        {
            var csep = new[] {','};
            var hsep = new[] {'-'};
            var ranges = csranges.Split(csep, StringSplitOptions.RemoveEmptyEntries);
            return ranges.Select(r =>
                              {
                                  var arr = r.Split(hsep, StringSplitOptions.RemoveEmptyEntries);
                                  return new Range(int.Parse(arr[0]), int.Parse(arr[1]));
                              }).ToArray();

        }
    }
}
