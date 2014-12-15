using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace MathNetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var matrix = new double[4,4];
            matrix[0, 0] = 3.0;
            matrix[0, 1] = 4.0;
            matrix[0, 2] = -1.0;
            matrix[0, 3] = 0.0;
            matrix[1, 0] = 4.0;
            matrix[1, 1] = 5.0;
            matrix[1, 2] = 0.0;
            matrix[1, 3] = -1.0;
            matrix[2, 0] = 5.0;
            matrix[2, 1] = 6.0;
            matrix[2, 2] = 0.0;
            matrix[2, 3] = 0.0;
            matrix[3, 0] = 6.0;
            matrix[3, 1] = 7.0;
            matrix[3, 2] = 0.0;
            matrix[3, 3] = 0.0;


            var rhs = new double[4,1];
            rhs[0, 0] = 0.0;
            rhs[1, 0] = 0.0;
            rhs[2, 0] = 20.0;
            rhs[3, 0] = 0.0;

            Matrix m = Matrix.Create(matrix);
            var r = Matrix.Create(rhs);

            var lu = new LUDecomposition(m);
            var sol = lu.Solve(r);
            Console.WriteLine(sol.ToString());
            Console.Read();
        }
    }
}
