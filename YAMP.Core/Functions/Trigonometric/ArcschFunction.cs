using System;

namespace YAMP
{
	[Description("The inverse of the csch(x) function.")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Inverse_hyperbolic_function")]
    sealed class ArcschFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue z)
        {
            return (1.0 / z + (1.0 / z.Square() + 1.0).Sqrt()).Ln();
        }
    }
}
