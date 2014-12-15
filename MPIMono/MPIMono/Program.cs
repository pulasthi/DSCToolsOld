using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MPI;
using Environment = MPI.Environment;

namespace MPIMono
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Environment env = new MPI.Environment(ref args))
            {
                Console.WriteLine("Rank " + Communicator.world.Rank + " has a high perf timer? " + Stopwatch.IsHighResolution);
                Stopwatch timer = Stopwatch.StartNew();
                Console.WriteLine("Hello, World! from rank " + Communicator.world.Rank
                  + " (running on " + MPI.Environment.ProcessorName + ")");
                timer.Stop();
                FunctionClass.SayHi();
                Console.WriteLine("Rank " + Communicator.world.Rank + " time: " + timer.ElapsedMilliseconds);
            }
        }
    }
}
