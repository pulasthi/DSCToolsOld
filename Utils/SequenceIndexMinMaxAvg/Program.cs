using System;
using System.IO;

namespace SequenceIndexMinMaxAvg
{
    class Program
    {
        static void Main(string[] args)
        {
            var idxFile = args[0];
            // NOTE. hack for Larry's data - getting the prefix
            /*var prefix = Path.GetFileNameWithoutExtension(Path.GetDirectoryName(Path.GetDirectoryName(idxFile)));*/
            var prefix = Path.GetFileNameWithoutExtension(idxFile);
            var sep = new[] {'\t'};
            using (var reader = new StreamReader(idxFile))
            {
                var min = int.MaxValue;
                var max = int.MinValue;
                var lengthSums = 0L;
                var count = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        var splits = line.Split(sep);
                        int length;
                        if (!int.TryParse(splits[2], out length)) return;
                        max = length > max ? length : max;
                        min = length < min ? length : min;
                        lengthSums += length;
                        ++count;
                    }
                }
                double avg = ((double) lengthSums)/count;
                Console.WriteLine(prefix + "\tmin\t{0}\tmax\t{1}\tavg\t{2}", min, max, avg);
                Console.Read();
            }
            
        }
    }
}
