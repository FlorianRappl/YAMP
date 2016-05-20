namespace YAMP
{
	[Description("TraceFunctionDescription")]
    [Kind(PopularKinds.Function)]
    [Link("TraceFunctionLink")]
    sealed class TraceFunction : ArgumentFunction
	{
		[Description("TraceFunctionDescriptionForMatrix")]
		[Example("trace([1,2;3,4])", "TraceFunctionExampleForMatrix1")]
		public ScalarValue Function(MatrixValue M)
		{
			return M.Trace();
		}

		[Description("TraceFunctionDescriptionForScalar")]
		[Example("trace(10)", "TraceFunctionExampleForScalar1")]
		public ScalarValue Function(ScalarValue x)
		{
			return x;
		}
	}
}
