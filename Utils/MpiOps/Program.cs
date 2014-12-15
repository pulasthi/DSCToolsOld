using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPI;

namespace MpiOps
{
    class Program
    {
        static void Main(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                Intracommunicator world = MPI.Communicator.world;
                int me = world.Rank;
                int size = world.Size;
                var txt = GenerateRandomText(size, me);
                Console.WriteLine(me + ":" + txt);
                var recv = world.Allreduce<String>(txt, Operation<string>.Add);
                Console.WriteLine(me + ":" + recv);


            }


        }
        private static String GenerateRandomText(int size, int rank)
        {
            Random r = new Random(rank);
            int l = r.Next(0,size * 10);
            var sb = new StringBuilder(l);
            for (int i = 0; i < l; ++i)
            {
                sb.Append((char)(r.Next(0,26) + 65));
            }
            return sb.ToString();
        }
    }
}
