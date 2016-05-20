namespace YAMP
{
    using YAMP.Numerics;

    [Description("ErfcFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class ErfcFunction : StandardFunction
	{
		protected override ScalarValue GetValue(ScalarValue value)
		{
            return ErrorFunction.Erfc(value);
		}
	}
}
