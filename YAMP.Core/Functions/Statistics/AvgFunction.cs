namespace YAMP
{
    [Kind(PopularKinds.Statistic)]
    [Description("AvgFunctionDescription")]
    [Link("AvgFunctionLink")]
    sealed class AvgFunction : ArgumentFunction
    {
        [Description("AvgFunctionDescriptionForMatrix")]
        [Example("avg([1, 4, 8])", "AvgFunctionExampleForMatrix1")]
        [Example("avg([1, 2, 3; 2, 3, 2])", "AvgFunctionExampleForMatrix2")]
        public Value Function(MatrixValue M)
        {
            return YMath.Average(M);
        }
    }
}
