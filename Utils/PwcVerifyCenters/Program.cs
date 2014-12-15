using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PwcVerifyCenters
{
    class Program
    {
        static void Main(string[] args)
        {
//            string file = @"C:\Users\sekanaya\Desktop\junkmatrix\centers\mina_6822\distance_6822.bin";
            string file = args[0];
//            string pointsFile = @"C:\Users\sekanaya\Desktop\junkmatrix\centers\mina_6822\SIMPLEpoints.txt";
            string pointsFile = args[1];
            int size = 6822;

            Hashtable cnumToPnums = new Hashtable();
            Hashtable pnumToCnum = new Hashtable(size);

            using (SimplePointsReader pointsReader = new SimplePointsReader(pointsFile))
            {
                while (!pointsReader.EndOfStream)
                {
                    Point p = pointsReader.ReadPoint();
                    pnumToCnum.Add(p.Index, p.Cluster);
                    if (cnumToPnums.ContainsKey(p.Cluster))
                    {
                        ((IList<int>)cnumToPnums[p.Cluster]).Add(p.Index);
                    }
                    else
                    {
                        cnumToPnums.Add(p.Cluster, new List<int> {p.Index});
                    }
                }
            }

            using (MatrixReader reader = new MatrixReader(file, MatrixType.Int16, size))
            {
                char [] sep = new[]{' '};
                do
                {
                    /* Commands */
                    // p idx1 idx2 --> distance between point in idx1 and idx2
                    // m idx --> mean distance from this point to all other points
                    // z idx --> check if all distances from the point in this idx are zero
                    Console.WriteLine("Enter command: ");
                    string line = Console.ReadLine();
                    string[] splits;
                    if (line.StartsWith("p"))
                    {
                        splits = line.Trim().Split(sep);
                        if (splits.Length == 3)
                        {
                            int idx1, idx2;
                            if (int.TryParse(splits[1], out idx1) && int.TryParse(splits[2], out idx2))
                            {
                                double distance = ((double) BitConverter.ToInt16(reader.Read(idx1, idx2), 0))/
                                                  Int16.MaxValue;
                                Console.WriteLine(distance);
                                continue;
                            }
                        }
                        else if (splits.Length > 3)
                        {
                            int idx1 = int.Parse(splits[1]);
                            for (int i = 2; i < splits.Length; ++i)
                            {
                                int idx = int.Parse(splits[i]);
                                double distance = ((double)BitConverter.ToInt16(reader.Read(idx1, idx), 0)) /
                                                  Int16.MaxValue;
                                Console.WriteLine(distance);
                            }
                            continue;
                        }
                    }
                    else if (line.StartsWith("m"))
                    {
                        splits = line.Trim().Split(sep);
                        if (splits.Length == 2)
                        {
                            int idx;
                            if (int.TryParse(splits[1], out idx))
                            {
                                double sum = 0.0;
                                IList<int> pnums = (IList<int>)cnumToPnums[pnumToCnum[idx]];
                                foreach (int pnum in pnums)
                                {
                                    if (pnum != idx)
                                    {
                                        sum += ((double) BitConverter.ToInt16(reader.Read(idx, pnum), 0))/
                                               Int16.MaxValue;
                                    }
                                }
                                Console.WriteLine(pnums.Count == 1 ? 0 : sum/pnums.Count);
                                continue;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                    Console.WriteLine("something bad happened :-/");
                    break;
                } while (true);
            }

        }
    }
}
