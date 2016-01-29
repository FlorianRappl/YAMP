using System;

namespace YAMP
{
	[Description("The inverse of the tan(x) function, which is sin(x) / cos(x).")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Inverse_trigonometric_function")]
    sealed class ArctanFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue z)
        {
            return z.Arctan();
        }
    }
}
