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
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;


namespace ClientSeqDotNetMandelbrot
{

  public class Mandelbrot
  {
    private int _startTime = 0;
    private int _serverID = -1;
    private BackgroundWorker _worker = null;

    private string _basedir;  // communication dir between client and server:

    private double _x;  // parameters of Mandelbrot computation:
    private double _y;
    private double _size;
    private int _pixels;

    public Mandelbrot(double x, double y, double size, int pixels)
    {
      _basedir = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "server");

      _x = x;  // parameters of Mandelbrot computation: 
      _y = y;
      _size = size;
      _pixels = pixels;
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
    // Delete all the files --- .cancel, .ready, and .raw --- from a previous run.
    //
    private void CleanupPreviousRun()
    {
      try
      {
        string filename;
        string[] files;

        filename = Path.Combine(_basedir, "Mandelbrot.cancel");
        if (File.Exists(filename))
          File.Delete(filename);

        files = Directory.GetFiles(_basedir, "*.raw");
        for (int i = 0; i < files.Length; i++)
          File.Delete(files[i]);

        files = Directory.GetFiles(_basedir, "*.ready");
        for (int i = 0; i < files.Length; i++)
          File.Delete(files[i]);
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error in Mandelbrot.CleanupPreviousRun: " + ex.Message);
        System.Environment.Exit(1);
      }
    }

    //
    // Designed to be called asynchronously as part of BackgroundWorker object.  Args for Mandelbrot
    // computation are passed in e->Argument, and method will report progress as it completes each
    // row of the Mandelbrot computation.
    //
    public void Calculate(Object sender, DoWorkEventArgs e)
    {
      //
      // void Calculate()
      //
      try
      {
        _worker = (BackgroundWorker)sender;

        int startRowInclusive = 0;
        int endRowExclusive = _pixels;

        //
        // cleanup previous run:
        //
        this.CleanupPreviousRun();

        //
        // okay, start new run by starting server-side Mandelbrot process...
        //
        _startTime = System.Environment.TickCount;

        string serverFile;
        serverFile = "ServerSeqDotNetMandelbrot.exe";

        string serverArgs;
        serverArgs = String.Format("{0} {1} {2} {3} {4} {5}",
          _x,
          _y,
          _size,
          _pixels,
          startRowInclusive,
          endRowExclusive);

        Process server = new Process();
        server.StartInfo.FileName = serverFile;
        server.StartInfo.Arguments = serverArgs;
        server.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        server.StartInfo.WorkingDirectory = _basedir;
        server.Start();

        _serverID = server.Id;

        //
        // Okay, now we start watching for & displaying files produced by the server:
        //
        int rowCount = 0;

        while (rowCount < _pixels)
        {
          //
          // check if we are supposed to cancel?
          //
          if (_worker.CancellationPending)
          {
            // 
            // tell the server to stop:
            //
            string filename = Path.Combine(_basedir, "Mandelbrot.cancel");
            using (StreamWriter writer = new StreamWriter(filename))
            {
              // just need to create the file, it can be empty:
            }

            //
            // now we stop:
            //
            e.Cancel = true;
            break;
          }

          //
          // Otherwise do we have some data to report?
          //
          // NOTE: tried to use FileSystemWatcher class to get events when new files were
          // available.  As concurrency increased, events were lost, so this low-tech approach
          // turns out to be the most reliable mechanism for client-server communication.
          //
          string[] files = Directory.GetFiles(_basedir, "*.ready");

          for (int i = 0; i < files.Length; i++, rowCount++)
            this.ProcessRow(files[i]);

          //
          // Did we process any files?  If not, and the server has stopped (crashed?), 
          // we should exit loop otherwise we may loop forever...
          //
          if (files.Length == 0 && server.HasExited)
            break;

          //
          // Pause a bit to let the rest of the system do some work:
          //
          Thread.Sleep(100);  // 1/10 sec.
        }//while

        //
        // We're done, cleanup!
        //
        server.WaitForExit();
        server.Close();
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error in Mandelbrot.Calculate: " + ex.Message);
        System.Environment.Exit(1);
      }
    }

    //
    // This method actually reads the data from the file and reports it as progress
    // back to the GUI.  The filename will be of the form "Mandelbrot.YP.ready" (where
    // YP is the row number), in which case we then open and read "Mandelbrot.YP.raw".
    // After file is processed, the ".ready" file is deleted so we don't reprocess.
    //
    private void ProcessRow(string filename)
    {
      try
      {
        string basename = Path.GetFileNameWithoutExtension(filename);  // Mandelbrot.YP
        string extension = Path.GetExtension(basename);              // .YP
        int yp = Convert.ToInt32(extension.Substring(1));  // this is the row #

        //
        // we have a ".raw" file to process, representing one row, so read it in:
        //
        string rawname = Path.Combine(_basedir, String.Format("Mandelbrot.{0}.raw", yp));
        StreamReader reader;

        while (true)  // repeatedly try to open until .RAW file is available:
        {
          try
          {
            reader = new StreamReader(rawname);
            break;
          }
          catch
          { }
        }//while

        //
        // We build new array of values for each line - we will be passing this array out 
        // when we report progress.  It is possible that event handlers may hang onto this 
        // array, which means we might well manage to generate several lines of data before 
        // the UI thread gets around to dealing with it.  So reusing the same array each 
        // time leads to incorrect results since the worker thread will keep going...
        //
        int[] values = new int[_pixels];

        for (int xp = 0; xp < _pixels; xp++)
          values[xp] = Convert.ToInt32(reader.ReadLine());

        reader.Close();

        //
        // We've generated a row, report this as progress for display (using a new array of
        // args each time for the same reasons as discussed above, i.e. to avoid race 
        // conditions):
        //
        Object[] args = new Object[3];

        args[0] = values;
        args[1] = AppDomain.GetCurrentThreadId();
        args[2] = _serverID;

        _worker.ReportProgress(yp, args);

        //
        // Okay, the last step is to delete the .ready file so we don't process again:
        //
        while (true)  // repeat until we successfully delete:
        {
          try
          {
            File.Delete(filename);
            break;
          }
          catch
          { }
        }//while
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error in Mandelbrot.ProcessRow: " + ex.Message);
        System.Environment.Exit(1);
      }
    }

  }//class
}//namespace
