using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Common.pviz
{
    public enum ColorSchemes
    {
        Matlab50NoBlue
    }
    public class ColorUtils
    {
        public static IList<System.Drawing.Color> GetColorsFor(ColorSchemes scheme)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Common.pviz.colors." + scheme + ".txt"))
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        IList<System.Drawing.Color> colorList = new List<System.Drawing.Color>();
                        char[] sep = new[] {' ', '\t'};
                        const int startIdx = 3;
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            if (!string.IsNullOrEmpty(line))
                            {
                                string[] splits = line.Trim().Split(sep);

                                string split = splits[0];
                                int r = int.Parse(split.Substring(startIdx, (split.Length - (startIdx + 1))));

                                split = splits[1];
                                int g = int.Parse(split.Substring(startIdx, (split.Length - (startIdx + 1))));

                                split = splits[2];
                                int b = int.Parse(split.Substring(startIdx, (split.Length - (startIdx + 1))));

                                split = splits[3];
                                int a = int.Parse(split.Substring(startIdx, (split.Length - (startIdx + 1))));

                                colorList.Add(System.Drawing.Color.FromArgb(a, r, g, b));
                            }
                        }
                        return colorList;
                    }
                }
                return new List<System.Drawing.Color>();
            }
        } 
    }
}
