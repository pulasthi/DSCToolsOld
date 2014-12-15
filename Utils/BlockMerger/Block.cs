using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BlockMerger
{
    public class Block <T>
    {
        private UInt16 _projectNameLen;
        private byte[] _projectName;
        private UInt16 _rank;
        private UInt16 _sysNameLen;
        private byte[] _sysName;
        private UInt16 _fastaNameLen;
        private byte[] _fastaName;
        private UInt16 _rbNum;
        private UInt16 _cbNum;
        private UInt32 _rowStartNum;
        private UInt32 _rowEndNum;
        private UInt32 _colStartNum;
        private UInt32 _colEndNum;
        private UInt16 _rowStartSeqNameLen;
        private byte[] _rowStartSeqName;
        private UInt16 _rowEndSeqNameLen;
        private byte[] _rowEndSeqName;
        private UInt16 _colStartSeqNameLen;
        private byte[] _colStartSeqName;
        private UInt16 _colEndSeqNameLen;
        private byte[] _colEndSeqName;
        private T[][] _distanceMatrix;

        public static readonly Func<BinaryReader, Int16> ReadfInt16 = reader => reader.ReadInt16();
        public static readonly Func<BinaryReader, UInt16> ReadfUInt16 = reader => reader.ReadUInt16();
        public static readonly Func<BinaryReader, Double> ReadfDouble = reader => reader.ReadDouble();


        public ushort ProjectNameLen
        {
            get { return _projectNameLen; }
        }

        public byte[] ProjectName
        {
            get { return _projectName; }
        }

        public ushort Rank
        {
            get { return _rank; }
        }

        public ushort SysNameLen
        {
            get { return _sysNameLen; }
        }

        public byte[] SysName
        {
            get { return _sysName; }
        }

        public ushort FastaNameLen
        {
            get { return _fastaNameLen; }
        }

        public byte[] FastaName
        {
            get { return _fastaName; }
        }

        public ushort RbNum
        {
            get { return _rbNum; }
        }

        public ushort CbNum
        {
            get { return _cbNum; }
        }

        public uint RowStartNum
        {
            get { return _rowStartNum; }
        }

        public uint RowEndNum
        {
            get { return _rowEndNum; }
        }

        public uint ColStartNum
        {
            get { return _colStartNum; }
        }

        public uint ColEndNum
        {
            get { return _colEndNum; }
        }

        public ushort RowStartSeqNameLen
        {
            get { return _rowStartSeqNameLen; }
        }

        public byte[] RowStartSeqName
        {
            get { return _rowStartSeqName; }
        }

        public ushort RowEndSeqNameLen
        {
            get { return _rowEndSeqNameLen; }
        }

        public byte[] RowEndSeqName
        {
            get { return _rowEndSeqName; }
        }

        public ushort ColStartSeqNameLen
        {
            get { return _colStartSeqNameLen; }
        }

        public byte[] ColStartSeqName
        {
            get { return _colStartSeqName; }
        }

        public ushort ColEndSeqNameLen
        {
            get { return _colEndSeqNameLen; }
        }

        public byte[] ColEndSeqName
        {
            get { return _colEndSeqName; }
        }

        public T[][] DistanceMatrix
        {
            get { return _distanceMatrix; }
        }

        public UInt32 Rows { get { return (_rowEndNum - _rowStartNum) + 1; } }
        public UInt32 Cols { get { return (_colEndNum - _colStartNum) + 1; } }

        public static Block<T> LoadFromFile(string path, bool transpose, Func<BinaryReader, T> readf)
        {
            Block<T> b = new Block<T>();
            T[][] matrix;
            using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
            {
                // Project name
                b._projectNameLen = reader.ReadUInt16();
                b._projectName = reader.ReadBytes(b._projectNameLen);
                // Rank
                b._rank = reader.ReadUInt16();
                // System name
                b._sysNameLen = reader.ReadUInt16();
                b._sysName = reader.ReadBytes(b._sysNameLen);
                // FASTA name
                b._fastaNameLen = reader.ReadUInt16();
                b._fastaName = reader.ReadBytes(b._fastaNameLen);

                if (transpose)
                {
                    // RB#, CB#
                    b._cbNum = reader.ReadUInt16();
                    b._rbNum = reader.ReadUInt16();
                    // ColStart, ColEnd
                    b._colStartNum = reader.ReadUInt32();
                    b._colEndNum = reader.ReadUInt32();
                    // RowStart, RowEnd
                    b._rowStartNum = reader.ReadUInt32();
                    b._rowEndNum = reader.ReadUInt32();
                    // ColStartSeq, ColEndSeq
                    b._colStartSeqNameLen = reader.ReadUInt16();
                    b._colStartSeqName = reader.ReadBytes(b._colStartSeqNameLen);
                    b._colEndSeqNameLen = reader.ReadUInt16();
                    b._colEndSeqName = reader.ReadBytes(b._colEndSeqNameLen);
                    // RowStartSeq, RowEndSeq
                    b._rowStartSeqNameLen = reader.ReadUInt16();
                    b._rowStartSeqName = reader.ReadBytes(b._rowStartSeqNameLen);
                    b._rowEndSeqNameLen = reader.ReadUInt16();
                    b._rowEndSeqName = reader.ReadBytes(b._rowEndSeqNameLen);

                    UInt32 rows = (b._rowEndNum - b._rowStartNum) + 1;
                    UInt32 cols = (b._colEndNum - b._colStartNum) + 1;

                    matrix = new T[rows][];
                    // Need to have all rows in the array instantiated as we are going
                    // to store column wise
                    for (UInt32 m = 0; m < rows; m++)
                    {
                        matrix[m] = new T[cols];
                    }

                    for (UInt32 m = 0; m <cols; m++)
                    {
                        for (UInt32 n = 0; n < rows; n++)
                        {
                            matrix[n][m] = readf(reader);
                        }
                    }
                    b._distanceMatrix = matrix;
                }
                else
                {
                    // RB#, CB#
                    b._rbNum = reader.ReadUInt16();
                    b._cbNum = reader.ReadUInt16();
                    // RowStart, RowEnd
                    b._rowStartNum = reader.ReadUInt32();
                    b._rowEndNum = reader.ReadUInt32();
                    // ColStart, ColEnd
                    b._colStartNum = reader.ReadUInt32();
                    b._colEndNum = reader.ReadUInt32();
                    // RowStartSeq, RowEndSeq
                    b._rowStartSeqNameLen = reader.ReadUInt16();
                    b._rowStartSeqName = reader.ReadBytes(b._rowStartSeqNameLen);
                    b._rowEndSeqNameLen = reader.ReadUInt16();
                    b._rowEndSeqName = reader.ReadBytes(b._rowEndSeqNameLen);
                    // ColStartSeq, ColEndSeq
                    b._colStartSeqNameLen = reader.ReadUInt16();
                    b._colStartSeqName = reader.ReadBytes(b._colStartSeqNameLen);
                    b._colEndSeqNameLen = reader.ReadUInt16();
                    b._colEndSeqName = reader.ReadBytes(b._colEndSeqNameLen);

                    UInt32 rows = (b._rowEndNum - b._rowStartNum) + 1;
                    UInt32 cols = (b._colEndNum - b._colStartNum) + 1;

                    matrix = new T[rows][];
                    for (UInt32 m = 0; m < rows; m++)
                    {
                        matrix[m] = new T[cols];
                        for (UInt32 n = 0; n < cols; n++)
                        {
                            matrix[m][n]= readf(reader);
                        }
                    }
                    b._distanceMatrix = matrix;
                }
            }
            return b;
        }

        public void PrintBlock(bool printDistances)
        {
            UInt32 rows = (_rowEndNum - _rowStartNum) + 1;
            UInt32 cols = (_colEndNum - _colStartNum) + 1;

            ASCIIEncoding enc = new ASCIIEncoding();
            Console.WriteLine("Project Name : " + enc.GetString(_projectName));
            Console.WriteLine("Rank         : " + _rank);
            Console.WriteLine("System Name  : " + enc.GetString(_sysName));
            Console.WriteLine("FASTA Name   : " + enc.GetString(_fastaName));
            Console.WriteLine("Dimension    : " + rows + " x " + cols);
            Console.WriteLine("RB#          : " + _rbNum);
            Console.WriteLine("CB#          : " + _cbNum);
            Console.WriteLine("Row Start#   : " + _rowStartNum);
            Console.WriteLine("Row End#     : " + _rowEndNum);
            Console.WriteLine("Col Start#   : " + _colStartNum);
            Console.WriteLine("Col End#     : " + _colEndNum);
            Console.WriteLine("Row Start Seq: " + enc.GetString(_rowStartSeqName));
            Console.WriteLine("Row End Seq  : " + enc.GetString(_rowEndSeqName));
            Console.WriteLine("Col Start Seq: " + enc.GetString(_colStartSeqName));
            Console.WriteLine("Col End Seq  : " + enc.GetString(_colEndSeqName));

            if (printDistances)
            {
                for (UInt32 m = 0; m < rows; m++)
                {
                    for (UInt32 n = 0; n < cols; n++)
                    {
                        Console.Write(_distanceMatrix[m][n] + "\t");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
