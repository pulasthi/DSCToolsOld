using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace PvizBuilderWithCenters
{
    public class Matlab50ColorPicker
    {
        private static List<Color> _colors = GenerateMatlab50Colors();

        public static Color Pick(int i)
        {
            return _colors[i % _colors.Count];
        }

        private static List<Color> GenerateMatlab50Colors()
        {
            using (StreamReader reader = new StreamReader("Matlab50.txt"))
            {
                List<Color> colors = new List<Color>();
                char[] sep = new[] { ' ', '\t' };
                string[] splits;
                string split;
                int startIdx = 3;
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

                    colors.Add(Color.FromArgb(a, r, g, b));
                }
                return colors;
            }
        }
    }
}
