namespace YAMP
{
    [Description("CosFunctionDescription")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("CosFunctionLink")]
	sealed class CosFunction : StandardFunction
	{
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Cos();
        }
	}
}

