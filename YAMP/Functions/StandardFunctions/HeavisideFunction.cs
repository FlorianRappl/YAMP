using System;

namespace YAMP
{
    class HeavisideFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return new ScalarValue(value.Value > 0);
        }
    }
}
