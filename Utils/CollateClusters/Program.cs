using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace CollateClusters
{
    class Program
    {
        private static List<Color> _matlab50Colors;
        private const string LabelsFileTag = "labelsFile";
        private const string NoneTag = "none";
        private const string LabelsSeperator = ".";
        private const int ClusterIndexBase = 0;
        static void Main(string[] args)
        {
            LoadMatlab50Colors();

            /*
             * pointsDir	path-to-points-directory
             * pointFilePattern file-name-pattern-of-points-file
             * clustersDir	path-to-cluster-directory
             * C is main region number
             * N is how many sub regions in C
             * x is sub region number, i.e. 0<= x <= N-1
             * y is how many sub sub regions for x
             * n is which value out of y to pick
             * clusterDirpattern	{C}({N})_{x}({y}) 
             * clusterFilePattern	cluster-M{y}-C{n}.txt ; n <= y
             * outDir	path-to-output-folder
             * outFilePattern file-name-pattern-for-output
             */
            using (StreamReader partitionReader = new StreamReader(args[0]))
            {
                char[] sep = new[] {' ','\t'};
                string pointsDir = partitionReader.ReadLine().Trim().Split(sep)[1];
                string pointFilePattern = partitionReader.ReadLine().Trim().Split(sep)[1];
                string clustersDir = partitionReader.ReadLine().Trim().Split(sep)[1];
                string clusterDirPattern = partitionReader.ReadLine().Trim().Split(sep)[1];
                string clusterFilePattern = partitionReader.ReadLine().Trim().Split(sep)[1];
                string outDir = partitionReader.ReadLine().Trim().Split(sep)[1];
                string outFilePattern = partitionReader.ReadLine().Trim().Split(sep)[1];
                string outPlotFilePattern = partitionReader.ReadLine().Trim().Split(sep)[1];
                string outLabelsFilePattern = partitionReader.ReadLine().Trim().Split(sep)[1];
                string sepereateFilesPattern = partitionReader.ReadLine().Trim().Split(sep)[1];

                string line;
                while (!partitionReader.EndOfStream)
                {
                    line = partitionReader.ReadLine();
                    if (!string.IsNullOrEmpty(line) && line.StartsWith("C"))
                    {
                        CollateRegion(int.Parse(line.Trim().Split(sep)[1]), partitionReader, pointsDir, pointFilePattern,
                                      clustersDir, clusterDirPattern, clusterFilePattern, outDir, outFilePattern, outPlotFilePattern, outLabelsFilePattern, sepereateFilesPattern, sep);
                    }
                }
            }
        }

        private static void LoadMatlab50Colors()
        {
            if (File.Exists("Matlab50.txt"))
            {
                using (StreamReader reader = new StreamReader("Matlab50.txt"))
                {
                    _matlab50Colors = new List<Color>();
                    char[] sep = new[] { ' ', '\t' };
                    string[] splits;
                    string split;
                    int startIdx = 3, endIdx;
                    int r, g, b, a;
                    while (!reader.EndOfStream)
                    {
                        splits = reader.ReadLine().Trim().Split(sep);

                        split = splits[0];
                        r = int.Parse(split.Substring(startIdx, (split.Length - (startIdx + 1))));

                        split = splits[1];
                        g = int.Parse(split.Substring(startIdx, (split.Length - (startIdx + 1))));

                        split = splits[2];
                        b = int.Parse(split.Substring(startIdx, (split.Length - (startIdx + 1))));

                        split = splits[3];
                        a = int.Parse(split.Substring(startIdx, (split.Length - (startIdx + 1))));

                        _matlab50Colors.Add(Color.FromArgb(a, r, g, b));
                    }
                }
            }
        }

        static void CollateRegion(int C, StreamReader reader,
            string pointsDir, string pointFilePattern, 
            string clusterDir, string clusterDirPattern, string clusterFilePattern, 
            string outDir, string outFilePattern, string outPlotFilePattern, string outLabelsPattern, string outSeperateFilePattern, char[] sep)
        {
            string readLine = reader.ReadLine();
            int N = int.Parse(readLine.Trim().Split(sep)[1]);

            Hashtable joinedTable = new Hashtable();
            Hashtable invJoinedTable = new Hashtable();
            readLine = reader.ReadLine();
            string[] groups = readLine.Trim().Split(sep);
            if (!NoneTag.Equals(groups[1]))
            {
                int[] group;
                for (int i = 1; i < groups.Length; ++i)
                {
                    group = groups[i].Split('+').Select(x => int.Parse(x)).ToArray();
                    Array.Sort(group);
                    for (int j = 1; j < group.Length; ++j)
                    {
                        joinedTable.Add(group[j], group[0]);
                    }
                    invJoinedTable.Add(group[0], group);
                }
            }

            readLine = reader.ReadLine();
            string labelsFile = readLine.Trim().Split(sep)[1];
            Hashtable labelsTable = new  Hashtable(N);
            if (NoneTag.Equals(labelsFile))
            {
                for (int i = 0; i < N; ++i)
                {
                    labelsTable.Add(i,
                                    joinedTable.Contains(i)
                                        ? ("{" + string.Join("+", invJoinedTable[joinedTable[i]]) + "}")
                                        : invJoinedTable.Contains(i)
                                              ? ("{" + string.Join("+", invJoinedTable[i]) + "}")
                                              : i.ToString(CultureInfo.InvariantCulture));
                }
            }
            else
            {
                if (labelsFile.StartsWith("\""))
                {
                    int idx = readLine.IndexOf(labelsFile, StringComparison.Ordinal)+1;
                    labelsFile = readLine.Substring(idx, readLine.Length - idx -1);
                }
                using (StreamReader labelsReader = new StreamReader(labelsFile))
                {
                    string[] splits;
                    // Note. labels file must have N number of entries
                    while (!labelsReader.EndOfStream)
                    {
                        splits = labelsReader.ReadLine().Trim().Split(sep);
                        labelsTable.Add(int.Parse(splits[0]), splits[1]);
                    }
                    Hashtable tempLabelsTable = new Hashtable();
                    for (int i = 0; i < labelsTable.Count; i++)
                    {
                        if (joinedTable.Contains(i) || invJoinedTable.Contains(i))
                        {
                            var joinedGroup = (int[])(joinedTable.Contains(i) ? invJoinedTable[joinedTable[i]] : invJoinedTable[i]);
                            tempLabelsTable[i] = "{" +
                                                 string.Join("+", joinedGroup.Select(c => (string) labelsTable[c])) +
                                                 "}";
                        }
                        else
                        {
                            tempLabelsTable[i] = labelsTable[i];
                        }
                    }
                    labelsTable = tempLabelsTable;
                }
            }
            reader.ReadLine();// ignore xyn line
            int[] xyn;
            Hashtable xyntable = new Hashtable();
            while (!reader.EndOfStream)
            {
                readLine = reader.ReadLine();
                
                if (string.IsNullOrEmpty(readLine)) break;
                xyn = readLine.Trim().Split(sep).Select(x => int.Parse(x)).ToArray();
                xyntable.Add(xyn[0], xyn);
            }

            string clusterFile;
            string clusterOutFile;
            Hashtable clusterReaders = new Hashtable(xyntable.Count);
            Hashtable clusterWriters = new Hashtable(xyntable.Count);

            foreach (int[] arr in xyntable.Values)
            {
                clusterFile = Path.Combine(clusterDir, C.ToString(),
                                           string.Format(clusterDirPattern, C, N, arr[0], arr[1]),
                                           string.Format(clusterFilePattern, arr[1], arr[2]));
                clusterOutFile = Path.Combine(clusterDir, C.ToString(),
                                           string.Format(clusterDirPattern, C, N, arr[0], arr[1]),
                                           string.Format(outSeperateFilePattern, arr[0]));
                clusterWriters.Add(arr[0], new StreamWriter(clusterOutFile));
                clusterReaders.Add(arr[0], new StreamReader(clusterFile));

            }

            Hashtable startTable = new Hashtable(N);
            int lastStartIdx = 0;
            int lastStart = 0;
            startTable[0] = lastStart;
            for (int i = 1; i < N; ++i)
            {
                if (!joinedTable.ContainsKey(i))
                {
                    startTable[i] = lastStart +
                                    (xyntable.ContainsKey(lastStartIdx) ? ((int[]) xyntable[lastStartIdx])[2] : 1);
                    lastStart = (int) startTable[i] ;
                    lastStartIdx = i;
                }

            }

            int outN = lastStart + (xyntable.ContainsKey(lastStartIdx) ? ((int[]) xyntable[lastStartIdx])[2] : 1);

            //string ptsFile = Path.Combine(pointsDir, C.ToString(), string.Format(pointFilePattern, C));
            //string outTxtFile = Path.Combine(outDir, C.ToString(), string.Format(outFilePattern, C, N, C, outN));
            //string outPlotFile = Path.Combine(outDir, C.ToString(), string.Format(outPlotFilePattern, C, N, C, outN));
            //string outLabelsFile = Path.Combine(outDir, C.ToString(), string.Format(outLabelsPattern, C, N, C, outN));

            string ptsFile = Path.Combine(pointsDir, string.Format(pointFilePattern, C));
            string outTxtFile = Path.Combine(outDir, string.Format(outFilePattern, C, N, C, outN));
            string outPlotFile = Path.Combine(outDir, string.Format(outPlotFilePattern, C, N, C, outN));
            string outLabelsFile = Path.Combine(outDir, string.Format(outLabelsPattern, C, N, C, outN));

            XElement plotviz = new XElement("plotviz");
            XElement plot = CreatePlotElement(Path.GetFileNameWithoutExtension(outPlotFile));
            XElement clusters = new XElement("clusters");
            XElement points = new XElement("points");
            plotviz.Add(plot);
            plotviz.Add(clusters);
            plotviz.Add(points);

            using (StreamReader ptsFileReader = new StreamReader(ptsFile))
            {
                using (StreamWriter outTxtWriter = new StreamWriter(outTxtFile), outLabelsWriter = new StreamWriter(outLabelsFile))
                {
                    string[] splits;
                    int x, outX, subClusterNumber, subIndex;
                    string outLabel;
                    string line;
                    Hashtable outLabelsTable = new Hashtable(outN);
                    while (!ptsFileReader.EndOfStream)
                    {
                        splits = ptsFileReader.ReadLine().Trim().Split(sep);
                        x = int.Parse(splits[4]);
                        x = joinedTable.Contains(x) ? (int)joinedTable[x] : x;
                        outX = ((int) startTable[x]);
                        line = (clusterReaders.ContainsKey(x)
                                                ? ((StreamReader)clusterReaders[x]).ReadLine() : null);
                        subClusterNumber = (line != null
                                                ? (int.Parse(
                                                    line.Trim().Split(sep)[1]) -
                                                   ClusterIndexBase) // ClusterIndexBase = 1 if PWC outputs 1 based indices or zero otherwise
                                                : 0);
                        subIndex = (line != null
                                                ? (int.Parse(
                                                    line.Trim().Split(sep)[0]) -
                                                   ClusterIndexBase) // ClusterIndexBase = 1 if PWC outputs 1 based indices or zero otherwise
                                                : 0);
                        outX += subClusterNumber;
                        outTxtWriter.WriteLine(splits[0] + "\t" + outX);

                        //((StreamWriter)clusterWriters[x]).WriteLine(subIndex + "\t" + splits[1] + "\t" + splits[2] + "\t" + splits[3] + "\t" + subClusterNumber + "\t" + subClusterNumber);
                        // Add point element
                        points.Add(CreatePointElement(int.Parse(splits[0]), outX, outX.ToString(), double.Parse(splits[1]),
                                                      double.Parse(splits[2]), double.Parse(splits[3])));

                        outLabel = ((string) labelsTable[x]);

                        // Note. xyntable.ContainsKey(x) is equivalent to clusterReaders.ContainsKey(x)
                        if (xyntable.ContainsKey(x))
                        {
                            outLabel += LabelsSeperator + subClusterNumber;
                        }

                        // Add cluster element
                        if (!outLabelsTable.ContainsKey(outX))
                        {
                            outLabelsTable.Add(outX, outLabel); 
                            clusters.Add(CreateClusterElement(outX, outLabel, _matlab50Colors[outX%50]));
                        }
                    }

                    foreach (DictionaryEntry kv in outLabelsTable)
                    {
                        outLabelsWriter.WriteLine(kv.Key+"\t"+kv.Value);
                    }

                    plotviz.Save(outPlotFile);
                }
            }

            //Close cluster writers
           // foreach (int[] arr in xyntable.Values)
         //   {
         //       ((StreamWriter)clusterWriters[arr[0]]).Close();
         //   }

            // Close cluster readers
            foreach (int[] arr in xyntable.Values)
            {
                clusterFile = Path.Combine(clusterDir, C.ToString(),
                                           string.Format(clusterDirPattern, C, N, arr[0], arr[1]),
                                           string.Format(clusterFilePattern, arr[1], arr[2]));
                var clusterReader = ((StreamReader) clusterReaders[arr[0]]);
                if (!clusterReader.EndOfStream)
                {
                  //  if (File.Exists(outTxtFile)) File.Delete(outTxtFile);
                    string remaining = string.Empty;
                    while (!clusterReader.EndOfStream)
                    {
                        remaining += clusterReader.ReadLine();
                    }
                  //  throw new Exception("Unfinished stream for file: " + clusterFile +"\n" + remaining);
                }
            }
        }

        private static XElement CreatePlotElement(string name)
        {
            XElement plot =
                new XElement("plot",
                             new XElement("title", name),
                             new XElement("pointsize", 1),
                             new XElement("glyph",
                                          new XElement("visible", 0),
                                          new XElement("scale", 1)));
            return plot;
        }

        private static XElement CreateClusterElement(int key, string label, Color defColor)
        {
            XElement cluster =
                new XElement("cluster",
                             new XElement("key", key),
                             new XElement("label", label),
                             new XElement("visible", 1),
                             new XElement("default", 0),
                             new XElement("color",
                                          new XAttribute("r", defColor.R),
                                          new XAttribute("g", defColor.G),
                                          new XAttribute("b", defColor.B),
                                          new XAttribute("a", defColor.A)),
                             new XElement("size", 1));
            return cluster;
        }

        private static XElement CreatePointElement(int key, int clusterKey, string label, double x, double y, double z)
        {
            XElement point =
                new XElement("point",
                             new XElement("key", key),
                             new XElement("clusterkey", clusterKey),
                             new XElement("label", label),
                             new XElement("location",
                                          new XAttribute("x", x),
                                          new XAttribute("y", y),
                                          new XAttribute("z", z)));
            return point;
        }
    }
}
