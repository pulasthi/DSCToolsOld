using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test2
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] myArray = new[] { 3, 5, 2, 8, 1, 7, 6, 9 };

            Console.WriteLine("Before setting up the query");

            IEnumerable<int> q1 = LessThan(myArray, 5);

            //IEnumerable<int> q2 = LessThan(myArray, 8);

            Console.WriteLine("Before iterating");
            int count = q1.Count<int>();
            Console.WriteLine(count);

            foreach (var i in q1)

                Console.WriteLine(i);

            Console.WriteLine("===");

            //foreach (var i in q2)

            //    Console.WriteLine(i);
            Console.Read();
        }

        public static IEnumerable<int> LessThan(IEnumerable<int> source, int x)
        {

            int z = x;
            //List<int> items = new List<int>();

            Console.WriteLine("Yield returning values less than {0}", z);

            foreach (var item in source)

                if (item < z)
                    //items.Add(item);
            //return items;

                    yield return item;

        }
    }
}
