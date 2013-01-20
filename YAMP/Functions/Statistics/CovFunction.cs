using System;

namespace YAMP
{
	[Description("In probability theory and statistics, covariance is a measure of how much fluctuations in one variable are accompanied by a fluctuations in another variable.")]
    [Kind(PopularKinds.Statistic)]
    [Link("http://en.wikipedia.org/wiki/Covariance")]
	class CovFunction : ArgumentFunction
	{
		[Description("This function returns a symmetric matrix with all variances on the diagonal and all covariances in the rest of the matrix.")]
        [Example("cov([3 + randn(100, 1), 10 + 2 * randn(100, 1)])", "Gives the covariance matrix for two independent random variables of variance [1,4]. In the limit of infinite datasets the resulting covariance matrix has the variances [1, 4] on the diagonal and 0 on the off-diagonal parts, as the two randomvariables are uncorrelated.")]
        public MatrixValue Function(MatrixValue M)
		{
            return Covariance(M);
		}

        public static MatrixValue Covariance(MatrixValue M)
        {
            if (M.Length == 0)
                return new MatrixValue();
            
            if (M.IsVector)
                return new MatrixValue(1, 1, VarFunction.Variance(M) as ScalarValue);

            var avg = AvgFunction.Average(M) as MatrixValue;
            double scale = 1.0;
            var s = new MatrixValue(M.DimensionX, M.DimensionX);

            for (int i = 1; i <= M.DimensionY; i++)
                for (int j = 1; j <= M.DimensionX; j++)
                    for (int k = 1; k <= M.DimensionX; k++)
                        s[k, j] += (M[i, j] - avg[j]) * (M[i, k] - avg[k]);

            scale /= M.DimensionY;

            for (var i = 1; i <= s.DimensionY; i++)
                for (int j = 1; j <= s.DimensionX; j++)
                    s[i, j] *= scale;

            return s;
        }
	}
}