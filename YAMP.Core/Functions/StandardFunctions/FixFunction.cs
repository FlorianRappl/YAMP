namespace YAMP
{
    using System;

    [Description("FixFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class FixFunction : StandardFunction
	{
		protected override ScalarValue GetValue(ScalarValue value)
		{
			var re = Math.Floor(value.Re);
			var im = Math.Floor(value.Im);
			return new ScalarValue(re, im);
		}
	}
}
