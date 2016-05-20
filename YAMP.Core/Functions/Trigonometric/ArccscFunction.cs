namespace YAMP
{
    [Description("ArccscFunctionDescription")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("ArccscFunctionLink")]
    sealed class ArccscFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue z)
        {
            return z.Arccsc();
        }
    }
}
