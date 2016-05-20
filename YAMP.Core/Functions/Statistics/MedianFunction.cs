namespace YAMP
{
	[Description("MedianFunctionDescription")]
    [Kind(PopularKinds.Statistic)]
    [Link("MedianFunctionLink")]
    sealed class MedianFunction : ArgumentFunction
	{
		[Description("MedianFunctionDescriptionForMatrix")]
		[Example("median([1, 5, 2, 8, 7])", "MedianFunctionExampleForMatrix1")]
		[Example("median([1, 6, 2, 8, 7, 2])", "MedianFunctionExampleForMatrix2")]
		public ScalarValue Function(MatrixValue M)
		{
            return YMath.Median(M);
		}
	}
}
