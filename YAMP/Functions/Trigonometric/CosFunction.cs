using System;

namespace YAMP
{
	[Description("The standard cos(x) function. This is the adjacent over the hypotenuse side.")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Trigonometric_functions")]
	sealed class CosFunction : StandardFunction
	{
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Cos();
        }
	}
}

