using System;

namespace YAMP
{
	[Description("The inverse of the cot(x) function, which is cos(x) / sin(x).")]
	[Kind(PopularKinds.Function)]
    class ArccotFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            if (value.ImaginaryValue == 0.0)
                return new ScalarValue(Math.PI / 2.0 - Math.Atan(value.Value), 0.0);

            var iv = ScalarValue.I * value;
            return (ScalarValue.I / new ScalarValue(0.0, -2.0)) * ((iv + 1.0) / (iv - 1.0)).Ln();
        }
    }
}
