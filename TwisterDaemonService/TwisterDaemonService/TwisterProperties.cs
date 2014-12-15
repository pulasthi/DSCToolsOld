using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwisterDaemonService
{
    public class TwisterProperties
    {
        public int DaemonsPerNode { get; set; }
        public int WorkersPerDaemon { get; set; }
        public string PubSubBroker { get; set; }
        public int DaemonPort { get; set; }
        public string NodesFile { get; set; }
        public string AppDirectory { get; set; }
        public string DataDirectory { get; set; }
        public string DaemonXmx { get; set; }
        public string DaemonXms { get; set; }
        public string DaemonXxSurvivorRatio { get; set; }

        private const string PropNameDaemonsPerNode = @"daemons_per_node";
        private const string PropNameWorkersPerDaemon = @"workers_per_daemon";
        private const string PropNamePubSubBroker = @"pubsub_broker";
        private const string PropNameDaemonPort = @"daemon_port";
        private const string PropNameDaemonXmx = @"daemon_xmx";
        private const string PropNameDaemonXms = @"daemon_xms";
        private const string PropNameDaemonXxSurvivorRatio = @"daemon_xx_survivor_ratio";
        private const string PropNameNodesFile = @"nodes_file";
        private const string PropNameAppDir = @"app_dir";
        private const string PropNameDataDir = @"data_dir";

        
        public static TwisterProperties LoadFrom(string file)
        {
            TwisterProperties tp = new TwisterProperties();
            using (StreamReader reader = new StreamReader(file))
            {
                char[] sep = new[] {'='};
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line) && !line.StartsWith("#"))
                    {
                        string[] splits = line.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                        if (splits.Length == 2)
                        {
                            string value = splits[1].Trim();
                            string prop = splits[0].Trim();
                            switch (prop)
                            {
                                case PropNameDaemonsPerNode:
                                    tp.DaemonsPerNode = int.Parse(value);
                                    break;
                                case PropNameWorkersPerDaemon:
                                    tp.WorkersPerDaemon = int.Parse(value);
                                    break;
                                case PropNamePubSubBroker:
                                    tp.PubSubBroker = value;
                                    break;
                                case PropNameDaemonPort:
                                    tp.DaemonPort = int.Parse(value);
                                    break;
                                case PropNameDaemonXmx:
                                    tp.DaemonXmx = value;
                                    break;
                                case PropNameDaemonXms:
                                    tp.DaemonXms = value;
                                    break;
                                case PropNameDaemonXxSurvivorRatio:
                                    tp.DaemonXxSurvivorRatio = value;
                                    break;
                                case PropNameNodesFile:
                                    tp.NodesFile = value;
                                    break;
                                case PropNameAppDir:
                                    tp.AppDirectory = value;
                                    break;
                                case PropNameDataDir:
                                    tp.DataDirectory = value;
                                    break;
                                default:
                                    throw new Exception("Unknow property: " + prop + "=" +value);
                            }
                        }
                    }
                }
            }
            return tp;

        }

    }
}
