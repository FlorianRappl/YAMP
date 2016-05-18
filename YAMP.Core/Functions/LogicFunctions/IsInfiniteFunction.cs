namespace YAMP
{
    using System;

    [Description("IsInfiniteFunctionDescription")]
    [Kind(PopularKinds.Logic)]
    [Link("IsInfiniteFunctionLink")]
    sealed class IsInfiniteFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return new ScalarValue(Double.IsInfinity(value.Re) || Double.IsInfinity(value.Im));
        }
    }
}
