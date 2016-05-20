namespace YAMP
{
	[Description("DetFunctionDescription")]
	[Kind(PopularKinds.Function)]
    [Link("DetFunctionLink")]
    sealed class DetFunction : ArgumentFunction
	{
		[Description("DetFunctionDescriptionForMatrix")]
		[Example("det([1,3;-1,0])", "DetFunctionExampleForMatrix1")]
		public ScalarValue Function(MatrixValue M)
		{
			return M.Det();
		}

		[Description("DetFunctionDescriptionForScalar")]
		[Example("det(5)", "DetFunctionExampleForScalar1")]
		public ScalarValue Function(ScalarValue x)
		{
			return x;
		}
	}
}

