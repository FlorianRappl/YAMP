using System;

namespace YAMP
{
    class ArsinhFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return (value + ((value * value) + 1.0).Sqrt()).Ln();
        }
    }
}
