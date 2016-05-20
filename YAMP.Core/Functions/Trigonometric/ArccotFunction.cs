namespace YAMP
{
    [Description("ArccotFunctionDescription")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("ArccotFunctionLink")]
    sealed class ArccotFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue z)
        {
            return z.Arccot();
        }
    }
}
