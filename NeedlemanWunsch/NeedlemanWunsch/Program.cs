using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBF.Algorithms.Alignment;
using MBF.IO;
using MBF.IO.Fasta;
using MBF.SimilarityMatrices;
using MBF;
using System.Reflection;
using Salsa.Core.Bio.Algorithms;
using Salsa.Core.Configuration;

namespace NeedlemanWunsch
{
    class Program
    {
        static void Main(string[] args)
        {
            //int[,] arr = new int[2,3];
            //arr[0, 0] = 00;
            //arr[0, 1] = 01;
            //arr[0, 2] = 02;

            //Console.WriteLine(arr[0,1]);
            //ScoringMatrix matrix = ScoringMatrix.Load("EDNAFULL");

            //String s = "A   T   G   C   S   W   R   Y   K   M   B   V   H   D   N   U";
            
            //ScoringMatrixTokenizer tokenizer = new ScoringMatrixTokenizer(s.Trim());
            //char[] acids = new char[tokenizer.Count];
            //for (int j = 0; tokenizer.HasMoreTokens(); j++)
            //{
            //    acids[j] = tokenizer.NextToken()[0];
            //}

            //Console.WriteLine(acids);



            //Assembly a = Assembly.LoadFrom("Salsa.CoreTPL.dll");
            //string[] names = a.GetManifestResourceNames();
            //Console.WriteLine("running");
            //foreach (string name in names)
            //{
            //    Console.WriteLine(name);
            //}

            

            //Console.WriteLine(seqs[0].MoleculeType);
            //Console.WriteLine(seqs[0].DisplayID);
            //Console.WriteLine(seqs[0].ID);
            //Console.WriteLine("{0}", seqs[0]);
            //Console.WriteLine(seqs[0].Count);

            //Bio.ISequence a = seqs[0];
            //Console.WriteLine(a.ElementAt(2) is Bio.ISequenceItem);

            
            //aligner.GapOpenCost = 16;
            //aligner.GapExtensionCost = 4;
            //aligner.SimilarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.Blosum45);
            //aligner.SimilarityMatrix = new SimilarityMatrix(@"C:\users\sekanaya\Desktop\ednafull.txt");
            //IList<IPairwiseSequenceAlignment> alignments = aligner.Align(seqs);
            //Console.WriteLine(alignments.Count);

            //SimilarityMatrix matrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.Blosum50);


            ConfigurationMgr mgr = new ConfigurationMgr();
            SimilarityMatrix matrix = mgr.NeedlemanWunschSection.LoadSimilarityMatrix("EDNAFULL", MoleculeType.DNA);
            //SimilarityMatrix matrix = mgr.NeedlemanWunschSection.LoadSimilarityMatrix("BLOSUM50", MoleculeType.Protein);
                        
            NeedlemanWunschAligner aligner = new NeedlemanWunschAligner();

            //ISequence seq1 = new MBF.Sequence(Alphabets.DNA, "AACTCTTGACATCCAGAGAAGAGGCTAGAGATAGCTTTGTGCCTTCGGATCGTCTGAGAC");
            //ISequence seq2 = new MBF.Sequence(Alphabets.DNA, "AGCACTTGACATACAACGAATTCGTCAGAGATGACTTAGTGCTACTTCGGTAGAACGTTGATACA");

            ISequence seq1 = new MBF.Sequence(Alphabets.DNA, "CCTTCGGATCGTCTGAGAC");
            ISequence seq2 = new MBF.Sequence(Alphabets.DNA, "CTACTTCGGTAGAACGTTGATACA");

            int gapOpen = -8;
            int gapExt = -2;

            // Note. 1
            IList<IPairwiseSequenceAlignment> psas = aligner.Align(matrix, gapOpen, gapExt, seq1, seq2);

            Console.WriteLine("Scoring Matrix: {0}", matrix.Name);
            Console.WriteLine("Gap Open: {0}", gapOpen);
            Console.WriteLine("Gap Ext: {0}\n", gapExt);

            Console.WriteLine("# of PairwiseSequenceAlignments (PSAS): {0}", psas.Count);

            // Note. 1.1
            IPairwiseSequenceAlignment psa = psas[0];
            Console.WriteLine("  --PSA0's FirstSequence  == original seq 1: {0}", psa.FirstSequence == seq1);
            Console.WriteLine("  --PSA0's SecondSequence == original seq 2: {0}", psa.SecondSequence == seq2);

            // Note. 2
            IList<PairwiseAlignedSequence> pass = psa.PairwiseAlignedSequences;
            Console.WriteLine("\n# of PairwiseAlignedSequences (PASS) in PSA0: {0}", pass.Count);
            
            // Note. 2.1
            PairwiseAlignedSequence pas = pass[0];

            ISequence alignedSeqA = pas.FirstSequence;
            ISequence alignedSeqB = pas.SecondSequence;
            Console.WriteLine("  --PAS0's FirstSequence>\n    {0}\n", alignedSeqA);
            Console.WriteLine("  --PAS0's SecondSequence>\n    {0}\n", alignedSeqB);
            Console.WriteLine("  --PAS0's Score: {0}", pas.Score);

            Console.WriteLine(ComputeKimuraDistance(pas));



            /*

            ISequenceParser parser = new FastaParser();
            IList<MBF.ISequence> seqs = parser.Parse(@"C:\sali\pti\data\mina-Nov11-2010\TEST_20seq_FS312_trimmed3.fa");
            Console.WriteLine("original seq1>\n{0}\n", seqs[0]);
            Console.WriteLine("original seq2>\n{0}\n", seqs[1]);

           
            //IList<IPairwiseSequenceAlignment> psas = aligner.AlignSimple(new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.Blosum50), -8, seqs.ElementAt(0), seqs.ElementAt(1));
            //IList<IPairwiseSequenceAlignment> psas = aligner.Align(new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.Blosum50), -8, -14, seqs.ElementAt(0), seqs.ElementAt(1));

            
            IList<IPairwiseSequenceAlignment> psas = aligner.Align(matrix, -20, -8, seqs.ElementAt(0), seqs.ElementAt(1));
            Console.WriteLine("# of PairwiseSequenceAlignments (PSAS):   {0}", psas.Count);
            IPairwiseSequenceAlignment psa = psas[0];
            Console.WriteLine("PSA's FirstSequence  == original seq 1:   {0}", psa.FirstSequence == seqs[0]);
            Console.WriteLine("pSA's SecondSequence == original seq 2:   {0}", psa.SecondSequence == seqs[1]);

            IList<PairwiseAlignedSequence> pass = psa.PairwiseAlignedSequences;
            Console.WriteLine("# of PairwiseAlignedSequences (PASS):     {0}", pass.Count);
            PairwiseAlignedSequence pas = pass[0];

            ISequence alignedSeqA = pas.FirstSequence;
            ISequence alignedSeqB = pas.SecondSequence;
            Console.WriteLine("PAS's FirstSequence>\n{0}\n", alignedSeqA);
            Console.WriteLine("PAS's SecondSequence>\n{0}\n", alignedSeqB);
            Console.WriteLine("PAS's Score                               {0}", pas.Score);

            Console.WriteLine(ComputeKimuraDistance(pas));

            //Console.WriteLine("# of pw aligned seqs in the alignment: " + alignment.PairwiseAlignedSequences.Count);
            //PairwiseAlignedSequence pas = alignment.PairwiseAlignedSequences[0];
            //IList<Bio.ISequence> temps = pas.Sequences;
            //Console.WriteLine(temps[0]);
            //Console.WriteLine("\n");
            //Console.WriteLine(temps[1]);
            //Console.WriteLine(pas.FirstOffset);
            //Console.WriteLine(pas.Consensus);
            //Console.WriteLine("\n");
            //Console.WriteLine(pas.FirstSequence);
            //Console.WriteLine("\n");
            //Console.WriteLine(pas.SecondSequence);

            //int score1 = alignment.PairwiseAlignedSequences[0].Score;
            ////int score2 = alignment.PairwiseAlignedSequences[1].Score;
            //Console.WriteLine(score1);
            //Console.WriteLine(alignment.PairwiseAlignedSequences[0].Consensus);
            //Console.WriteLine(alignment.AlignedSequences[0].Metadata["score"]);
            //Console.WriteLine(alignment.Sequences.Count);

            */
            Console.Read();
            
           


        }

