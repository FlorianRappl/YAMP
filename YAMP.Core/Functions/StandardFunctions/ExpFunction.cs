namespace YAMP
{
	[Description("ExpFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class ExpFunction : StandardFunction
	{
		protected override ScalarValue GetValue(ScalarValue value)
		{
			return value.Exp();
		}
	}
}

