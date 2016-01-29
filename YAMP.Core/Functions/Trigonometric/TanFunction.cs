using System;

namespace YAMP
{
	[Description("The standard tan(x) function, which is sin(x) / cos(x). This is the opposite over the adjacent side.")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Trigonometric_functions")]
    sealed class TanFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Sin() / value.Cos();
        }
    }
}
