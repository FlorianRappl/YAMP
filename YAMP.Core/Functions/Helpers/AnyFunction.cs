namespace YAMP
{
    [Description("AnyFunctionDescription")]
    [Kind(PopularKinds.Logic)]
    sealed class AnyFunction : ArgumentFunction
    {
        [Description("AnyFunctionDescriptionForMatrix")]
        [Example("any([0, 0; 0, 0])", "AnyFunctionExampleForMatrix1")]
        [Example("any(eye(3))", "AnyFunctionExampleForMatrix2")]
        public ScalarValue Function(MatrixValue M)
        {
            return new ScalarValue(M.HasElements);
        }
    }
}
