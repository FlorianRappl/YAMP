using System;

namespace YAMP
{
	[Description("The inverse of the sin(x) function.")]
	[Kind(PopularKinds.Function)]
    class ArcsinFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            if (value.ImaginaryValue == 0.0)
                return arcsin(value.Value);

            return (-ScalarValue.I) * (ScalarValue.I * value + (1.0 - (value * value)).Sqrt()).Ln();
        }

        ScalarValue arcsin(double value)
        {
            return new ScalarValue(Math.Asin(value));
        }
    }
}
