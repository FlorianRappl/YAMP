namespace YAMP
{
    using System;

    [Description("RoundFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class RoundFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            var re = Math.Round(value.Re);
            var im = Math.Round(value.Im);
            return new ScalarValue(re, im);
        }
    }
}
