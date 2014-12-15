using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempFastaQuickFix
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var reader = new StreamReader(args[0]))
            {
                using (var writer = new StreamWriter(Path.Combine(Path.GetDirectoryName(args[0])??string.Empty, Path.GetFileNameWithoutExtension(args[0])+"_fixed.fa")))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (string.IsNullOrEmpty(line)) continue;
                        writer.WriteLine(">" + line);
                        writer.WriteLine(reader.ReadLine());
                    }
                }
            }
        }
    }
}
