using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bio.IO.GenBank;
using Salsa.Core.Bio.Algorithms;
using Salsa.Core.Configuration;
using Salsa.Core.Configuration.Sections;


namespace NbackuTester
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
            swgms.MoleculeType = MoleculeType.DNA;
            swgms.NodeCount = 1;
            swgms.ProcessPerNodeCount = 3;
            swgms.ProjectName = "NBackupTest";
            swgms.ScoringMatrixName = "EDNAFULL";
            swgms.SequenceCount = 8;
            swgms.SummaryFile = @"C:\Users\sekanaya\Desktop\swgmstester\out\summary.txt";
            swgms.TimingFile = @"C:\Users\sekanaya\Desktop\swgmstester\out\timing.txt";
            swgms.WriteAlignments = false;
            swgms.WriteAlignmentsFile = @"C:\Users\sekanaya\Desktop\swgmstester\out\alignments.txt";
            swgms.WriteFullMatrix = true;

            mgr.SaveAs(@"C:\Users\sekanaya\Desktop\swgmstester\config.xml");
        }
    }
}
