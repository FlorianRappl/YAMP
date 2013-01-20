using System;

namespace YAMP
{
	[Description("In probability theory and statistics, correlation is a measure of how much fluctuations in one variable are accompanied by a fluctuations in another variable. It is the covariance normalized with the variances.")]
    [Kind(PopularKinds.Statistic)]
    [Link("http://en.wikipedia.org/wiki/Correlation_and_dependence")]
	class CorFunction : ArgumentFunction
	{
        [Description("This function returns a symmetric matrix with ones on the diagonal and all correlation coefficents in the rest of the matrix.")]
        [Example("cor([3 + randn(100, 1), 10 + 2 * randn(100, 1)])", "Gives the correlation matrix for two independent random variables of variance [1, 4]. In the limit of infinite datasets the resulting correlation matrix has 0 on the off-diagonal parts, as the two random variables are uncorrelated.")]
        public MatrixValue Function(MatrixValue M)
		{
            return Correlation(M);
		}

        public static MatrixValue Correlation(MatrixValue M)
        {
            if (M.Length == 0)
                return new MatrixValue();

            var result = CovFunction.Covariance(M);

            for (int i = 1; i <= M.DimensionX; i++)
            {
                var temp = 1 / result[i, i].Sqrt();

                for (int j = 1; j <= M.DimensionX; j++)
                {
                    result[i, j] *= temp;
                    result[j, i] *= temp;
                }
            }

            return result;
        }
	}
}