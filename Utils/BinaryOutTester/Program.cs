using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BinaryOutTester
{
    class Program
    {
        static void Main(string[] args)
        {
            using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(@"C:\users\sekanaya\desktop\bytes.bin")))
            {
//                byte b0 = 4;
//                byte b1 = 1;
//                writer.Write(b0);
//                writer.Write(b1);
                short x = 260;
                writer.Write(x);
            }

            using (BinaryReader reader = new BinaryReader(File.OpenRead(@"C:\users\sekanaya\desktop\bytes.bin")))
            {
                byte b0 = reader.ReadByte();
                byte b1 = reader.ReadByte();
                Console.WriteLine("b0: " + b0);
                Console.WriteLine("b1: " + b1);

                short x = (short) (b1 << 8 | b0);
                Console.WriteLine(x);

            }

//            byte b1 = 0xFF;
//            byte b2 = 0;
//            byte b3;
//            unchecked
//            {
//                b3 = ((byte)257);
//            }
//
//            Console.WriteLine(b1);
//            Console.WriteLine(b2);
//            Console.WriteLine(b3); // should read as 1

            Console.Read();

        }
    }
}
