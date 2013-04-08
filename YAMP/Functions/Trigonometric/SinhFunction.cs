using System;

namespace YAMP
{
    [Description("The standard sinh(x) function. This is the hyperbolic sine.")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Hyperbolic_function")]
    sealed class SinhFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return (value.Exp() - (-value).Exp()) / 2.0;
        }
    }
}
