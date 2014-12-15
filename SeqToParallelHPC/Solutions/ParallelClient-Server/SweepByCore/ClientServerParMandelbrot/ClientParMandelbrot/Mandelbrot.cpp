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

using namespace ClientParMandelbrot;


Mandelbrot::Mandelbrot(double x, double y, double size, int pixels) : _startTime(0), _serverID(-1), _worker(nullptr)
{
  _basedir = ConfigurationSettings::AppSettings["PublicNetworkShare"];

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
  //
  // make sure target directory exists (it may not yet):
  //
  if (!Directory::Exists(_basedir))
    return;

  try
  {
    String ^filename;
    array<String^> ^files;

    filename = Path::Combine(_basedir, "Mandelbrot.cancel");
    if (File::Exists(filename))
      File::Delete(filename);

    files = Directory::GetFiles(_basedir, "*.raw");
    for (int i = 0; i < files->Length; i++)
      File::Delete(files[i]);

    files = Directory::GetFiles(_basedir, "*.ready");
    for (int i = 0; i < files->Length; i++)
      File::Delete(files[i]);
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

    //
    // cleanup previous run:
    //
    this->CleanupPreviousRun();

    //
    // okay, the first step is to deploy the server-side .exe to the cluster's public 
    // share so it can be deployed to the compute nodes:
    //
    String ^serverEXEName = ConfigurationSettings::AppSettings["ServerSideEXEName"];

    if (!Directory::Exists(_basedir))
      Directory::CreateDirectory(_basedir);

    String ^localexe = System::AppDomain::CurrentDomain->BaseDirectory;
    localexe = Path::Combine(localexe, "server");
    localexe = Path::Combine(localexe, serverEXEName);

    String ^destexe = Path::Combine(_basedir, serverEXEName);

    File::Copy(localexe, destexe, true /*overwrite*/);

    //
    // Next, connect to the cluster and create our job:
    //
    IScheduler ^scheduler = gcnew Scheduler();

    scheduler->Connect(ConfigurationSettings::AppSettings["ClusterHeadnode"]);

    ISchedulerJob ^job = scheduler->CreateJob();

    job->Name = ConfigurationSettings::AppSettings["RunAsUserName"] + " Mandelbrot";
    job->IsExclusive = true;        // we want resources to ourselves for better perf
    job->RunUntilCanceled = false;  // stop as soon as tasks finish/fail

    job->AutoCalculateMin = false;  // disable so we can manually set number of resources:
    job->AutoCalculateMax = false;
    job->UnitType = JobUnitType::Core;

    // we want to use all the cores in the cluster, so let's count how many there are:
    ISchedulerCollection ^nodes = scheduler->GetNodeList(nullptr, nullptr);  // all nodes:

    int numCores = 0;
    for (int i = 0; i < nodes->Count; i++)
    {
      ISchedulerNode ^n = (ISchedulerNode^) nodes[i];
      numCores += n->NumberOfCores;
    }

    job->MinimumNumberOfCores = 1;  // request 1..ALL cores in the cluster:
    job->MaximumNumberOfCores = numCores;

    //
    // When the job executes, the first thing it needs to do is deploy the server-side 
    // .EXE down to each compute node's local working folder.  So create a set of tasks
    // to perform this duty, one task for each node:
    //
    localexe = Path::Combine(
      ConfigurationSettings::AppSettings["LocalWorkDirectory"], 
      serverEXEName);

    ISchedulerTask ^task;

    for (int i = 0; i < nodes->Count; i++)  // for each node in cluster:
    {
      //
      // create task to copy .EXE from public share down to local work directory:
      //
      task = job->CreateTask();
      task->CommandLine = String::Format("mkdir {0} & copy /Y {1} {2}", 
        ConfigurationSettings::AppSettings["LocalWorkDirectory"],
        destexe, 
        localexe);

      ISchedulerNode ^n = (ISchedulerNode^) nodes[i];

      // run copy command on this node:
      task->RequiredNodes = gcnew StringCollection();  // must run on this node:
      task->RequiredNodes->Add(n->Name);
      // run just 1 instance:
      task->MinimumNumberOfCores = 1;
      task->MaximumNumberOfCores = 1;
      // name task in a unique way since execution task depends on all deployment tasks:
      task->Name = "Deploy-to-" + n->Name;

      // finally, add to job so it executes:
      job->AddTask(task);
    }

    //
    // Now we define the task to actually run the .EXE.  We parallelize using a parametric
    // sweep, running the .EXE on *every* core in the cluster...
    //
    task = job->CreateTask();

    task->Name = "Mandelbrot Parametric Sweep";
    task->IsParametric = true;
    task->StartValue   = 1;
    task->EndValue     = numCores;

    String ^theArgs= String::Format("{0} {1} {2} {3} * {4}", 
      _x, 
      _y, 
      _size, 
      _pixels, 
      numCores);

    task->CommandLine    = String::Format("{0} {1}", localexe, theArgs);
    task->WorkDirectory  = _basedir;
    task->StdOutFilePath = "MandelbrotOut.*.txt";
    task->StdErrFilePath = "MandelbrotErr.*.txt";

    // the execution task can't run until all the deployment tasks have finished:
    task->DependsOn = gcnew StringCollection();
    for (int i = 0; i < nodes->Count; i++)
    {
      ISchedulerNode ^n = (ISchedulerNode^) nodes[i];
      task->DependsOn->Add("Deploy-to-" + n->Name);
    }

    // all set, add task to the job:
    job->AddTask(task);

    //
    // OK, the job is ready, submit to the cluster for execution!
    //
    scheduler->SetInterfaceMode(false /*GUI*/, (IntPtr) nullptr /*no parent window*/);

    String ^runAsUser = ConfigurationSettings::AppSettings["RunAsUserName"];
    String ^runAsPwd  = ConfigurationSettings::AppSettings["RunAsPassword"];
    if (runAsPwd == "") // force prompt if there's no password:
      runAsPwd = nullptr;  

    scheduler->SubmitJob(job, runAsUser, runAsPwd);

    //
    // At this point the job has been submitted, and eventually the server-side .EXEs 
    // will start running on the cluster.  As they do, they will output image row files.
    // So we wait for wait for data files to appear and process as they do...
    //
    _startTime = clock();

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
        // cancel the job on the cluster:
        //
        scheduler->CancelJob(job->Id, "Canceled by user");

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
      // Did we process any files?  If not, and the job has stopped (failed?), 
      // we should exit loop otherwise we may loop forever...
      //
      if (files->Length == 0) 
      {
        job->Refresh();  // get current state of job on cluster:
      
        if (job->State == JobState::Canceled || job->State == JobState::Failed || job->State == JobState::Finished)
          break;
      }

      //
      // Pause a bit to let the rest of the system do some work:
      //
      Thread::Sleep(100);  // 1/10 sec.
    }//while

    //
    // We're done, but let's check to see why we are done...  If job failed, let's give 
    // the user some feedback:
    //
    job->Refresh();
    if (job->State == JobState::Failed)
    {
      String ^msg = String::Format("Job {0} failed: '{1}'.", job->Id, job->ErrorMessage);
      MessageBox::Show(msg);
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
