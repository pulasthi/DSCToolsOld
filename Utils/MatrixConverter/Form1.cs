using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace MatrixConverter
{
    public partial class Form1 : Form
    {
        private string _delim = "\t";
        public Form1()
        {
            InitializeComponent();
        }

        private void inBrowseBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    inFileTxt.Text = dlg.FileName;
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    outPathTxt.Text = dlg.SelectedPath;
                }
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            switch (typeCmbo.SelectedIndex)
            {
                case 0:
                    ConvertInt16ToString(inFileTxt.Text, Path.Combine(outPathTxt.Text, string.Format("{0}.txt", (Path.GetFileNameWithoutExtension(inFileTxt.Text)))), int.Parse(totalTxt.Text));
                    MessageBox.Show("Done");
                    break;
                default:
                    MessageBox.Show("Nothing to do");
                    break;
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            typeCmbo.SelectedIndex = 0;
            outPathTxt.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //GenerateDummy();
        }

        private void GenerateDummy()
        {
            using (Stream stream = File.OpenWrite(@"C:\users\sekanaya\Desktop\matrixtest\dist.bin"))
            {
                using (var strmWriter = new StreamWriter(@"C:\users\sekanaya\Desktop\matrixtest\dist.txt"))
                {
                    using (var binaryWriter = new BinaryWriter(stream))
                    {
                        Int16 x;
                        for (Int16 i = 0; i < 20; i++)
                        {
                            for (Int16 j = 0; j < 20; j++)
                            {
                                if (i == j)
                                {
                                    x = 0 * Int16.MaxValue;
                                    binaryWriter.Write(x);
                                    strmWriter.Write(x + @"	");
                                }
                                else
                                {
//                                    byte [] bytes = new byte[8];
//                                    RandomNumberGenerator.Create().GetBytes(bytes);

//                                    x = ((short)((BitConverter.ToDouble(bytes,0) / Double.MaxValue) * Int16.MaxValue));
                                    x = ((short)((new Random().NextDouble()) * Int16.MaxValue));
                                   
                                    binaryWriter.Write(x);
                                    strmWriter.Write(x + @"	");
                                    Thread.Sleep(150);

                                }
                            }
                            strmWriter.WriteLine("\n");
                        }
                    }
                }
            }
        }

        private void ConvertInt16ToString(string inFile, string outFile, int size)
        {
            using (Stream inStream = File.OpenRead(inFile))
            {
                using (BinaryReader reader = new BinaryReader(inStream))
                {
                    using (StreamWriter writer = new StreamWriter(outFile))
                    {
                        Int16 x;
                        for (int i = 0; i < size; i++)
                        {
                            for (int j = 0; j < size; j++)
                            {
                                if (j == 0)
                                {
                                    writer.Write(i);
                                }
                                writer.Write(_delim);
                                x = reader.ReadInt16();
                                writer.Write(x);

                                if ((j + 1) % size == 0)
                                {
                                    writer.Write("\n");
                                }
                            }
                        }
 
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GenerateDummy();
        }
    }
}
