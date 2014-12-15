using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MBF;
using MBF.Algorithms.Alignment;
using MBF.IO.Fasta;
using MBF.SimilarityMatrices;
using Salsa.Core.Configuration;
using Salsa.Core.Configuration.Sections;

namespace MFB1._0SWGMS
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = @"C:\Users\sekanaya\Desktop\swgmstester\100k_sample_fasta_clusters_21+5_100.txt";
            string distFile = @"C:\Users\sekanaya\Desktop\swgmstester\MBF1.0Test\mbf1.0_dist.bin";
            IList<ISequence> seqs = new FastaParser().Parse(file);
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
                for (int j =0; j < i; j++)
                {
                    IList<IPairwiseSequenceAlignment> psas = aligner.Align(matrix, go, ge, seqs[i], seqs[j]);
                    IPairwiseSequenceAlignment psa = psas[0]; // Take the first alignment
                    IList<PairwiseAlignedSequence> pass = psa.PairwiseAlignedSequences;
                    PairwiseAlignedSequence pas = pass[0]; // Take the first PairwisedAlignedSequence
                    dists[i][j] = dists[j][i] = (short)((1.0f - ComputePercentIdentity(pas)) * short.MaxValue);
                }
                Console.WriteLine("Row "+ i + " done.");
            }
            WriteDistanceFile(distFile, dists);
            Console.WriteLine("Done.");
            Console.Read();
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
