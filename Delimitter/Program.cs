using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Delimitter
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles(args[0]);
            foreach (string file in files)
            {
                Delmit(file, args[1]);
            }
            Console.Read();
            
        }

        static void Delmit(string repeatMaskerFile, string outputDir)
        {
            string outfile = outputDir + Path.DirectorySeparatorChar + repeatMaskerFile.Substring(repeatMaskerFile.LastIndexOf(Path.DirectorySeparatorChar));
            string[] hdr = new string[]
                {
                    "Score",        //0
                    "PercDiv",      //1
                    "PercDel",      //2
                    "PercIns",      //3
                    "Query",        //4
                    "QueryBegin",   //5
                    "QueryEnd",     //6
                    "QueryLeft",    //7
                    "Complement",   //8
                    "Repeat",       //9
                    "RepeatClass",  //10
                    "RepeatBegin",  //11
                    "RepeatEnd",    //12
                    "RepeatLeft",   //13
                    "Id"            //14
                };
            using (StreamWriter writer = new StreamWriter(outfile))
            {
                writer.WriteLine(string.Join("\t", hdr));
                using (StreamReader reader = new StreamReader(repeatMaskerFile))
                {
                    while (reader.EndOfStream == false)
                    {
                        string line = reader.ReadLine();
                        string[] fields = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (fields.Length == 15)
                        {
                            fields[7] = fields[7].Replace('(', ' ');
                            fields[7] = fields[7].Replace(')', ' ');
                            fields[7] = fields[7].Trim();
                            fields[11] = fields[11].Replace('(', ' ');
                            fields[11] = fields[11].Replace(')', ' ');
                            fields[11] = fields[11].Trim();
                            fields[13] = fields[13].Replace('(', ' ');
                            fields[13] = fields[13].Replace(')', ' ');
                            fields[13] = fields[13].Trim();
                            writer.WriteLine(string.Join("\t", fields));
                        }
                    }
            
                }
            }
            //Console.WriteLine(string.Join("\t", hdr));
            //using (StreamReader reader = new StreamReader(repeatMaskerFile))
            //{
            //    while (reader.EndOfStream == false)
            //    {
            //        string line = reader.ReadLine();
            //        string[] fields = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //        if (fields.Length == 15)
            //        {
            //            fields[7] = fields[7].Replace('(', ' ');
            //            fields[7] = fields[7].Replace(')', ' ');
            //            fields[7] = fields[7].Trim();
            //            fields[11] = fields[11].Replace('(', ' ');
            //            fields[11] = fields[11].Replace(')', ' ');
            //            fields[11] = fields[11].Trim();
            //            fields[13] = fields[13].Replace('(', ' ');
            //            fields[13] = fields[13].Replace(')', ' ');
            //            fields[13] = fields[13].Trim();
            //            Console.WriteLine(string.Join("\t", fields));
            //        }
            //    }
            //}
        }
    }
}
