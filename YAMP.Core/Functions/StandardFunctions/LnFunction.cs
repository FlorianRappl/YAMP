namespace YAMP
{
	[Description("LnFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class LnFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Ln();
        }
    }
}

