using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.ServiceProcess;
using System.Threading;

namespace TwisterDaemonService
{
    public partial class TwisterDaemonService : ServiceBase
    {
        private const string TwisterHomeEnvVarName = @"TWISTER_HOME";
        private const string JavaHomeEnvVarName = @"JAVA_HOME";
        private string _classpath = string.Empty;
        private string _javaHome;
        private Dictionary<string, int> _nodes;
        private string _twisterHome;
        private TwisterProperties _twisterProperties;

        private IList<StreamWriter> _logWriters;
        private IList<Process> _daemons;

        public TwisterDaemonService()
        {
            InitializeComponent();
            if (!EventLog.SourceExists("TwisterDaemonEventLogSource"))
            {
                EventLog.CreateEventSource(
                    "TwisterDaemonEventLogSource", "TwisterDaemonEventLog");
            }
            twisterEventLog.Source = "TwisterDaemonEventLogSource";
            twisterEventLog.Log = "TwisterDaemonEventLog";
        }

        protected override void OnStart(string[] args)
        {
            twisterEventLog.WriteEntry("In OnStart.");
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
                              RedirectStandardOutput = true,
                              CreateNoWindow = false,
                          };

            string fullHostName = Dns.GetHostEntry("").HostName.ToUpper();
            int shift = _nodes[fullHostName] * _twisterProperties.DaemonsPerNode;

            _logWriters = new List<StreamWriter>();
            _daemons = new List<Process>();
            for (int i = 0; i < _twisterProperties.DaemonsPerNode; ++i)
            {
                StreamWriter writer = new StreamWriter(Path.Combine(_twisterHome, "Daemon." + (shift+i) + ".log.txt"));
                _logWriters.Add(writer);
                psi.Arguments = string.Format(javaArgs, _twisterProperties.DaemonXmx, _twisterProperties.DaemonXms,
                                              _twisterProperties.DaemonXxSurvivorRatio, _classpath, (shift + i),
                                              _twisterProperties.WorkersPerDaemon);
                Process p =  new Process();
                _daemons.Add(p);
                p.OutputDataReceived += (sender, eventArgs) => writer.WriteLine(eventArgs.Data);
                p.StartInfo = (psi);
                p.Start();
                p.BeginOutputReadLine();
//                Thread.Sleep(1000);
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

        protected override void OnStop()
        {
            EventLog.WriteEntry("In OnStop.");
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
            foreach (var writer in _logWriters)
            {
                writer.Flush();
                writer.Close();
                writer.Dispose();
            }

            foreach (var daemon in _daemons)
            {
                daemon.CancelOutputRead();
            }
        }
    }
}