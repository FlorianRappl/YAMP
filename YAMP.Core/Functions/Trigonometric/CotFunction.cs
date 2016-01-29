using System;

namespace YAMP
{
	[Description("The standard cot(x) function, which is cos(x) / sin(x). This is the adjacent over the opposite side.")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Trigonometric_functions")]
    sealed class CotFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Cos() / value.Sin();
        }
    }
}
