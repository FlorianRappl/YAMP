namespace YAMP
{
    using YAMP.Numerics;

    [Description("ErfFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class ErfFunction : StandardFunction
	{
		protected override ScalarValue GetValue(ScalarValue value)
		{
			return ErrorFunction.Erf(value);
		}
	}
}
