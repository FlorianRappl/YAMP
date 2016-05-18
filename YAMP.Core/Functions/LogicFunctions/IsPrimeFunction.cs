namespace YAMP
{
	[Description("IsPrimeFunctionDescription")]
	[Kind(PopularKinds.Logic)]
    [Link("IsPrimeFunctionLink")]
    sealed class IsPrimeFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return new ScalarValue(value.IsPrime);
        }
    }
}
