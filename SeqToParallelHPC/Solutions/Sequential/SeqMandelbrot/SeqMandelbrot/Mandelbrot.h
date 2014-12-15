/* Mandelbrot.h */

//
// Mandelbrot generation with managed Visual C++
// Joe Hummel, Fritz Onion and Mike Woodring
// Pluralsight LLC
//
// Based on original WinForms C# application developed by Ian Griffiths
//
#pragma once

namespace SeqMandelbrot
{
	using namespace System;
	using namespace System::ComponentModel;
  using namespace System::Windows::Forms;

  public ref class Mandelbrot
  {
  private:
    clock_t _startTime;
    BackgroundWorker ^_worker;

    double _x;  // parameters of Mandelbrot computation:
    double _y;
    double _size;
    int _pixels;

    int MandelbrotColor(double yp, double xp, double y, double x, double size, int pixels);

  public:
    Mandelbrot(double x, double y, double size, int pixels);
    double TimeTaken();
    void Calculate(Object ^sender, DoWorkEventArgs ^e);
  };

}