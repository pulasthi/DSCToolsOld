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

namespace ComputationParClient
{
  public partial class Form1 : Form
  {
    private Session session;
    private MathServiceClient proxy;

    public Form1()
    {
      InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      this.textBox1.Text = "0";
      this.textBox2.Text = "0";

      try
      {
        string broker = "headnode";
        string service = "ComputationServices";

        SessionStartInfo info = new SessionStartInfo(broker, service);

        info.Username = @"minihpc\hummel";
        info.Password = null;  // prompt for pwd:

        info.ResourceUnitType = JobUnitType.Core;
        info.MinimumUnits = 1;
        info.MaximumUnits = 8;

        Session.SetInterfaceMode(false /*GUI*/, (IntPtr)null /*no parent*/);

        session = Session.CreateSession(info);

        proxy = new MathServiceClient(
            new NetTcpBinding(SecurityMode.Transport, false),
            session.EndpointReference);
      }
      catch (Exception ex)
      {
        string msg;
        msg = string.Format("Error: {0} \n\nInner: {1}",
          ex.Message, ex.InnerException == null ? "N/A" : ex.InnerException.Message);

        MessageBox.Show(msg, "ComputationParClient", MessageBoxButtons.OK, MessageBoxIcon.Error);
        MessageBox.Show("NOTE: 'WCF Service' jobs may be running on cluster, use Cluster/Job Manager to cancel jobs before running client again.", "ComputationParClient", MessageBoxButtons.OK, MessageBoxIcon.Information);
        System.Environment.Exit(1);
      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
      try
      {
        int a = Convert.ToInt32(textBox1.Text);
        int b = Convert.ToInt32(textBox2.Text);

        int sum = proxy.Add(a, b);

        textBox3.Text = sum.ToString();
      }
      catch (Exception ex)
      {
        string msg;
        msg = string.Format("Error: {0} \n\nInner: {1}",
          ex.Message, ex.InnerException == null ? "N/A" : ex.InnerException.Message);

        MessageBox.Show(msg, "ComputationParClient", MessageBoxButtons.OK, MessageBoxIcon.Error);
        MessageBox.Show("NOTE: 'WCF Service' jobs are probably still running on cluster, use Cluster/Job Manager to cancel jobs before running client again.", "ComputationParClient", MessageBoxButtons.OK, MessageBoxIcon.Information);
        System.Environment.Exit(1);
      }
    }

    private void Form1_FormClosed(object sender, FormClosedEventArgs e)
    {
      try
      {
        proxy.Close();
        session.Dispose();
      }
      catch
      { /*ignore*/ }
    }

  }//class
}//namespace
