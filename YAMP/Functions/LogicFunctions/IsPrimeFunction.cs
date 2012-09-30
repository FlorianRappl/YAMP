using System;

namespace YAMP
{
    [Description("Returns a boolean matrix to state if the given numbers are prime integers.")]
    class IsPrimeFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.IsPrime();
        }
    }
}
