using System;

namespace YAMP
{
    [Description("This is decimal logarithm, i.e. the logarithm with base 10.")]
    [Kind(PopularKinds.Function)]
    sealed class Log10Function : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Log10();
        }
    }
}
