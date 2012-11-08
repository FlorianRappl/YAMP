using System;

namespace YAMP
{
    [Description("Represents the gamma function, which is taken as the faculty approximation for non-integers.")]
    class GammaFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            if (value.ImaginaryValue == 0.0 && value.Value == Math.Floor(value.Value))
                return (value - 1).Factorial();
            else if (value.ImaginaryValue == Math.Floor(value.ImaginaryValue) && value.Value == 0.0)
                return (value - ScalarValue.I).Factorial();

            return Math.Sqrt(2 * Math.PI) * (value.Power(value - 0.5) as ScalarValue) * (-value).Exp();
        }
    }
}
