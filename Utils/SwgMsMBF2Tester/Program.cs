using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
//using Bio;
//using Bio.Algorithms.Alignment;
//using Bio.IO.FastA;
//using Bio.IO.GenBank;
//using Bio.SimilarityMatrices;
using Bio;
using Bio.Algorithms.Alignment;
using Bio.IO.FastA;
using Bio.IO.GenBank;
using Bio.SimilarityMatrices;
using Salsa.Core;
using Salsa.Core.Bio.Algorithms;
using Salsa.Core.Configuration;
using Salsa.Core.Configuration.Sections;

namespace SwgMsMBF2Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigurationMgr mgr = new ConfigurationMgr();
            SmithWatermanMS swgms = mgr.SmithWatermanMS;
            swgms.BlockDir = @"C:\Users\sekanaya\Desktop\swgmstester\block";
            swgms.BlockWriteFrequency = 1;
            swgms.DistanceFunctionType = DistanceFunctionType.PercentIdentity;
            swgms.DistanceMatrixFile = @"C:\Users\sekanaya\Desktop\swgmstester\out\dist.bin";
            swgms.FastaFile = @"C:\Users\sekanaya\Desktop\swgmstester\8seq.txt";
            swgms.GapExtensionPenalty = -4;
            swgms.GapOpenPenalty = -16;
            swgms.IndexFile = @"C:\Users\sekanaya\Desktop\swgmstester\out\index.txt";
            swgms.LogDir = @"C:\Users\sekanaya\Desktop\swgmstester\log";
            swgms.LogWriteFrequency = 2;
//            swgms.MoleculeType = 


//            string name = "SRR042317.6";
//            Console.WriteLine(name.Length);
//
//            System.Text.ASCIIEncoding encoding = new ASCIIEncoding();
//            byte[] bytes = encoding.GetBytes(name);
//            Console.WriteLine(bytes.Length);
//            Console.WriteLine(encoding.GetByteCount(name));
//            Console.WriteLine(encoding.GetString(bytes));
//            string str = new string(bytes.to);
//            Console.Read();

//            using (StreamReader reader = new StreamReader(@"C:\Users\sekanaya\Desktop\debug\seq2.txt"))
//            {
//                using (StreamWriter writer = new StreamWriter(@"C:\Users\sekanaya\Desktop\debug\seq2_csv.txt"))
//                {
//                    string txt = string.Empty;
//                    while (!reader.EndOfStream)
//                    {
//                        txt += reader.ReadLine().Trim() + ",";
//                    }
//                    writer.WriteLine(txt);
//                }
//            }

