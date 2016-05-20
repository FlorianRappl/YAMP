namespace YAMP
{
    using YAMP.Numerics;

    [Description("Bessel1FunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class Bessel1Function : StandardFunction
	{
		protected override ScalarValue GetValue(ScalarValue value)
		{
			return new ScalarValue(Bessel.j1(value.Re));
		}
	}
}
