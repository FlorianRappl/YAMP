using System;

namespace YAMP
{
    [Description("Returns a boolean matrix to state if the given values have imaginary parts.")]
    class IsComplexFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return new ScalarValue(value.IsComplex);
        }
    }
}
