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
#include <omp.h>

using namespace OpenMPMandelbrot;

Mandelbrot::Mandelbrot(double x, double y, double size, int pixels) : _startTime(0), _worker(nullptr)
{
  _x = x;  // parameters of Mandelbrot computation: 
  _y = y;
  _size = size;
  _pixels = pixels;
}

double Mandelbrot::TimeTaken()
{
  clock_t curTime = clock();
  double time = (double(curTime) - double(_startTime)) / CLOCKS_PER_SEC;

  return time;
}

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
// Designed to be called asynchronously as part of BackgroundWorker object.  Args for Mandelbrot
// computation are passed in e->Argument, and method will report progress as it completes each
// row of the Mandelbrot computation.
//
void Mandelbrot::Calculate(Object ^sender, DoWorkEventArgs ^e)
{
  //
  // void Mandelbrot::Calculate(nullptr)
  //
  try
  {
    _worker = (BackgroundWorker ^) sender;

    _startTime = clock();

    //
    // now start computing Mandelbrot set, row by row:
    //
    omp_set_num_threads(4);

#pragma omp parallel for schedule(dynamic, 20)
    for (int yp=0; yp<_pixels; yp++)
    {
      //
      // check if we are supposed to cancel?
      //
      if (_worker->CancellationPending)
      {
        e->Cancel = true;
        continue;
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
      array<int> ^values = gcnew array<int>(_pixels);

      for (int xp = 0; xp < _pixels; xp++)
        values[xp] = MandelbrotColor(yp, xp, _y, _x, _size, _pixels);

      //
      // Set value in last 5 pixels of each row to the thread number, in particular a negative
      // value in the range -1..-N (where N is the number of threads).  Note this intentionally
      // overrides the value set earlier, but we want to leave the calculation above as is on
      // each iteration for fair comparisons between sequential and parallel execution times.
      //
      int threadID = omp_get_thread_num() + 1;  // 1..N:

      for (int xp = _pixels-5; xp < _pixels; xp++)
        values[xp] = -threadID;

      //
      // we've generated a row, report this as progress for display:
      //
      array<Object^> ^args = gcnew array<Object^>(2);

      args[0] = values;
      args[1] = AppDomain::CurrentDomain->GetCurrentThreadId();

      _worker->ReportProgress(yp, args);
    }
  }
  catch(System::Exception ^ex)
  {
    MessageBox::Show("Error in Mandelbrot.Calculate: " + ex->Message);
    System::Environment::Exit(1);
  }
  catch(...)
  {
    MessageBox::Show("Unknown Error in Mandelbrot.Calculate");
    System::Environment::Exit(1);
  }
}
