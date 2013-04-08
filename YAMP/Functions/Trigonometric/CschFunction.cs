using System;

namespace YAMP
{
    [Description("The standard csch(x) function. This is the hyperbolic cosecant.")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Hyperbolic_function")]
    sealed class CschFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return 2.0 / (value.Exp() - (-value).Exp());
        }
    }
}
