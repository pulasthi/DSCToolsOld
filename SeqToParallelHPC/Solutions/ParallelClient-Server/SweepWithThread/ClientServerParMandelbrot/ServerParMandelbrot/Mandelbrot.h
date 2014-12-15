/* Mandelbrot.h */

//
// Mandelbrot generation with managed Visual C++
// Joe Hummel, Fritz Onion and Mike Woodring
// Pluralsight LLC
//
// Based on original WinForms C# application developed by Ian Griffiths
//
#pragma once

namespace ServerParMandelbrot
{
  using namespace System;
  using namespace System::IO;
  using namespace System::Threading;

  public ref class Mandelbrot
  {
  private:
    int MandelbrotColor(double yp, double xp, double y, double x, double size, int pixels);
    void DoCalculate(Object ^arg);

  public:
    Mandelbrot();
    int Calculate(double x, double y, double size, int pixels, int startRowInclusive, int endRowExclusive, int stepBy, int instanceNum);
  };

}