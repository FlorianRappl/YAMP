using System;

namespace YAMP
{
	[Description("The inverse of the sech(x) function.")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Inverse_hyperbolic_function")]
    sealed class ArsechFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue z)
        {
            var zi = 1.0 / z;
            return (zi + (zi + 1.0).Sqrt() * (zi - 1.0).Sqrt()).Ln();
        }
    }
}
