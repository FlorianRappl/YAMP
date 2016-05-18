namespace YAMP
{
    using System;
    using System.Collections.Generic;

    [Description("FindFunctionDescription")]
    [Kind(PopularKinds.Function)]
    sealed class FindFunction : ArgumentFunction
    {
        [Description("FindFunctionDescriptionForMatrix")]
        [Example("find(isprime(1:1000))", "FindFunctionExampleForMatrix1")]
        public MatrixValue Function(MatrixValue M)
        {
            return GetIndices(M, M.Length, 0);
        }

        [Description("FindFunctionDescriptionForMatrixScalar")]
        [Example("find(isprime(1:1000), 10)", "FindFunctionExampleForMatrixScalar1")]
        public MatrixValue Function(MatrixValue M, ScalarValue n)
        {
            return GetIndices(M, n.GetIntegerOrThrowException("n", Name), 0);
        }

        [Description("FindFunctionDescriptionForMatrixScalarScalar")]
        [Example("find(isprime(1:1000), 10, -10)", "FindFunctionExampleForMatrixScalarScalar1")]
        public MatrixValue Function(MatrixValue M, ScalarValue n, ScalarValue sigma)
        {
            return GetIndices(M, n.GetIntegerOrThrowException("n", Name), sigma.GetIntegerOrThrowException("sigma", Name));
        }

        static MatrixValue GetIndices(MatrixValue x, Int32 n, Int32 offset)
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
                        {
                            ind.Add(new ScalarValue(i));
                        }
                        else
                        {
                            toskip--;
                        }
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
                        {
                            ind.Add(new ScalarValue(i));
                        }
                        else
                        {
                            toskip--;
                        }
                    }
                }
            }

            if (offset < 0)
            {
                ind.Reverse();
            }

            return new MatrixValue(ind.ToArray(), ind.Count, 1);
        }
    }
}
