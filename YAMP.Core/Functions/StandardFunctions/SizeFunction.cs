namespace YAMP
{
    [Description("SizeFunctionDescription")]
    [Kind(PopularKinds.Function)]
    sealed class SizeFunction : ArgumentFunction
    {
        [Description("SizeFunctionDescriptionForMatrix")]
        [Example("size([1,2,3,4,5])", "SizeFunctionExampleForMatrix1")]
        [Example("size(rand(2))", "SizeFunctionExampleForMatrix2")]
        public MatrixValue Function(MatrixValue M)
        {
            var m = new MatrixValue();
            m[1, 1] = new ScalarValue(M.DimensionY);
            m[1, 2] = new ScalarValue(M.DimensionX);
            return m;
        }

        [Description("SizeFunctionDescriptionForScalar")]
        public MatrixValue Function(ScalarValue x)
        {
            var m = new MatrixValue();
            m[1, 1] = new ScalarValue(1);
            m[1, 2] = new ScalarValue(1);
            return m;
        }

        [Description("SizeFunctionDescriptionForString")]
        [Example("size(\"hello\")", "SizeFunctionExampleForString1")]
        public MatrixValue Function(StringValue str)
        {
            var m = new MatrixValue();
            m[1, 1] = new ScalarValue(1);
            m[1, 2] = new ScalarValue(str.Value.Length);
            return m;
        }
    }
}
