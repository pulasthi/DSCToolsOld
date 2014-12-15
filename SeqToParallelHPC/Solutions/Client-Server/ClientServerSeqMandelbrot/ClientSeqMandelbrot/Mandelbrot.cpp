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

using namespace ClientSeqMandelbrot;


Mandelbrot::Mandelbrot(double x, double y, double size, int pixels) : _startTime(0), _serverID(-1), _worker(nullptr)
{
  _basedir = IO::Path::Combine(System::AppDomain::CurrentDomain->BaseDirectory, "server");

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
// Delete all the files --- .cancel, .ready, and .raw --- from a previous run.
//
void Mandelbrot::CleanupPreviousRun()
{
  try
  {
    String ^filename;
    array<String^> ^files;

    filename = IO::Path::Combine(_basedir, "Mandelbrot.cancel");
    if (IO::File::Exists(filename))
      IO::File::Delete(filename);

    files = IO::Directory::GetFiles(_basedir, "*.raw");
    for (int i = 0; i < files->Length; i++)
      IO::File::Delete(files[i]);

    files = IO::Directory::GetFiles(_basedir, "*.ready");
    for (int i = 0; i < files->Length; i++)
      IO::File::Delete(files[i]);
  }
  catch(System::Exception ^ex)
  {
    MessageBox::Show("Error in Mandelbrot.CleanupPreviousRun: " + ex->Message);
    System::Environment::Exit(1);
  }
  catch(...)
  {
    MessageBox::Show("Unknown Error in Mandelbrot.CleanupPreviousRun");
    System::Environment::Exit(1);
  }
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

    int startRowInclusive = 0;
    int endRowExclusive = _pixels;

    //
    // cleanup previous run:
    //
    this->CleanupPreviousRun();

    //
    // okay, start new run by starting server-side Mandelbrot process...
    //
    _startTime = clock();

    String ^serverFile;
    serverFile = "ServerSeqMandelbrot.exe";

    String ^serverArgs;
    serverArgs = String::Format("{0} {1} {2} {3} {4} {5}", 
      _x,
      _y,
      _size,
      _pixels,
      startRowInclusive,
      endRowExclusive);

    Process ^server = gcnew Process();
    server->StartInfo->FileName = serverFile;
    server->StartInfo->Arguments = serverArgs;
    server->StartInfo->WindowStyle = Diagnostics::ProcessWindowStyle::Hidden;
    server->StartInfo->WorkingDirectory = _basedir;
    server->Start();

    _serverID = server->Id;

    //
    // Server process is now running, wait for data files to appear and process as they do...
    //
    int rowCount = 0;

    while (rowCount < _pixels)
    {
      //
      // check if we are supposed to cancel?
      //
      if (_worker->CancellationPending)
      {
        // 
        // tell the server to stop:
        //
        String ^filename = Path::Combine(_basedir, "Mandelbrot.cancel");
        StreamWriter ^writer = gcnew StreamWriter(filename);
        // just need to create the file, it can be empty:
        writer->Close();

        //
        // now we stop:
        //
        e->Cancel = true;
        break;
      }

      //
      // Otherwise do we have some data to report?
      //
      // NOTE: tried to use FileSystemWatcher class to get events when new files were
      // available.  As concurrency increased, events were lost, so this low-tech approach
      // turns out to be the most reliable mechanism for client-server communication.
      //
      array<String^> ^files = Directory::GetFiles(_basedir, "*.ready");

      for (int i = 0; i < files->Length; i++, rowCount++)
        this->ProcessRow(files[i]);

      //
      // Did we process any files?  If not, and the server has stopped (crashed?), 
      // we should exit loop otherwise we may loop forever...
      //
      if (files->Length == 0 && server->HasExited)
        break;

      //
      // Pause a bit to let the rest of the system do some work:
      //
      Thread::Sleep(100);  // 1/10 sec.
    }//while

    //
    // We're done, cleanup!
    //
    server->WaitForExit();
    server->Close();
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


//
// This method actually reads the data from the file and reports it as progress
// back to the GUI.  The filename will be of the form "Mandelbrot.YP.ready" (where
// YP is the row number), in which case we then open and read "Mandelbrot.YP.raw".
// After file is processed, the ".ready" file is deleted so we don't reprocess.
//
void Mandelbrot::ProcessRow(String ^filename)
{
  try
  {
    String ^basename = Path::GetFileNameWithoutExtension(filename);  // Mandelbrot.YP
    String ^extension = Path::GetExtension(basename);  // .YP
    int yp = Convert::ToInt32(extension->Substring(1));

    //
    // we have a ".raw" file to process, representing one row, so read it in:
    //
    String ^rawname = Path::Combine(_basedir, String::Format("Mandelbrot.{0}.raw", yp));
    StreamReader ^reader;
    
    while (true)  // repeatedly try to open until .RAW file is available:
    {
      try
      {
        reader = gcnew StreamReader(rawname);
        break;
      }
      catch(Exception^)
      { }
    }//while

    //
    // We build new array of values for each line - we will be passing this array out 
    // when we report progress.  It is possible that event handlers may hang onto this 
    // array, which means we might well manage to generate several lines of data before 
    // the UI thread gets around to dealing with it.  So reusing the same array each 
    // time leads to incorrect results since the worker thread will keep going...
    //
    array<int> ^values = gcnew array<int>(_pixels);

    for (int xp = 0; xp < _pixels; xp++)
      values[xp] = Convert::ToInt32(reader->ReadLine());

    reader->Close();

    //
    // We've generated a row, report this as progress for display (using a new array of
    // args each time for the same reasons as discussed above, i.e. to avoid race 
    // conditions):
    //
    array<Object^> ^args = gcnew array<Object^>(3);

    args[0] = values;
    args[1] = AppDomain::CurrentDomain->GetCurrentThreadId();
    args[2] = _serverID;

    _worker->ReportProgress(yp, args);

    //
    // Okay, the last step is to delete the .ready file so we don't process again:
    //
    while (true)  // repeat until we successfully delete:
    {
      try
      {
        File::Delete(filename);
        break;
      }
      catch(Exception^)
      { }
    }
  }
  catch(System::Exception ^ex)
  {
    MessageBox::Show("Error in Mandelbrot.ProcessRow: " + ex->Message);
    System::Environment::Exit(1);
  }
  catch(...)
  {
    MessageBox::Show("Unknown Error in Mandelbrot.ProcessRow");
    System::Environment::Exit(1);
  }
}
