using System;

namespace YAMP
{
	[Description("The inverse of the tan(x) function, which is sin(x) / cos(x).")]
	[Kind(PopularKinds.Function)]
    class ArctanFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            if (value.ImaginaryValue == 0.0)
                return new ScalarValue(Math.Atan(value.Value), 0.0);

            var iv = ScalarValue.I * value;
            return 0.5 * ScalarValue.I * ((1.0 - iv) / (1.0 + iv)).Ln();
        }
    }
}
