using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Salsa.Core;
using System.IO;

namespace MatrixConverterConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //Load the command line args into our helper class which allows us to name arguments
            Arguments pargs = new Arguments(args);
            // dataType {0=int16, 1=uint16, 2=double}
            pargs.Usage = "Usage: MatrixConverterConsole.exe /inFile=<string> /outFile=<string>" +  
                "/xsize=<int> /ysize=<int> /xoffset=<int> /yoffset=<int> /bigXSize=<int> /bigYSize=<int> /dataType=<int>";

            if (pargs.CheckRequired(new string[] { "inFile", "outFile", "xsize", "ysize", "xoffset", "yoffset", "bigXSize", "bigYSize", "dataType" }) == false)
            {
                Console.WriteLine(pargs.Usage);
                return;
            }

            string inFile = pargs.GetValue<string>("inFile");
            string outFile = pargs.GetValue<string>("outFile");
            int xsize = pargs.GetValue<int>("xsize");
            int ysize = pargs.GetValue<int>("ysize");
            int xoffset = pargs.GetValue<int>("xoffset");
            int yoffset = pargs.GetValue<int>("yoffset");
            int bigXSize = pargs.GetValue<int>("bigXSize");
            int bigYSize = pargs.GetValue<int>("bigYSize");
            int dataType = pargs.GetValue<int>("dataType");
            
            Console.Write("\nConverting ...");
            ConvertBinToString(inFile, outFile, xsize, ysize, xoffset, yoffset, bigXSize, bigYSize, dataType);
            Console.WriteLine("Done.");
            Console.Read();



        }

        private static void ConvertBinToString(string inFile, string outFile, int size, int type)
        {
            string delim = "\t";
            using (Stream inStream = File.OpenRead(inFile))
            {
                using (BinaryReader reader = new BinaryReader(inStream))
                {
                    using (StreamWriter writer = new StreamWriter(outFile))
                    {
                        for (int i = 0; i < size; i++)
                        {
                            for (int j = 0; j < size; j++)
                            {
                                if (j == 0)
                                {
                                    writer.Write(i);
                                }
                                writer.Write(delim);
                                switch (type)
                                {
                                    case 0:
                                        writer.Write(reader.ReadInt16());
                                        break;
                                    case 1:
                                        writer.Write(reader.ReadUInt16());
                                        break;
                                    case 2:
                                        writer.Write(reader.ReadDouble());
                                        break;
                                }


                                if ((j + 1) % size == 0)
                                {
                                    writer.Write("\n");
                                }
                            }
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Extract the given portion of binary matrix and convert it to text with one additional column.
        /// The extracted portion is [yoffset, yoffset + ysize - 1] x [xoffset, xoffset + xsize -1] where
        /// top left corner of the matrix is taken as (0,0). The extracted matrix is ysize rows x xsize cols.
        /// The generated text matrix is ysize rows x xsize+1 cols where the extracted matrix starts from
        /// (1,0) coordinates (note. the first value in coordinate is x and second y). x = 0 column is just
        /// the row number in this matrix starting from zero.
        /// </summary>
        /// <param name="inFile">Path to binary matrix</param>
        /// <param name="outFile">Path to write the converted text based matrix</param>
        /// <param name="xsize">Number of columns to extract</param>
        /// <param name="ysize">Number of rows to extract</param>
        /// <param name="xoffset">Starting column number of the extracted matrix in the original matrix based on zero index</param>
        /// <param name="yoffest">Starting row number of the extracted matrix in the original matrix based on zero index</param>
        /// <param name="type">Data type of the binary matrix. 
        ///                    Possible values {0=int16, 1=unit16, 2=double}</param>
        ///<param name="bigxsize">Total number of columns in the original matrix</param>
        ///<param name="bigysize">Totoal number of rows in the original matrix</param>
        private static void ConvertBinToString(string inFile, string outFile, int xsize, int ysize, int xoffset, int yoffest, int bigxsize, int bigysize, int type)
        {
            string delim = "\t";
            int leftSkipCols = xoffset;
            int rightSkipCols = (bigxsize - (xoffset + xsize));
            int topSkipRows = yoffest;

            int leftSkipBytes;
            int rightSkipBytes;
            int topSkipBytes;
            int dataTypeInBytes;
            switch (type)
            {
                case 0:
                    dataTypeInBytes = 2;
                    break;
                case 1: 
                    dataTypeInBytes = 2;
                    break;
                case 2:
                    dataTypeInBytes = 8;
                    break;
                default:
                    dataTypeInBytes = 2;
                    break;
            }

            leftSkipBytes = leftSkipCols * dataTypeInBytes;
            rightSkipBytes = rightSkipCols * dataTypeInBytes;
            topSkipBytes = topSkipRows * bigxsize * dataTypeInBytes;

            using (Stream inStream = File.OpenRead(inFile))
            {
                using (BinaryReader reader = new BinaryReader(inStream))
                {
                    using (StreamWriter writer = new StreamWriter(outFile))
                    {
                        // Skip bytes from top
                        reader.ReadBytes(topSkipBytes);
                        for (int i = 0; i < ysize; i++)
                        {
                            // Skip bytes from left of sub matrix
                            reader.ReadBytes(leftSkipBytes);

                            // Start reading sub matrix
                            for (int j = 0; j < xsize; j++)
                            {
                                if (j == 0)
                                {
                                    writer.Write(i);
                                }

                                writer.Write(delim);
                                switch (type)
                                {
                                    case 0:
                                        // skip xoffset times 2(size of int16) bytes
                                        writer.Write(reader.ReadInt16());
                                        break;
                                    case 1:
                                        writer.Write(reader.ReadUInt16());
                                        break;
                                    case 2:
                                        writer.Write(reader.ReadDouble());
                                        break;
                                }


                                if ((j + 1) % xsize == 0)
                                {
                                    writer.Write("\n");
                                }
                            }
                            // Skip bytes to the right of sub matrix
                            reader.ReadBytes(rightSkipBytes);
                        }

                    }
                }
            }
        }


        // todo: change signature and complete
        /// <summary>
        /// Extracts sub distance matrices for each cluster in the sortedClusterMap.
        /// </summary>
        /// <param name="distFile">Path to binary distance matrix</param>
        /// <param name="sortedClusterMap">Sorted dictionary of <point#, cluster#> </param>
        /// <param name="outDir">Path to output directory</param>
        /// <param name="bigc">Total number of columns in the input distance matrix</param>
        /// <param name="bigr">Total number of rows in the input distance matrix</param>
        /// <param name="type">Data type of the binary matrix. 
        ///                    Possible values {0=int16, 1=unit16, 2=double}</param>
        /*
        private static void ExtractPoints(string distFile, Dictionary<int,int> sortedClusterMap, string outDir, int bigc, int bigr, int type)
        {
            string baseName = Path.GetFileNameWithoutExtension(distFile);
            int byteSizeForType;
            switch (type)
            {
                case 0:
                    byteSizeForType = 2;
                    break;
                case 1:
                    byteSizeForType = 2;
                    break;
                case 2:
                    byteSizeForType = 8;
                    break;
                default:
                    byteSizeForType = 2;
                    break;
            }

            int leftSkipCols = xoffset;
            int rightSkipCols = (bigxsize - (xoffset + xsize));
            int topSkipRows = yoffest;

            int leftSkipBytes;
            int rightSkipBytes;
            int topSkipBytes;

            leftSkipBytes = leftSkipCols * byteSizeForType;
            rightSkipBytes = rightSkipCols * byteSizeForType;
            topSkipBytes = topSkipRows * bigxsize * byteSizeForType;

            using (Stream inStream = File.OpenRead(distFile))
            {
                using (BinaryReader reader = new BinaryReader(inStream))
                {
                    Dictionary<int, StreamWriter> writers = new Dictionary<int, StreamWriter>();
                    StreamWriter writer;
                    foreach(KeyValuePair<int, int> kv in sortedClusterMap)
                    {
                        if (!writers.ContainsKey(kv.Value))
                        {
                            writers[kv.Value] = 
                                new StreamWriter(Path.Combine(new string[]{outDir, baseName, "_", kv.Value.ToString(), ".bin"}));
                        }
                        writer = writers[kv.Value];


                    }
                        // Skip bytes from top
                        reader.ReadBytes(topSkipBytes);
                        for (int i = 0; i < ysize; i++)
                        {
                            // Skip bytes from left of sub matrix
                            reader.ReadBytes(leftSkipBytes);

                            // Start reading sub matrix
                            for (int j = 0; j < xsize; j++)
                            {
                                if (j == 0)
                                {
                                    writer.Write(i);
                                }

                                writer.Write(delim);
                                switch (type)
                                {
                                    case 0:
                                        // skip xoffset times 2(size of int16) bytes
                                        writer.Write(reader.ReadInt16());
                                        break;
                                    case 1:
                                        writer.Write(reader.ReadUInt16());
                                        break;
                                    case 2:
                                        writer.Write(reader.ReadDouble());
                                        break;
                                }


                                if ((j + 1) % xsize == 0)
                                {
                                    writer.Write("\n");
                                }
                            }
                            // Skip bytes to the right of sub matrix
                            reader.ReadBytes(rightSkipBytes);
                        }

                    }
                }
            } */
        
    }


}
 
         

