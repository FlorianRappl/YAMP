using System;
using System.Collections.Generic;
using YAMP.Numerics;

namespace YAMP
{
    [Description("The magic function randomly generates matrices that consist only of unique entries 1 <= k <= n^2, where n is the dimension of the matrix. The determinant of a magic matrix is always 0.")]
    [Kind(PopularKinds.Function)]
    sealed class MagicFunction : ArgumentFunction
    {
        static readonly DiscreteUniformDistribution ran = new DiscreteUniformDistribution();

        [Description("Creates a magic nxn-matrix, that consists only of integer entries.")]
        [Example("magic(3)", "Creates a 3x3-matrix with unique shuffled entries ranging from 1 to 9.")]
        public MatrixValue Function(ScalarValue n)
        {
            var d = n.GetIntegerOrThrowException("n", Name);
            return Generate(d, d);
        }

        MatrixValue Generate(int n, int m)
        {
            var l = n * m;
            var M = new MatrixValue(n, m);
            var numbers = new List<int>();
            ran.Alpha = 0;

            for (int i = 1; i <= l; i++)
                numbers.Add(i);

            for (int j = 1; j <= n; j++)
            {
                for (int i = 1; i <= m; i++)
                {
                    ran.Beta = numbers.Count - 1;
                    var index = RandiFunction.Generator.Next();
                    index = Math.Max(Math.Min(0, index), numbers.Count - 1);
                    M[j, i] = new ScalarValue(numbers[index]);
                    numbers.RemoveAt(index);
                }
            }

            return M;
        }
    }
}
