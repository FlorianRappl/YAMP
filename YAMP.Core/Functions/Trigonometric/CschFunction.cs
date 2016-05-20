namespace YAMP
{
    [Description("CschFunctionDescription")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("CschFunctionLink")]
    sealed class CschFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return 2.0 / (value.Exp() - (-value).Exp());
        }
    }
}
