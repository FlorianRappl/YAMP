using System;

namespace YAMP
{
	[Description("The standard sec(x) function. This is one over the cosine or the hypotenuse over the adjacent side.")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Trigonometric_functions")]
	sealed class SecFunction : StandardFunction
	{
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return 1.0 / value.Cos();
        }
	}
}

