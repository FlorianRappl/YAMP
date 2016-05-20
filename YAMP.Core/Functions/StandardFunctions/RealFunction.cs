namespace YAMP
{
	[Description("RealFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class RealFunction : StandardFunction
	{
		protected override ScalarValue GetValue(ScalarValue value)
		{
			return new ScalarValue(value.Re);
		}
	}
}
