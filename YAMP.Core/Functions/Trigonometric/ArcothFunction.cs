namespace YAMP
{
    [Description("ArcothFunctionDescription")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("ArcothFunctionLink")]
    sealed class ArcothFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return 0.5 * ((1.0 + value) / (value - 1.0)).Ln();
        }
    }
}
