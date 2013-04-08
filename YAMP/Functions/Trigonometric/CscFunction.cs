using System;

namespace YAMP
{
	[Description("The standard csc(x) function. This is 1.0 over the sine or the hypotenuse over the opposite side.")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Trigonometric_functions")]
	sealed class CscFunction : StandardFunction
	{
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return 1.0 / value.Sin();
        }
	}
}

