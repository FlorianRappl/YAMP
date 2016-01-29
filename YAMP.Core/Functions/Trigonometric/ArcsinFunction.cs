using System;

namespace YAMP
{
	[Description("The inverse of the sin(x) function.")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Inverse_trigonometric_function")]
    sealed class ArcsinFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue z)
        {
            return z.Arcsin();
        }
    }
}
