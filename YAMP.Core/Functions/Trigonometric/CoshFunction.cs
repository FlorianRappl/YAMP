using System;

namespace YAMP
{
    [Description("The standard cosh(x) function. This is the hyperbolic cosine.")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Hyperbolic_function")]
    sealed class CoshFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return (value.Exp() + (-value).Exp()) / 2.0;
        }
    }
}
