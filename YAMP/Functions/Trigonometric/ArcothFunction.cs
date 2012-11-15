using System;

namespace YAMP
{
	[Description("The inverse of the coth(x) function, which is cosh(x) / sinh(x).")]
	[Kind(PopularKinds.Function)]
    class ArcothFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return 0.5 * ((1.0 + value) / (value - 1.0)).Ln();
        }
    }
}
