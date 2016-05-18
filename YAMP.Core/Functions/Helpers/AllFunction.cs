namespace YAMP
{
    [Description("AllFunctionDescription")]
    [Kind(PopularKinds.Logic)]
    sealed class AllFunction : ArgumentFunction
    {
        [Description("AllFunctionDescriptionForMatrix")]
        [Example("all([0, 0; 0, 0])", "AllFunctionExampleForMatrix1")]
        [Example("all(eye(3))", "AllFunctionExampleForMatrix2")]
        [Example("all([2,1;5,2])", "AllFunctionExampleForMatrix3")]
        public ScalarValue Function(MatrixValue M)
        {
            return new ScalarValue(M.HasNoZeros);
        }
    }
}
