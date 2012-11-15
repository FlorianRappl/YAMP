using System;

namespace YAMP
{
	[Description("The inverse of the cos(x) function.")]
	[Kind(PopularKinds.Function)]
    class ArccosFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            if (value.ImaginaryValue == 0.0)
                return arccos(value.Value);

            return (-ScalarValue.I) * (value + (value * value - 1.0).Sqrt()).Ln();
        }

        ScalarValue arccos(double value)
        {
            return new ScalarValue(Math.Acos(value));
        }
    }
}
