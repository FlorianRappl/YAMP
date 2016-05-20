namespace YAMP
{
    using System;

    [Description("ACorFunctionDescription")]
    [Kind(PopularKinds.Statistic)]
    [Link("ACorFunctionLink")]
    sealed class ACorFunction : ArgumentFunction
    {
        [Description("ACorFunctionDescriptionForMatrix")]
        [Example("acor(3 + randn(100, 1))", "ACorFunctionExampleForMatrix1")]
        public MatrixValue Function(MatrixValue M)
        {
            if (M.Length > 1)
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

                return YMath.CrossCorrelation(M, M, nOffset);
            }

            return new MatrixValue();
        }

        [Example("acor(3+randn(100,1), 4)", "ACorFunctionExampleForMatrixScalar1")]
        public MatrixValue Function(MatrixValue M, ScalarValue nLag)
        {
            if (M.Length > 1)
            {
                var nOffset = nLag.GetIntegerOrThrowException("nLag", Name);

                if (nOffset < 0)
                {
                    nOffset = 0;
                }
                else if (nOffset >= M.Length)
                {
                    nOffset = M.Length - 1;
                }

                return YMath.CrossCorrelation(M, M, nOffset);
            }

            return new MatrixValue();
        }
    }
}