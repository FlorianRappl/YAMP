using System;

namespace YAMP
{
    [Description("This is binary logarithm, i.e. the logarithm with base 2.")]
    [Kind(PopularKinds.Function)]
    sealed class Log2Function : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Log(2.0);
        }
    }
}
