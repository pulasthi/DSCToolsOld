using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DistanceFormatChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            writer.Write(_rowBlocksMetaData.ProjectNameLen);
            writer.Write(_rowBlocksMetaData.ProjectName);
            writer.Write(((UInt16) _rank));
            writer.Write(_rowBlocksMetaData.SysNameLen);
            writer.Write(_rowBlocksMetaData.SysName);
            writer.Write(_rowBlocksMetaData.FastaNameLen);
            writer.Write(_rowBlocksMetaData.FastaName);
            writer.Write(((UInt16) block.RowBlockNumber));
            writer.Write(((UInt16) block.ColumnBlockNumber));
            writer.Write(((UInt32) block.RowRange.StartIndex));
            writer.Write(((UInt32) block.RowRange.EndIndex));
            writer.Write(((UInt32) block.ColumnRange.StartIndex));
            writer.Write(((UInt32) block.ColumnRange.EndIndex));
            writer.Write(((UInt16) rowRange.StartSeqName.Length));
            writer.Write(enc.GetBytes(rowRange.StartSeqName));
            writer.Write(((UInt16) rowRange.EndSeqName.Length));
            writer.Write(enc.GetBytes(rowRange.EndSeqName));
            writer.Write(((UInt16) colRange.StartSeqName.Length));
            writer.Write(enc.GetBytes(colRange.EndSeqName));
            writer.Write(((UInt16) colRange.EndSeqName.Length));
            writer.Write(enc.GetBytes(colRange.EndSeqName));
             
            for (int m = rowRange.StartIndex; m <=rowRange.EndIndex; m++)
            {
                for (int n = colRange.StartIndex; n <= colRange.EndIndex; n++)
                {
                    writer.Write(_partialMatrix[m,n]);
                }
            }
             */

            string distDir = @"C:\Users\sekanaya\Desktop\debug\partialfiles";
//            string distDir = @"C:\Users\sekanaya\Desktop\swgmstester\block";
            string[] distFiles = Directory.GetFiles(distDir);
            for (int i = 0; i < distFiles.Length; i++)
            {
                Int16 tdist;
                UInt16 len;
                UInt16 x;
                UInt32 rowStart, rowEnd, colStart, colEnd;
                ASCIIEncoding enc = new ASCIIEncoding();
                string distFile = distFiles[i];
                using (BinaryReader reader = new BinaryReader(File.OpenRead(distFile)))
                {
                    len = reader.ReadUInt16();
                    Console.WriteLine("Project Name Length: " + len);
                    Console.WriteLine("Project Name:        " + enc.GetString(reader.ReadBytes(len)));
                    x = reader.ReadUInt16();
                    Console.WriteLine("Rank:                " + x);
                    len = reader.ReadUInt16();
                    Console.WriteLine("System Name Length:  " + len);
                    Console.WriteLine("System Name:         " + enc.GetString(reader.ReadBytes(len)));
                    len = reader.ReadUInt16();
                    Console.WriteLine("FASTA Name Length:  " + len);
                    Console.WriteLine("FASTA Name:         " + enc.GetString(reader.ReadBytes(len)));
                    x = reader.ReadUInt16();
                    Console.WriteLine("Row Block#:         " + x);
                    x = reader.ReadUInt16();
                    Console.WriteLine("Column Block#:      " + x);
                    rowStart = reader.ReadUInt32();
                    Console.WriteLine("Row Start#:         " + rowStart);
                    rowEnd = reader.ReadUInt32();
                    Console.WriteLine("Row End#:           " + rowEnd);
                    colStart = reader.ReadUInt32();
                    Console.WriteLine("Column Start#:      " + colStart);
                    colEnd = reader.ReadUInt32();
                    Console.WriteLine("Column End#:        " + colEnd);
                    len = reader.ReadUInt16();
                    Console.WriteLine("Row Start Seq Len:  " + len);
                    Console.WriteLine("Row Start Seq Name: " + enc.GetString(reader.ReadBytes(len)));
                    len = reader.ReadUInt16();
                    Console.WriteLine("Row End Seq Len:    " + len);
                    Console.WriteLine("Row End Seq Name:   " + enc.GetString(reader.ReadBytes(len)));
                    len = reader.ReadUInt16();
                    Console.WriteLine("Col Start Seq Len:  " + len);
                    Console.WriteLine("Col Start Seq Name: " + enc.GetString(reader.ReadBytes(len)));
                    len = reader.ReadUInt16();
                    Console.WriteLine("Col End Seq Len:    " + len);
                    Console.WriteLine("Col End Seq Name:   " + enc.GetString(reader.ReadBytes(len)));

                    for (UInt32 m = rowStart; m <= rowEnd; m++)
                    {
                        for (UInt32 n = colStart; n <= colEnd; n++)
                        {
                            tdist = reader.ReadInt16();
//                            Console.Write(tdist + "\t");
                        }
//                        Console.WriteLine();
                    }
                    Console.WriteLine("\n\n------------------------");
                }
            }
            Console.WriteLine("\nDone.");
            Console.Read();
        }
    }
}
