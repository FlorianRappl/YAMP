namespace YAMP
{
	[Description("IsComplexFunctionDescription")]
	[Kind(PopularKinds.Logic)]
    [Link("IsComplexFunctionLink")]
    sealed class IsComplexFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return new ScalarValue(value.IsComplex);
        }
    }
}
