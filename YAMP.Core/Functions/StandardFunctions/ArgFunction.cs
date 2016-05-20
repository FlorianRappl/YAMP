namespace YAMP
{
    [Description("ArgFunctionDescription")]
    [Kind(PopularKinds.Function)]
    sealed class ArgFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return new ScalarValue(value.Arg());
        }
    }
}
