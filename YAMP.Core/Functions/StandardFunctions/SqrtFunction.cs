namespace YAMP
{
	[Description("SqrtFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class SqrtFunction : StandardFunction
	{
		protected override ScalarValue GetValue(ScalarValue value)
		{
			return value.Sqrt();
		}
	}
}

