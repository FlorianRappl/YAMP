using System;

namespace YAMP
{
	[Description("The standard cosh(x) function.")]
	[Kind(PopularKinds.Function)]
    class CoshFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return (value.Exp() + (-value).Exp()) / 2.0;
        }
    }
}
