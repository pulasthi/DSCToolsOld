using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Salsa.Core;

namespace DistanceTransform
{
    class Program
    {
        static void Main(string[] args)
        {
//            As INPUT Max 9.9802E-001 Average 7.6942E-001 Sigma 7.0154E-002 Estimated Dimension 240.58



            if (args.Length == 3)
            {
                string inputFile = args[0];
                string outputFile = args[1];
                int size = int.Parse(args[2]);
                double[] maxAndMean = GetMaxAndMean(inputFile, size);
                Console.WriteLine("Max: " + maxAndMean[0]);
                Console.WriteLine("Mean: " + maxAndMean[1]);
                double variance = GetVariance(inputFile, size, maxAndMean[1]);
                double sigma = Math.Sqrt(variance);
                Console.WriteLine("Varience: " + variance);
                Console.WriteLine("Sigma: " + sigma);
                TransformSQRT4D(maxAndMean[1], maxAndMean[0], sigma, inputFile, outputFile, size);
            }
            else
            {
                /*
                 * Max: 0.99801629688406
                 * Mean: 0.769411837659903
                 * Variance: 0.00492765495584265
                 * Sigma: 0.0701972575
                 */

                const string file = @"F:\Salsa\saliya\cog\100k\input\cog_95672_nw_c#.bin";
                const string outputFile = @"F:\Salsa\saliya\cog\100k\input\cog_95672_nw_sqrt4d_c#.bin";
                const int size = 95672;

                double mean = 0.769411837659903;
                double maxDistance = 0.99801629688406;
                double sigma = 0.0701972575;

                TransformSQRT4D(mean, maxDistance, sigma, file, outputFile, size);
            }
            Console.WriteLine("Done.");
            Console.Read();
        }

        private static void TransformSQRT4D(double mean, double maxDistance, double sigma, string inputFile, string outputFile, int size)
        {
            double estimatedDimension = 2.0 * mean * mean / (sigma * sigma);
            double individualSigma = Math.Sqrt(mean/estimatedDimension);
            double scaleFactor  = 2.0 * individualSigma * individualSigma;
            double maxSQRT4DDistance = Math.Sqrt(scaleFactor*
                                            Transform4D(SpecialFunction.igamc(estimatedDimension*0.5,
                                                                              maxDistance/scaleFactor)));
            using (BinaryWriter writer = new BinaryWriter(File.Create(outputFile)))
            {
                using (BinaryReader reader = new BinaryReader(File.OpenRead(inputFile)))
                {
                    long count = ((long) size)*size;

                    long streamLength = reader.BaseStream.Length/sizeof (Int16);

                    Console.WriteLine("count: " + count + " stream-len: " + streamLength);
                    if (count != streamLength)
                    {
                        return;
                    }

                    double d;
                    Int16 shortDistance;
                    for (long i = 0L; i < count; ++i)
                    {
                        d = ((double) reader.ReadInt16())/Int16.MaxValue;
                        d = Math.Sqrt(scaleFactor*
                                      Transform4D(SpecialFunction.igamc(estimatedDimension*0.5,
                                                                        d/scaleFactor))) / maxSQRT4DDistance;
                        shortDistance = ((Int16) (d*Int16.MaxValue));
                        writer.Write(shortDistance);

                       /* writer.Write(
                            ((Int16)
                             ((Math.Sqrt(scaleFactor*
                                        Transform4D(SpecialFunction.igamc(estimatedDimension*0.5,
                                                                          (((double) reader.ReadInt16())/Int16.MaxValue)/
                                                                          scaleFactor)))/maxSQRT4DDistance)*
                             Int16.MaxValue)));*/
                    }
                }
            }
        }

        
        //  Inverse of 4D Gaussion Distribution of distance differences
        public static double Transform4D(double HigherDimInput)
        {
            double Einit = -Math.Log(HigherDimInput);
            //         double Einit = HigherDimInput;
            double E = Einit;
            double Eold = E;
            double diff;

            for (int recurse = 0; recurse < 50; recurse++)
            {
                E = Einit + Math.Log(1.0 + E);
                //      E = 1.0 + E - E / 2;
                /*      diff = E - Eold;
                      if (diff < 0)
                      {
                          diff = Eold - E;
                      }*/
                //                if (diff < 0.00001)
                if (Math.Abs(E - Eold) < 0.00001)
                    return E;
                Eold = E;
            }

            return E;
        }
    
        private static double[] GetMaxAndMean(string file, int size)
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(file)))
            {
                double max = 0.0;
                double sum = 0.0;
                double d;
                long count = 0L;
                for (int i = 0; i < size; ++i)
                {
                    for (int j = 0; j < size; ++j)
                    {
                        d = ((double)reader.ReadInt16())/Int16.MaxValue;
                        sum += d;
                        if (d > max)
                        {
                            max = d;
                        }
                        ++count;
                    }
                }
                return new[]{max, (sum/count)};
            }
        }

        public static double GetVariance(string file, int size, double mean)
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(file)))
            {
                double sum = 0.0;
                long count = 0L;
                double d, diff;
                for (int i = 0; i < size; ++i)
                {
                    for (int j = 0; j < size; ++j)
                    {
                        d = ((double)reader.ReadInt16()) / Int16.MaxValue;
                        diff = mean - d;
                        sum += diff*diff;
                        ++count;
                    }
                }
                return sum / count;
            }
        }

        

        
    }  
}
