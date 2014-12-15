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

using namespace ParMandelbrot;

Mandelbrot::Mandelbrot(double x, double y, double size, int pixels) : _startTime(0), _worker(nullptr), _canceled(false)
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
// Computes the Mandelbrot image for a given set of rows passed in arg:  arg[0] is the start
// row (inclusive), arg[1] is the ending row (exclusive), and arg[2] is a thread id in the
// range 1..N (where N is the number of threads).  As each row is generated, it is reported
// back to the main form for display.
//
void Mandelbrot::DoCalculate(Object ^arg)
{
  try
  {
    array<Object^> ^args = (array<Object^> ^) arg;

    int startRowInclusive = System::Convert::ToInt32(args[0]);
    int endRowExclusive = System::Convert::ToInt32(args[1]);
    int threadID = System::Convert::ToInt32(args[2]);

    System::Diagnostics::Debug::Assert(threadID > 0, "Thread ID needs to be positive, 1..N");

    for (int yp=startRowInclusive; yp<endRowExclusive; yp++)
    {
      //
      // check if we are supposed to cancel?
      //
      if (_worker->CancellationPending)
      {
        _canceled = true;  // let parent thread know:
        return;            // stop working!
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

      for (int xp = 0; xp <_pixels; xp++)
        values[xp] = MandelbrotColor(yp, xp, _y, _x, _size, _pixels);

      //
      // Set value in last 5 pixels of each row to the thread number, in particular a negative
      // value in the range -1..-N (where N is the number of threads).  Note this intentionally
      // overrides the value set earlier, but we want to leave the calculation above as is on
      // each iteration for fair comparisons between sequential and parallel execution times.
      //
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
    
    //
    // done!
    //
  }
  catch(System::Exception ^ex)
  {
    MessageBox::Show("Error in Mandelbrot.DoCalculate: " + ex->Message);
    System::Environment::Exit(1);
  }
  catch(...)
  {
    MessageBox::Show("Unknown Error in Mandelbrot.DoCalculate");
    System::Environment::Exit(1);
  }
}

//
// Designed to be called asynchronously as part of BackgroundWorker object.  Args for Mandelbrot
// computation are passed in e->Argument, and method will report progress as it completes each
// row of the Mandelbrot computation.
//
// This is now the multi-threaded version, in which threads are created to do the work, and
// the computation is actually done by DoCalculate method.
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

    _canceled = false;  // threads will set this to true if user cancels:

    //
    // now start computing Mandelbrot set by creating a set of threads (one per core) to
    // generate the image row by row:
    //
    int numCores = System::Environment::ProcessorCount;
    int chunkSize = _pixels / numCores;
    int leftOver  = _pixels % numCores;

    array<Thread^> ^threads = gcnew array<Thread^>(numCores);
    array<Object^> ^args;

    for (int i = 0; i < numCores; i++)  // for each core, create & start one thread:
    {
      int start = i*chunkSize;
      int end   = start + chunkSize;
      int id    = i+1;
      
      if (leftOver > 0 && i==numCores-1)  // give any extra rows to the last thread:
        end += leftOver;

      args = gcnew array<Object^>(3);
      args[0] = start;  // startRowInclusive:
      args[1] = end;    // endRowExclusive:
      args[2] = id;    // some sort of thread id, in range 1..N:

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
    if (_canceled)
      e->Cancel = true;
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
