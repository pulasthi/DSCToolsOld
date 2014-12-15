using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Bio;
using Bio.IO.FastA;

namespace Sampler
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
        }

        private void Sample()
        {
            string path =
                @"C:\Users\sekanaya\Documents\Magic Briefcase\SALSA2\Millions\OriginalSequences\~400K\uniqs\length_gt200\allreads_uniques_gt200_446041_random.txt";
            string outDir =
                @"C:\Users\sekanaya\Documents\Magic Briefcase\SALSA2\Millions\OriginalSequences\~400K\uniqs\length_gt200\100k_parts_from_random";

            string outNamePrefix = "allreads_uniques_gt200_440641_random_100k_";
            int [] sizes = new[] {100000, 200000, 300000, 400000, 446041};
            
//            int i = 0;
            int i = 400000;

            StreamWriter writer;
            FastAFormatter formatter;
            using (FastAParser parser = new FastAParser(path))
            {
                IList<ISequence> seqs = parser.Parse().ToList();
                for (int j = 0; j < sizes.Length; j++)
                {
                    j = 4;

                    using (formatter = new FastAFormatter(Path.Combine(outDir, outNamePrefix + j + ".txt")))
                    {
                        using (writer = new StreamWriter(Path.Combine(outDir, outNamePrefix + j + "_index.txt")))
                        {
                            for (; i < sizes[j]; i++)
                            {
                                formatter.Write(seqs[i]);
                                writer.WriteLine((i - 100000 * j) +"\t" + seqs[i].ID + "\t" + seqs[i].Count);
                            }
                            writer.Flush();
                        }
                        formatter.Flush();
                    }
                }
            }



        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path =
//                @"C:\Users\sekanaya\Documents\Magic Briefcase\SALSA2\Millions\OriginalSequences\~400K\allreads_uniques_482158.txt";
                @"C:\Users\sekanaya\Documents\Magic Briefcase\SALSA2\Millions\OriginalSequences\~400K\uniqs\length_gt200\allreads_uniques_gt200_440641_random.txt";
            FastAParser parser = new FastAParser(path);
            IEnumerable<ISequence> seqs = parser.Parse();
//            string allgt200 =
//                @"C:\Users\sekanaya\Documents\Magic Briefcase\SALSA2\Millions\OriginalSequences\~400K\allreads_uniques_gt200.txt";
            string gt200first100k =
                @"C:\Users\sekanaya\Documents\Magic Briefcase\SALSA2\Millions\OriginalSequences\~400K\uniqs\length_gt200\100k_parts_from_random\allreads_uniques_gt200_440641_random_fourth100k.txt";

            string gt200first100kIndex =
                @"C:\Users\sekanaya\Documents\Magic Briefcase\SALSA2\Millions\OriginalSequences\~400K\uniqs\length_gt200\100k_parts_from_random\allreads_uniques_gt200_440641_random_fourth100k_index.txt";

//            FastAFormatter allgt200Formatter = new FastAFormatter(allgt200);
            FastAFormatter gt200firs100kFormatter = new FastAFormatter(gt200first100k);
                
            int count = 0;
            using (StreamWriter writer = new StreamWriter(gt200first100kIndex))
            {
                foreach (ISequence seq in seqs)
                {
                    if (seq.Count > 200)
                    {
//                        allgt200Formatter.Write(seq);
                        if (count >=300000 && count < 400000)
                        {
                            gt200firs100kFormatter.Write(seq);
                            writer.WriteLine(count + "\t" + seq.ID + "\t" + seq.Count);
                        }
                        count++;
                    }
                }
                gt200firs100kFormatter.Flush();
                gt200firs100kFormatter.Close();
//                allgt200Formatter.Flush();
//                allgt200Formatter.Close();
            }
            MessageBox.Show("allgt200 Count " + count);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Sample();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string path =
                @"C:\Users\sekanaya\Documents\Magic Briefcase\SALSA2\Millions\OriginalSequences\~400K\uniqs\length_gt200\100k_parts_from_direct\allreads_uniques_gt200_100k_";
            string outDir =
                @"C:\Users\sekanaya\Documents\Magic Briefcase\SALSA2\Millions\OriginalSequences\~400K\uniqs\length_gt200\100k_parts_from_direct";

            for (int i = 0; i<5; i++)
            {
                i = 4;
                HistogramFasta(path + i + ".txt", outDir);
            }

        }

        private void HistogramFasta(string fastaFile, string outDir)
        {
            string fastaName = Path.GetFileNameWithoutExtension(fastaFile);

            string outFile = Path.Combine(outDir, fastaName + "_histogram.txt");

            Hashtable ht = new Hashtable();
            long seqLen;
            using (FastAParser parser = new FastAParser(fastaFile))
            {
                IList<ISequence> seqs = parser.Parse().ToList();
                foreach (ISequence seq in seqs)
                {
                    seqLen = seq.Count;
                    if (ht.ContainsKey(seqLen))
                    {
                        ht[seqLen] = ((int) ht[seqLen]) + 1;
                    }
                    else
                    {
                        ht.Add(seqLen, 1);
                    }
                }
            }

            PrintHt(ht, outFile);
        }

        private void PrintHt(Hashtable ht, string outFile)
        {
            using (StreamWriter writer = new StreamWriter(outFile))
            {
                foreach (var key in ht.Keys)
                {
                    writer.WriteLine(string.Format("{0}\t{1}", key, ht[key]));
                }
                writer.Flush();
            }
        }
    }
}
