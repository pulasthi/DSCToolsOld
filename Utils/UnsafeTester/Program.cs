using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace UnsafeTester
{
    class Program
    {
        unsafe static void Main(string[] args)
        {
//            ArrayTest();
//            ForLoopTest2();
//            ForLoopTest3();
            string empty = "adf";
            string name = "somename";
            Console.WriteLine(Path.Combine(empty, name));

            TestSubStringMethod();
            Console.Read();
        }

        private static void TestSubStringMethod()
        {
            string tag = "TAG";
            string line = "            <h3>TAGmanxcat_nameTAG</h3>";
            Console.WriteLine(new string('^', 40));
            Console.WriteLine("line: " + line);
            Console.WriteLine("linelength:" + line.Length);
            int tagStartIdx = line.IndexOf(tag);
            Console.WriteLine("tagstart:" + tagStartIdx);
            int tagEndIdx = line.IndexOf(tag, tagStartIdx + tag.Length) +
                            (tag.Length - 1);
            Console.WriteLine("tagend:" + tagEndIdx);
            string prefix = line.Substring(0, tagStartIdx);
            Console.WriteLine("prefix:" + prefix);
            string suffix = line.Substring(tagEndIdx + 1);
            Console.WriteLine("suffix:" + suffix);
            string meat = line.Substring(tagStartIdx + tag.Length,
                                         ((tagEndIdx + 1) - (tagStartIdx + 2 * tag.Length)));

        }

        unsafe static void ArrayTest()
        {
            short seqLen = 492;
            short numSeq = 100;

            sbyte dummmy = 0;
            sbyte[] arr;


            short row, col;
            int cell = 0;

            arr = new sbyte[seqLen * seqLen];
            Stopwatch timer = new Stopwatch();
            timer.Start();
            for (int i = 0; i < numSeq; i++)
            {
                for (int j = 0; j < numSeq; j++)
                {
                    cell = 0;
//                    arr = new sbyte[seqLen * seqLen];

                    fixed (sbyte* arre = &arr[0])
                    {
                        for (row = 0; row < seqLen; row++)
                        {
                            for (col = 0; col < seqLen; col++, cell++)
                            {
//                                arr[cell] = dummmy;
                                arre[cell] = dummmy;
                            }
                        }
                    }
                }
//                Console.WriteLine("Row " + i + " done");
            }
            timer.Stop();
            Console.WriteLine("Elapsed: " + timer.ElapsedMilliseconds + "ms");
            Console.Read();
            
        }


        unsafe public static void ForLoopTest()
        {
            short seqLen = 492;
            short numSeq = 100;

            sbyte dummmy = 0;
            sbyte[] arr;


            int row, col;
            int cell = 0;

            Stopwatch timer = new Stopwatch();
            timer.Start();

            int len = numSeq*numSeq;
            for (int i = 0; i < len ; i++)
            {
                cell = 0;
                arr = new sbyte[seqLen * seqLen];

                fixed (sbyte* arre = &arr[0])
                {
                    int len2 = seqLen*seqLen;
                    for (row = 0; row < len2; row++)
                    {
//                        arre[cell] = dummmy;
                    }
                }
            }
            timer.Stop();
            Console.WriteLine("Elapsed: " + timer.ElapsedMilliseconds + "ms");
            Console.Read();
            
        }

        static void ForLoopTest2()
        {
            Console.WriteLine("working ...");
            long len = 2420640000; // 100 x 100 sequences each pair with 492 x 492
            Stopwatch timer = new Stopwatch();
            timer.Start();
            for (long i = 0; i < len; i++)
            {
                
            }
            timer.Stop();
            Console.WriteLine("Elapsed: " + timer.ElapsedMilliseconds + "ms");
            Console.Read();
        }

        static void ForLoopTest3()
        {
            Console.WriteLine("working ...");
            int seqLen = 492;
            int numSeq = 100;
            
            sbyte [] arr = new sbyte[seqLen * seqLen];
            int cell = 0;

            Stopwatch timer = new Stopwatch();
            timer.Start();
            for (int i = 0; i < numSeq; i++)
            {
                for(int j = 0; j < numSeq; j++)
                {
                    cell = -1;
                    for (short row = 0; row < seqLen; row++)
                    {
                        for (short col = 0; col < seqLen; )
                        {
                            arr[++cell] = 0;
                            col++;
                        }
                    }
                }
            }
            timer.Stop();
            Console.WriteLine("Elapsed: " + timer.ElapsedMilliseconds + "ms");
            Console.Read();
        }
    }
}
