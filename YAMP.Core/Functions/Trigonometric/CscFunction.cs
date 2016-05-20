namespace YAMP
{
    [Description("CscFunctionDescription")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("CscFunctionLink")]
	sealed class CscFunction : StandardFunction
	{
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return 1.0 / value.Sin();
        }
	}
}

