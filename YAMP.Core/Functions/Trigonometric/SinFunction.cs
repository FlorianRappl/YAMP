using System;

namespace YAMP
{
	[Description("The standard sin(x) function. This is the opposite over the hypotenuse side.")]
    [Kind(PopularKinds.Trigonometric)]
    [Link("http://en.wikipedia.org/wiki/Sine")]
	sealed class SinFunction : StandardFunction
	{
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Sin();
        }
	}
}

