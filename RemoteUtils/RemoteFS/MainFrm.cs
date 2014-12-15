using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RemoteFS
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
        }

        private void remoteMkdir_Click(object sender, EventArgs e)
        {
            string remPath = @"\\madrid-headnode\c$\salsa\saliya\remotetest\testmkdir\nice";
            Directory.CreateDirectory(remPath);
        }

        private void remoteCopy_Click(object sender, EventArgs e)
        {
            string localPath = @"C:\Users\sekanaya\Desktop\GoodFiles.txt";
            string remPath = @"\\madrid-headnode\c$\salsa\saliya\remotetest\test.txt";
            File.Copy(localPath,remPath);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string dir = @"\\madrid-headnode.ads.iu.edu\c$\salsa";
            try
            {
                

                if (Directory.Exists(dir))
                {
                    Console.WriteLine("good");
                }
                else
                {
                    Console.WriteLine("bad");
                }
            }
            catch (Exception exception)
            {

                Console.WriteLine(exception);
            }
        }
    }
}
