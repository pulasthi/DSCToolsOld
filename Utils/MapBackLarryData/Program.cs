using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bio;
using Bio.IO.FastA;

namespace MapBackLarryData
{
    class Program
    {
        static void Main(string[] args)
        {
            var dir = @"G:\My Box Files\SalsaBio\larry\phyloshop\rnafragments";
            var fasta = @"G:\My Box Files\SalsaBio\larry\phyloshop\unique_14150_rnafragments_all.fa";
            var clusterByGroup =
                @"G:\My Box Files\SalsaBio\larry\phyloshop\unique_14150_rnafragments_all_cluster_by_group.txt";
            var clusterByDataSet =
                @"G:\My Box Files\SalsaBio\larry\phyloshop\unique_14150_rnafragments_all_cluster_by_dataset.txt";

            var dirs = Directory.GetDirectories(dir);
            var nameToGroupAndDatasetTable = new Dictionary<string, string[]>();
            foreach (string s in dirs)
            {
                var dataset = Path.GetFileName(s);
                var group = dataset.Substring(0, 2);
                using (var parser = new FastAParser(Path.Combine(s,"meta.rna")))
                {
                    var seqs = parser.Parse();
                    foreach (ISequence seq in seqs)
                    {
                        nameToGroupAndDatasetTable.Add(seq.ID + SeqToString(seq), new []{group,dataset});
                    }
                }
            }

            using (var parser = new FastAParser(fasta))
            {
                var seqs = parser.Parse().ToList();
                using(StreamWriter groupClusterWriter = new StreamWriter(clusterByGroup), 
                    datasetClusterWriter = new StreamWriter(clusterByDataSet))
                {
                    var count = 0;
                    foreach (ISequence seq in seqs)
                    {
                        var groupAndDataset = nameToGroupAndDatasetTable[seq.ID+SeqToString(seq)];
                        groupClusterWriter.WriteLine(count +"\t" + groupAndDataset[0]);
                        datasetClusterWriter.WriteLine(count +"\t" + groupAndDataset[1]);
                    }
                }
            }
            Console.WriteLine("Done.");
            Console.Read();

        }

        private static string SeqToString(IEnumerable<byte> seq)
        {
            var asciiEncoding = new ASCIIEncoding();
            return asciiEncoding.GetString(seq.ToArray()).ToUpper();
        }
    }
}
