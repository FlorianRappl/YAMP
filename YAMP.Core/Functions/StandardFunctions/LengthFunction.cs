namespace YAMP
{
	[Description("LengthFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class LengthFunction : ArgumentFunction
	{
        [Description("LengthFunctionDescriptionForMatrix")]
        [Example("length([1,2,3,4,5;6,7,8,9,10])", "LengthFunctionExampleForMatrix1")]
        public ScalarValue Function(MatrixValue M)
        {
            return new ScalarValue(M.Length);
        }

        [Description("LengthFunctionDescriptionForScalar")]
        public ScalarValue Function(ScalarValue x)
        {
            return new ScalarValue(1);
        }

        [Description("LengthFunctionDescriptionForString")]
        [Example("length(\"hello\")", "LengthFunctionExampleForString1")]
        public ScalarValue Function(StringValue str)
        {
            return new ScalarValue(str.Length);
        }
	}
}

