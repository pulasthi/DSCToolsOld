using System;
using System.Text;
using System.Windows.Forms;
using HPC.Utilities;

namespace ArrayPerfComparator
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
        }

        private void Button1Click(object sender, EventArgs e)
        {
            int x, y;

            if (Int32.TryParse(xTxt.Text, out x) && Int32.TryParse(yTxt.Text, out y))
            {
                var sb = new StringBuilder();
                var timer = new HiPerfTimer();
                timer.Start();
                var rarr = new int[x,y];
                timer.Stop();
                double t = timer.Duration;
                sb.AppendLine("Rectangular Array Creation: " + t);

                t = 0;
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        timer.Start();
                        rarr[i, j] = 0;
                        timer.Stop();
                        t += timer.Duration;
                    }
                }
                sb.AppendLine("Rectangular Array Initialization: " + t);

                timer.Start();
                var jarr = new int[x][];
                timer.Stop();
                t = timer.Duration;
                sb.AppendLine("Jagged Array Creation: " + t);

                t = 0;
                for (int i = 0; i < x; i++)
                {
                    timer.Start();
                    jarr[i] = new int[y];
                    timer.Stop();
                    t += timer.Duration;
                }

                sb.AppendLine("Jagged Array Creation of Sub Arrays: " + t);

                t = 0;
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        timer.Start();
                        jarr[i][j] = 0;
                        timer.Stop();
                        t += timer.Duration;
                    }
                }

                sb.AppendLine("Jagged Array Initialization: " + t);

                MessageBox.Show(sb.ToString());
            }
        }
    }
}