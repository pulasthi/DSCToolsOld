/* Main.cpp */

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


namespace ServerSeqDotNetMandelbrot
{

  class Program
  {

    static void Main(string[] args)
    {
      String version, platform;

#if DEBUG
      version = "debug";
#else
  version = "release";
#endif

#if _WIN64
  platform = "64-bit";
#elif _WIN32
      platform = "32-bit";
#else 
  platform = "any-cpu";
#endif

      if (args.Length != 6)
      {
        Console.WriteLine();
        Console.WriteLine("Usage: ServerSeqDotNetMandelbrot.exe x y size pixels startRowInclusive endRowExclusive");
        Console.WriteLine("Exiting...");
        return;
      }

      try
      {
        double x = Convert.ToDouble(args[0]);
        double y = Convert.ToDouble(args[1]);
        double size = Convert.ToDouble(args[2]);
        int pixels = Convert.ToInt32(args[3]);
        int startRowInclusive = Convert.ToInt32(args[4]);
        int endRowExclusive = Convert.ToInt32(args[5]);

        Console.WriteLine("** Server-Side Sequential Mandelbrot .NET [{0} {1}] **", platform, version);
        Console.WriteLine("   x:        {0:0.00}", x);
        Console.WriteLine("   y:        {0:0.00}", y);
        Console.WriteLine("   size:     {0:0.00}", size);
        Console.WriteLine("   pixels:   {0}", pixels);
        Console.WriteLine("   startRow (inclusive): {0}", startRowInclusive);
        Console.WriteLine("   endRow (exclusive):   {0}", endRowExclusive);
        Console.WriteLine();

        int startTime, stopTime;
        double time;
        int rc;

        startTime = System.Environment.TickCount;

        Mandelbrot mandelbrot = new Mandelbrot();
        rc = mandelbrot.Calculate(x, y, size, pixels, startRowInclusive, endRowExclusive);

        if (rc < 0)
          Console.WriteLine("Cancelled...");

        stopTime = System.Environment.TickCount;
        time = (stopTime - startTime) / 1000.0;

        Console.WriteLine();
        Console.WriteLine("** Done! Time: {0:#,##0.00} secs", time);
        Console.WriteLine("** Execution complete.");
      }
      catch (Exception ex)
      {
        Console.WriteLine();
        Console.WriteLine("** Error in ServerSide.Main: " + ex.Message);
        Console.WriteLine("** Halting...");
      }
    }

  }//class
}//namespace
