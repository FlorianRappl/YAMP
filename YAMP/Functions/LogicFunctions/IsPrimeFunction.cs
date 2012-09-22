using System;

namespace YAMP
{
    class IsPrimeFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.IsPrime();
        }
    }
}
