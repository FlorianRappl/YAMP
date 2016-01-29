using System;

namespace YAMP
{
	[Description("In probability theory and statistics, cross-correlation is a measure of how much two random variables are correlated at different offsets.")]
    [Kind(PopularKinds.Statistic)]
    [Link("http://en.wikipedia.org/wiki/Cross-correlation")]
    sealed class XCorFunction : ArgumentFunction
	{
        [Description("This function returns a vector with cross-correlations for diferent offsets. All matrices are treated as vectors.")]
        [Example("xcor(3 + randn(100, 1), 10 + 2 * randn(100, 1))", "Gives the cross-correlation for two independent random variables of variance [1, 4] and different offsets.")]
        public MatrixValue Function(MatrixValue M, MatrixValue N)
		{
            if (M.Length != N.Length || M.Length <= 1)
                return new MatrixValue();

            int nOffset = (int)(10 * Math.Log10(M.Length));

            if(nOffset < 0)
                nOffset = 0;
            else if(nOffset >= M.Length)
                nOffset = M.Length - 1;

            return YMath.CrossCorrelation(M, N, nOffset);
		}

        [Description("This function returns a vector with the defined cross-correlations for diferent offsets. All matrices are treated as vectors.")]
        [Example("xcor(3 + randn(100, 1), 10 + 2 * randn(100, 1), 4)", "Gives the first 4 cross-correlations for two independent random variables of variance [1, 4].")]
        public MatrixValue Function(MatrixValue M, MatrixValue N, ScalarValue lag)
        {
            if (M.Length != N.Length || M.Length <= 1)
                return new MatrixValue();
            
            int nOffset = lag.GetIntegerOrThrowException("lag", Name);

            if (nOffset < 0)
                nOffset = 0;
            else if (nOffset >= M.Length)
                nOffset = M.Length - 1;

            return YMath.CrossCorrelation(M, N, nOffset);
        }
    }
}