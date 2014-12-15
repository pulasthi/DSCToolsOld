using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bio;
using Bio.IO.FastA;
using Common;

namespace CenterPointNumberMap
{
    class Program
    {
        private static string _centerFilesDir;
        private static string _centerSequences;
        private static string _pointsFilesDir;
        private static string _outDir;
        private static string _indexFilesDir;

        static void Main(string[] args)
        {
            if (ParseArguments(args))
            {
                string outFile = Path.Combine(_outDir, "420_point_coordinates_from_megaregions.txt");

                using (FastAParser parser = new FastAParser(_centerSequences))
                {
                    IList<ISequence> seqs = parser.Parse().ToList();

                    // The point numbers in centerTable are local to particular megaregion
                    // Format: seqName->[region#,point#]
                    Hashtable centerTable = ReadCenterFiles(_centerFilesDir, _indexFilesDir);
                    // Format: region|-(point#->Point)
                    Hashtable[] pointsTable = ReadPointsFiles(_pointsFilesDir);

                    using (StreamWriter writer = new StreamWriter(outFile))
                    {
                        int count = 0;
                        foreach (ISequence sequence in seqs)
                        {
                            string name = sequence.ID.Substring(0, (sequence.ID.IndexOf("Fox") - 1)).Trim();
                            int[] arr = (int[]) centerTable[name];
                            Point p = (Point) pointsTable[arr[0]][arr[1]];
                            writer.WriteLine(count + "\t" + p.X + "\t" + p.Y + "\t" + p.Z + "\t" + 0);
                            ++count;
                        }
                    }

                    Console.WriteLine("Done.");
                    Console.Read();
                }


            }
        }

        private static Hashtable[] ReadPointsFiles(string pointsFilesDir)
        {
            string[] pointsFiles = Directory.GetFiles(pointsFilesDir);
            Hashtable[] pointsTable = new Hashtable[pointsFiles.Length];
            foreach (string pointsFile in pointsFiles)
            {
                string name = Path.GetFileNameWithoutExtension(pointsFile);
                int region = int.Parse(name.Substring(1, 1));
                pointsTable[region] = new Hashtable();
                using (SimplePointsReader reader = new SimplePointsReader(pointsFile))
                {
                    while (!reader.EndOfStream)
                    {
                        Point p = reader.ReadPoint();
                        pointsTable[region][p.Index] = p;
                    }
                }
            }
            return pointsTable;
        }

        private static Hashtable ReadCenterFiles(string centerFilesDir, string indexFilesDir)
        {
            string[] indexFiles = Directory.GetFiles(indexFilesDir);
            Hashtable[] indexTable = new Hashtable[indexFiles.Length];
            foreach (string indexFile in indexFiles)
            {
                string name = Path.GetFileNameWithoutExtension(indexFile);
                int region = int.Parse(name.Substring(1, 1));
                //Format: region#|-(seqName->pnum)
                indexTable[region] = new Hashtable();
                using (StreamReader reader = new StreamReader(indexFile))
                {
                    while (!reader.EndOfStream)
                    {
                        string[] splits = reader.ReadLine().Split('\t');
                        indexTable[region][splits[1].Trim()] = int.Parse(splits[0]);
                    }
                }
            }

            // The point numbers in centerTable are local to particular megaregion
            // Format: seqName->[region#,point#]
            Hashtable ht = new Hashtable();
            string [] centerFiles = Directory.GetFiles(centerFilesDir);
            foreach (string centerFile in centerFiles)
            {
                using (StreamReader reader = new StreamReader(centerFile))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line))
                        {
                            int idx1 = line.IndexOf("Sequence");
                            idx1 = line.IndexOf('=', idx1) + 1;
                            int idx2 = line.IndexOf("Length", idx1) - 1;
                            string seqName = line.Substring(idx1, (idx2 - idx1)).Trim();

                            int region = int.Parse(Path.GetFileNameWithoutExtension(centerFile).Substring(1, 1));
                            int pnum = (int) indexTable[region][seqName];
                            
                            if(!ht.ContainsKey(seqName))
                            {
                                ht[seqName] = new int[] {region, pnum};
                            }
                        }
                    }
                }
            }
            return ht;
        }

        private static bool ParseArguments(string[] args)
        {
            if (args.Length != 1 || !File.Exists(args[0]))
            {
                Console.WriteLine("Usage: CenterPointNumberMap.exe <config-file>");
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
                                case "CenterFilesDir":
                                    _centerFilesDir = value;
                                    break;
                                case "CenterSequences":
                                    _centerSequences = value;
                                    break;
                                case "PointsFilesDir":
                                    _pointsFilesDir = value;
                                    break;
                                case "IndexFilesDir":
                                    _indexFilesDir = value;
                                    break;
                                case "OutDir":
                                    _outDir = value;
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
