namespace YAMP
{
    [Description("SinhFunctionDescription")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("SinhFunctionLink")]
    sealed class SinhFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return (value.Exp() - (-value).Exp()) / 2.0;
        }
    }
}
