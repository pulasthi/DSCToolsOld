using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using TwisterDaemonService;

namespace DaemonTestUtility
{
    internal class Program
    {
        private const string TwisterHomeEnvVarName = @"TWISTER_HOME";
        private const string JavaHomeEnvVarName = @"JAVA_HOME";
        private string _classpath = string.Empty;
        private string _javaHome;
        private Dictionary<string, int> _nodes;
        private string _twisterHome;
        private TwisterProperties _twisterProperties;
        
        private void OnStart(string[] args)
        {
            _twisterHome = Environment.GetEnvironmentVariable(TwisterHomeEnvVarName);
            _javaHome = Environment.GetEnvironmentVariable(JavaHomeEnvVarName);
            if (string.IsNullOrEmpty(_twisterHome))
            {
                throw new TwisterDaemonException("Unable to find " + TwisterHomeEnvVarName + " environment variable");
            }

            if (string.IsNullOrEmpty(_javaHome))
            {
                throw new TwisterDaemonException("Unable to find " + JavaHomeEnvVarName + " environment variable");
            }
            _twisterProperties = TwisterProperties.LoadFrom(Path.Combine(_twisterHome, @"bin\twister.properties"));
            _nodes = LoadNodesFrom(Path.Combine(_twisterHome, @"bin\nodes"));
            _classpath = BuildClassPath(_twisterHome);
            const string javaArgs =
                @"-Xmx{0} -Xms{1} -XX:SurvivorRatio={2} -classpath {3} cgl.imr.worker.TwisterDaemon {4} {5}";
            var psi = new ProcessStartInfo(Path.Combine(_javaHome, @"bin\java.exe"))
            {
                UseShellExecute = false,
                RedirectStandardOutput = false,
                CreateNoWindow = false,
            };
            string fullHostName = Dns.GetHostEntry("").HostName.ToUpper();
            int shift = _nodes[fullHostName]*_twisterProperties.DaemonsPerNode;
            for (int i = 0; i < _twisterProperties.DaemonsPerNode; ++i)
            {
                psi.Arguments = string.Format(javaArgs, _twisterProperties.DaemonXmx, _twisterProperties.DaemonXms,
                                              _twisterProperties.DaemonXxSurvivorRatio, _classpath, (shift+i),
                                              _twisterProperties.WorkersPerDaemon);
                Process.Start(psi);
                Thread.Sleep(1000);
            }
        }

        private string BuildClassPath(string twisterHome)
        {
            string twisterBin = Path.Combine(twisterHome, @"bin");
            string twisterLib = Path.Combine(twisterHome, @"lib");
            string path = twisterBin;
            string[] jars = Directory.GetFiles(twisterLib, "*.jar", SearchOption.AllDirectories);
            path += ";" + string.Join(";", jars);
            return path;
        }

        private Dictionary<string, int> LoadNodesFrom(string nodesFile)
        {
            var nodes = new Dictionary<string, int>();
            using (var reader = new StreamReader(nodesFile))
            {
                int count = 0;
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        nodes.Add(line.Trim().ToUpper(), count);
                        ++count;
                    }
                }
            }
            return nodes;
        }

        public void OnStop()
        {
            string fullHostName = Dns.GetHostEntry("").HostName.ToUpper();
            int count = _nodes[fullHostName];
            const string javaArgs =
                @"-Xmx260m -Xms260m -XX:SurvivorRatio=10 -classpath {0} cgl.imr.script.StopDaemons {1} {2} {3}";
            var psi = new ProcessStartInfo(Path.Combine(_javaHome, @"bin\java.exe"))
            {
                UseShellExecute = false,
                RedirectStandardOutput = false,
                CreateNoWindow = false,
                Arguments =
                    string.Format(javaArgs, _classpath, fullHostName, count,
                                  _twisterProperties.DaemonsPerNode),
            };
            Process.Start(psi);
        }

        public static void Main(string[] args)
        {
            var p = new Program();
            p.OnStart(args);
            Console.WriteLine("Twister Daemon Started on node " + Dns.GetHostEntry("").HostName.ToUpper());
            Console.WriteLine("Press any key to stop");
            Console.Read();
            p.OnStop();
            Console.WriteLine("Twister Daemon Stopped on node " + Dns.GetHostEntry("").HostName.ToUpper());
            Console.WriteLine("Press any key to exit");


            /*string fullHostName = Dns.GetHostEntry("").HostName.ToUpper();
            Console.WriteLine(fullHostName);
            Console.WriteLine("Starting process ...");
            StartProcess();
            Console.WriteLine("Done.");*/
            Console.Read();
        }

        public static void StartProcess()
        {
            var start = new ProcessStartInfo(@"C:\sali\pti\sub\salsa\Saliya\c#\TwisterDaemonService\SimpleConsoleApp\bin\x64\Release\SimpleConsoleApp.exe")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false,
            };
            Process p = Process.Start(start);
            p.OutputDataReceived += ProcessOutputHandler;
            p.BeginOutputReadLine();
//            p.WaitForExit();
        }

        private static void ProcessOutputHandler(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            Console.WriteLine(dataReceivedEventArgs.Data);
        }
    }
}