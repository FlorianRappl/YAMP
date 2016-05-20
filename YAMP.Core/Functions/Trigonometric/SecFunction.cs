namespace YAMP
{
    [Description("SecFunctionDescription")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("SecFunctionLink")]
	sealed class SecFunction : StandardFunction
	{
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return 1.0 / value.Cos();
        }
	}
}

