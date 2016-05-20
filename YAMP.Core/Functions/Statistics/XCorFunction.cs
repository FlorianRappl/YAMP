namespace YAMP
{
    using System;

    [Description("XCorFunctionDescription")]
    [Kind(PopularKinds.Statistic)]
    [Link("XCorFunctionLink")]
    sealed class XCorFunction : ArgumentFunction
	{
        [Description("XCorFunctionDescriptionForMatrixMatrix")]
        [Example("xcor(3 + randn(100, 1), 10 + 2 * randn(100, 1))", "XCorFunctionExampleForMatrixMatrix1")]
        public MatrixValue Function(MatrixValue M, MatrixValue N)
		{
            if (M.Length == N.Length && M.Length > 1)
            {
                var nOffset = (Int32)(10 * Math.Log10(M.Length));

                if (nOffset < 0)
                {
                    nOffset = 0;
                }
                else if (nOffset >= M.Length)
                {
                    nOffset = M.Length - 1;
                }

                return YMath.CrossCorrelation(M, N, nOffset);
            }

            return new MatrixValue();
		}

        [Description("XCorFunctionDescriptionForMatrixMatrixScalar")]
        [Example("xcor(3 + randn(100, 1), 10 + 2 * randn(100, 1), 4)", "XCorFunctionExampleForMatrixMatrixScalar1")]
        public MatrixValue Function(MatrixValue M, MatrixValue N, ScalarValue lag)
        {
            if (M.Length == N.Length && M.Length > 1)
            {
                var nOffset = lag.GetIntegerOrThrowException("lag", Name);

                if (nOffset < 0)
                {
                    nOffset = 0;
                }
                else if (nOffset >= M.Length)
                {
                    nOffset = M.Length - 1;
                }

                return YMath.CrossCorrelation(M, N, nOffset);
            }

            return new MatrixValue();
        }
    }
}