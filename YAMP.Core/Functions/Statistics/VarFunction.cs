namespace YAMP
{
    [Description("VarFunctionDescription")]
    [Kind(PopularKinds.Statistic)]
    [Link("VarFunctionLink")]
    sealed class VarFunction : ArgumentFunction
    {
        [Description("VarFunctionDescriptionForMatrix")]
        [Example("var([1, 2, 3, 4, 5, 6])", "VarFunctionExampleForMatrix1")]
        [Example("var([1, 2, 3; 4, 5, 6; 7, 8, 9])", "VarFunctionExampleForMatrix2")]
        public Value Function(MatrixValue M)
        {
            return YMath.Variance(M);
        }
    }
}