namespace YAMP
{
    [Description("ArcsecFunctionDescription")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("ArcsecFunctionLink")]
    sealed class ArcsecFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue z)
        {
            return z.Arcsec();
        }
    }
}
