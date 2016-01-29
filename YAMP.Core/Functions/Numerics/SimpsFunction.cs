using System;
using YAMP.Numerics;

namespace YAMP
{
	[Description("Integrates a given vector of values with Simpson's rule (perfect for third order polynomials, i.e. cubic functions).")]
	[Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/Simpson's_rule")]
    sealed class SimpsFunction : ArgumentFunction
    {
        [Description("Computes the integral for a given range of y values with its x vector.")]
        [Example("simps(sin(0:0.1:Pi), 0:0.1:Pi)", "Computes the value of the sinus function between 0 and pi (analytic result is 2).")]
        public ScalarValue Function(MatrixValue y, MatrixValue x)
        {
            var integral = new SimpsonIntegrator(y);
            return integral.Integrate(x);
        }
    }
}
