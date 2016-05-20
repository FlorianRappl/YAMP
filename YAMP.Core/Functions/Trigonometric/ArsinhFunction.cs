namespace YAMP
{
    [Description("ArsinhFunctionDescription")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("ArsinhFunctionLink")]
    sealed class ArsinhFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return (value + ((value * value) + 1.0).Sqrt()).Ln();
        }
    }
}
