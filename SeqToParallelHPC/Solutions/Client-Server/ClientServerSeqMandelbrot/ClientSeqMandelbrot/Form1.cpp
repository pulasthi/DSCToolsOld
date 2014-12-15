/* Form1.cpp */

//
// Mandelbrot generation with managed Visual C++
// Joe Hummel, Fritz Onion and Mike Woodring
// Pluralsight LLC
//
// Based on original WinForms C# application developed by Ian Griffiths
//
#include "stdafx.h"
#include "Form1.h"

using namespace ClientSeqMandelbrot;


//
// Constructor:
//
Form1::Form1() : _image(nullptr), _running(false), _mandelbrot(nullptr), _worker(nullptr)
{
  InitializeComponent();

  _imagePanel->Paint += gcnew PaintEventHandler(this, &Form1::_imagePanel_Paint);
}


//
// Finalizer:
//
Form1::~Form1()
{
  if (components)
    delete components;
}


//
// Called when form is first loaded for display:
//
void Form1::Form1_Load(Object^ sender, EventArgs^ e)
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

  _versionLabel->Text = String::Format("Client-server sequential, {0} {1}", platform, version);

  _clientLabel->Text = String::Format("Process:{0}, Main UI:{1}, Worker:{2}.",
    Diagnostics::Process::GetCurrentProcess()->Id,
    AppDomain::CurrentDomain->GetCurrentThreadId(),
    "?");
  _serverLabel->Text = String::Format("Process:{0}.",
    "?");
}


//
// User clicks the "Go" button to start the mandelbrot calculation, or "Cancel" to stop:
//
void Form1::GoButton_Click(Object ^sender, EventArgs ^e)
{
  //
  // button operates in two states, "Go" or "Cancel"...
  //
  if (!_running)  // GO:
  {
    try
    {
      //
      // parameters for a standard Mandelbrot computation and graphic display:
      //
      double x = -0.70;
      double y = 0;
      double size = 2.5;
      int pixels = 600;

      _mandelbrot = gcnew Mandelbrot(x, y, size, pixels);

      //
      // Reset bitmap and display:
      //
      if (_image != nullptr)
        delete _image;

      // Size panel:
      _imagePanel->Width = pixels;
      _imagePanel->Height = pixels;

      _image = gcnew Bitmap(pixels, pixels);
      _imagePanel->Invalidate();

      _goButton->Text = "&Cancel";

      _running = true;

      // 
      // Okay, here we go, kick off the Mandelbrot computation on a worker (background) thread.
      // We use a separate thread for computation so that the main thread can monitor the UI and
      // be responsive to the user (e.g. cancel) and show progres...
      //
      _worker = gcnew System::ComponentModel::BackgroundWorker();

      _worker->WorkerReportsProgress = true;
      _worker->WorkerSupportsCancellation = true;

      // setup event handler for the actual Mandelbrot computation:
      _worker->DoWork += gcnew System::ComponentModel::DoWorkEventHandler(_mandelbrot, &Mandelbrot::Calculate);
      // setup event handler for progress reports (each row of Mandelbrot set):
      _worker->ProgressChanged += gcnew System::ComponentModel::ProgressChangedEventHandler(this, &Form1::OnProgress);
      // setup event handler for completion notification:
      _worker->RunWorkerCompleted += gcnew System::ComponentModel::RunWorkerCompletedEventHandler(this, &Form1::OnComplete);

      //
      // Ok, start the worker running!
      //
      _worker->RunWorkerAsync(nullptr);  // start worker thread running!
    }
    catch(System::Exception ^ex)
    {
      MessageBox::Show("Error in Form1.goButton_Click.Go: " + ex->Message);
      System::Environment::Exit(1);
    }
    catch(...)
    {
      MessageBox::Show("Unknown Error in Form1.goButton_Click.Go");
      System::Environment::Exit(1);
    }
  }
  else  // Cancel:
  {
    try
    {
      //
      // Button was in 'Cancel' mode...  Tell worker to stop, and wait for completion callback.
      // For now, just gray out the button and change the mouse pointer...
      //
      _goButton->Enabled = false;
      this->Cursor = Cursors::AppStarting;

      _worker->CancelAsync();
    }
    catch(System::Exception ^ex)
    {
      MessageBox::Show("Error in Form1.goButton_Click.Cancel: " + ex->Message);
      System::Environment::Exit(1);
    }
    catch(...)
    {
      MessageBox::Show("Unknown Error in Form1.goButton_Click.Cancel");
      System::Environment::Exit(1);
    }
  }
}


