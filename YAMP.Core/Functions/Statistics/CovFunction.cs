namespace YAMP
{
	[Description("CovFunctionDescription")]
    [Kind(PopularKinds.Statistic)]
    [Link("CovFunctionLink")]
    sealed class CovFunction : ArgumentFunction
	{
		[Description("CovFunctionDescriptionForMatrix")]
        [Example("cov([3 + randn(100, 1), 10 + 2 * randn(100, 1)])", "CovFunctionExampleForMatrix1")]
        public MatrixValue Function(MatrixValue M)
		{
            return YMath.Covariance(M);
		}
	}
}