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


namespace SeqDotNetMandelbrot
{

  public class Mandelbrot
  {
    private int _startTime = 0;
    private BackgroundWorker _worker = null;

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
    // Returns a color reflecting the value of the Mandelbrot set element at this position.
    //
    private int MandelbrotColor(double yp, double xp, double y, double x, double size, int pixels)
    {
      //
      // compute pixel position:
      //
      double ypos = y + size * (yp - pixels / 2) / ((double)pixels);
      double xpos = x + size * (xp - pixels / 2) / ((double)pixels);

      //
      // now setup for color computation:
      //
      // Reference: http://en.wikipedia.org/wiki/Mandelbrot_set
      //
      y = ypos;
      x = xpos;

      double y2 = y * y;
      double x2 = x * x;

      int color = 1;

      const int MAXCOLOR = 69887; // affects rendering color

      // This magic number happens to produce a colour approximating black with my 
      // colour picker calculation.  It also makes things pretty slow, which is handy.

      //
      // Repeat until we know pixel is not in Mandelbrot set, or until we have reached max # of
      // iterations, in which case pixel is probably in the set.  In the latter, color will be
      // black.
      //
      while ((y2 + x2) <= 4 && color < MAXCOLOR)
      {
        y = 2 * x * y + ypos;
        x = x2 - y2 + xpos;

        y2 = y * y;
        x2 = x * x;

        color++;
      }

      return color;
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
            return;
          }

          //
          // no cancel, so compute Mandelbrot set for this row:
          //

          //
          // We build new array of values for each line - we will be passing this array out 
          // when we report progress.  It is possible that event handlers may hang onto this 
          // array, which means we might well manage to generate several lines of data before 
          // the UI thread gets around to dealing with it.  So reusing the same array each 
          // time leads to incorrect results since the worker thread will keep going...
          //

          int[] values = new int[_pixels];

          for (int xp = 0; xp < _pixels; ++xp)
            values[xp] = MandelbrotColor(yp, xp, _y, _x, _size, _pixels);

          //
          // we've generated a row, report this as progress for display:
          //
          Object[] args = new Object[2];

          args[0] = values;
          args[1] = AppDomain.GetCurrentThreadId();

          _worker.ReportProgress(yp, args);
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error in Mandelbrot.Calculate: " + ex.Message);
        System.Environment.Exit(1);
      }
    }

  }//class
}//namespace

