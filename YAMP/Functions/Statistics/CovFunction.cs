using System;

namespace YAMP
{
	[Description("In probability theory and statistics, covariance is a measure of how much fluctuations in one variable are accompanied by a fluctuations in another variable.")]
    [Kind(PopularKinds.Statistic)]
    [Link("http://en.wikipedia.org/wiki/Covariance")]
    sealed class CovFunction : ArgumentFunction
	{
		[Description("This function returns a symmetric matrix with all variances on the diagonal and all covariances in the rest of the matrix.")]
        [Example("cov([3 + randn(100, 1), 10 + 2 * randn(100, 1)])", "Gives the covariance matrix for two independent random variables of variance [1,4]. In the limit of infinite datasets the resulting covariance matrix has the variances [1, 4] on the diagonal and 0 on the off-diagonal parts, as the two randomvariables are uncorrelated.")]
        public MatrixValue Function(MatrixValue M)
		{
            return YMath.Covariance(M);
		}
	}
}