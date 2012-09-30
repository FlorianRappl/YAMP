using System;

namespace YAMP
{
    [Description("Represents the round function to round up or down to the nearest integer.")]
    class RoundFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            var re = Math.Round(value.Value);
            var im = Math.Round(value.ImaginaryValue);
            return new ScalarValue(re, im);
        }
    }
}
