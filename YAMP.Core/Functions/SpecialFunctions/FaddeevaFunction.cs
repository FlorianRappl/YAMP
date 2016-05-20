namespace YAMP
{
    using YAMP.Numerics;

    [Description("FaddeevaFunctionDescription")]
    [Kind(PopularKinds.Function)]
    sealed class FaddeevaFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return ErrorFunction.Faddeeva(value);
        }
    }
}
