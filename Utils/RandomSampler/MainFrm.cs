using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Bio;
using Bio.IO.FastA;

namespace RandomSampler
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
        }

        private void BrowseBtnClick(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                if (DialogResult.OK == dlg.ShowDialog(this))
                {
                    inPathTxt.Text = dlg.FileName;
                }
            }
        }

        private void GenBtnClick(object sender, EventArgs e)
        {
            if (IsValid())
            {
                var sizes = sizeBox.Text.Split(' ').Select(int.Parse);
                int lenMin, lenMax;
                if (!int.TryParse(lenGtEqTxt.Text, out lenMin)) lenMin = 0;
                if (!int.TryParse(lenLtEqTxt.Text, out lenMax)) lenMax = int.MaxValue;
                

                var excludes = new HashSet<string>();
                if (!string.IsNullOrEmpty(excludeTxt.Text))
                {
                    using (var parser = new FastAParser(excludeTxt.Text))
                    {
                        var seqs = parser.Parse();
                        foreach (var seq in seqs)
                        {
                            var seqStr = SeqToString(seq);
                            if (excludes.Contains(seqStr)) continue;
                            excludes.Add(seqStr);
                        }
                    }
                }
                GenerateSamples(inPathTxt.Text, outPathBox.Text, sizes, lenMin, lenMax, excludes);
                MessageBox.Show("File(s) generated successfully", "Done", MessageBoxButtons.OK);
            }
            
        }

        private Boolean IsValid()
        {
            if (String.IsNullOrEmpty(sizeBox.Text))
            {
                MessageBox.Show("At least one sample size should be specified", "Invalid Data", MessageBoxButtons.OK);                
                return false;
            }

            if (String.IsNullOrEmpty(inPathTxt.Text))
            {
                MessageBox.Show("Input file cannot be null", "Invalid Data", MessageBoxButtons.OK);
                return false;
            }

            if (string.IsNullOrEmpty(outPathBox.Text))
            {
                MessageBox.Show("Output path cannot be null", "Invalid Data", MessageBoxButtons.OK);
                return false;
            }

            if (!File.Exists(inPathTxt.Text))
            {
                MessageBox.Show("Input file does not exist", "I/O Error", MessageBoxButtons.OK);
                return false;
            }

            if (!Directory.Exists(outPathBox.Text))
            {
                MessageBox.Show("Output directory does not exist", "I/O Error", MessageBoxButtons.OK);
                return false;
            }
            
            return true;
        }

        private static void GenerateSamples(string fastaPath, string outPath, IEnumerable<int> sizes, int lenMin, int lenMax, HashSet<string> excludes)
        {

            using (var fastaParser = new FastAParser(fastaPath))
            {
                IList<ISequence> sequences = fastaParser.Parse().ToList();

                var max = sequences.Count;

                // Populate random indices
                var ht = new Hashtable();
                var rand = new Random();

                var fastaName = Path.GetFileNameWithoutExtension(fastaPath);

                

                foreach (var size in sizes)
                {
                    if (size > max)
                    {
                        continue;
                    }

                    var outFastaName = fastaName + "_" + size + ".txt";
                    var mappingIndexFile = fastaName + "_" + size + "_mapping.txt";
                    using (StreamWriter outFastaWriter = new StreamWriter(Path.Combine(outPath, outFastaName)),
                        mappingIndexWriter = new StreamWriter(Path.Combine(outPath, mappingIndexFile)))
                    {
                        mappingIndexWriter.WriteLine("local#\torig#\tseqname\tlenght");
                        for (var j = 0; j < size; ++j)
                        {
                            int index;
                            do
                            {
                                index = rand.Next(max);
                            } while (ht.ContainsKey(index));
                            ht.Add(index, index);
                            var seq = sequences[index];
                            if (seq.Count < lenMin || seq.Count > lenMax || excludes.Contains(SeqToString(seq)))
                            {
                                --j;
                                continue;
                            }
                            outFastaWriter.WriteLine(">" + seq.ID);
                            outFastaWriter.WriteLine(SeqToString(seq));
                            mappingIndexWriter.WriteLine(j + "\t" + index + "\t" + seq.ID + "\t" +
                                                    sequences[index].Count);
                        }
                    }

                }
            }

        }

        private static string SeqToString(IEnumerable<byte> seq)
        {
            var asciiEncoding = new ASCIIEncoding();
            return asciiEncoding.GetString(seq.ToArray()).ToUpper();
        }

        private void OutBrowseBtnClick(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                if (DialogResult.OK == dlg.ShowDialog(this))
                {
                    outPathBox.Text = dlg.SelectedPath;
                }
            }
        }

        private void MainFrmLoad(object sender, EventArgs e)
        {
            outPathBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }
    }
}
