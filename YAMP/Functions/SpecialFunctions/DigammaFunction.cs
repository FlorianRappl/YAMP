using System;
using YAMP.Numerics;

namespace YAMP
{
    [Description("In mathematics, the digamma function is defined as the logarithmic derivative of the gamma function. The digamma function, often denoted also as ψ0(x), ψ0(x) or  (after the shape of the archaic Greek letter Ϝ digamma), is related to the harmonic numbers.")]
    [Kind(PopularKinds.Function)]
    sealed class DigammaFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return new ScalarValue(Gamma.Psi(value.Re));
        }
    }
}
