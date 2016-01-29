using System;

namespace YAMP
{
	[Description("The inverse of the tanh(x) function, which is sinh(x) / cosh(x).")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Inverse_hyperbolic_function")]
    sealed class ArtanhFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return 0.5 * ((1.0 + value) / (1.0 - value)).Ln();
        }
    }
}
