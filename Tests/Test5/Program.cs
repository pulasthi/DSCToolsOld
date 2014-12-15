using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Test5
{
    class Program
    {
        static void Main(string[] args)
        {
            //string s = @"C:\users\sekanaya\Desktop";
            //Console.WriteLine(Directory.Exists(s));

            //Console.WriteLine(new DirectoryInfo(s).FullName);
            //Console.WriteLine(Path.GetDirectoryName(s));

            //string str = @"C:\users\sekanaya\Desktop\BioDotNet.chm";
            //Console.WriteLine(Path.GetExtension(str));
            //Console.WriteLine(Path.GetFileName(str));
            string str = Path.GetFullPath("test");
            Console.WriteLine(Path.Combine(str, "NewTextDocument.txt"));
            Console.Read();
        }
    }
}
