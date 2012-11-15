using System;

namespace YAMP
{
	[Description("The standard cot(x) function, which is cos(x) / sin(x).")]
	[Kind(PopularKinds.Function)]
    class CotFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Cos() / value.Sin();
        }
    }
}
