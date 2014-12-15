using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffectiveParameters
{
    class Program
    {
        static void Main(string[] args)
        {
//            FindEffectiveParams();
            AlignParamFiles();
        }

        private static void AlignParamFiles()
        {
            var referenceParams = @"C:\Users\sekanaya\Desktop\junkcharge2effectiveparams.txt";
            var toAlignParams = @"C:\Users\sekanaya\Desktop\junkeffectiveparams.txt";
            var alignedParams = @"C:\Users\sekanaya\Desktop\junkeffectiveparamsTocharg2effectivparams.txt";

            using (StreamReader reader = new StreamReader(referenceParams))
            {
                var sep = new[] { '=' };
                var paramToIndex = new Dictionary<string, int>();
                var lines = new List<string>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line))
                    {
                        lines.Add(" ");
                        continue;
                    }
                    line = line.Trim();
                    if (line.StartsWith(@"//"))
                    {
                        lines.Add(line);
                        continue;
                    }

                    var splits = line.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                    if (paramToIndex.ContainsKey(splits[0]))
                    {
                        lines[paramToIndex[splits[0]]] = @"//" + lines[paramToIndex[splits[0]]];
                        //                        lines[paramToIndex[splits[0]]] = " ";
                    }
                    lines.Add(line);
                    paramToIndex[splits[0]] = lines.Count - 1;
                }
                using (var writer = new StreamWriter(alignedParams))
                {
                    foreach (var line in lines)
                    {
                        //                        if (string.IsNullOrEmpty(line)) continue;
                        writer.WriteLine(line);
                    }
                }
            }

        }

        // Has bugs when LHS is used in RHS
        private static void FindEffectiveParams()
        {
//            var allparamsfile = @"C:\Users\sekanaya\Desktop\junkallaparams.txt";
            var allparamsfile = @"C:\Users\sekanaya\Desktop\junkcharge2allparams.txt";
//            var effectiveparamsfile = @"C:\Users\sekanaya\Desktop\junkeffectiveparams.txt";
//            var effectiveparamsfile = @"C:\Users\sekanaya\Desktop\junkmyeffectiveeffectiveparams.txt";
            var effectiveparamsfile = @"C:\Users\sekanaya\Desktop\junkcharge2effectiveparams.txt";

            using (var reader = new StreamReader(allparamsfile))
            {
                var sep = new[] {'='};
                var paramToIndex = new Dictionary<string, int>();
                var lines = new List<string>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line))
                    {
                        lines.Add(" ");
                        continue;
                    }
                    line = line.Trim();
                    if (line.StartsWith(@"//"))
                    {
                        lines.Add(line);
                        continue;
                    }

                    var splits = line.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                    if (paramToIndex.ContainsKey(splits[0]))
                    {
                        lines[paramToIndex[splits[0]]] = @"//" + lines[paramToIndex[splits[0]]];
//                        lines[paramToIndex[splits[0]]] = " ";
                    }
                    lines.Add(line);
                    paramToIndex[splits[0]] = lines.Count - 1;
                }
                using (var writer = new StreamWriter(effectiveparamsfile))
                {
                    foreach (var line in lines)
                    {
//                        if (string.IsNullOrEmpty(line)) continue;
                        writer.WriteLine(line);
                    }
                }
            }
        }
    }
}
