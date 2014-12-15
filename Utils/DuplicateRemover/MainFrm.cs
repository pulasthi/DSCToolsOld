using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using Bio;
using Bio.IO.FastA;

namespace DuplicateRemover
{
    public partial class MainFrm : Form
    {
        private string _path;
        private MetaInfo _metaInfo;

        public MainFrm()
        {
            InitializeComponent();
        }

        private void browseBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                if (DialogResult.OK == dlg.ShowDialog(this))
                {
                    _path = pathTxt.Text = dlg.FileName;
                    _metaInfo = ExtractMetaInfo();
                    
                    countLb.Text = _metaInfo.Count.ToString();
                    uniqueCountLb.Text = _metaInfo.UniqueCount.ToString();

                    
                    using (var writer = new StreamWriter(Path.GetDirectoryName(_path) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(_path) + "_dup.txt"))
                    {
                        
                        foreach (var key in _metaInfo.Duplicates.Keys)
                        {
//                            writer.WriteLine(((IList<string>)_metaInfo.Duplicates[key]).Count + "\t" + key);
                            writer.WriteLine(String.Join(Environment.NewLine,(IList<string>)_metaInfo.Duplicates[key]).ToArray());
                        }
                    }
                    // todo: complete this
//                    dupBx.DataSource = _metaInfo.DuplicateNames;
//                    dupBx.DataSource = _metaInfo.Duplicates;

//                    dgView.da

                    pureDubCountLb.Text = _metaInfo.DuplicateCount.ToString();
                    reducibleUniqsCountLb.Text = _metaInfo.DuplicateGroupCount.ToString();

                }
            }
        }

        private MetaInfo ExtractMetaInfo()
        {
//            List<string> duplicateNames = new List<string>();
            Hashtable uniques = new Hashtable();
            Hashtable duplicates = new Hashtable();

            using (FastAParser fastaParser = new FastAParser(_path))
            {
                IList<ISequence> sequences = fastaParser.Parse().ToList();
                string sequenceString;
                int count = 0;
                foreach (ISequence sequence in sequences)
                {
                    sequenceString = SeqToString(sequence);
                    if (uniques.ContainsKey(sequenceString))
                    {
                        if (duplicates.ContainsKey(sequenceString))
                        {
//                            ((IList<string>) duplicates[sequenceString]).Add(sequence.ID);
                            ((IList<string>) duplicates[sequenceString]).Add(count.ToString());
                        }
                        else
                        {
                            IList<string> ls = new List<string>();
//                            ls.Add(sequence.ID);
                            ls.Add(count.ToString());
                            duplicates.Add(sequenceString, ls);
                        }
                        //                    duplicateNames.Add(sequence.DisplayID);
                    }
                    else
                    {
                        uniques.Add(sequenceString, sequence);
                    }
                    ++count;
                }
            }
            return new MetaInfo(uniques, duplicates);
        }

        static string SeqToString(ISequence sequence)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            return encoding.GetString(sequence.ToArray()).ToUpper();
        }

        private void remBtn_Click(object sender, EventArgs e)
        {
            string parent = outDirBox.Text;
            string path = Path.Combine(parent, ("unique_" + _metaInfo.UniqueCount + "_" 
                + Path.GetFileName(_path)));

            using (FastAFormatter formatter = new FastAFormatter(path))
            {

                Hashtable uniques = _metaInfo.Uniques;
                foreach (string key in uniques.Keys)
                {
                    formatter.Write((ISequence) uniques[key]);
                }

                if (summaryChk.Checked)
                {
                    path = parent + "summary_" + Path.GetFileName(_path);
                    using (StreamWriter writer = new StreamWriter(path))
                    {
                        writer.WriteLine("Original File:   " + _path);
                        writer.WriteLine("Count:           " + _metaInfo.Count);
                        writer.WriteLine("Unique Count:    " + _metaInfo.UniqueCount);
                        writer.WriteLine("Duplicate Count: " + _metaInfo.DuplicateCount);
                        writer.WriteLine("Duplicate Names:");
                        //                    foreach (string name in _metaInfo.DuplicateNames)
                        //                    {
                        //                        writer.WriteLine("\t" + name);
                        //                    }
                    }
                }
            }
            MessageBox.Show("File(s) created at: " + parent);
        }

        private void dirBrowseBtn_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    outDirBox.Text = dlg.SelectedPath;
 
                }

            }
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            outDirBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }
    }
}
