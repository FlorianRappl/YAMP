using System;

namespace YAMP
{
	[Description("The standard tan(x) function, which is sin(x) / cos(x).")]
	[Kind(PopularKinds.Function)]
    class TanFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Sin() / value.Cos();
        }
    }
}
