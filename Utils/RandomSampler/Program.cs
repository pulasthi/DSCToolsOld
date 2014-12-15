using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace RandomSampler
{
    class Program
    {
        
        /*
         * Earlier command line code
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("\n\tUsage: RandomSampler.exe <input file> <sample1 size> <sample2 size> ...");
            }
            Console.Write("Running ... ");

            // Store sample sizes
            int [] sizes = new int[args.Length - 1];
            for (int i = 1; i < args.Length; i++)
            {
                sizes[i - 1] = Int32.Parse(args[i]);
            }
            
            TextReader reader;
            List<string> names = new List<string>();
            List<string> seqs = new List<string>();
            string str;
            using (reader = new StreamReader(args[0]))
            {
                while ((str = reader.ReadLine()) != null)
                {
                    seqs.Add(str);
                    names.Add(reader.ReadLine());
                }
            }
            
            int max = seqs.Count;
            
            // Populate random indices
            int[] randoms = new int[max];
            Random rand = new Random();
            for (int i = 0; i < max; i++)
            {
                randoms[i] = rand.Next(max);
            }

            string[][] samples = new string[sizes.Length][];

            str = new FileInfo(args[0]).FullName;
            str = str.Substring(0, str.IndexOf('.'));
            
            string name;
            int size, index;
            TextWriter writer;
            for (int i = 0; i < sizes.Length; i++)
            {
                size = sizes[i];
                if (size > max)
                {
                    Console.WriteLine("Oops! sample size" + size + 
                        "is large than total number of seqs");
                    return;
                }

                name = str + "_" + size + ".txt";
                using (writer = new StreamWriter(name))
                {
                    for (int j = 0; j < sizes[i]; j++)
                    {
                        index = randoms[j];
                        writer.WriteLine(seqs.ElementAt(index));
                        writer.WriteLine(names.ElementAt(index));
                    }
                }
                
                //A way to verify the process is correct.
                //int count = 0;
                //using (reader = new StreamReader(name))
                //{
                //    while (reader.ReadLine() != null)
                //    {
                //        count++;
                //    }
                //}
                //Console.WriteLine("\n\tSample " + i + " - # of sequences: " + count /2);

            }

            Console.WriteLine("Done.");
            Console.Read();
        }
         */

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainFrm());
        }
    }
}

        