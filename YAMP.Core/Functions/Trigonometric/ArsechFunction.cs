namespace YAMP
{
    [Description("ArsechFunctionDescription")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("ArsechFunctionLink")]
    sealed class ArsechFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue z)
        {
            var zi = 1.0 / z;
            return (zi + (zi + 1.0).Sqrt() * (zi - 1.0).Sqrt()).Ln();
        }
    }
}
