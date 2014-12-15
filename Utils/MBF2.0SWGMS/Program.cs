using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bio;
using Bio.Algorithms.Alignment;
using Bio.IO.FastA;
using Bio.IO.GenBank;
using Bio.SimilarityMatrices;
using Salsa.Core.Configuration;
using Salsa.Core.Configuration.Sections;

namespace MBF2._0SWGMS
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = @"C:\Users\sekanaya\Desktop\hui\haixu_region_6_1.fa";
//            string file = @"C:\Users\sekanaya\Desktop\swgmstester\17and21.txt";
//            string distFile = @"C:\Users\sekanaya\Desktop\swgmstester\17and21.bin";
            string distFile = @"C:\Users\sekanaya\Desktop\hui\haixu_region_6_1_744x744.bin";
            
            IList<ISequence> seqs = new FastAParser(file).Parse().ToList();
            int count = seqs.Count;

            SmithWatermanMS swgmssection = new ConfigurationMgr().SmithWatermanMS;
            SimilarityMatrix matrix = swgmssection.LoadSimilarityMatrix("EDNAFULL", MoleculeType.DNA);
            int go = -16;
            int ge = -4;

            short[][] dists = new short[count][];
            SmithWatermanAligner aligner = new SmithWatermanAligner();

            Console.WriteLine("Working ...");
            for (int i = 0; i < count; i++)
            {
                dists[i] = new short[count];
            }

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    if (i != j)
                    {
                        IList<IPairwiseSequenceAlignment> psas = aligner.Align(matrix, go, ge, seqs[i], seqs[j]);
                        IPairwiseSequenceAlignment psa = psas[0]; // Take the first alignment
                        IList<PairwiseAlignedSequence> pass = psa.PairwiseAlignedSequences;
                        PairwiseAlignedSequence pas = pass[0]; // Take the first PairwisedAlignedSequence
                        dists[i][j] = dists[j][i] = (short) ((1.0f - ComputePercentIdentity(pas))*short.MaxValue);
//                        short d = (short) ((1.0f - ComputePercentIdentity(pas))*short.MaxValue);
                    }
                    
//                    Console.WriteLine("("  +seqs[i].ID +"," + seqs[j].ID + ") : " + dists[i][j]);
                }
                Console.WriteLine("Row " + i + " done.");
            }
            WriteDistanceFile(distFile, dists);
            Console.WriteLine("Done.");
            Console.Read();
        }

        private static bool IsOdd(int value)
        {
            return (value & 1) == 1;
        }

        private static void WriteDistanceFile(string distFile, short[][] dist)
        {
            using (var distWriter = new BinaryWriter(File.OpenWrite(distFile)))
            {
                foreach (short[] t in dist)
                {
                    for (int j = 0; j < dist.Length; j++)
                    {
                        distWriter.Write(t[j]);
                    }
                }
            }
        }

        static float ComputePercentIdentity(PairwiseAlignedSequence pas)
        {
            ISequence alignedSeqA = pas.FirstSequence;
            ISequence alignedSeqB = pas.SecondSequence;
            float identity = 0.0f;
            for (int i = 0; i < alignedSeqA.Count; i++)
            {
                if (alignedSeqA[i] == alignedSeqB[i])
                {
                    identity++;
                }

            }
            return identity / alignedSeqA.Count;
        }

    }
}
