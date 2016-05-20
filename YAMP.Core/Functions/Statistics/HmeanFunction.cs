namespace YAMP
{
    [Kind(PopularKinds.Statistic)]
    [Description("HmeanFunctionDescription")]
    [Link("HmeanFunctionLink")]
    sealed class HmeanFunction : ArgumentFunction
    {
        [Description("HmeanFunctionDescriptionForMatrix")]
        [Example("hmean([1, 4, 8])", "HmeanFunctionExampleForMatrix1")]
        [Example("hmean([1, 4, 8; 2, 5, 7])", "HmeanFunctionExampleForMatrix2")]
        public Value Function(MatrixValue M)
        {
            return YMath.HarmonicMean(M);
        }
    }
}
