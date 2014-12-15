using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;

namespace TweakClusterFile
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
        }

        private void clusterTxt_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
                clusterTxt.Text = fileNames[0];
            }
        }

        private void clusterTxt_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void buildBtn_Click(object sender, EventArgs e)
        {
            string clusterFile = clusterTxt.Text;
            if (!string.IsNullOrEmpty(clusterFile) && File.Exists(clusterFile))
            {
                using (var reader = new SimpleClusterReader(clusterFile))
                {
                    string suffix = string.Empty;
                    string[] tweaks = tweaksTxtBx.Lines;

                    int deltaInput = inputOneBaseRadioBtn.Checked ? -1 : 0;
                    int deltaTweaks = tweaksOneBaseRadioBtn.Checked ? -1 : 0;
                    int deltaOut = outputOneBaseRadioBtn.Checked ? 1 : 0;

                    Hashtable tweakTable = generateTweakTable(tweaks,deltaTweaks);

                    suffix = tweaks.Aggregate(suffix,
                                              (current, tweak) =>
                                              current +
                                              ("{" +
                                               (string.Join("+",
                                                            tweak.Split(new[] {'+'}).Select(
                                                                x =>
                                                                (int.Parse(x) + deltaTweaks + deltaOut).ToString(
                                                                    CultureInfo.InvariantCulture)))) + "}_"));
                    suffix +=  outputZeroBaseRadioBtn.Checked ? "zeroidx" : "oneidx";
                    string outFile = Path.Combine((Path.GetDirectoryName(clusterFile) ?? string.Empty),
                                                  Path.GetFileNameWithoutExtension(clusterFile) + "_" + suffix + ".txt");
                    using (var writer = new StreamWriter(outFile))
                    {
                        while (!reader.EndOfStream)
                        {
                            Cluster c = reader.ReadCluster();
                            c.Cnum += deltaInput;
                            int cnum = (tweakTable.Contains(c.Cnum) ? (int)tweakTable[c.Cnum] : c.Cnum);
                            writer.WriteLine(c.Pnum +"\t" + (cnum+deltaOut));
                        }
                    }
                }
            }
            MessageBox.Show("Done.");
        }

        private Hashtable generateTweakTable(string[] tweaks, int deltaTweaks)
        {
            char[] sep = new[] {'+'};
            Hashtable ht = new Hashtable();
            foreach (string tweak in tweaks)
            {
                int[] splits = tweak.Trim().Split(sep).Select(x=>int.Parse(x)).ToArray();
                Array.Sort(splits);
                int c = splits[0]+deltaTweaks;
                foreach (int split in splits)
                {
                    ht.Add(split+deltaTweaks, c);
                }
            }
            return ht;
        }
    }
}
