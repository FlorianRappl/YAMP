namespace YAMP
{
    using YAMP.Numerics;

    [Description("Bessel0FunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class Bessel0Function : StandardFunction
	{
		protected override ScalarValue GetValue(ScalarValue value)
		{
			return new ScalarValue(Bessel.j0(value.Re));
		}
	}
}
