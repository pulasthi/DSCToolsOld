using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bio;
using Bio.IO.FastA;
using Common;
using MPI;
using Salsa.Core;
using Salsa.Core.Blas;
#if USE_UINT16
using TDistance = System.UInt16;
#elif USE_INT16
using TDistance = System.Int16;

#else
using TDistance = System.Double;
#endif

namespace ScattersLargeScale
{    
    internal class Program
    {
        private static string _aMat;
        private static string _bMat;
        private static int _cols;
        private static int _rows;
        private static string _outdir;
        private static bool _useTDistanceMaxForA;
        private static bool _useTDistanceMaxForB;
        private static double _xmaxbound = 1.0;
        private static double _ymaxbound = 1.0;
        private static int _xres = 50;
        private static int _yres = 50;
        private static double _alpha = 2.0;
        private static double _pcutf = 0.85;
        private static bool _zto1 = true;
        private static int _aTransfm, _bTransfm;
        private static double _aTransfp, _bTransfp;
        private static double _distcutA;
        private static double _distcutB;
        private static double _mindistA;
        private static double _mindistB;
        private static string _aName;
        private static string _bName;
        private static string _clusterfile;
        private static string _title;
        private static double _lengthCut;
        private static bool _useClusters; // indicates if clustering data is available
        private static double[] _denomcuts;
        private static string _oldscoremat;
        private static string _newscoremat;
        private static bool _denomcutsenabled;
        private static bool _readPointsA; // indicates if the distance file is actually a 3D coordinate file for A
        private static bool _readPointsB; // indicates if the distance file is actually a 3D coordinate file for B
        
        private static double[] _xmaxWhole, _xminWhole, _ymaxWhole, _yminWhole;
        private static double[] _xmaxSelected, _xminSelected, _ymaxSelected, _yminSelected;
        private static double[] _xmaxSelectedInter, _xminSelectedInter, _ymaxSelectedInter, _yminSelectedInter;

        private static double[] _deltaxWhole,
                                _deltayWhole,
                                _deltaxSelected,
                                _deltaySelected,
                                _deltaxSelectedInter,
                                _deltaySelectedInter;

        // Surface area of each small 2D square
        private static double[] _deltasWhole, _deltasSelected, _deltasSelectedInter;

        private static long _totalPairs = 0, _totalIntraPairs = 0, _totalInterPairs = 0;
        private static long[] _consideredPairs, _consideredPairsIntra, _consideredPairsInter;

        private static readonly HashSet<int> SelectedCnums = new HashSet<int>();
        private static readonly Hashtable PnumToCnum = new Hashtable();
        private static List<ISequence> _seqs = new List<ISequence>();

        private static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: ScatterLargeScale.exe <configfile>");
            }
            
