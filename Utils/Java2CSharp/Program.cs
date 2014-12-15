using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Java2CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: Java2CSharp.exe [path-to-java-dist-file] [num-of-columns] [num-of-rows]");
                return;
            }

            string javaDistFile = args[0];
            string cshaprtDistFile = Path.Combine((Path.GetDirectoryName(javaDistFile) ?? string.Empty),
                                                  Path.GetFileNameWithoutExtension(javaDistFile) + "_c#.bin");
            int bigc = int.Parse(args[1]);
            int bigr = int.Parse(args[2]);

            using (BufferedStream instream = new BufferedStream(new FileStream(javaDistFile, FileMode.Open)))
            {
                using (FileStream stream = new FileStream(cshaprtDistFile, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    byte[] bytes = new byte[2];
                    long length = ((long) bigr)*bigc;
                    for (long i = 0; i < length; ++i)
                    {
                        double d = ((double) BitConverter.ToInt16(bytes.Reverse().ToArray(), 0)) / Int16.MaxValue;
                        if (d < 0)
                        {
                            long r = i/bigc;
                            long c = i%bigc;
                            Console.WriteLine("negative values found at row: " +  r  + " and col: " + c);
                            
                        }
                        
                        instream.Read(bytes, 0, 2);
                        stream.WriteByte(bytes[1]);
                        stream.WriteByte(bytes[0]);

                    }
                }
            }
        }
    }
}
