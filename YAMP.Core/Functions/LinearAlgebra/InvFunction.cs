namespace YAMP
{
	[Description("InvFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class InvFunction : ArgumentFunction
	{
		[Description("InvFunctionDescriptionForScalar")]
		[Example("inv(5)", "InvFunctionExampleForScalar1")]
		public ScalarValue Function(ScalarValue x)
		{
			return 1.0 / x;
		}

		[Description("InvFunctionDescriptionForMatrix")]
		[Example("inv([0,2;1,0])", "InvFunctionExampleForMatrix1")]
		public MatrixValue Function(MatrixValue M)
		{
			return M.Inverse();
		}
	}
}