            using (new MPI.Environment(ref args))
            {
                ReadConfiguration(args[0]);
                InitializeArrays();

                if (_denomcutsenabled)
                {
                    Array.Sort(_denomcuts);
                }
                
                if (_useClusters)
                {
                    PopulatePnumToCnum();
                }

                int rank = Communicator.world.Rank;
                int worldSize = Communicator.world.Size;

                Block[][] processToCloumnBlocks = BlockPartitioner.Partition(_rows, _cols, worldSize, worldSize);
                Block[] myColumnBlocks = processToCloumnBlocks[rank];

                PartialMatrix<TDistance> myRowStripMatrixForA =
                    new PartialMatrix<TDistance>(myColumnBlocks[0].RowRange, new Range(0, _cols - 1));
                PartialMatrix<TDistance> myRowStripMatrixForB =
                    new PartialMatrix<TDistance>(myColumnBlocks[0].RowRange, new Range(0, _cols - 1));
                PartialMatrix<byte> myRowStripMatrixForDenomCut = new PartialMatrix<byte>(
                    myColumnBlocks[0].RowRange, new Range(0, _cols - 1));

                InitalizeDenomMask(myColumnBlocks, myRowStripMatrixForDenomCut);

                ReadDistanceBlocks(myRowStripMatrixForA, myRowStripMatrixForB, myColumnBlocks,
                                   myRowStripMatrixForDenomCut);

                _xminWhole = Communicator.world.Allreduce(_xminWhole, Operation<double>.Min);
                _xmaxWhole = Communicator.world.Allreduce(_xmaxWhole, Operation<double>.Max);
                _yminWhole = Communicator.world.Allreduce(_yminWhole, Operation<double>.Min);
                _ymaxWhole = Communicator.world.Allreduce(_ymaxWhole, Operation<double>.Max);

                _totalPairs = Communicator.world.Reduce(_totalPairs, Operation<long>.Add, 0);
                _consideredPairs = Communicator.world.Reduce(_consideredPairs, Operation<long>.Add, 0);
                

                if (_useClusters)
                {
                    _xminSelected = Communicator.world.Allreduce(_xminSelected, Operation<double>.Min);
                    _xmaxSelected = Communicator.world.Allreduce(_xmaxSelected, Operation<double>.Max);
                    _yminSelected = Communicator.world.Allreduce(_yminSelected, Operation<double>.Min);
                    _ymaxSelected = Communicator.world.Allreduce(_ymaxSelected, Operation<double>.Max);

                    _xminSelectedInter = Communicator.world.Allreduce(_xminSelectedInter, Operation<double>.Min);
                    _xmaxSelectedInter = Communicator.world.Allreduce(_xmaxSelectedInter, Operation<double>.Max);
                    _yminSelectedInter = Communicator.world.Allreduce(_yminSelectedInter, Operation<double>.Min);
                    _ymaxSelectedInter = Communicator.world.Allreduce(_ymaxSelectedInter, Operation<double>.Max);

                    _totalIntraPairs = Communicator.world.Reduce(_totalIntraPairs, Operation<long>.Add, 0);
                    _totalInterPairs = Communicator.world.Reduce(_totalInterPairs, Operation<long>.Add, 0);

                    _consideredPairsIntra = Communicator.world.Reduce(_consideredPairsIntra, Operation<long>.Add, 0);
                    _consideredPairsInter = Communicator.world.Reduce(_consideredPairsInter, Operation<long>.Add, 0);
                }

                // Output min/max
                if (rank == 0)
                {
                    Console.WriteLine(new string('*',20));
                    Console.WriteLine("Denomcut Enabled: " + _denomcutsenabled);
                    for (int i = 0; i < _denomcuts.Length; i++)
                    {
                        Console.WriteLine("\n\tDenomcut: {0}", _denomcuts[i]);
                        Console.WriteLine("\txmaxwhole:{0} xminwhole:{1} ymaxwhole:{2} yminwhole:{3}", _xmaxWhole[i],
                                          _xminWhole[i],_ymaxWhole[i], _yminWhole[i]);
                        if (_useClusters)
                        {
                            Console.WriteLine("\txmaxselected:{0} xminselected:{1} ymaxselected:{2} yminselected:{3}",
                                              _xmaxSelected[i], _xminSelected[i], _ymaxSelected[i], _yminSelected[i]);
                            Console.WriteLine(
                                "\txmaxselectedinter:{0} xminselectedinter:{1} ymaxselectedinter:{2} yminselectedinter:{3}",
                                _xmaxSelectedInter[i], _xminSelectedInter[i], _ymaxSelectedInter[i], _yminSelectedInter[i]);
                        }
                    }
                }

                for (int i = 0; i < _denomcuts.Length; i++)
                {
                    // global xmax, xmin, ymax, and ymin should be set by now
                    _deltaxWhole[i] = (_xmaxWhole[i] - _xminWhole[i]) / _xres;
                    _deltayWhole[i] = (_ymaxWhole[i] - _yminWhole[i]) / _yres;
                    _deltasWhole[i] = _deltaxWhole[i] * _deltayWhole[i];

                    if (_useClusters)
                    {
                        _deltaxSelected[i] = (_xmaxSelected[i] - _xminSelected[i]) / _xres;
                        _deltaySelected[i] = (_ymaxSelected[i] - _yminSelected[i]) / _yres;
                        _deltasSelected[i] = _deltaxSelected[i] * _deltaySelected[i];

                        _deltaxSelectedInter[i] = (_xmaxSelectedInter[i] - _xminSelectedInter[i]) / _xres;
                        _deltaySelectedInter[i] = (_ymaxSelectedInter[i] - _yminSelectedInter[i]) / _yres;
                        _deltasSelectedInter[i] = _deltaxSelectedInter[i] * _deltaySelectedInter[i];
                    }
                }

                long[][][] histCellsForWholeSample = new long[_denomcuts.Length][][];
                long[][][] histCellsForSelectedClusters = new long[_denomcuts.Length][][];
                long[][][] histCellsForSelectedClustersInter = new long[_denomcuts.Length][][];
                for (int i = 0; i < _denomcuts.Length; i++)
                {
                    histCellsForWholeSample[i] = new long[_yres][];
                    histCellsForSelectedClusters[i] = new long[_yres][];
                    histCellsForSelectedClustersInter[i] = new long[_yres][];
                    for (int j = 0; j < _yres; j++)
                    {
                        histCellsForWholeSample[i][j] = new long[_xres];
                        histCellsForSelectedClusters[i][j] = new long[_xres];
                        histCellsForSelectedClustersInter[i][j] = new long[_xres];
                        for (int k = 0; k < _xres; k++)
                        {
                            histCellsForWholeSample[i][j][k] = 0;
                            histCellsForSelectedClusters[i][j][k] = 0;
                            histCellsForSelectedClustersInter[i][j][k] = 0;
                        }
                    }
                }
                GeneratePartialHistograms(histCellsForWholeSample, histCellsForSelectedClusters,
                                          histCellsForSelectedClustersInter,
                                          myRowStripMatrixForA, myRowStripMatrixForB, myColumnBlocks,
                                          myRowStripMatrixForDenomCut);

                histCellsForWholeSample = Communicator.world.Reduce(histCellsForWholeSample, Sum2DArray,0);

                if (_useClusters)
                {
                    histCellsForSelectedClusters = Communicator.world.Reduce(histCellsForSelectedClusters, Sum2DArray,0);
                    histCellsForSelectedClustersInter = Communicator.world.Reduce(histCellsForSelectedClustersInter, Sum2DArray,0);
                }


                if (rank == 0)
                {
                    // Rank 0 should have all the cells from each process by now.
                    for (int i = 0; i < _denomcuts.Length; i++)
                    {
                        double denomcut = _denomcuts[i];

                        double pairFraction = ((double) _consideredPairs[i])/_totalPairs;

                        Console.WriteLine(
                            "Rank 0 starting to write density data file for whole sample with denomcut " + denomcut);
                        GenerateDensityDataFile(histCellsForWholeSample[i], _xmaxWhole[i], _xminWhole[i], _ymaxWhole[i],
                                                _yminWhole[i], _deltaxWhole[i], _deltayWhole[i], _deltasWhole[i],
                                                "whole", denomcut, pairFraction, _totalPairs);
                        Console.WriteLine("Rank 0 done writing density data file for whole sample with denomcut " +
                                          denomcut);

                        if (_useClusters)
                        {
                            pairFraction = ((double) _consideredPairsIntra[i])/_totalIntraPairs;
                            Console.WriteLine(
                                "Rank 0 starting to write density data file for selected clusters with denomcut " +
                                denomcut);
                            GenerateDensityDataFile(histCellsForSelectedClusters[i], _xmaxSelected[i], _xminSelected[i],
                                                    _ymaxSelected[i], _yminSelected[i], _deltaxSelected[i],
                                                    _deltaySelected[i], _deltasSelected[i], "selected", denomcut, pairFraction, _totalIntraPairs);
                            Console.WriteLine(
                                "Rank 0 done writing density data file for selected clusters with denomcut " +
                                denomcut);

                            pairFraction = ((double) _consideredPairsInter[i])/_totalInterPairs;
                            Console.WriteLine(
                                "Rank 0 starting to write density data file for selected clusters inter with denomcut " +
                                denomcut);
                            GenerateDensityDataFile(histCellsForSelectedClustersInter[i], _xmaxSelectedInter[i],
                                                    _xminSelectedInter[i], _ymaxSelectedInter[i], _yminSelectedInter[i],
                                                    _deltaxSelectedInter[i], _deltaySelectedInter[i],
                                                    _deltasSelectedInter[i], "selected-inter", denomcut, pairFraction, _totalInterPairs);
                            Console.WriteLine(
                                "Rank 0 done writing density data file for selected clusters inter with denomcut " +
                                denomcut);
                        }
                    }
                }

                Communicator.world.Barrier();
            }
        }

        private static void InitalizeDenomMask(Block[] myColumnBlocks, PartialMatrix<byte> myRowStripMatrixForDenomCut)
        {
            foreach (Block block in myColumnBlocks)
            {
                for (int r = block.RowRange.StartIndex; r <= block.RowRange.EndIndex; ++r)
                {
                    for (int c = block.ColumnRange.StartIndex; c <= block.ColumnRange.EndIndex; ++c)
                    {
                        myRowStripMatrixForDenomCut[r, c] = byte.MaxValue;
                    }
                        
                }
            }
        }

        private static void InitializeArrays()
        {
            int length = _denomcuts.Length; // if denomcuts not enabled then this will be just 1
            _xmaxWhole = new double[length];
            _xminWhole = new double[length];
            _ymaxWhole = new double[length];
            _yminWhole = new double[length];

            _xmaxSelected = new double[length];
            _xminSelected = new double[length];
            _ymaxSelected = new double[length];
            _yminSelected = new double[length];

            _xmaxSelectedInter = new double[length];
            _xminSelectedInter = new double[length];
            _ymaxSelectedInter = new double[length];
            _yminSelectedInter = new double[length];

            _deltaxWhole = new double[length];
            _deltayWhole = new double[length];
            _deltaxSelected = new double[length];
            _deltaySelected = new double[length];
            _deltaxSelectedInter = new double[length];
            _deltaySelectedInter = new double[length];

            _deltasWhole = new double[length];
            _deltasSelected = new double[length];
            _deltasSelectedInter = new double[length];

            _consideredPairs = new long[length];
            _consideredPairsIntra = new long[length];
            _consideredPairsInter = new long[length];

            for (int i = 0; i < length; i++)
            {
                _xmaxWhole[i] = _xmaxSelected[i] = _xmaxSelectedInter[i] = double.NegativeInfinity;
                _ymaxWhole[i] = _ymaxSelected[i] = _ymaxSelectedInter[i] = double.NegativeInfinity;

                _xminWhole[i] = _xminSelected[i] = _xminSelectedInter[i] = double.PositiveInfinity;
                _yminWhole[i] = _yminSelected[i] = _yminSelectedInter[i] = double.PositiveInfinity;

                _consideredPairs[i] = 0;
                _consideredPairsIntra[i] = 0;
                _consideredPairsInter[i] = 0;
            }
        }

        private static void PopulatePnumToCnum()
        {
            using (
                StreamReader reader =
                    new StreamReader(File.Open(_clusterfile, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                char[] sep = new[] {' ', '\t'};
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        string[] splits = line.Split(sep);
                        int idx = int.Parse(splits[0]);
                        int cnum = int.Parse(splits[1]);
                        PnumToCnum[idx] = cnum;
                    }
                }
            }
        }


        private static void GenerateDensityDataFile(long[][] cells, double xmax, double xmin,
                                                    double ymax, double ymin,
                                                    double deltax, double deltay, double deltas, string prefix,
                                                    double denomcut, double pairFraction, long totalPairs)
        {
            long[] xHist = new long[_xres];
            long[] yHist = new long[_yres];

            for (int i = 0; i < _xres; i++)
            {
                xHist[i] = 0;
            }

            for (int i = 0; i < _yres; i++)
            {
                yHist[i] = 0;
            }

            long cellmax = 0, count = 0, v;
            for (int i = 0; i < _yres; i++)
            {
                for (int j = 0; j < _xres; j++)
                {
                    v = cells[i][j];
                    xHist[j] += v;
                    yHist[i] += v;
                    count += v;
                    if (v > cellmax) {cellmax = v;}
                }
            }

            
            double cellmean = ((double) count)/(_xres*_yres);
            double power = cellmax < (_alpha*cellmean) ? 1.0 : (Math.Log(_alpha)/Math.Log(cellmax/cellmean));
            // Constant value by which the number of points in a 2D square is multiplied.
            // The resulting value is independent of the total number of points as well as 
            // the x,y resolution. The mult value is a factor changing the z value scale.
            double c = _zto1 ? (1.0/cellmax) : (1.0/(count*deltas));

            // Output density values
            Console.WriteLine(new string('*', 40));
            Console.WriteLine("DataSet\t" + prefix);
            Console.WriteLine("CellMean\t" + cellmean);
            Console.WriteLine("CellMax\t" + cellmax);
            Console.WriteLine("Power\t" + power);
            Console.WriteLine("Const\t" + c);
            Console.WriteLine("TotalPairs\t" + totalPairs);
            Console.WriteLine("PairFraction\t" + pairFraction);
            for (int i = 0; i < 10; i++)
            {
                double density = i/10.0;
                double densityToCount = Math.Pow(density, (1/power))/c;
                Console.WriteLine(density + "\t" + densityToCount);
            }
            Console.WriteLine(new string('*', 40));

            int xpointcount = 2*_xres;
            int ypointcount = 2*_yres;

            string aNameFinal = _aTransfm > -1 ? "Transformed-" + _aName : _aName;
            string bNameFinal = _bTransfm > -1 ? "Transformed-" + _bName : _bName;

            string dCutStringA = "DCut[" + (_mindistA > -1 ? _mindistA.ToString() : "none") + "," + (_distcutA > -1 ? _distcutA.ToString() : "none") + "]";
            string dCutStringB = "DCut[" + (_mindistB > -1 ? _mindistB.ToString() : "none") + "," + (_distcutB > -1 ? _distcutB.ToString() : "none") + "]";
            string vsString = bNameFinal + "-Vs-" + aNameFinal;

            string dir = _outdir;
            if (_denomcutsenabled)
            {
                dir = Path.Combine(_outdir, "denomcut_" + denomcut.ToString());
                Directory.CreateDirectory(dir);
            }

            string densityFile = Path.Combine(dir, prefix + "-density-" + dCutStringA + "-" + dCutStringB + "-" + vsString + ".txt");

            string xHistFile = Path.Combine(dir, prefix + "-xHist-" + dCutStringA + "-" + dCutStringB + "-" + vsString + ".txt");

            string yHistFile = Path.Combine(dir, prefix + "-yHist-" + dCutStringA + "-" + dCutStringB + "-" + vsString + ".txt");

            string gnuplotScriptFileLarge = Path.Combine(dir,
                                                         prefix + "-gnuplot-" + dCutStringA + "-" + dCutStringB + "-" + vsString +
                                                         "-large.txt");
            string gnuplotScriptFileSmall = Path.Combine(dir,
                                                         prefix + "-gnuplot-" + dCutStringA + "-" + dCutStringB + "-" + vsString +
                                                         "-small.txt");

            string plotBat = Path.Combine(dir, "plot.bat");

            using (StreamWriter densityFileWriter = new StreamWriter(densityFile),
                                xHistWriter = new StreamWriter(xHistFile),
                                yHistWriter = new StreamWriter(yHistFile),
                                gnuplotWriterLarge = new StreamWriter(gnuplotScriptFileLarge),
                                gnuplotWriterSmall = new StreamWriter(gnuplotScriptFileSmall),
                                plotBatWriter = new StreamWriter(plotBat, true))
            {
                // Generating plot bat
                plotBatWriter.WriteLine("gnuplot " + Path.GetFileName(gnuplotScriptFileLarge));

                densityFileWriter.WriteLine("#xcoord\tycoord\thistogramValue");
                xHistWriter.WriteLine("#xval\thistogramvalue");
                yHistWriter.WriteLine("#yval\thistogramvalue");

                // Generating x histogram
                double xoffset = xmin + 0.5*deltax;
                for (int i = 0; i < _xres; ++i)
                {
                    double xcoord = xoffset + i*deltax;
                    xHistWriter.WriteLine(xcoord + "\t" + xHist[i]);
                }

                // Generating y histogram
                double yoffset = ymin + 0.5*deltay;
                for (int i = 0; i < _yres; ++i)
                {
                    double ycoord = yoffset + i*deltay;
                    yHistWriter.WriteLine(ycoord + "\t" + yHist[i]);
                }

                for (int i = 0; i < xpointcount; i++)
                {
                    double x = xmin + ((IsOdd(i) ? (i + 1)/2 : i/2)*deltax);
                    int cellx = IsOdd(i) ? (i - 1)/2 : i/2;

                    for (int j = 0; j < ypointcount; j++)
                    {
                        double y = ymin + ((IsOdd(j) ? (j + 1)/2 : j/2)*deltay);
                        int celly = IsOdd(j) ? (j - 1)/2 : j/2;

                        double cellvalue = Math.Pow((cells[celly][cellx]*c),power);

                        // todo: commented for now
                        // cellvalue = cellvalue > pcutf ? pcutf : cellvalue < ncutf ? ncutf : cellvalue;
                        cellvalue = _pcutf > -1 && cellvalue > _pcutf ? _pcutf : cellvalue;

                        densityFileWriter.WriteLine(x + "\t" + y + "\t" + cellvalue);
                    }
                    densityFileWriter.WriteLine();
                }

                if (_xmaxbound == -1)
                {
                    _xmaxbound = xmax;
                }

                if (_ymaxbound == -1)
                {
                    _ymaxbound = ymax;
                }

                // Fill up the remaining region from beyond x=xmax and y=ymax as zero 
                densityFileWriter.WriteLine();
                densityFileWriter.WriteLine(xmin + "\t" + ymax + "\t" + 0.0);
                densityFileWriter.WriteLine(xmin + "\t" + _ymaxbound + "\t" + 0.0);
                densityFileWriter.WriteLine();
                densityFileWriter.WriteLine(xmax + "\t" + ymax + "\t" + 0.0);
                densityFileWriter.WriteLine(xmax + "\t" + _ymaxbound + "\t" + 0.0);
                densityFileWriter.WriteLine();
                densityFileWriter.WriteLine(xmax + "\t" + ymin + "\t" + 0.0);
                densityFileWriter.WriteLine(xmax + "\t" + _ymaxbound + "\t" + 0.0);
                densityFileWriter.WriteLine();
                densityFileWriter.WriteLine(_xmaxbound + "\t" + ymin + "\t" + 0.0);
                densityFileWriter.WriteLine(_xmaxbound + "\t" + _ymaxbound + "\t" + 0.0);


                /* Writing Gnuplot script */
                WriteGnuplotScript(bNameFinal, aNameFinal, prefix, vsString, densityFile, xHistFile, yHistFile,
                                   gnuplotWriterLarge, gnuplotWriterSmall, denomcut, pairFraction, totalPairs);
            }
        }

        private static void WriteGnuplotScript(string bNameFinal, string aNameFinal, string prefix, string vsString,
                                               string densityFile, string xHistFile, string yHistFile,
                                               StreamWriter gnuplotWriterLarge, StreamWriter gnuplotWriterSmall,
                                               double denomcut, double pairFraction, long totalPairs)
        {
            gnuplotWriterLarge.WriteLine("set terminal png truecolor nocrop font arial 14 size 1200,1200");
            gnuplotWriterSmall.WriteLine("set terminal png truecolor nocrop font arial 14 size 1000,500");

            gnuplotWriterLarge.WriteLine();

            string pngfile = prefix + "-plot-" + vsString + "DensitySat[" + (_pcutf > -1 ? _pcutf.ToString() : "none") +
                             "]" + (_denomcutsenabled ? "-DenomCut[" + denomcut + "]-" : "-") + "large.png";
            gnuplotWriterLarge.WriteLine("set output '" + pngfile + "'");
            pngfile = prefix + "-plot-" + vsString + "DensitySat[" + (_pcutf > -1 ? _pcutf.ToString() : "none") + "]" +
                      (_denomcutsenabled ? "-DenomCut[" + denomcut + "]-" : "-") + "small.png";
            gnuplotWriterSmall.WriteLine("set output '" + pngfile + "'");

            gnuplotWriterLarge.WriteLine("set size 1.0, 1.0");
            gnuplotWriterLarge.WriteLine("set multiplot");
            gnuplotWriterSmall.WriteLine("set multiplot");

            gnuplotWriterLarge.WriteLine();
            gnuplotWriterSmall.WriteLine();

            // Title box
            gnuplotWriterLarge.WriteLine("set origin 0.0, 0.85");
//            gnuplotWriterSmall.WriteLine("set origin 0.0, 0.7");
            gnuplotWriterLarge.WriteLine("set size 0.95, 0.1");
//            gnuplotWriterSmall.WriteLine("set size 0.95, 0.1");
            gnuplotWriterLarge.WriteLine("set border linecolor rgbcolor \"white\"");
//            gnuplotWriterSmall.WriteLine("set border linecolor rgbcolor \"white\"");
            gnuplotWriterLarge.WriteLine("unset key");
//            gnuplotWriterSmall.WriteLine("unset key");
            string dcutStringA = (_mindistA > -1 ? _mindistA.ToString() : "none") + "," + (_distcutA > -1 ? _distcutA.ToString() : "none");
            string dcutStringB = (_mindistB > -1 ? _mindistB.ToString() : "none") + "," + (_distcutB > -1 ? _distcutB.ToString() : "none");
            string title = string.Format(_title, (_pcutf > -1 ? _pcutf.ToString() : "none"), bNameFinal, dcutStringB,
                                         aNameFinal, dcutStringA,
                                         (prefix.Equals("whole")
                                              ? "Whole Sample"
                                              : (prefix.Equals("selected")
                                                     ? "Selected Clusters Intra Pairs"
                                                     : "Selected Clusters Inter Pairs")));
            if (_denomcutsenabled)
            {
                title += "\\nDenomCut[" + denomcut + "] PairFraction[" + Math.Round(pairFraction,6) +"] TotalPairs[" + totalPairs + "]";
            }
            gnuplotWriterLarge.WriteLine("set title \"" + title + "\" textcolor rgbcolor \"black\"");
//            gnuplotWriterSmall.WriteLine("set title \"" + title + "textcolor rgbcolor \"black\"");
            gnuplotWriterLarge.WriteLine("plot [0:1] [0:1] 0.0 lt rgb \"white\"");
//            gnuplotWriterSmall.WriteLine("plot [0:1] [0:1] 0.0 lt rgb \"white\"");

            gnuplotWriterLarge.WriteLine("set border linecolor rgbcolor \"black\"");
            gnuplotWriterSmall.WriteLine("set border linecolor rgbcolor \"black\"");

            gnuplotWriterLarge.WriteLine("set dummy u,v");
            gnuplotWriterSmall.WriteLine("set dummy u,v");

            gnuplotWriterLarge.WriteLine("unset key");
            gnuplotWriterSmall.WriteLine("unset key");

            gnuplotWriterLarge.WriteLine("set size ratio 1.0");
//            gnuplotWriterSmall.WriteLine("set size ratio 1.0");

            gnuplotWriterLarge.WriteLine("set style fill  solid 0.85 noborder");
            gnuplotWriterSmall.WriteLine("set style fill  solid 0.85 noborder");

            gnuplotWriterLarge.WriteLine("set style line 1 lt 1 lw 4");
            gnuplotWriterSmall.WriteLine("set style line 1 lt 1 lw 4");

            gnuplotWriterLarge.WriteLine("set pm3d map");
            gnuplotWriterSmall.WriteLine("set pm3d map");

            gnuplotWriterLarge.WriteLine("set palette rgbformulae 30,31,32 model RGB negative");
            gnuplotWriterSmall.WriteLine("set palette rgbformulae 30,31,32 model RGB negative");

            gnuplotWriterLarge.WriteLine();
            gnuplotWriterSmall.WriteLine();

            // Y histogram (rotated)
            gnuplotWriterLarge.WriteLine("set origin 0.0, 0.45");
            gnuplotWriterLarge.WriteLine("set size 0.45, 0.45");
            gnuplotWriterLarge.WriteLine("set xtics rotate by -90");
            string xlabel = "Count";
            gnuplotWriterLarge.WriteLine("set xlabel \"" + xlabel + "\" textcolor rgbcolor \"black\"");
            string ylabel = bNameFinal;
            gnuplotWriterLarge.WriteLine("set ylabel \"" + ylabel + "\" textcolor rgbcolor \"black\"");
            title = "Histogram (rotated) of " + bNameFinal + " distances";
            gnuplotWriterLarge.WriteLine("set title \"" + title + "\" textcolor rgbcolor \"black\"");
            gnuplotWriterLarge.WriteLine("plot [][:" + _ymaxbound + "] '" + Path.GetFileName(yHistFile) +
                                         "' using 2:1 with filledcurves y1 lt rgb \"black\"");

            gnuplotWriterLarge.WriteLine("set xtics rotate by 0");
            gnuplotWriterLarge.WriteLine();
            

            // Density plot
            gnuplotWriterLarge.WriteLine("set origin 0.45, 0.45");
            gnuplotWriterSmall.WriteLine("set origin 0.0, 0.0");
            gnuplotWriterLarge.WriteLine("set size 0.5, 0.5");
            gnuplotWriterSmall.WriteLine("set size square 0.5, 1.0");
            xlabel = aNameFinal;
            gnuplotWriterLarge.WriteLine("set xlabel \"" + xlabel + "\" textcolor rgbcolor \"black\"");
            gnuplotWriterSmall.WriteLine("set xlabel \"" + xlabel + "\" textcolor rgbcolor \"black\"");
            ylabel = bNameFinal;
            gnuplotWriterLarge.WriteLine("set ylabel \"" + ylabel + "\" textcolor rgbcolor \"black\"");
            gnuplotWriterSmall.WriteLine("set ylabel \"" + ylabel + "\" textcolor rgbcolor \"black\"");
            title = "Heat Map of " + vsString;
            gnuplotWriterLarge.WriteLine("set title \"" + title + "\" textcolor rgbcolor \"black\"");
            gnuplotWriterSmall.WriteLine("set title \"" + title + "\" textcolor rgbcolor \"black\"");
            gnuplotWriterLarge.WriteLine("splot [:" + _xmaxbound + "] [:" + _ymaxbound + "] '" +
                                         Path.GetFileName(densityFile) + "'");
            gnuplotWriterSmall.WriteLine("splot [:" + _xmaxbound + "] [:" + _ymaxbound + "] '" +
                                         Path.GetFileName(densityFile) + "'");

            gnuplotWriterLarge.WriteLine("unset pm3d");


            gnuplotWriterLarge.WriteLine();
            gnuplotWriterSmall.WriteLine();

            // Y histogram (unrotated)
            gnuplotWriterLarge.WriteLine("set origin 0.0, 0.0");
            gnuplotWriterLarge.WriteLine("set size 0.45, 0.45");
            xlabel = bNameFinal;
            gnuplotWriterLarge.WriteLine("set xlabel \"" + xlabel + "\" textcolor rgbcolor \"black\"");
            ylabel = "Count";
            gnuplotWriterLarge.WriteLine("set ylabel \"" + ylabel + "\" textcolor rgbcolor \"black\"");
            title = "Histogram of " + bNameFinal + " distances";
            gnuplotWriterLarge.WriteLine("set title \"" + title + "\" textcolor rgbcolor \"black\"");
            gnuplotWriterLarge.WriteLine("plot [:" + _ymaxbound + "] []'" + Path.GetFileName(yHistFile) +
                                         "' with filledcurves x1 lt rgb \"black\"");


            gnuplotWriterLarge.WriteLine();

            // X histogram
            gnuplotWriterLarge.WriteLine("set origin 0.45, 0.0");
            gnuplotWriterSmall.WriteLine("set origin 0.5, 0.08");
            gnuplotWriterLarge.WriteLine("set size 0.45, 0.45");
            gnuplotWriterSmall.WriteLine("set size square 0.5, 0.85");
            xlabel = aNameFinal;
            gnuplotWriterLarge.WriteLine("set xlabel \"" + xlabel + "\" textcolor rgbcolor \"black\"");
            gnuplotWriterSmall.WriteLine("set xlabel \"" + xlabel + "\" textcolor rgbcolor \"black\"");
            ylabel = "Count";
            gnuplotWriterLarge.WriteLine("set ylabel \"" + ylabel + "\" textcolor rgbcolor \"black\"");
            gnuplotWriterSmall.WriteLine("set ylabel \"" + ylabel + "\" textcolor rgbcolor \"black\"");
            title = "Histogram of " + aNameFinal + " distances";
            gnuplotWriterLarge.WriteLine("set title \"" + title + "\" textcolor rgbcolor \"black\"");
            gnuplotWriterSmall.WriteLine("set title \"" + title + "\" textcolor rgbcolor \"black\"");
            gnuplotWriterLarge.WriteLine("plot [:" + _xmaxbound + "] []'" + Path.GetFileName(xHistFile) +
                                         "' with filledcurves x1 lt rgb \"black\"");
            gnuplotWriterSmall.WriteLine("plot [:" + _xmaxbound + "] []'" + Path.GetFileName(xHistFile) +
                                         "' with filledcurves x1 lt rgb \"black\"");

            gnuplotWriterLarge.WriteLine();
            gnuplotWriterSmall.WriteLine();

            gnuplotWriterLarge.WriteLine("unset multiplot");
        }

        private static void GeneratePartialHistograms(long[][][] histCellsForWholeSample,
                                                      long[][][] histCellsForSelectedClusters,
                                                      long[][][] histCellsForSelectedClustersInter,
                                                      PartialMatrix<TDistance> myRowStripMatrixForA,
                                                      PartialMatrix<TDistance> myRowStripMatrixForB, Block[] myBlocks,
                                                      PartialMatrix<byte> myRowStripMatrixForDenomCut)
        {
            DistanceReader distanceReaderA = null, distanceReaderB = null;
            if (_readPointsA)
            {
#if USE_UINT16
                distanceReaderA = new DistanceReader(_aMat, MatrixType.UInt16, _cols, _readPointsA);
#elif USE_INT16
                distanceReaderA = new DistanceReader(_aMat, MatrixType.Int16, _cols, _readPointsA);
#else
                distanceReaderA = new DistanceReader(_aMat, MatrixType.Double, _cols, _readPointsA);
#endif
            }

            if (_readPointsB)
            {
#if USE_UINT16
                distanceReaderB = new DistanceReader(_aMat, MatrixType.UInt16, _cols, _readPointsB);
#elif USE_INT16
                distanceReaderB = new DistanceReader(_bMat, MatrixType.Int16, _cols, _readPointsB);
#else
                distanceReaderB = new DistanceReader(_aMat, MatrixType.Double, _cols, _readPointsB);
#endif
            }
            
            for (int i = 0; i < myBlocks.Length; ++i)
            {
                Block block = myBlocks[i];
                // Non diagonal block
                for (int r = block.RowRange.StartIndex; r <= block.RowRange.EndIndex; ++r)
                {
                    long l1 = _lengthCut > -1 ? _seqs[r].Count : -1;
                    for (int c = block.ColumnRange.StartIndex; c <= block.ColumnRange.EndIndex; ++c)
                    {
                        long l2 = _lengthCut > -1 ? _seqs[c].Count : -1;

                        // Each pair in block
                        double x = !_readPointsA
                                       ? (_useTDistanceMaxForA
                                              ? ((double) myRowStripMatrixForA[r, c])/TDistance.MaxValue
                                              : myRowStripMatrixForA[r, c])
                                       : distanceReaderA.ReadDistanceFromPointsFile(r, c);
                        double y = !_readPointsB
                                       ? (_useTDistanceMaxForB
                                              ? ((double) myRowStripMatrixForB[r, c])/TDistance.MaxValue
                                              : myRowStripMatrixForB[r, c])
                                       : distanceReaderB.ReadDistanceFromPointsFile(r, c);

                        // Ignore x or y values greater than distcutA or discutB respectively when distcut values are specified
                        if ((_distcutA > -1 && x > _distcutA) || (_distcutB > -1 && y > _distcutB)) continue;

                        // Ignore x or y values smaller than mindistA or mindistB respectively when mindist values are specified
                        if ((_mindistA > -1 && x < _mindistA) || (_mindistB > -1 && y < _mindistB)) continue;

                        // Ignore if the corresponding two sequence lengths are not within the given lengthcut
                        if (_lengthCut > -1 && (Math.Abs(l1 - l2) > _lengthCut*((l1 + l2)/2.0))) continue;

                        // Perform transforms (no transform if transform method is -1 for the respective matrix)
                        x = Transform(x, _aTransfm, _aTransfp);
                        y = Transform(y, _bTransfm, _bTransfp);

                        for (int j = 0; j < _denomcuts.Length; j++)
                        {
                            if (myRowStripMatrixForDenomCut[r, c] > j)
                            {
                                UpdateCells(x, y, _xmaxWhole[j], _xminWhole[j], _ymaxWhole[j], _yminWhole[j],
                                            _deltaxWhole[j], _deltayWhole[j], histCellsForWholeSample[j], r, c);

                                if (_useClusters)
                                {
                                    int rCnum = ((int) PnumToCnum[r]);
                                    int cCnum = ((int) PnumToCnum[c]);
                                    if (SelectedCnums.Contains(rCnum) && SelectedCnums.Contains(cCnum))
                                    {
                                        if (rCnum == cCnum)
                                        {
                                            // Intra cluster distances
                                            UpdateCells(x, y, _xmaxSelected[j], _xminSelected[j],
                                                        _ymaxSelected[j], _yminSelected[j], _deltaxSelected[j],
                                                        _deltaySelected[j], histCellsForSelectedClusters[j], r,
                                                        c);
                                        }
                                        else
                                        {
                                            // Inter cluster distances
                                            UpdateCells(x, y, _xmaxSelectedInter[j], _xminSelectedInter[j],
                                                        _ymaxSelectedInter[j], _yminSelectedInter[j],
                                                        _deltaxSelectedInter[j], _deltaySelectedInter[j],
                                                        histCellsForSelectedClustersInter[j], r, c);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }            
        }

        private static double Transform(double val, int transfm, double transfp)
        {
            if (transfm == 10)
            {
                val = Math.Min(1.0, val);
                return Math.Pow(val, transfp);
            }
            return val;
        }

        private static void ReadDistanceBlocks(PartialMatrix<TDistance> myRowStripMatrixForA,
                                               PartialMatrix<TDistance> myRowStripMatrixForB, Block[] myColumnBlocks,
                                               PartialMatrix<byte> myRowStripMatrixForDenomCut)
        {
#if USE_UINT16
            using (DistanceReader matReaderA = new DistanceReader(_aMat, MatrixType.UInt16, _cols, _readPointsA),
                                matReaderB = new DistanceReader(_bMat, MatrixType.UInt16, _cols, _readPointsB))
#elif USE_INT16
            using (DistanceReader matReaderA = new DistanceReader(_aMat, MatrixType.Int16, _cols, _readPointsA),
                                matReaderB = new DistanceReader(_bMat, MatrixType.Int16, _cols, _readPointsB))
#else
            using (DistanceReader matReaderA = new DistanceReader(_aMat, MatrixType.Double, _cols, _readPointsA),
                                matReaderB = new DistanceReader(_bMat, MatrixType.Double, _cols, _readPointsB))
#endif
            {
                MatrixReader oldScoreReader = null, newScoreReader = null;
                if (_denomcutsenabled)
                {
                    oldScoreReader = new MatrixReader(_oldscoremat, MatrixType.Int16, _cols);
                    newScoreReader = new MatrixReader(_newscoremat, MatrixType.Int16, _cols);
                }

                for (int i = 0; i < myColumnBlocks.Length; ++i)
                {
                    Block block = myColumnBlocks[i];
                    for (int r = block.RowRange.StartIndex; r <= block.RowRange.EndIndex; ++r)
                    {
                        long l1 = _lengthCut > -1 ? _seqs[r].Count : -1;
                        for (int c = block.ColumnRange.StartIndex; c <= block.ColumnRange.EndIndex; ++c)
                        {
                            long l2 = _lengthCut > -1 ? _seqs[c].Count : -1;

                            // Each pair in block
                            if (!_readPointsA)
                            {
                                TDistance tA = FromArrayToTdist(matReaderA.ReadDistanceFromMatrix(r, c));
                                myRowStripMatrixForA[r, c] = tA;
                            }
                            if (!_readPointsB)
                            {
                                TDistance tB = FromArrayToTdist(matReaderB.ReadDistanceFromMatrix(r, c));
                                myRowStripMatrixForB[r, c] = tB;
                            }
                            ++_totalPairs;

                            if (_useClusters)
                            {
                                int rCnum = ((int) PnumToCnum[r]);
                                int cCnum = ((int) PnumToCnum[c]);
                                if (SelectedCnums.Contains(rCnum) && SelectedCnums.Contains(cCnum))
                                {
                                    if (rCnum == cCnum)
                                    {
                                        ++_totalIntraPairs;
                                    }
                                    else
                                    {
                                        ++_totalInterPairs;
                                    }
                                }
                            }

                            double x = !_readPointsA
                                           ? (_useTDistanceMaxForA
                                                  ? ((double) myRowStripMatrixForA[r, c])/TDistance.MaxValue
                                                  : myRowStripMatrixForA[r, c])
                                           : matReaderA.ReadDistanceFromPointsFile(r, c);
                            double y = !_readPointsB
                                           ? (_useTDistanceMaxForB
                                                  ? ((double) myRowStripMatrixForB[r, c])/TDistance.MaxValue
                                                  : myRowStripMatrixForB[r, c])
                                           : matReaderB.ReadDistanceFromPointsFile(r, c);

                            // Ignore x or y values greater than distcutA or discutB respectively when distcut values are specified
                            if ((_distcutA > -1 && x > _distcutA) || (_distcutB > -1 && y > _distcutB)) continue;

                            // Ignore x or y values smaller than mindistA or mindistB respectively when mindist values are specified
                            if ((_mindistA > -1 && x < _mindistA) || (_mindistB > -1 && y < _mindistB)) continue;

                            // Ignore if the corresponding two sequence lengths are not within the given lengthcut
                            if (_lengthCut > -1 && (Math.Abs(l1 - l2) > _lengthCut*((l1 + l2)/2.0))) continue;

                            // Perform transforms (no transform if transform method is -1 for the respective matrix)
                            x = Transform(x, _aTransfm, _aTransfp);
                            y = Transform(y, _bTransfm, _bTransfp);

                            double newnomoveroldnom = -1;
                            if (_denomcutsenabled)
                            {
                                double oldscoredist = ((double) FromArrayToTdist(oldScoreReader.Read(r, c)))/
                                                      TDistance.MaxValue;
                                double newscoredist = ((double) FromArrayToTdist(newScoreReader.Read(r, c)))/
                                                      TDistance.MaxValue;
                                newnomoveroldnom = (1.0 - oldscoredist)/(1.0 - newscoredist);

                                if (newnomoveroldnom < 0)
                                    throw new Exception("Bad should not happen: negative ratio");
                            }

                            for (int j = _denomcuts.Length - 1; j >= 0; --j)
                            {
                                if (_denomcutsenabled && newnomoveroldnom < _denomcuts[j])
                                {
                                    myRowStripMatrixForDenomCut[r, c] = (byte) j;
                                }

                                if (myRowStripMatrixForDenomCut[r, c] > j)
                                {
                                    ++_consideredPairs[j];
                                    UpdateMinMax(x, y, ref _xmaxWhole[j], ref _xminWhole[j], ref _ymaxWhole[j],
                                                 ref _yminWhole[j]);
                                    if (_useClusters)
                                    {
                                        int rCnum = ((int) PnumToCnum[r]);
                                        int cCnum = ((int) PnumToCnum[c]);
                                        if (SelectedCnums.Contains(rCnum) && SelectedCnums.Contains(cCnum))
                                        {
                                            if (rCnum == cCnum)
                                            {
                                                // Intra cluster distances
                                                ++_consideredPairsIntra[j];
                                                UpdateMinMax(x, y, ref _xmaxSelected[j], ref _xminSelected[j],
                                                             ref _ymaxSelected[j],
                                                             ref _yminSelected[j]);
                                            }
                                            else
                                            {
                                                // Inter cluster distances
                                                ++_consideredPairsInter[j];
                                                UpdateMinMax(x, y, ref _xmaxSelectedInter[j],
                                                             ref _xminSelectedInter[j],
                                                             ref _ymaxSelectedInter[j], ref _yminSelectedInter[j]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (_denomcutsenabled)
                {
                    if (oldScoreReader != null) oldScoreReader.Dispose();
                    if (newScoreReader != null) newScoreReader.Dispose();
                }
            }
        }

        private static void UpdateMinMax(double x, double y, ref double xmax, ref double xmin, ref double ymax,
                                         ref double ymin)
        {
            if (x > xmax)
            {
                xmax = x;
            }

            if (x < xmin)
            {
                xmin = x;
            }

            if (y > ymax)
            {
                ymax = y;
            }

            if (y < ymin)
            {
                ymin = y;
            }
        }

        private static void UpdateCells(double x, double y, double xmax, double xmin, double ymax, double ymin,
                                        double deltax, double deltay, long[][] cells, int r, int c)
        {
            // cell number based on zero index from bottom left corner
            // if x is equal to xmax then it's placed in the last cell, which is xres-1 in zero based index
            // same is done for y when y == ymax
            int cellx = x == xmax ? _xres - 1 : (int) Math.Floor((x - xmin)/deltax);
            int celly = y == ymax ? _yres - 1 : (int) Math.Floor((y - ymin)/deltay);

            if (x > xmax || y > ymax || x < xmin || y < ymin)
            {
                // now this should never be reached
                throw new Exception("bad(1)-> x: " + x + " y: " + y + " xmax: " + xmax + " xmin: " + xmin + " ymax: " +
                                    ymax + " ymin: " + ymin + "lengthcut: " + _lengthCut + " row: " + r + " col: " + c);
            }

            if (cellx >= _xres || celly >= _yres)
            {
                // now this should never be reached
                throw new Exception("bad(2)-> x: " + x + " y:" + y + " xmax: " + xmax + " xmin: " + xmin + " ymax: " +
                                    ymax + " ymin: " + ymin + " cellx: " + cellx + " celly: " + celly);
            }

            ++cells[celly][cellx];
            // todo. remove after testing
//            string cell = cellx + "," + celly;
//            cells[cell] = cells.ContainsKey(cell) ? ((long) cells[cell]) + 1 : 1L;
        }

        private static TDistance FromArrayToTdist(byte[] xarr)
        {
#if USE_UINT16
           return BitConverter.ToUInt16(xarr, 0);
#elif USE_INT16
            return BitConverter.ToInt16(xarr, 0);
#else
           return BitConverter.ToDouble(xarr, 0);
#endif
        }

        private static void ReadConfiguration(string configFile)
        {
            Console.WriteLine(configFile);
            /* Reading parameters file */
            using (StreamReader reader = new StreamReader(configFile))
            {
                char[] sep = new[] {'\t'};
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    // Skip null/empty and comment lines
                    if (!string.IsNullOrEmpty(line) && !line.StartsWith("#"))
                    {
                        string[] splits = line.Trim().Split(sep);
                        if (splits.Length >= 2)
                        {
                            string value = splits[1];
                            switch (splits[0])
                            {
                                case "Amat":
                                    _aMat = value;
                                    break;
                                case "Aname":
                                    _aName = value;
                                    break;
                                case "Atransfm":
                                    _aTransfm = int.Parse(value);
                                    break;
                                case "Atransfp":
                                    _aTransfp = double.Parse(value);
                                    break;
                                case "Bmat":
                                    _bMat = value;
                                    break;
                                case "Bname":
                                    _bName = value;
                                    break;
                                case "Btransfm":
                                    _bTransfm = int.Parse(value);
                                    break;
                                case "Btransfp":
                                    _bTransfp = double.Parse(value);
                                    break;
                                case "usetdistancemaxforA":
                                    _useTDistanceMaxForA = bool.Parse(value);
                                    break;
                                case "usetdistancemaxforB":
                                    _useTDistanceMaxForB = bool.Parse(value);
                                    break;
                                case "readPointsA":
                                    _readPointsA = bool.Parse(value);
                                    break;
                                case "readPointsB":
                                    _readPointsB = bool.Parse(value);
                                    break;
                                case "cols":
                                    _cols = int.Parse(value);
                                    break;
                                case "rows":
                                    _rows = int.Parse(value);
                                    break;
                                case "outdir":
                                    _outdir = value;
                                    break;
                                case "xmaxbound":
                                    _xmaxbound = double.Parse(value);
                                    break;
                                case "ymaxbound":
                                    _ymaxbound = double.Parse(value);
                                    break;
                                case "xres":
                                    _xres = int.Parse(value);
                                    break;
                                case "yres":
                                    _yres = int.Parse(value);
                                    break;
                                case "alpha":
                                    _alpha = double.Parse(value);
                                    break;
                                case "pcutf":
                                    _pcutf = double.Parse(value);
                                    break;
                                case "zto1":
                                    _zto1 = bool.Parse(value);
                                    break;
                                case "distcutA":
                                    _distcutA = double.Parse(value);
                                    break;
                                case "distcutB":
                                    _distcutB = double.Parse(value);
                                    break;
                                case "mindistA":
                                    _mindistA = double.Parse(value);
                                    break;
                                case "mindistB":
                                    _mindistB = double.Parse(value);
                                    break;
                                case "clusterfile":
                                    if (!"none".Equals(value) && File.Exists(value))
                                    {
                                        _clusterfile = value;
                                        _useClusters = true;
                                    }
                                    else
                                    {
                                        _useClusters = false;
                                    }
                                    break;
                                case "clusters":
                                    if (!"none".Equals(value) && value.Contains(","))
                                    {
                                        char[] commasep = new[] {','};
                                        foreach (int c in value.Trim().Split(commasep).Select(x => int.Parse(x)))
                                        {
                                            if (!SelectedCnums.Contains(c))
                                            {
                                                SelectedCnums.Add(c);
                                            }
                                        }
                                    }
                                    break;
                                case "title":
                                    _title = line.Substring(5).Trim();
                                    break;
                                case "seqfile":
                                    if (!"none".Equals(value) && File.Exists(value))
                                    {
                                        using (FastAParser parser = new FastAParser(value))
                                        {
                                            IEnumerable<ISequence> seqenum = parser.Parse();
                                            foreach (ISequence sequence in seqenum)
                                            {
                                                _seqs.Add(sequence);
                                            }
                                        }
                                    }
                                    break;
                                case "lengthcut":
                                    _lengthCut = double.Parse(value);
                                    break;
                                case "denomcuts":
                                    _denomcuts = value.Trim().Split(',').Select(x => double.Parse(x)).ToArray();
                                    _denomcutsenabled = !(_denomcuts.Length == 1 && _denomcuts[0] == -1);

                                    break;
                                case "oldscoremat":
                                    _oldscoremat = value;
                                    break;
                                case "newscoremat":
                                    _newscoremat = value;
                                    break;
                                default:
                                    throw new Exception("Invalide line in configuration file: " + line);
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid line in configuration file: " + line);
                        }
                    }
                }
            }
        }

        private static long[][] Sum2DArray(long[][] a, long[][]b)
        {
            long r = a.Length;
            long c = a[0].Length;
            long[][] sum = new long[r][];
            for (int i = 0; i < r; i++)
            {
                sum[i] = new long[c];
                for (int j = 0; j < c; j++)
                {
                    sum[i][j] = a[i][j] + b[i][j];
                }
            }
            return sum;
        }

        private static bool IsOdd(int value)
        {
            return (value & 1) == 1;
        }
    }
}
