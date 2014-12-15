using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.ServiceModel;
using Microsoft.Hpc.Scheduler;
using Microsoft.Hpc.Scheduler.Properties;
using Microsoft.Hpc.Scheduler.Session;

namespace ComputationClient
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      this.textBox1.Text = "0";
      this.textBox2.Text = "0";
    }

    private void button1_Click(object sender, EventArgs e)
    {
      try
      {
        //
        // parameters for our cluster:
        //
        string broker = "headnode";
        string service = "ComputationServices";

        SessionStartInfo info = new SessionStartInfo(broker, service);

        info.Username = @"minihpc\hummel";
        info.Password = null;  // prompt for pwd:

        info.ResourceUnitType = JobUnitType.Core;
        info.MinimumUnits = 1;
        info.MaximumUnits = 8;

        //
        // Now connect to cluster and start our service:
        //
        Session.SetInterfaceMode(false /*GUI*/, (IntPtr)null /*no parent*/);

        textBox3.Text = "starting...";
        textBox3.Refresh();

        Session session = Session.CreateSession(info);

        textBox3.Text = "creating...";
        textBox3.Refresh();

        //
        // Finally, instantiate proxy and make the call!
        //
        MathServiceClient proxy = new MathServiceClient(
            new NetTcpBinding(SecurityMode.Transport, false),
            session.EndpointReference);

        int a = Convert.ToInt32(textBox1.Text);
        int b = Convert.ToInt32(textBox2.Text);

        textBox3.Text = "calling...";
        textBox3.Refresh();

        int sum = proxy.Add(a, b);

        textBox3.Text = sum.ToString();
        textBox3.Refresh();

        MessageBox.Show("Done!", "ComputationClient", MessageBoxButtons.OK, MessageBoxIcon.Information);

        proxy.Close();
        session.Dispose();
      }
      catch (Exception ex)
      {
        string msg;
        msg = string.Format("Error: {0} \n\nInner: {1}",
          ex.Message, ex.InnerException == null ? "N/A" : ex.InnerException.Message);

        MessageBox.Show(msg, "ComputationClient", MessageBoxButtons.OK, MessageBoxIcon.Error);
        MessageBox.Show("NOTE: 'WCF Service' jobs are probably still running on cluster, use Cluster/Job Manager to cancel jobs before running client again.", "ComputationClient", MessageBoxButtons.OK, MessageBoxIcon.Information);
        System.Environment.Exit(1);
      }
    }

  }//class
}//namespace
