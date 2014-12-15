/* Mandelbrot.cpp */

//
// Mandelbrot generation with managed Visual C++
// Joe Hummel, Fritz Onion and Mike Woodring
// Pluralsight LLC
//
// Based on original WinForms C# application developed by Ian Griffiths
//
#include "stdafx.h"
#include "Mandelbrot.h"

using namespace ServerSeqMandelbrot;


Mandelbrot::Mandelbrot()
{ }


//
// Returns a color reflecting the value of the Mandelbrot set element at this position.
//
int Mandelbrot::MandelbrotColor(double yp, double xp, double y, double x, double size, int pixels)
{
  //
  // compute pixel position:
  //
  double ypos = y + size * (yp - pixels/2) / ((double)pixels);
  double xpos = x + size * (xp - pixels/2) / ((double)pixels);

  //
  // now setup for color computation:
  //
  // Reference: http://en.wikipedia.org/wiki/Mandelbrot_set
  //
  y = ypos;
  x = xpos;

  double y2 = y*y;
  double x2 = x*x;

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
    y = 2*x*y + ypos;
    x = x2-y2 + xpos;

    y2 = y*y;
    x2 = x*x;

    color++;
  }

  return color;
}

int Mandelbrot::Calculate(double x, double y, double size, int pixels, int startRowInclusive, int endRowExclusive, int stepBy)
{
  //
  // Start computing Mandelbrot set, row by row:
  //
  for (int yp = startRowInclusive; yp < endRowExclusive; yp += stepBy)
  {
    //
    // are we supposed to cancel?
    //
    if (File::Exists("Mandelbrot.cancel"))
      return -1;

    //
    // no cancel, so compute next row...
    //
    array<int> ^values = gcnew array<int>(pixels);

    for (int xp = 0; xp < pixels; xp++)
      values[xp] = MandelbrotColor(yp, xp, y, x, size, pixels);

    //
    // we've generated a row, write out as a file for client to process.  We write out the data
    // to a ".YP.raw" file, where YP denotes the row.  Then we write out a ".YP.ready" file to
    // denote that the .raw file is ready for processing.  
    //
    String ^filename;
    StreamWriter ^writer;

    filename = String::Format("Mandelbrot.{0}.raw", yp);
    writer = gcnew StreamWriter(filename);

    for (int xp = 0; xp < pixels; xp++)
      writer->WriteLine(values[xp]);

    writer->Close();

    filename = String::Format("Mandelbrot.{0}.ready", yp);
    writer = gcnew StreamWriter(filename);
    // just need to create the file, it can be empty:
    writer->Close();

    //
    // debug output:
    //
    Console::WriteLine(L"Generated row {0}...", yp);
  }//for-each-row

  //
  // done!
  // 
  return 0;
}
