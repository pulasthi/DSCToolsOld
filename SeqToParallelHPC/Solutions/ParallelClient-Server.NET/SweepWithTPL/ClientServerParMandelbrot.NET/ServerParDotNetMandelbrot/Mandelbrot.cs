/* Mandelbrot.cs */

//
// Mandelbrot generation with managed Visual C++
// Joe Hummel, Fritz Onion and Mike Woodring
// Pluralsight LLC
//
// Based on original WinForms C# application developed by Ian Griffiths
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;


namespace ServerParDotNetMandelbrot
{

  public class Mandelbrot
  {

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

    public int Calculate(double x, double y, double size, int pixels, int startRowInclusive, int endRowExclusive, int stepBy, int instanceNum)
    {
      //
      // Start computing Mandelbrot set, row by row:
      //
      Parallel.For(startRowInclusive, endRowExclusive, stepBy, (yp, state) =>
      {
        //
        // are we supposed to cancel?
        //
        if (File.Exists("Mandelbrot.cancel"))
          state.Stop();

        //
        // no cancel, so compute next row...
        //
        int[] values = new int[pixels];

        for (int xp = 0; xp < pixels; xp++)
          values[xp] = MandelbrotColor(yp, xp, y, x, size, pixels);

        //
        // Set value in last 5 pixels of each row to the instance number, in particular a negative
        // value in the range -1..-N (where N is the number of instances).  Note this intentionally
        // overrides the value set earlier, but we want to leave the calculation above as is on
        // each iteration for fair comparisons between sequential and parallel execution times.
        //
        int threadID = ((instanceNum - 1) * Environment.ProcessorCount) + Thread.CurrentThread.ManagedThreadId;
        for (int xp = pixels - 5; xp < pixels; xp++)
          values[xp] = -threadID;

        //
        // we've generated a row, write out as a file for client to process.  We write out 
        // the data to a ".YP.raw" file, where YP denotes the row.  Then we write out a 
        // ".YP.ready" file to denote that the .raw file is ready for processing.  
        //
        string filename;

        filename = String.Format("Mandelbrot.{0}.raw", yp);
        using (StreamWriter writer = new StreamWriter(filename))
        {
          for (int xp = 0; xp < pixels; xp++)
            writer.WriteLine(values[xp]);
        }

        filename = String.Format("Mandelbrot.{0}.ready", yp);
        using (StreamWriter writer = new StreamWriter(filename))
        {
          // just need to create the file, it can be empty:
        }

        //
        // debug output:
        //
        Console.WriteLine("Generated row {0}...", yp);
      });

      //
      // done!
      // 
      if (File.Exists("Mandelbrot.cancel"))
        return -1;
      else
        return 0;
    }

  }//class
}//namespace
