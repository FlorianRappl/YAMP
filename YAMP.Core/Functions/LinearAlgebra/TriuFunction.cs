namespace YAMP
{
    using YAMP.Numerics;

    [Description("TriuFunctionDescription")]
    [Kind(PopularKinds.Function)]
    sealed class TriuFunction : ArgumentFunction
    {
        [Description("TriuFunctionDescriptionForMatrix")]
        [Example("triu(rand(4))", "TriuFunctionExampleForMatrix1")]
        public MatrixValue Function(MatrixValue M)
        {
            var lu = new LUDecomposition(M);
            return lu.U;
        }
    }
}
