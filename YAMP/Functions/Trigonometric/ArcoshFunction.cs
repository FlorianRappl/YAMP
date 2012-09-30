using System;

namespace YAMP
{
    [Description("The inverse of the cosh(x) function.")]
    class ArcoshFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return (value + ((value * value) - 1.0).Sqrt()).Ln();
        }
    }
}
