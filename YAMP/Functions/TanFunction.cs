using System;

namespace YAMP
{
    class TanFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Sin() / value.Cos();
        }
    }
}
