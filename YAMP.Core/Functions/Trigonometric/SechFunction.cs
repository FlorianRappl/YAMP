using System;

namespace YAMP
{
    [Description("The standard sech(x) function. This is the hyperbolic secant.")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Hyperbolic_function")]
    sealed class SechFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return 2.0 / (value.Exp() + (-value).Exp());
        }
    }
}
