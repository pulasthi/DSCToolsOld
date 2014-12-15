/* Main.cpp */

//
// Mandelbrot generation with managed Visual C++
// Joe Hummel, Fritz Onion and Mike Woodring
// Pluralsight LLC
//
// Based on original WinForms C# application developed by Ian Griffiths
//

#include "stdafx.h"
#include "Mandelbrot.h"

using namespace System;
using namespace ServerSeqMandelbrot;


int main(array<System::String ^> ^args)
{
  System::String ^version, ^platform;

#ifdef _DEBUG
  version = "debug";
#else
  version = "release";
#endif

#ifdef _WIN64
  platform = "64-bit";
#else
  platform = "32-bit";
#endif

  if (args->Length != 6)
  {
    Console::WriteLine();
    Console::WriteLine(L"Usage: ServerSeqMandelbrot.exe x y size pixels instance numInstances");
    Console::WriteLine(L"Exiting...");
    return 0;
  }

  try
  {
    double x = Convert::ToDouble(args[0]);
    double y = Convert::ToDouble(args[1]);
    double size = Convert::ToDouble(args[2]);
    int pixels = Convert::ToInt32(args[3]);
    int instance = Convert::ToInt32(args[4]);
    int numInstances = Convert::ToInt32(args[5]);

    int startRowInclusive = instance - 1;
    int endRowExclusive   = pixels;
    int stepBy            = numInstances;

    Console::WriteLine(L"** Server-Side Sequential Mandelbrot [{0} {1}] **", platform, version);
    Console::WriteLine(L"   x:        {0:0.00}", x);
    Console::WriteLine(L"   y:        {0:0.00}", y);
    Console::WriteLine(L"   size:     {0:0.00}", size);
    Console::WriteLine(L"   pixels:   {0}", pixels);
    Console::WriteLine(L"   instance:     {0}", instance);
    Console::WriteLine(L"   numInstances: {0}", numInstances);
    Console::WriteLine(L"   startRow (inclusive): {0}", startRowInclusive);
    Console::WriteLine(L"   endRow (exclusive):   {0}", endRowExclusive);
    Console::WriteLine(L"   stepBy:               {0}", stepBy);
    Console::WriteLine();

    clock_t startTime, stopTime;
    double  time;
    int  rc;

    startTime = clock();

    Mandelbrot ^mandelbrot = gcnew Mandelbrot();
    rc = mandelbrot->Calculate(x, y, size, pixels, startRowInclusive, endRowExclusive, stepBy);

    if (rc < 0)
      Console::WriteLine(L"Cancelled...");

    stopTime = clock();
    time = (double(stopTime) - double(startTime)) / CLOCKS_PER_SEC;

    Console::WriteLine();
    Console::WriteLine(L"** Done! Time: {0:#,##0.00} secs", time);
    Console::WriteLine(L"** Execution complete.");

    return 0;
  }
  catch(System::Exception ^ex)
  {
    Console::WriteLine();
    Console::WriteLine(L"** Error in ServerSide.Main: " + ex->Message);
    Console::WriteLine(L"** Halting...");
    return -1;
  }
  catch(...)
  {
    Console::WriteLine();
    Console::WriteLine(L"** Unknown Error in ServerSide.Main");
    Console::WriteLine(L"** Halting...");
    return -1;
  }
}