//            byte[] seq1 = new byte[] { 71, 65, 71, 84, 84, 84, 67, 65, 67, 67, 71, 84, 84, 71, 67, 67, 71, 71, 67, 71, 84, 65, 67, 84, 67, 67, 67, 67, 65, 71, 71, 84, 71, 71, 65, 65, 84, 65, 67, 84, 84, 65, 65, 67, 71, 67, 84, 84, 84, 67, 71, 67, 84, 84, 71, 71, 67, 67, 71, 67, 84, 84, 71, 67, 65, 71, 84, 65, 84, 65, 84, 67, 71, 67, 65, 65, 65, 67, 65, 71, 67, 71, 65, 71, 84, 65, 84, 84, 67, 65, 84, 67, 71, 84, 84, 84, 65, 67, 67, 71, 84, 71, 84, 71, 71, 65, 67, 84, 65, 67, 67, 65, 71, 71, 71, 84, 65, 84, 67, 84, 65, 65, 84, 67, 67, 84, 71, 84, 84, 84, 71, 65, 84, 65, 67, 67, 67, 65, 67, 65, 67, 84, 84, 84, 67, 71, 65, 71, 67, 67, 84, 67, 65, 65, 84, 71, 84, 67, 65, 71, 84, 84, 71, 67, 65, 71, 67, 84, 84, 65, 71, 67, 65, 71, 71, 67, 84, 71, 67, 67, 84, 84, 67, 71, 67, 65, 65, 84, 67, 71, 71, 65, 71, 84, 84, 67, 84, 84, 67, 71, 84, 71, 65, 84, 65, 84, 67, 84, 65, 65, 71, 67, 65, 84, 84, 84, 67, 65, 67, 67, 71, 67, 84, 65, 67, 65, 67, 67, 65, 67, 71, 65, 65, 84, 84, 67, 67, 71, 67, 67, 84, 71, 67, 67, 84, 67, 65, 65, 67, 84, 71, 67, 65, 67, 84, 67, 65, 65, 71, 65, 84, 65, 84, 67, 67, 65, 71, 84, 65, 84, 67, 65, 65, 67, 84, 71, 67, 65, 65, 84, 84, 84, 84, 65, 67, 71, 71, 84, 84, 71, 65, 71, 67, 67, 71, 67, 65, 65, 65, 67, 84, 84, 84, 67, 65, 67, 65, 65, 67, 84, 71, 65, 67, 84, 84, 65, 65, 65, 67, 65, 84, 67, 67, 65, 84, 67, 84, 65, 67, 71, 67, 84, 67, 67, 67, 84, 84, 84, 65, 65, 65, 67, 67, 67, 65, 65, 84, 65, 65, 65, 84, 67, 67, 71, 71, 65, 84, 65, 65, 67, 71, 67, 84, 67, 71, 71, 65, 84, 67, 67, 84, 67, 67, 71, 84, 65, 84, 84, 65, 67, 67, 71, 67, 71, 71, 67, 84, 71, 67, 84, 71, 71, 67, 65, 67, 71, 71, 65, 71, 84, 84, 65, 71, 67, 67, 71, 65, 84, 67, 67, 84, 84, 65, 84, 84, 67, 65, 84, 65, 65, 65, 71, 84, 65, 67, 65, 84, 71, 65, 67, 65, 65, 65, 67, 71, 71, 71, 84, 65, 84, 67, 67, 65, 84, 65, 67, 67, 67, 71, 65, 67, 84, 84, 84, 65, 84, 84, 67, 67, 84, 84, 84, 65, 65, 84, 65, 65, 65, 65, 71, 65, 65, 71, 84, 84, 84, 65, 67, 65, 65, 67, 67, 67, 84, 65, 84, 65, 71, 71, 71, 67, 65, 71, 84, 67, 65, 84, 67, 67, 84, 84, 67, 65, 67, 71};
//            byte[] seq2 = new byte[] { 71, 65, 71, 84, 84, 84, 67, 65, 65, 67, 67, 84, 84, 71, 67, 71, 71, 84, 67, 71, 84, 65, 67, 84, 67, 67, 67, 67, 65, 71, 71, 67, 71, 71, 65, 71, 84, 71, 67, 84, 84, 65, 65, 84, 71, 67, 71, 84, 84, 65, 71, 67, 84, 71, 67, 65, 71, 67, 65, 67, 84, 65, 65, 71, 71, 71, 71, 67, 71, 71, 65, 65, 65, 67, 67, 67, 67, 67, 84, 65, 65, 67, 65, 67, 84, 84, 65, 71, 67, 65, 67, 84, 67, 65, 84, 67, 71, 84, 84, 84, 65, 67, 71, 71, 67, 71, 84, 71, 71, 65, 67, 84, 65, 67, 67, 65, 71, 71, 71, 84, 65, 84, 67, 84, 65, 65, 84, 67, 67, 84, 71, 84, 84, 84, 71, 65, 84, 67, 67, 67, 67, 65, 67, 71, 67, 84, 84, 84, 67, 71, 67, 65, 67, 65, 84, 67, 65, 71, 67, 71, 84, 67, 65, 71, 84, 84, 65, 67, 65, 71, 65, 67, 67, 65, 71, 65, 65, 65, 71, 84, 67, 71, 67, 67, 84, 84, 67, 71, 67, 67, 65, 67, 84, 71, 71, 84, 71, 84, 84, 67, 67, 84, 67, 67, 65, 84, 65, 84, 67, 84, 67, 84, 71, 67, 71, 67, 65, 84, 84, 84, 67, 65, 67, 67, 71, 67, 84, 65, 67, 65, 67, 65, 84, 71, 71, 65, 65, 84, 84, 67, 67, 65, 67, 84, 84, 84, 67, 67, 84, 67, 84, 84, 67, 84, 71, 67, 65, 67, 84, 67, 65, 65, 71, 84, 84, 84, 84, 67, 67, 65, 71, 84, 84, 84, 67, 67, 65, 65, 84, 71, 65, 67, 67, 67, 84, 67, 67, 65, 67, 71, 71, 84, 84, 71, 65, 71, 67, 67, 71, 84, 71, 71, 71, 67, 84, 84, 84, 67, 65, 67, 65, 84, 67, 65, 71, 65, 67, 84, 84, 65, 65, 65, 65, 65, 65, 67, 67, 71, 67, 67, 84, 65, 67, 71, 67, 71, 67, 71, 67, 84, 84, 84, 65, 67, 71, 67, 67, 67, 65, 65, 84, 65, 65, 84, 84, 67, 67, 71, 71, 65, 84, 65, 65, 67, 71, 67, 84, 84, 71, 67, 67, 65, 67, 67, 84, 65, 67, 71, 84, 65, 84, 84, 65, 67, 67, 71, 67, 71, 71, 67, 84, 71, 67, 84, 71, 71, 67, 65, 67, 71, 84, 65, 71, 84, 84, 65, 71, 67, 67, 71, 84, 71, 71, 67, 84, 84, 84, 67, 84, 71, 65, 84, 84, 65, 71, 71, 84, 65, 67, 67, 71, 84, 67, 65, 65, 71, 65, 84, 71, 84, 71, 67, 65, 67, 65, 71, 84, 84, 65, 67, 84, 84, 65, 67, 65, 67, 65, 84, 65, 84, 71, 84, 84, 67, 84, 67, 67, 84, 65, 65, 84, 84, 65, 65, 67, 65, 71, 65, 71, 84, 84, 84, 84, 65, 67, 71, 65, 84, 67, 67, 71, 65, 65, 71, 65, 67, 67, 84, 84, 67, 65, 84, 67, 65, 67, 84, 67, 65, 67, 71, 67, 71, 71, 67, 71, 84, 84, 71, 67};
//
//            Console.WriteLine(seq1.Length);
//            Console.WriteLine(seq2.Length);
//            
//        
//            ISequence si = new Sequence(Alphabets.DNA, seq1);
//            ISequence sj = new Sequence(Alphabets.DNA, seq2);
//
//            const int go = -16;
//            const int ge = -4;
//            const string matrixName = "EDNAFULL";
//            const MoleculeType mt = MoleculeType.DNA;
//
//            var mgr = new ConfigurationMgr();
//            SimilarityMatrix scoringMatrix = mgr.SmithWatermanMS.LoadSimilarityMatrix(matrixName, mt);
//
//            var aligner = new SmithWatermanAligner();
//            IList<IPairwiseSequenceAlignment> psas = aligner.Align(scoringMatrix, go, ge, si, sj);
//            IPairwiseSequenceAlignment psa = psas[0]; // Take the first alignment
//            IList<PairwiseAlignedSequence> pass = psa.PairwiseAlignedSequences;
//            PairwiseAlignedSequence pas = pass[0]; // Take the first PairwisedAlignedSequence
//
//            Console.WriteLine("Alignment Length " + pas.FirstSequence.Count);
//            Console.Read();)


            return;

            // Load the command line args into our helper class which allows us to name arguments
            var pargs = new Arguments(args)
                            {
                                Usage = "Usage: SalsaMsMBF2Tester.exe /fastaFile /outDir"
                            };

            if (pargs.CheckRequired(new[] { "fastaFile", "outDir" }) == false)
            {
                Console.WriteLine(pargs.Usage);
                return;
            }

            var fasta = pargs.GetValue<string>("fastaFile");
            var outDir = pargs.GetValue<string>("outDir"); 

            // Read the fasta input file using FASTA parser.      
            var parser = new FastAParser(fasta);

            IEnumerable<ISequence> seqs = parser.Parse();
            var sequences = seqs.ToList();


            /*
             * Rank = RowBlock#:    618 (zero based index)
             * RowBlock Range:      [85902,86040] 
             * ColumnBlocks:        180, 182
             * ColumnBlocks Ranges: {[25020,25158],[25298,25436]}
             */

            const int rowStart618 = 85902;
            const int rowEnd618 = 86040;
            const int colStart180 = 25020;
            const int colEnd180 = 25158;
            const int colStart182 = 25298;
            const int colEnd182 = 25436;

            for (int i = 0; i < 100; i++)
            {
                int colBlockNumber = 180;
                Console.WriteLine("Beginning Column Block " + colBlockNumber + " for " + i + "iteration");
                Compute(sequences, rowStart618, rowEnd618, colStart180, colEnd180, colBlockNumber, i.ToString(), outDir);
                Console.WriteLine("End Column Block " + colBlockNumber);

                colBlockNumber = 182;
                Console.WriteLine("Beginning Column Block " + colBlockNumber);
                Compute(sequences, rowStart618, rowEnd618, colStart182, colEnd182, colBlockNumber, i.ToString(), outDir);
                Console.WriteLine("End Column Block " + colBlockNumber);
                
            }
           

            Console.WriteLine("Done.");
            Console.Read();
        }

        private static void Compute(List<ISequence> sequences, 
            int rowStart, 
            int rowEnd, 
            int colStart, 
            int colEnd,
            int colBlockNumber, 
            string suffix,
            string outDir)
        {
            string logFile = Path.Combine(outDir, "log_col_block_" + colBlockNumber + suffix + ".txt");
            string distFile = Path.Combine(outDir, "dist_col_block_" + colBlockNumber + suffix + ".bin");
            
            using(var writer = new StreamWriter(logFile))
            {
                short[][] dist = Align(sequences, rowStart, rowEnd, colStart, colEnd, writer);
//                WriteDistanceFile(distFile, dist);
            }
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

        

        private static short[][] Align(IList<ISequence> seqs, int rowStart, int rowEnd, int colStart, int colEnd, StreamWriter logWriter)
        {
            const int go = -16;
            const int ge = -4;
            const string matrixName = "EDNAFULL";
            const MoleculeType mt = MoleculeType.DNA;

            var mgr = new ConfigurationMgr();
            SimilarityMatrix scoringMatrix = mgr.SmithWatermanMS.LoadSimilarityMatrix(matrixName, mt);

            const string badSeqI = "SRR042382.3886";
            const string badSeqJ = "SRR042332.17927";

            int rowCount = (rowEnd - rowStart) + 1;
            int colCount = (colEnd - colStart) + 1;

            int totalPairs = rowCount*colCount;

            var dist = new short[rowCount][];

            var aligner = new SmithWatermanAligner();
            ISequence si, sj;
            var marker = new string('-', 20);
            bool inbad = false;

            int pairCount = 0;
            int pairPercent = 0;

//            int timeConst = 5000; // 5000 ms
//            Stopwatch logTimer = new Stopwatch();
            Console.WriteLine(" Completed " + pairPercent + "%");
//            logTimer.Start();

//            Stopwatch timer = new Stopwatch();
//            timer.Start();

            int pairsInterval = 200;
            int count = 0;
            for (int i = rowStart; i <= rowEnd; i++)
            {
                si = seqs[i];
                dist[i - rowStart] = new short[colCount];
                for (int j = colStart; j <= colEnd; j++)
                {
                    sj = seqs[j];
                    
                    if (badSeqI.Equals(si.ID) && badSeqJ.Equals(sj.ID))
                    {
                        inbad = true;
                        logWriter.WriteLine("bad,");
                    }
                    logWriter.Write(si.ID + "," + sj.ID + ",align");

                    IList<IPairwiseSequenceAlignment> psas = aligner.Align(scoringMatrix, go, ge, si, sj);
                    IPairwiseSequenceAlignment psa = psas[0]; // Take the first alignment
                    IList<PairwiseAlignedSequence> pass = psa.PairwiseAlignedSequences;
                    PairwiseAlignedSequence pas = pass[0]; // Take the first PairwisedAlignedSequence
                    
                    logWriter.Write(",done");
                    logWriter.Write(",dist");

                    dist[i - rowStart][j - colStart] = (short) ((1.0f - ComputePercentIdentity(pas))*short.MaxValue);

                    pairPercent = (100 * pairCount)/totalPairs;
                    logWriter.Write(",done," + pairPercent + "%");
//                    timer.Stop();
//                    logWriter.WriteLine(",elapsed(ms)," + timer.ElapsedMilliseconds);
//                    timer.Start();

//                    logTimer.Stop();
//                    if (logTimer.ElapsedMilliseconds > timeConst)
                    if (count > pairsInterval)
                    {
//                        timer.Stop();
//                        Console.WriteLine(" Completed " + pairPercent + "% elapsed " + timer.ElapsedMilliseconds + "ms" );
                        Console.WriteLine(" Completed " + pairPercent + "%" );
                        count = 0;
//                        logTimer.Restart();
//                        timer.Start();
                    }
                    else
                    {
                        count++;
//                        logTimer.Start();
                    }


                    if (inbad)
                    {
                        inbad = false;
                    }
                    pairCount++;
                    
                }
//                timer.Stop();
            }
            return dist;
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
