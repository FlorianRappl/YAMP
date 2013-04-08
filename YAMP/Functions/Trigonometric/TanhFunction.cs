using System;

namespace YAMP
{
    [Description("The standard tanh(x) function, which is sinh(x) / cosh(x). This is the hyperbolic tangent.")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Hyperbolic_function")]
    sealed class TanhFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            var a = value.Exp();
            var b = (-value).Exp();
            return (a - b) / (a + b);
        }
    }
}
