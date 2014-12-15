using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace MDSTryout
{
    class MDSRunnable
    {
        private bool _killed;

        private Process _proc;
        private ProcessStartInfo _startInfo;
        private proc_AfterEffectsDelegate _procAfterEffects;
        private proc_OutputDataReceivedDelegate _procOutputDataReceived;
        private proc_ErrorDataReceivedDelegate _procErrorDataReceived;

        public delegate void proc_AfterEffectsDelegate();
        public delegate void proc_ErrorDataReceivedDelegate(object sender, DataReceivedEventArgs e);
        public delegate void proc_OutputDataReceivedDelegate(object sender, DataReceivedEventArgs e);

        public MDSRunnable(string runDir, proc_OutputDataReceivedDelegate procOuputDataReceived, proc_ErrorDataReceivedDelegate procErrorDataReceived, proc_AfterEffectsDelegate procAfterEffects)
        {
            _startInfo = new ProcessStartInfo(Path.Combine(runDir, "go.bat"));
            _startInfo.UseShellExecute = false;
            _startInfo.RedirectStandardOutput = true;
            _startInfo.RedirectStandardError = true;
            _startInfo.CreateNoWindow = true;

            _procOutputDataReceived = procOuputDataReceived;
            _procErrorDataReceived = procErrorDataReceived;
            _procAfterEffects = procAfterEffects;
        }

        private void ProcOnOutputDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            _procOutputDataReceived(sender, dataReceivedEventArgs);
        }

        private void ProcOnErrorDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            _procErrorDataReceived(sender, dataReceivedEventArgs);
        }

        public void run()
        {
            using (_proc = new Process())
            {
                _proc.StartInfo = _startInfo;
                _proc.OutputDataReceived += ProcOnOutputDataReceived;
                _proc.ErrorDataReceived += ProcOnErrorDataReceived;
                _proc.Start();
                _proc.BeginOutputReadLine();
                _proc.BeginErrorReadLine();
                while (!_killed && !_proc.HasExited)
                {
                    //busy wait; seems ugly though :-/
                }

                if (!_killed)
                {
                    _procAfterEffects();
                }
                
            }
        }

        public void kill()
        {
            if (_proc != null && !_proc.HasExited)
            {
                _proc.OutputDataReceived -= ProcOnOutputDataReceived;
                _proc.ErrorDataReceived -= ProcOnErrorDataReceived;
                _proc.Kill();
//                _proc.WaitForExit();
               
                // Todo: Saliya - not thread safe
                _killed = true;
            }
        }
    }
}
