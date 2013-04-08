using System;
using System.Collections.Generic;

namespace YAMP
{
    [Description("Find indices and values of nonzero elements.")]
    [Kind(PopularKinds.Function)]
    sealed class FindFunction : ArgumentFunction
    {
        [Description("Locates all nonzero elements of the matrix M, and returns the linear indices of those elements in a column vector. If M contains no nonzero elements, then an empty matrix will be returned.")]
        [Example("find(isprime(1:1000))", "Returns all indices of the matrix which are related to non-zero entries.")]
        public MatrixValue Function(MatrixValue M)
        {
            return GetIndices(M, M.Length, 0);
        }

        [Description("Returns at most the first n indices corresponding to the nonzero entries of M, where n must be a positive integer.")]
        [Example("find(isprime(1:1000), 10)", "Returns the first 10 indices which are non-zero.")]
        public MatrixValue Function(MatrixValue M, ScalarValue n)
        {
            return GetIndices(M, n.GetIntegerOrThrowException("n", Name), 0);
        }

        [Description("Returns at n indices with offset sigma corresponding to the nonzero entries of M, where n must be a positive integer. If sigma is negative, then it will start from the end of the list.")]
        [Example("find(isprime(1:1000), 10, -10)", "Returns the last 10 indices which are non-zero.")]
        public MatrixValue Function(MatrixValue M, ScalarValue n, ScalarValue sigma)
        {
            return GetIndices(M, n.GetIntegerOrThrowException("n", Name), sigma.GetIntegerOrThrowException("sigma", Name));
        }

        static MatrixValue GetIndices(MatrixValue x, int n, int offset)
        {
            var m = x.Length;
            var ind = new List<ScalarValue>();
            var toskip = offset < 0 ? Math.Abs(offset + 1) : offset;

            if (offset >= 0)
            {
                for (var i = 1; i <= m && ind.Count != n; i++)
                {
                    if (x[i] != ScalarValue.Zero)
                    {
                        if (toskip == 0)
                            ind.Add(new ScalarValue(i));
                        else
                            toskip--;
                    }
                }
            }
            else
            {
                for (var i = m; i >= 1 && ind.Count != n; i--)
                {
                    if (x[i] != ScalarValue.Zero)
                    {
                        if (toskip == 0)
                            ind.Add(new ScalarValue(i));
                        else
                            toskip--;
                    }
                }
            }

            if (offset < 0)
                ind.Reverse();

            return new MatrixValue(ind.ToArray(), ind.Count, 1);
        }
    }
}
