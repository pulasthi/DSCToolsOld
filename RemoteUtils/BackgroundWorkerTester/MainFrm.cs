using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Hpc.Scheduler;
using Microsoft.Hpc.Scheduler.Properties;

namespace BackgroundWorkerTester
{
    public partial class MainFrm : Form
    {
        private BackgroundWorker _bw;

        public MainFrm()
        {
            InitializeComponent();
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            _bw = new BackgroundWorker
                      {
                          WorkerReportsProgress = true,
                          WorkerSupportsCancellation = true
                      };
            _bw.DoWork += BwDoWork;
            _bw.ProgressChanged +=BwProgressChanged;
            _bw.RunWorkerCompleted += BwRunWorkerCompleted;
        }

        void BwRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show(@"bw cancelled");
            }
            else if (e.Error != null)
            {
                MessageBox.Show(@"bw error:" + e.Error);
            }
            else
            {
                MessageBox.Show(@"bw complete:" + e.Result);
            }
        }

        private void BwProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            State s = (State) e.UserState;
            statusTxt.Text = s.Txt;
            statusTxt.Refresh();
        }

        void BwDoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
               if (_bw.IsBusy)
               {
                   Console.WriteLine("I am busy - cancellation pending: " +_bw.CancellationPending);
               } 
               else
               {
                   Console.WriteLine("Not busy - cancellation pending: " + _bw.CancellationPending);
               }
                Thread.Sleep(500);
            }
            /*
             * // The super busy code
            int count = 0;
            int max = (int) e.Argument;
            for (int i = 0; i < max; ++i)
            {

                if(_bw.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                } 
                count = i + (int) Math.Sqrt(i) * count;
                if (i % 100 == 0)
                {
                    _bw.ReportProgress((int) Math.Round(((float) i)/max), new State("going well in " + i + " of " + max));
                }
                Thread.Sleep(2);
            }
            e.Result = "do work done";*/
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            if(!_bw.IsBusy) _bw.RunWorkerAsync(50000);
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            if (_bw.IsBusy)
            {
                _bw.CancelAsync();
            }
        }

        private ManualResetEvent evt = new ManualResetEvent(false);
        private void openJobBtn_Click(object sender, EventArgs e)
        {
            using (IScheduler scheduler = new Scheduler())
            {
                scheduler.Connect("Tempest.ads.iu.edu");
                ISchedulerJob job = scheduler.OpenJob(int.Parse(jobidTxt.Text));
                Console.WriteLine(job.State);
                job.OnJobState += new EventHandler<JobStateEventArg>(job_OnJobState);
                evt.WaitOne();
                MessageBox.Show("good "+ job.State);
                
            }
        }

        void job_OnJobState(object sender, JobStateEventArg e)
        {
            MessageBox.Show(e.NewState.ToString());
            evt.Set();
        }
    }
}
