using System;

namespace YAMP
{
	[Description("In probability theory and statistics, cross-correlation is a measure of how much two random variables are correlated at different offsets.")]
	[Kind(PopularKinds.Function)]
    class XCorFunction : ArgumentFunction
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

            return CrossCorrelation(M, N, nOffset);
		}

        [Description("This function returns a vector with the defined cross-correlations for diferent offsets. All matrices are treated as vectors.")]
        [Example("xcor(3 + randn(100, 1), 10 + 2 * randn(100, 1), 4)", "Gives the first 4 cross-correlations for two independent random variables of variance [1, 4].")]
        public MatrixValue Function(MatrixValue M, MatrixValue N, ScalarValue lag)
        {
            if (M.Length != N.Length || M.Length <= 1)
                return new MatrixValue();
            
            int nOffset = lag.IntValue;

            if (nOffset < 0)
                nOffset = 0;
            else if (nOffset >= M.Length)
                nOffset = M.Length - 1;

            return CrossCorrelation(M, N, nOffset);
        }

        public static MatrixValue CrossCorrelation(MatrixValue M, MatrixValue N, int n)
        {
            var result = new MatrixValue(1, n + 1);
            var avgM = (ScalarValue)AvgFunction.Average(M);
            var avgN = (ScalarValue)AvgFunction.Average(N);
            var errM = ((ScalarValue)VarFunction.Variance(M)).Sqrt();
            var errN = ((ScalarValue)VarFunction.Variance(N)).Sqrt();
            var length = M.Length;

            for (int i = 0; i <= n; i++)
            {
                var scale = 1.0 / ((length - i) * errM * errN);

                for (int j = 1; j <= length - i; j++)
                    result[i + 1] += (M[j] - avgM) * (N[j + i] - avgN);

                result[i + 1] *= scale;
            }

            return result;
        }
    }
}