using System;
using YAMP;

namespace YAMP.Physics
{
    [Description("In mathematics and engineering, the sinc function, denoted by sinc(x), has two slightly different definitions. In mathematics, the historical unnormalized sinc function is defined by sinc(x) = sin(x) / x. Here we use the other definition, given by sinc(x) = sin(pi * x) / (pi * x).")]
    [Kind(PopularKinds.Function)]
    class SincFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            if (value.Re == 0.0 && value.Im == 0.0)
                return ScalarValue.One;

            var arg = value * Math.PI;
            return arg.Sin() / arg;
        }
    }
}
