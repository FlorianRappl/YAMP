namespace YAMP
{
    [Kind(PopularKinds.Statistic)]
	[Description("MeanFunctionDescription")]
    [Link("MeanFunctionLink")]
    sealed class MeanFunction : ArgumentFunction
	{
		[Description("MeanFunctionDescriptionForMatrix")]
		[Example("mean([1, 4, 8])", "MeanFunctionExampleForMatrix1")]
		[Example("mean([1, 4, 8; 2, 5, 7])", "MeanFunctionExampleForMatrix2")]
		public Value Function(MatrixValue M)
		{
			return YMath.Mean(M);
		}
	}
}
