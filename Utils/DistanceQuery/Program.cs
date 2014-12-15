using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Common;

namespace DistanceQuery
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 3) return;
            bool isDouble = bool.Parse(args[2]);
            using (MatrixReader reader = new MatrixReader(args[0], isDouble? MatrixType.Double : MatrixType.Int16, int.Parse(args[1])))
            {
                Regex pairRegex = new Regex(@"^(\d+)(,|\s+)(\d+)$");
                Regex quitRegex = new Regex(@"^(q|quit|Quit|QUIT|exit|Exit|EXIT)$");
                bool quit = false;
                do
                {
                    Console.Write("> ");
                    string s = Console.ReadLine();
                    if (!string.IsNullOrEmpty(s))
                    {
                        Match m = pairRegex.Match(s);
                        if (m.Success)
                        {
                            int r = int.Parse(m.Groups[1].Value);
                            int c = int.Parse(m.Groups[3].Value);
                            byte[] value = reader.Read(r, c);
                            double dist = isDouble ? BitConverter.ToDouble(value,0) : (((double)BitConverter.ToInt16(value, 0))/Int16.MaxValue);
                            Console.WriteLine("  Distance: " + dist);
                        }
                        else if (quitRegex.IsMatch(s))
                        {
                            quit = true;
                        }
                        else
                        {
                            Console.WriteLine("  Unidentified input");
                        }
                    }
                } while (!quit);
            }
        }
    }
}
