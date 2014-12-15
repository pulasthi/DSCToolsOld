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

using namespace ServerParMandelbrot;


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


//
// Computes the Mandelbrot image for a given set of rows passed in arg:  arg[0] is the start
// row (inclusive), arg[1] is the ending row (exclusive), and arg[2] is a thread id in the
// range 1..N (where N is the number of threads).  As each row is generated, it is reported
// back to the main form for display.
//
void Mandelbrot::DoCalculate(Object ^arg)
{
  array<Object^> ^args = (array<Object^> ^) arg;

  double x    = System::Convert::ToDouble(args[0]);
  double y    = System::Convert::ToDouble(args[1]);
  double size = System::Convert::ToDouble(args[2]);
  int pixels  = System::Convert::ToInt32(args[3]);

  int startRowInclusive = System::Convert::ToInt32(args[4]);
  int endRowExclusive   = System::Convert::ToInt32(args[5]);
  int stepBy            = System::Convert::ToInt32(args[6]);
  int threadID          = System::Convert::ToInt32(args[7]);

  //
  // Start computing Mandelbrot set, row by row:
  //
  for (int yp = startRowInclusive; yp < endRowExclusive; yp += stepBy)
  {
    //
    // are we supposed to cancel?
    //
    if (File::Exists("Mandelbrot.cancel"))
      return;

    //
    // no cancel, so compute next row...
    //
    array<int> ^values = gcnew array<int>(pixels);

    for (int xp = 0; xp < pixels; xp++)
      values[xp] = MandelbrotColor(yp, xp, y, x, size, pixels);

    //
    // Set value in last 5 pixels of each row to the instance number, in particular a negative
    // value in the range -1..-N (where N is the number of instances).  Note this intentionally
    // overrides the value set earlier, but we want to leave the calculation above as is on
    // each iteration for fair comparisons between sequential and parallel execution times.
    //
    for (int xp = pixels-5; xp < pixels; xp++)
      values[xp] = -threadID; 

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
}

int Mandelbrot::Calculate(double x, double y, double size, int pixels, int startRowInclusive, int endRowExclusive, int stepBy, int instanceNum)
{
  //
  // now start computing Mandelbrot set by creating a set of threads (one per core) to
  // generate the image row by row:
  //
  int numCores = System::Environment::ProcessorCount;

  array<Thread^> ^threads = gcnew array<Thread^>(numCores);
  array<Object^> ^args;

  for (int i = 0; i < numCores; i++)  // for each core, create & start one thread:
  {
    //
    // recall that we are stepping through the iteration space by "stepBy".  For example,
    // assuming 2 instances, then stepBy is 2, yielding:
    //
    //   instance 1 is stepping 0, 2, 4, 6, 8, 10, ...
    //   instance 2 is stepping 1, 3, 5, 7, 9, 11, ...
    //
    // So we need to parallelize so that the threads start at different places (e.g. 0 and 2),
    // and then step by larger values (e.g. 4).
    //
    int start = startRowInclusive + (i * stepBy);
    int step  = stepBy * 2;

    args = gcnew array<Object^>(8);
    args[0] = x;
    args[1] = y;
    args[2] = size;
    args[3] = pixels;
    args[4] = start;
    args[5] = endRowExclusive;
    args[6] = step;
    args[7] = ((instanceNum - 1) * numCores) + i + 1;  // some sort of thread id:

    threads[i] = gcnew Thread(gcnew ParameterizedThreadStart(this, &Mandelbrot::DoCalculate));
    threads[i]->Start(args);
  }

  // 
  // now we have to wait for the threads to finish:
  //
  for (int i = 0; i < numCores; i++)
    threads[i]->Join();

  // 
  // Done!  Check to see if user canceled the computation, and if so, report back:
  //
  if (File::Exists("Mandelbrot.cancel"))
    return -1;
  else
    return 0;
}
