using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common;

namespace SequenceNameOverlap
{
    class Program
    {
        static void Main(string[] args)
        {
            var largeFastaIndex = args[0];
            var smallFastaIndex = args[1];

            var overlapIndexClusterFileForLargeFasta =
                Path.Combine(Path.GetDirectoryName(largeFastaIndex) ?? string.Empty,
                                                   Path.GetFileNameWithoutExtension(largeFastaIndex) +
                                                   "_overlap_cluster.txt");

            var overlapIndexClusterFileForSmallFasta =
                Path.Combine(Path.GetDirectoryName(smallFastaIndex) ?? string.Empty,
                                                   Path.GetFileNameWithoutExtension(smallFastaIndex) +
                                                   "_overlap_cluster.txt");

            using (StreamWriter largeWriter = new StreamWriter(overlapIndexClusterFileForLargeFasta), 
                smallWriter = new StreamWriter(overlapIndexClusterFileForSmallFasta))
            {
                using (
                    IndexReader largeReader = new IndexReader(largeFastaIndex),
                                smallReader = new IndexReader(smallFastaIndex))
                {
                    var ht = new Hashtable();
                    var namelist = new List<string>();
                    while (!largeReader.EndOfStream)
                    {
                        var largeEntry = largeReader.ReadIndexEntry();
                        namelist.Add(largeEntry.Name);
                        if (!ht.Contains(largeEntry.Name))
                        {
                            ht.Add(largeEntry.Name, 0); // 0 indicates no overlap. These will later be changed to 1s for the overlapping sequences
                        }
                    }

                    var overlapCount = 0;
                    while (!smallReader.EndOfStream)
                    {
                        var smallEntry = smallReader.ReadIndexEntry();

                        if (ht.Contains(smallEntry.Name))
                        {
                            ++overlapCount;
                            ht[smallEntry.Name] = 1;// mark this as an overlap in large sequence set
                            smallWriter.WriteLine(smallEntry.Pnum + "\t1");
                        }
                        else
                        {
                            smallWriter.WriteLine(smallEntry.Pnum + "\t0");
                        }
                    }

                    var count = 0;
                    foreach (var name in namelist)
                    {
                        largeWriter.WriteLine(count + "\t" + (ht[name]));
                        ++count;
                    }
                    Console.WriteLine(overlapCount);
                    Console.Read();
                }
            }
        }
    }
}
