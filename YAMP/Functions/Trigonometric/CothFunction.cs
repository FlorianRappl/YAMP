using System;

namespace YAMP
{
    [Description("The standard coth(x) function, which is cosh(x) / sinh(x). This is the hyperbolic cotangent.")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Hyperbolic_function")]
    sealed class CothFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            var a = value.Exp();
            var b = (-value).Exp();
            return (a + b) / (a - b);
        }
    }
}
