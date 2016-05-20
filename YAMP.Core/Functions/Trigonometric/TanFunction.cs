namespace YAMP
{
    [Description("TanFunctionDescription")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("TanFunctionLink")]
    sealed class TanFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Sin() / value.Cos();
        }
    }
}
