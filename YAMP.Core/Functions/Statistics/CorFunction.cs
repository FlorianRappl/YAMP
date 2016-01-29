using System;

namespace YAMP
{
	[Description("In probability theory and statistics, correlation is a measure of how much fluctuations in one variable are accompanied by a fluctuations in another variable. It is the covariance normalized with the variances.")]
    [Kind(PopularKinds.Statistic)]
    [Link("http://en.wikipedia.org/wiki/Correlation_and_dependence")]
    sealed class CorFunction : ArgumentFunction
	{
        [Description("This function returns a symmetric matrix with ones on the diagonal and all correlation coefficents in the rest of the matrix.")]
        [Example("cor([3 + randn(100, 1), 10 + 2 * randn(100, 1)])", "Gives the correlation matrix for two independent random variables of variance [1, 4]. In the limit of infinite datasets the resulting correlation matrix has 0 on the off-diagonal parts, as the two random variables are uncorrelated.")]
        public MatrixValue Function(MatrixValue M)
		{
            return YMath.Correlation(M);
		}
	}
}