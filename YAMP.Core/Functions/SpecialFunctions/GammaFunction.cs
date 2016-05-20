namespace YAMP
{
    using YAMP.Numerics;

    [Description("GammaFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class GammaFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
			return Gamma.LinearGamma(value);
        }
    }
}
