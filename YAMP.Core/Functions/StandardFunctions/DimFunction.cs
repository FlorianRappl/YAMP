namespace YAMP
{
    using System;

    [Description("DimFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class DimFunction : ArgumentFunction
	{
		[Description("DimFunctionDescriptionForMatrix")]
		[Example("dim([1,2,3,4,5])", "DimFunctionExampleForMatrix1")]
		public ScalarValue Function(MatrixValue M)
		{
            return new ScalarValue(Math.Max(M.DimensionX, M.DimensionY));
		}

        [Description("DimFunctionDescriptionForScalar")]
        public ScalarValue Function(ScalarValue str)
        {
            return new ScalarValue(1);
        }

        [Description("DimFunctionDescriptionForString")]
        [Example("dim(\"hello\")", "DimFunctionExampleForString1")]
        public ScalarValue Function(StringValue str)
        {
            return new ScalarValue(str.Length);
        }
	}
}

