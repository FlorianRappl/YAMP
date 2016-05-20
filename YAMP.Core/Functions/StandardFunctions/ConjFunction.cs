namespace YAMP
{
    [Description("ConjFunctionDescription")]
    [Kind(PopularKinds.Function)]
    sealed class ConjFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Conjugate();
        }
    }
}
