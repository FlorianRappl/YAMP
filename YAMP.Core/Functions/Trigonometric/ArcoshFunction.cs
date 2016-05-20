namespace YAMP
{
    [Description("ArcoshFunctionDescription")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("ArcoshFunctionLink")]
    sealed class ArcoshFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return (value + ((value * value) - 1.0).Sqrt()).Ln();
        }
    }
}
