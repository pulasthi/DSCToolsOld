using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrzCoefficientCheckInPWCResults
{
    class Program
    {
        static void Main(string[] args)
        {
            double frzCutOff = 0.01;
            string dir = args[0];
            
            if (Directory.Exists(dir))
            {
                string[] dirs = Directory.GetDirectories(dir);
                string outputFile = Path.Combine(dir, "FrzCoefficients_" +Path.GetFileName(dir) + ".txt");
                using (StreamWriter writer = new StreamWriter(outputFile))
                {
                    writer.WriteLine("RunName,Cluster,FrzCoeff,FrzCut,Decision");
                    foreach (string s in dirs)
                    {
                        AppendFrzCoeffs(writer, s, frzCutOff);
                    }
                }
            }
            Console.WriteLine("Done.");
            Console.Read();            
        }

        private static void AppendFrzCoeffs(StreamWriter writer, string dir, double frzCutOff)
        {
            string clusteringRunName = Path.GetFileName(dir);
//            writer.WriteLine("\n\nFrz Coefficients for " + clusteringRunName);
//            writer.WriteLine(new string('-',40));

            string[] files = Directory.GetFiles(dir, "*summary.txt");
            if (files.Length > 0)
            {
                
                using (StreamReader reader = new StreamReader(Path.Combine(dir, files[0])))
                {
                    string frzLine = string.Empty;
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line) && line.StartsWith("0 C"))
                        {
                            frzLine = line;
                        }
                    }

                    string[] frzClusters = frzLine.Split(new[] {'*'}, StringSplitOptions.RemoveEmptyEntries);
                    if (frzClusters.Length < 1) throw new Exception("Wrong frzClusters: " + frzLine);
                    foreach (string frzCluster in frzClusters)
                    {
                        string[] frzInfo = frzCluster.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                        if (frzInfo.Length != 9 && frzInfo.Length != 7)
                            throw new Exception("Wrong frzInfo: " + frzCluster);
                        double frzCoeff = double.Parse(frzInfo[4].Substring(0, frzInfo[4].Length - 1));

                        if (frzCoeff > frzCutOff)
                        {
//                            string decision = frzCoeff <= frzCutOff ? "Converged" : "NotConverged";
                            writer.WriteLine(clusteringRunName + "," + frzInfo[0] + "," + frzCoeff + "," + frzCutOff +
                                             ",NotConverged");
                        }

                    }
                }
            }
            else
            {
//                writer.WriteLine("\tSummary file not found!");
            }
        }
    }
}
