using System;

namespace YAMP
{
    [Description("Returns a boolean matrix to state if the given numbers are infinite.")]
    [Kind(PopularKinds.Logic)]
    [Link("http://en.wikipedia.org/wiki/Infinity")]
    sealed class IsInfiniteFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return new ScalarValue(double.IsInfinity(value.Re) || double.IsInfinity(value.Im));
        }
    }
}
