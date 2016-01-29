using System;
using YAMP.Numerics;

namespace YAMP
{
    [Description("The Riemann zeta function or Euler–Riemann zeta function, ζ(s), is a function of a complex variable s that analytically continues the sum of the infinite series with n = 1 to infinity, that sums 1 over n to the power of s. It converges when the real part of s is greater than 1.")]
    [Kind(PopularKinds.Function)]
    sealed class ZetaFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return Zeta.RiemannZeta(value);
        }
    }
}
