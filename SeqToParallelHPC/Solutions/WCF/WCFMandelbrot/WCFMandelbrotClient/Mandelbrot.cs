/* Mandelbrot.cs */

//
// Mandelbrot generation with managed C#
// Joe Hummel, Fritz Onion and Mike Woodring
// Pluralsight LLC
//
// Based on original WinForms C# application developed by Ian Griffiths
//

using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Threading;
using System.ServiceModel;
using Microsoft.Hpc.Scheduler;
using Microsoft.Hpc.Scheduler.Properties;
using Microsoft.Hpc.Scheduler.Session;


namespace WCFMandelbrotClient
{

  public class Mandelbrot
  {
    private int _startTime = 0;
    private BackgroundWorker _worker = null;
    private Session _session = null;
    private MandelbrotServiceClient _proxy = null;
    private long _asyncCallsOutstanding = 0;

    private double _x;  // parameters of Mandelbrot computation:
    private double _y;
    private double _size;
    private int _pixels;

    public Mandelbrot(double x, double y, double size, int pixels)
    {
      _x = x;  // parameters of Mandelbrot computation: 
      _y = y;
      _size = size;
      _pixels = pixels;

      try
      {
        string broker = ConfigurationSettings.AppSettings["ClusterBrokernode"];
        string service = ConfigurationSettings.AppSettings["ServiceName"];

        SessionStartInfo info = new SessionStartInfo(broker, service);

        String runAsUser = ConfigurationSettings.AppSettings["RunAsUserName"];
        String runAsPwd = ConfigurationSettings.AppSettings["RunAsPassword"];
        if (runAsPwd == "") // force prompt if there's no password:
          runAsPwd = null;

        info.Username = runAsUser;
        info.Password = runAsPwd;

        info.ResourceUnitType = JobUnitType.Core;
        info.MinimumUnits = 1;
        info.MaximumUnits = 8;

        Session.SetInterfaceMode(false /*GUI*/, (IntPtr)null /*no parent*/);

        _session = Session.CreateSession(info);

        _proxy = new MandelbrotServiceClient(
            new NetTcpBinding(SecurityMode.Transport, false),
            _session.EndpointReference);
      }
      catch (Exception ex)
      {
        string msg;
        msg = string.Format("Error in Mandelbrot.constructor: {0} \n\nInner: {1}",
          ex.Message, ex.InnerException == null ? "N/A" : ex.InnerException.Message);

        MessageBox.Show(msg);
        MessageBox.Show("NOTE: 'WCFMandelbrotService' jobs may be running on cluster, use Cluster/Job Manager to cancel jobs before running client again.");
        System.Environment.Exit(1);
      }

    }

    public double TimeTaken()
    {
      int curTime;
      double time;

      curTime = System.Environment.TickCount;
      time = (curTime - _startTime) / 1000.0;

      return time;
    }

    //
    // Designed to be called asynchronously as part of BackgroundWorker object.  Args for Mandelbrot
    // computation are passed in e->Argument, and method will report progress as it completes each
    // row of the Mandelbrot computation.
    //
    public void Calculate(Object sender, DoWorkEventArgs e)
    {
      //
      // void Calculate(null)
      //
      try
      {
        _worker = (BackgroundWorker)sender;

        _startTime = System.Environment.TickCount;

        _asyncCallsOutstanding = 0;

        //
        // now start computing Mandelbrot set, row by row:
        //
        for (int yp = 0; yp < _pixels; yp++)
        {
          //
          // check if we are supposed to cancel?
          //
          if (_worker.CancellationPending)
          {
            e.Cancel = true;
            break;  // exit loop, wait for outstanding calls to finish, then we'll stop:
          }

          //
          // no cancel, so start Mandelbrot computation for this row:
          //
          Interlocked.Increment(ref _asyncCallsOutstanding);

          _proxy.BeginGenerateMandelbrotRow(yp, _y, _x, _size, _pixels, this.MandelbrotRowCallback, yp /*state*/);          
        }

        // 
        // At this point we have started exection of all rows, so now we have to wait for 
        // them to finish --- otherwise, if we exit now, .NET will think the worker thread
        // has finished, triggering the OnComplete event too soon:
        //
        while (Interlocked.Read(ref _asyncCallsOutstanding) > 0)
          Thread.Sleep(200);        
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error in Mandelbrot.Calculate: " + ex.Message);
        MessageBox.Show("NOTE: 'WCFMandelbrotService' jobs may be running on cluster, use Cluster/Job Manager to cancel jobs before running client again.");
        System.Environment.Exit(1);
      }
    }

    //
    // Callback when GenerateMandelbrotRow completes:
    //
    private void MandelbrotRowCallback(IAsyncResult result)
    {
      // 
      // did user close the form and shutdown the app prematurely?  In that case, 
      // no more results are coming through:
      //
      if (_proxy.State == CommunicationState.Closed)
        return;

      //
      // else proxy is still up and running, and so results should be available:
      //
      int yp = (int)result.AsyncState;  // retrieve which row this represents:
       
      int[] values = _proxy.EndGenerateMandelbrotRow(result);  // now harvest results:

      //
      // we've generated a row, report this as progress for display:
      //
      Object[] args = new Object[2];

      args[0] = values;
      args[1] = AppDomain.GetCurrentThreadId();

      _worker.ReportProgress(yp, args);

      //
      // count this row as done:
      //
      Interlocked.Decrement(ref _asyncCallsOutstanding);
    }

    //
    // Call to cleanup when done:
    //
    public void Close()
    {
      try
      {
        _proxy.Close();
        _session.Dispose();
      }
      catch
      { /*ignore*/ }
    }

  }//class
}//namespace