        static double ComputeKimuraDistance(PairwiseAlignedSequence pas)
        {
            double length = 0;
            double gapCount = 0;
            double transitionCount = 0;    // P = A -> G | G -> A | C -> T | T -> C
            double transversionCount = 0;  // Q = A -> C | A -> T | C -> A | C -> G | T -> A  | T -> G | G -> T | G -> C

            ISequence alignedSeqA = pas.FirstSequence;
            ISequence alignedSeqB = pas.SecondSequence;

            ISequenceItem itemA;
            ISequenceItem itemB;

            for (int i = 0; i < alignedSeqA.Count; i++)
            {
                length++;
                itemA = alignedSeqA[i]; //nucleotide 1
                itemB = alignedSeqB[i]; //nucleotide 2

                if (itemA != itemB)
                {
                    // Don't consider gaps at all in this computation;
                    if (itemA.IsGap || itemB.IsGap)
                    {
                        gapCount++;
                    }
                    else if ((itemA.Symbol == 'A' && itemB.Symbol == 'G') ||
                        (itemA.Symbol == 'G' && itemB.Symbol == 'A') ||
                        (itemA.Symbol == 'C' && itemB.Symbol == 'T') ||
                        (itemA.Symbol == 'T' && itemB.Symbol == 'C'))
                    {
                        transitionCount++;
                    }
                    else
                    {
                        transversionCount++;
                    }
                }
            }

            string msg1 = "\nseqA:\n" + alignedSeqA.ToString() + "\nseqB:\n" + alignedSeqB.ToString() +
                   "\ntransitions: " + transitionCount + "\ntransversions: " + transversionCount +
                   "\nlength: " + length + "\ngaps: " + gapCount + "\n";

            Console.Write(msg1);

            double P = transitionCount / (length - gapCount);
            double Q = transversionCount / (length - gapCount);

            if (1.0 - (2.0 * P + Q) <= double.Epsilon)
            {
                string msg = "seqA:\n" + alignedSeqA.ToString() + "\nseqB:\n" + alignedSeqB.ToString() +
                    "\ntransitions: " + transitionCount + "\ntransversions: " + transversionCount +
                    "\nlength: " + length + "\ngaps: " + gapCount;
                throw new Exception("\n---------\nKimura2 Distance Undefined - Log(Zero)\n---------\n" + msg);
            }
            if (1.0 - (2.0 * Q) <= double.Epsilon)
            {
                string msg = "seqA:\n" + alignedSeqA.ToString() + "\nseqB:\n" + alignedSeqB.ToString() +
                    "\ntransitions: " + transitionCount + "\ntransversions: " + transversionCount +
                    "\nlength: " + length + "\ngaps: " + gapCount;
                throw new Exception("\n---------\nKimura2 Distance Undefined - Log(Zero)\n---------\n" + msg);
            }

            return (-0.5 * Math.Log(1.0 - 2.0 * P - Q) - 0.25 * Math.Log(1.0 - 2.0 * Q));
        }
    }
}
