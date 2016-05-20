namespace YAMP
{
	[Description("CorFunctionDescription")]
    [Kind(PopularKinds.Statistic)]
    [Link("CorFunctionLink")]
    sealed class CorFunction : ArgumentFunction
	{
        [Description("CorFunctionDescriptionForMatrix")]
        [Example("cor([3 + randn(100, 1), 10 + 2 * randn(100, 1)])", "CorFunctionExampleForMatrix1")]
        public MatrixValue Function(MatrixValue M)
		{
            return YMath.Correlation(M);
		}
	}
}