namespace YAMP
{
    using YAMP.Numerics;

    [Description("ZetaFunctionDescription")]
    [Kind(PopularKinds.Function)]
    sealed class ZetaFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return Zeta.RiemannZeta(value);
        }
    }
}
