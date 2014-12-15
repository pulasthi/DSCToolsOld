using System;
using System.IO;

namespace PwcVerifyCenters
{
    public class MatrixReader : IDisposable
    {
        private readonly FileStream _stream;
        private readonly int _size;
        private readonly Func<FileStream, byte[]> _read;
        private readonly int _tsize;

        private Int64 _readCount;
        public MatrixReader(string fileName, MatrixType matrixType, int size)
        {
            _stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            _size = size;

            switch (matrixType)
            {
                case MatrixType.Int16:
                    _tsize = 2;
                    _read = ReadInt16;
                    break;
                case MatrixType.UInt16:
                    _tsize = 2;
                    _read = ReadUInt16;
                    break;
                case MatrixType.Double:
                    _tsize = 8;
                    _read = ReadDouble;
                    break;
                default:
                    _tsize = 2;
                    _read = ReadInt16;
                    break;
            }
        }

        public byte[] Read(int row, int col)
        {
            Int64 pnum = ((Int64)row) * _size + col;
            if (pnum > _readCount)
            {
                Int64 skip = (pnum - _readCount) * _tsize;
                _stream.Seek(skip, SeekOrigin.Current);
                _readCount += (pnum - _readCount);
            }
            else if (pnum < _readCount)
            {
                Int64 skip = pnum * _tsize;
                _stream.Seek(skip, SeekOrigin.Begin);
                _readCount = pnum;
            }
            // if pnum = _readCount then stream is already on top of the byte we want to start reading

            byte[] buff = _read(_stream);
            ++_readCount;
            return buff;
        }

        public void Dispose()
        {
            if (_stream != null)
            {
                _stream.Close();
                _stream.Dispose();
            }

        }

        private static byte[] ReadInt16(FileStream fStream)
        {
            var bytes = new byte[2];
            fStream.Read(bytes, 0, 2);
            return bytes;
        }

        private static byte[] ReadUInt16(FileStream fStream)
        {
            var bytes = new byte[2];
            fStream.Read(bytes, 0, 2);
            return bytes;
        }

        private static byte[] ReadDouble(FileStream fStream)
        {
            var bytes = new byte[8];
            fStream.Read(bytes, 0, 8);
            return bytes;
        }

    }

    public enum MatrixType
    {
        Int16,
        UInt16,
        Double
    }
}
