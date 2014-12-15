using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bio;
using Bio.IO.FastA;
using Common;
using Common.pviz;
using Cluster = Common.pviz.Cluster;
using Point = Common.Point;

namespace Map420Centers
{
    class Program
    {
        static void Main(string[] args)
        {
            const string center420SeqFastaFile = @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\centers\420centers\center_sequences.txt";
            IList<ISequence> centerSeqs = ParseSeqs(center420SeqFastaFile);
            Console.WriteLine("Done parsing center fasta file");

            string[] regionFastaFiles = new[]
                                            {@"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\haixu_release\sequences\mega_regions\0\haixu_region_0.fa",
                                                @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\haixu_release\sequences\mega_regions\1\haixu_region_1.fa",
                                                @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\haixu_release\sequences\mega_regions\2\haixu_region_2.fa",
                                                @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\haixu_release\sequences\mega_regions\3\haixu_region_3.fa",
                                                @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\haixu_release\sequences\mega_regions\4\haixu_region_4.fa",
                                                @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\haixu_release\sequences\mega_regions\5\haixu_region_5.fa",
                                                @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\haixu_release\sequences\mega_regions\6\haixu_region_6.fa"
                                            };
            
            string[] regionPointFiles = new[]
                                            {@"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\haixu_release\plots\mega_regions\0\refined_haixu_0(15)_zeroidx.txt",
                                                @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\haixu_release\plots\mega_regions\1\refined_haixu_1(19)_zeroidx.txt",
                                                @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\haixu_release\plots\mega_regions\2\haixu_region_2(25)_zeroidx.txt",
                                                @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\haixu_release\plots\mega_regions\3\haixu_region_3(23)_zeroidx.txt",
                                                @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\haixu_release\plots\mega_regions\4\refined_haixu_4(29)_zeroidx.txt",
                                                @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\haixu_release\plots\mega_regions\5\haixu_region_5(14)_zeroidx.txt",
                                                @"G:\SugarSyncSharedByMe\SalsaBio\millions\446K_uniq\regions\haixu_release\plots\mega_regions\6\refined_haixu_6(15)_zeroidx.txt"
                                            };


            int length = regionFastaFiles.Length;
            Hashtable allRegionSequencesToPnumInRegion = new Hashtable();
            IList<Point>[] regionPointsArray = new IList<Point>[length];
            string[] regionPlotNamesArray = new string[length];
            StreamWriter[] regionPointWritersArray = new StreamWriter[length];
            int[] maxCnums = new int[length];
            for (int i = 0; i < length; i++)
            {
                AddToHash(ParseSeqs(regionFastaFiles[i]), allRegionSequencesToPnumInRegion);

                string regionPointFile = regionPointFiles[i];
                regionPointsArray[i] = ParsePointsFile(regionPointFile);
                maxCnums[i] = regionPointsArray[i].Max(p => p.Cluster);
                string directoryName = Path.GetDirectoryName(regionPointFile);
                directoryName = string.IsNullOrEmpty(directoryName) ? string.Empty : directoryName;
                regionPlotNamesArray[i] = (Path.Combine(directoryName, Path.GetFileNameWithoutExtension(regionPointFile)+"_with_3centers.pviz"));
                regionPointWritersArray[i] = new StreamWriter(Path.Combine(directoryName, Path.GetFileNameWithoutExtension(regionPointFile)+"_with_3centers.txt"));
            }

            Console.WriteLine("Done parsing sequences and creating writers");

            Hashtable centerTypes = new Hashtable();
            centerTypes.Add("SmallestDistanceMeans",1);
            centerTypes.Add("SmallestMDSDistanceMeans", 2);
            centerTypes.Add("SmallestMDSCoG", 3);

            foreach (var centerSeq in centerSeqs)
            {
                string seqStr = SeqToString(centerSeq);
                CenterSeqIdStruct idStruct = ParseCenterSeqId(centerSeq.ID);

                string centerType = idStruct.FoxMethod;
                int region = idStruct.FoxRegion;
                if (allRegionSequencesToPnumInRegion.Contains(seqStr))
                {
                    int pnumInRegion = (int) allRegionSequencesToPnumInRegion[seqStr];
                    IList<Point> regionPoints = regionPointsArray[region];
                    Point p = regionPoints[pnumInRegion].Clone();
                    p.Label = "Region:" + region + "_Cluster:" + p.Cluster +"_OrgPnum:" + p.Index + "_CenterType" + centerType;
                    /* This will make one center cluster for each center type for entire set of clusters in the region */
                    // p.Cluster = maxCnums[region] + (int) centerTypes[centerType];

                    /* This will make a center cluster for each center type for each cluster in the region*/
                    p.Cluster = p.Cluster*4 + (int) centerTypes[centerType];
                    p.Index = regionPoints.Count;
                    regionPoints.Add(p);
                        // Adding happens to the end so no change to the mapping in allRegionSequencesToPnumInRegion
                }
            }

            for (int i = 0; i < length; i++)
            {
                StreamWriter pointsWriter = regionPointWritersArray[i];
                WritePoints(regionPointsArray[i], pointsWriter, regionPlotNamesArray[i], centerTypes);
                pointsWriter.Flush();
                pointsWriter.Close();
                pointsWriter.Dispose();
            }


            Console.WriteLine("All done");
            Console.Read();


        }

