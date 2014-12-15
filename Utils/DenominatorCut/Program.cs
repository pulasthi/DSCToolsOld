using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common;

namespace DenominatorCut
{
    class Program
    {
        static void Main(string[] args)
        {
            string swgoldscorematrix = args[0];
            string swgnewscorematrix = args[1];
            int size = int.Parse(args[2]);
            string outdir = args[3];

            using (MatrixReader oldReader = new MatrixReader(swgoldscorematrix, MatrixType.Int16, size),
                newReader = new MatrixReader(swgnewscorematrix,MatrixType.Int16,size))
            {
                double [] cuts = new double[]{0.1,0.25,0.5,0.75};
                StreamWriter [] writers = new StreamWriter[cuts.Length];
                for (int i = 0; i < cuts.Length; i++)
                {
                    writers[i] = new StreamWriter(Path.Combine(outdir, "cut-" + cuts[i] + ".txt"));
                }

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        double olddist = ((double) BitConverter.ToInt16(oldReader.Read(i, j), 0)/Int16.MaxValue);
                        double newdist = ((double) BitConverter.ToInt16(newReader.Read(i, j), 0)/Int16.MaxValue);

                        double newnormoveroldnorm = (1.0 - olddist) / (1.0 - newdist);
                            
                        if (newnormoveroldnorm < 0) throw new Exception("Bad: negative ratio");

                        for (int k = 0; k < cuts.Length; k++)
                        {
                            if (newnormoveroldnorm < cuts[k])
                            {
                                writers[k].WriteLine(i +"\t" + j);
                            }
                        }


                    }
                }

                for (int i = 0; i < cuts.Length; i++)
                {
                    writers[i].Close();
                }
                
            }
        }
    }
}
