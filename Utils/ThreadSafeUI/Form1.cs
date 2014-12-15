using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ThreadSafeUI
{
    public partial class Form1 : Form
    {
        private Thread demoThread;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.demoThread = new Thread(new ThreadStart(this.ThreadProcSafe));
            demoThread.Start();
        }

        private void ThreadProcSafe()
        {
            for (int i = 0; i < 10; i++ )
            {
                this.SetText("This text was set safely " + i + Environment.NewLine);   
                Thread.Sleep(100);
            }
        }

        private delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] {text});
            }
            else
            {
                this.textBox1.AppendText(text);

            }
        }
    }
}