        static void WritePoints(IEnumerable<Point> points, StreamWriter pointsWriter, string plotFile, Hashtable centerTypes )
        {
            Plot plotElement = new Plot();
            plotElement.Title = Path.GetFileNameWithoutExtension(plotFile);
            plotElement.PointSize = 1;
            plotElement.Glyph = new Glyph(0, 1);

            List<Cluster> clusterElements = new List<Cluster>();
            List<Common.pviz.Point> pointElements = new List<Common.pviz.Point>();

            PvizModel pviz = new PvizModel {Plot = plotElement, Points = pointElements, Clusters = clusterElements};

            HashSet<int> cnums = new HashSet<int>();
            IList<System.Drawing.Color> colorScheme = ColorUtils.GetColorsFor(ColorSchemes.Matlab50NoBlue);
            foreach (var point in points)
            {
                /* This will make a center cluster for each center type for each cluster in the region*/
                if (string.IsNullOrEmpty(point.Label) && !centerTypes.Contains(point.Label))
                {
                    point.Cluster *= 4;
                }
                int cnum = point.Cluster;
                if (!cnums.Contains(cnum))
                {
                    cnums.Add(cnum);
                    int mod4 = cnum % 4;
                    Cluster c = new Cluster
                                    {
                                        Key = cnum,
                                        Label =
                                            "C-" + (cnum/4).ToString(CultureInfo.InvariantCulture) + "-" +
                                            (string.IsNullOrEmpty(point.Label)
                                                 ? "Points"
                                                 : point.Label),
                                        Visible = 1,
                                        Default = mod4 == 0 ? 1 : 0,
                                        Color = new Color(colorScheme[cnum/4]),
                                                                                Size = mod4 == 0 ? 1 : 3,
//                                        Size = 1,
                                        Shape = mod4 == 0 ? 3 : mod4 + 3
                                    };
                    
                    clusterElements.Add(c);
                }

                Common.pviz.Point p = new Common.pviz.Point
                                          {
                                              Key = point.Index,
                                              Label = string.IsNullOrEmpty(point.Label) ? point.Index.ToString(CultureInfo.InvariantCulture) : point.Label,
                                              ClusterKey = point.Cluster,
                                              Location = new Location(point.X, point.Y, point.Z)
                                          };
                pointElements.Add(p);

                pointsWriter.WriteLine(point.SimplePointString());
            }
            pviz.SaveAs(plotFile);
        }

        static CenterSeqIdStruct ParseCenterSeqId(string id)
        {
            char[] sep = new[] {' ', '\t'};
            string[] splits = id.Split(sep);
            if (splits.Length != 8) throw new Exception("Invalid number of splits in id: " + id);
            return new CenterSeqIdStruct(splits[0].Substring(1), int.Parse(splits[5].Substring(10)), splits[7].Substring(10));
        }
        

        static string SeqToString(ISequence seq)
        {
            ASCIIEncoding enc = new ASCIIEncoding();
            return enc.GetString(seq.ToArray()).ToUpper();
        }

        static void AddToHash(IList<ISequence> seqs, Hashtable ht )
        {
            for (int i = 0; i < seqs.Count; i++)
            {
                ht.Add(SeqToString(seqs[i]), i);
            }
        }

        static IList<ISequence> ParseSeqs(string seqsFileName)
        {
            using (FastAParser parser = new FastAParser(seqsFileName))
            {
                return parser.Parse().ToList();
            }
        }

        static IList<Point> ParsePointsFile(string pointsFileName)
        {
            List<Point> l = new List<Point>();
            using (SimplePointsReader reader = new SimplePointsReader(pointsFileName))
            {
                while (!reader.EndOfStream)
                {
                    l.Add(reader.ReadPoint());
                }
            }
            return l;
        }
    }

    public struct CenterSeqIdStruct
    {
        private readonly string _id;
        private readonly int _foxRegion;
        private readonly string _foxMethod;

        public CenterSeqIdStruct(string id, int foxRegion, string foxMethod)
        {
            _id = id;
            _foxRegion = foxRegion;
            _foxMethod = foxMethod;
        }

        public string Id { get { return _id; } }
        public int FoxRegion { get { return _foxRegion; } }
        public string FoxMethod { get { return _foxMethod; } }
    }
}
