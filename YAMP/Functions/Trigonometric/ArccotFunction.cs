using System;

namespace YAMP
{
	[Description("The inverse of the cot(x) function, which is cos(x) / sin(x).")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Inverse_trigonometric_function")]
    sealed class ArccotFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue z)
        {
            return z.Arccot();
        }
    }
}
