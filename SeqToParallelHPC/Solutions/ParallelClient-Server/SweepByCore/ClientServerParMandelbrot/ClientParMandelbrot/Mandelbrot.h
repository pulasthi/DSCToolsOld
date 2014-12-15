/* Mandelbrot.h */

//
// Mandelbrot generation with managed Visual C++
// Joe Hummel, Fritz Onion and Mike Woodring
// Pluralsight LLC
//
// Based on original WinForms C# application developed by Ian Griffiths
//
#pragma once

namespace ClientParMandelbrot
{
	using namespace System;
	using namespace System::ComponentModel;
  using namespace System::Diagnostics;
  using namespace System::IO;
  using namespace System::Threading;
  using namespace System::Windows::Forms;
  using namespace System::Configuration;
  using namespace Microsoft::Hpc::Scheduler;
  using namespace Microsoft::Hpc::Scheduler::Properties;

  public ref class Mandelbrot
  {
  private:
    clock_t _startTime;
    int _serverID;
    BackgroundWorker ^_worker;
    String ^_basedir;

    double _x;  // parameters of Mandelbrot computation:
    double _y;
    double _size;
    int _pixels;

    void CleanupPreviousRun();
    void Mandelbrot::ProcessRow(String ^filename);

  public:
    Mandelbrot(double x, double y, double size, int pixels);
    double TimeTaken();
    void Calculate(Object ^sender, DoWorkEventArgs ^e);
  };

}