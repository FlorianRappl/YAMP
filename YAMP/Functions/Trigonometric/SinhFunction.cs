using System;

namespace YAMP
{
	[Description("The standard sinh(x) function.")]
	[Kind(PopularKinds.Function)]
    class SinhFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return (value.Exp() - (-value).Exp()) / 2.0;
        }
    }
}
