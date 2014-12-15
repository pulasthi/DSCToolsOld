using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("I am just a dummy app");
            Thread.Sleep(1000);
            Console.WriteLine("Press any key to exit");
            Console.Read();
        }
    }
}
