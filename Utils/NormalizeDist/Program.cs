using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace NormalizeDist
{
    class Program
    {
        static void Main(string[] args)
        {
            string distFile = args[0];
            int size = int.Parse(args[1]);
            
#if USE_DOUBLE
            double max = double.MinValue;
            double min = double.MaxValue;
            using (MatrixReader reader = new MatrixReader(distFile, MatrixType.Double, size))
#else
            Int16 max = Int16.MinValue;
            double min = double.MaxValue;
            using (MatrixReader reader = new MatrixReader(distFile, MatrixType.Int16, size))
#endif
            {
                
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        
#if USE_DOUBLE
                        double d = BitConverter.ToDouble(reader.Read(i, j), 0);
#else
                        Int16 d = BitConverter.ToInt16(reader.Read(i, j), 0);
#endif
                        if (d > max) max = d;
                        if (d < min) min = d;
                        if (d < 0) throw new Exception("error: negative distance at " + i +", " + j);  
                    }
                    
                }
            }
            
            Console.WriteLine("max: " + max);
            Console.WriteLine("min: " + min);
//            if (max > 1)
            {
                string dir = Path.GetDirectoryName(distFile) ?? string.Empty;

                string normalizedDistFile = Path.Combine(dir,
                                                         Path.GetFileNameWithoutExtension(distFile) +
                                                         "_normalized_c#.bin");
                using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(normalizedDistFile)))
                {
#if USE_DOUBLE
                    using (MatrixReader reader = new MatrixReader(distFile, MatrixType.Double, size))
#else
                    using (MatrixReader reader = new MatrixReader(distFile, MatrixType.Int16, size))
#endif
                    {
                        for (int i = 0; i < size; i++)
                        {
                            for (int j = 0; j < size; j++)
                            {
#if USE_DOUBLE
                                Int16 d = (Int16) (((BitConverter.ToDouble(reader.Read(i, j), 0))/max)*Int16.MaxValue);
#else
                                Int16 d = (Int16) ((((double) BitConverter.ToInt16(reader.Read(i, j), 0))/max)*Int16.MaxValue);
#endif
                                writer.Write(d);
                            }

                        }
                    }
                }
            }

            Console.WriteLine("done.");
            Console.Read();

        }
    }
}
