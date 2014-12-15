using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bio;
using Bio.IO.FastA;
using Salsa.Core;

namespace SequenceIndexGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            //Load the command line args into our helper class which allows us to name arguments
            Arguments pargs = new Arguments(args);
            // dataType {0=int16, 1=uint16, 2=double}
            pargs.Usage = "Usage: SequenceIndexGenerator.exe /fastaFile=<string>";

            if (pargs.CheckRequired(new string[] { "fastaFile"}) == false)
            {
                Console.WriteLine(pargs.Usage);
                return;
            }

            string fastaFile = pargs.GetValue<string>("fastaFile");
            IList<ISequence> seqs;
            using (FastAParser parser = new FastAParser(fastaFile))
            {
                seqs = parser.Parse().ToList();
            }



            string outDir = Path.GetDirectoryName(fastaFile ?? string.Empty);
            string outPath = Path.Combine(outDir, Path.GetFileNameWithoutExtension(fastaFile) + "_index.txt");
            
            using (StreamWriter writer = new StreamWriter(outPath))
            {
                ISequence seq;
                for (int i = 0; i < seqs.Count; i++)
                {
                    seq = seqs[i];
                    writer.WriteLine(i + "\t" + seq.ID + "\t" + seq.Count());
                        
                }
                writer.Flush();
            }

//            for (int i = 0; i < seqs.Count; i++)
//            {
//                ISequence sequence = seqs[i];
//                string str = sequence.ToString();
//                Console.WriteLine(sequence.DisplayID + " " + str.Length);
//            }
//
//            using (StreamReader reader = new StreamReader(fastaFile))
//            {
//                string str;
//                while (!reader.EndOfStream)
//                {
//                    Console.WriteLine(reader.ReadLine() + " " + reader.ReadLine().Length);
//                }
//            }
            Console.WriteLine("Done.");

//            Console.Read();
        }
    }

    // =IF(G1>=500,1,IF(AND(G1>=475,G1<500),2,IF(AND(G1>=450,G1<475),3,IF(AND(G1>=425,G1<450),4,IF(AND(G1>=400,G1<425),5,IF(AND(G1>=375,G1<400),6,IF(AND(G1>=350,G1<375),7,IF(AND(G1>=325,G1<350),8,IF(AND(G1>=300,G1<325),9,10)))))))))

    // =IF(H2>=500,1,IF(AND(H2>=475,H2<500),2,IF(AND(H2>=450,H2<475),3,IF(AND(H2>=425,H2<450),4,IF(AND(H2>=400,H2<425),5,IF(AND(H2>=375,H2<400),6,IF(AND(H2>=350,H2<375),7,IF(AND(H2>=325,H2<350),8,IF(AND(H2>=300,H2<325),9,10)))))))))
    
    // =IF(J1>=500,1,IF(AND(J1>=475,J1<500),2,IF(AND(J1>=450,J1<475),3,IF(AND(J1>=425,J1<450),4,IF(AND(J1>=400,J1<425),5,IF(AND(J1>=375,J1<400),6,IF(AND(J1>=350,J1<375),7,IF(AND(J1>=325,J1<350),8,IF(AND(J1>=300,J1<325),9,10)))))))))

    // =IF(J2>=500,1,IF(AND(J2>=475,J2<500),2,IF(AND(J2>=450,J2<475),3,IF(AND(J2>=425,J2<450),4,IF(AND(J2>=400,J2<425),5,IF(AND(J2>=375,J2<400),6,IF(AND(J2>=350,J2<375),7,IF(AND(J2>=325,J2<350),8,IF(AND(J2>=300,J2<325),9,10)))))))))
}
