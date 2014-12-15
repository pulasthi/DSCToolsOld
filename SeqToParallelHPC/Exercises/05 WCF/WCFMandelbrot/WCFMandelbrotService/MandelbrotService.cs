using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFMandelbrotService
{

  [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
  public class MandelbrotService : IMandelbrotService
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
    
    //
    // Generate Mandelbrot set for this row:
    //
    public int[] GenerateMandelbrotRow(double yp, double y, double x, double size, int pixels)
    {
      int[] values = new int[pixels];

      for (int xp = 0; xp < pixels; ++xp)
        values[xp] = MandelbrotColor(yp, xp, y, x, size, pixels);

      return values;
    }

  }//class

}//namespace
