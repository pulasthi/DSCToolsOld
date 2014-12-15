using System;
using System.IO;
using System.Reflection;

namespace MBF2Java2Csharp
{
    public class SimilarityMatrix
    {
        private int[][] _values;
        private byte[] _symbols;
        private string _name;

        // Row number is same as byte value corresponding
        // to sequence symbol on the row.
        // Column number is same as byte value corresponding
        // to sequence symbol on the column.
        public int GetValueAt(int row, int col)
        {
            return _values[row][col];
        }

        public int[][] Values
        {
            get { return _values; }
            set { _values = value; }
        }


        public byte[] Symbols
        {
            get { return _symbols; }
            set { _symbols = value; }
        }


        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


        public static SimilarityMatrix EDNAFULL {
            get { return LoadMatrix(GetReader("EDNAFULL"), "EDNAFULL"); }
        }

        public static SimilarityMatrix LoadMatrixFromFile(string path, string matrixName) {
            using (Stream fs = File.OpenRead(path))
            {
                return LoadMatrix(fs, matrixName);
            }
        }

        private static SimilarityMatrix LoadMatrix(Stream stream, string matrixName)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                SimilarityMatrix mat = new SimilarityMatrix();

                char commentStarter = '#';
                string line;
                // Skip comments
                while ((line = reader.ReadLine()) != null && line.Trim()[0] == commentStarter)
                {
                }

                mat.Name = matrixName; // Matrix name

                char[] delimitters = new[] {'\t', ' '};

                byte[] syms = new byte[byte.MaxValue];
                if (line != null) // The symbol line
                {
                    string[] splits = line.Trim().Split(delimitters, StringSplitOptions.RemoveEmptyEntries);
                    string split;
                    byte u, l;
                    for (int i = 0; i < splits.Length; i++)
                    {
                        split = splits[i];
                        u = (byte) split.ToUpperInvariant()[0];
                        l = (byte) split.ToLowerInvariant()[0];
                        // Store split in both lower case and upper case positions in syms
                        syms[u] = u;
                        syms[l] = l;
                    }
                    mat.Symbols = syms;

                    int[][] values = new int[byte.MaxValue][];
                    for (int i = 0; i < byte.MaxValue; i++)
                    {
                        values[i] = new int[byte.MaxValue];
                    }

                    foreach (string r in splits)
                    {
                        int count = 1;
// ReSharper disable PossibleNullReferenceException
                        string[] rowSplits = reader.ReadLine().Split(delimitters, StringSplitOptions.RemoveEmptyEntries);
// ReSharper restore PossibleNullReferenceException
                        int[] rowValuesU = values[(byte) r.ToUpperInvariant()[0]];
                        int[] rowValuesL = values[(byte) r.ToLowerInvariant()[0]];
                        foreach (string c in splits)
                        {
                            rowValuesU[(byte) c.ToUpperInvariant()[0]] = int.Parse(rowSplits[count]);
                            rowValuesL[(byte) c.ToLowerInvariant()[0]] = int.Parse(rowSplits[count++]);
                        }
                    }

                    mat.Values = values;
                }
                return mat;
            }
        }

        private static Stream GetReader(string fname)
        {
            return
                Assembly.GetExecutingAssembly().GetManifestResourceStream("MBF2Java2Csharp.Matrices." + fname);
        }

    }
}