//
// Computation has completed, we get called back by the worker thread when this happens.
//
void Form1::OnComplete(Object ^sender, RunWorkerCompletedEventArgs ^e)
{
  //
  // void Form1::OnComplete()
  //
  try
  {
    if (e->Error != nullptr)  // worker failed:
    {
      _timeLabel->Text += "  (ERROR)";

      MessageBox::Show("Error: " + e->Error->ToString());
    }
    else if (e->Cancelled)  // worker was told to cancel:
    {
      _timeLabel->Text += "  (CANCELED)";
    }
    else  // worker successfully ran to completion:
    {
      _timeLabel->Text += "  (DONE)";
    }

    //
    // No matter what, reset the UI:
    //
    _goButton->Enabled = true;
    _goButton->Text = "&Go";
    this->Cursor = Cursors::Default;

    _running = false;
  }
  catch(System::Exception ^ex)
  {
    MessageBox::Show("Error in Form1.OnComplete: " + ex->Message);
    System::Environment::Exit(1);
  }
  catch(...)
  {
    MessageBox::Show("Unknown Error in Form1.OnComplete");
    System::Environment::Exit(1);
  }
}


//
// Called each time we have new data to display:  this is called by the worker thread each
// time it calculates a new row of the Mandelbrot set.
//
void Form1::OnProgress(Object ^sender, ProgressChangedEventArgs ^e)
{
  //
  // void Form1::OnProgress(int currLine, array<int> ^values, int workerID, int serverID);
  //
  try
  {
    int currLine = e->ProgressPercentage;

    array<Object^> ^args = (array<Object^> ^) e->UserState;
    array<int> ^values   = (array<int> ^) args[0];
    int workerID         = (int) args[1];
    int serverID         = (int) args[2];

    _clientLabel->Text = String::Format("Process:{0}, Main UI:{1}, Worker:{2}.",
      Diagnostics::Process::GetCurrentProcess()->Id,
      AppDomain::CurrentDomain->GetCurrentThreadId(),
      workerID);
    _serverLabel->Text = String::Format("Process:{0}.",
      serverID);

    for (int x=0; x<values->Length; x++)
    {
      int v = values[x];
      Color c = Color::FromArgb(255-(v*3 % 256), 255-(v*7 % 256), 255-(v*13 % 256));

      _image->SetPixel(x, currLine, c);
    }

    System::Drawing::Rectangle invalidRect(0, currLine, _image->Width, 1);
    _imagePanel->Invalidate(invalidRect);
    _timeLabel->Text = _mandelbrot->TimeTaken().ToString("F4") + "s";
  }
  catch(System::Exception ^ex)
  {
    MessageBox::Show("Error in Form1.OnProgress: " + ex->Message);
    System::Environment::Exit(1);
  }
  catch(...)
  {
    MessageBox::Show("Unknown Error in Form1.OnProgress");
    System::Environment::Exit(1);
  }
}


//
// Called to update the display:
//
void Form1::_imagePanel_Paint(Object ^sender, PaintEventArgs ^e)
{
  if (_image != nullptr)
    e->Graphics->DrawImage(_image, 0, 0);
}


//
// User is closing the form, stop any work if we have some running:
//
void Form1::Form1_FormClosed(Object ^sender, EventArgs ^e)
{
  if (_running)
  {
    _worker->CancelAsync();
    System::Threading::Thread::Sleep(500);  // give worker time to process:
  }
}
