namespace YAMP
{
    [Description("VectorFunctionDescription")]
    [Kind(PopularKinds.Function)]
    sealed class VectorFunction : ArgumentFunction
    {
        [Description("VectorFunctionDescriptionForMatrix")]
        [Example("vector([1,2;3,4])", "VectorFunctionExampleForMatrix1")]
        public MatrixValue Function(MatrixValue M)
        {
            var v = new MatrixValue(M.Length, 1);
            var k = 1;

            for (var i = 1; i <= M.DimensionX; i++)
            {
                for (var j = 1; j <= M.DimensionY; j++)
                {
                    v[k++] = M[j, i];
                }
            }

            return v;
        }
    }
}
