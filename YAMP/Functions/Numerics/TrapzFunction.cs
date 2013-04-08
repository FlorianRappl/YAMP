using System;
using YAMP.Numerics;

namespace YAMP
{
	[Description("Integrates a given vector of values with the Trapezoidal rule (perfect for first order polynomials, i.e. linear functions).")]
	[Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/Trapezoidal_rule")]
    sealed class TrapzFunction : ArgumentFunction
    {
        [Description("Computes the integral for a given range of y values with its x vector.")]
        [Example("trapz(sin(0:0.1:Pi), 0:0.1:Pi)", "Computes the value of the sinus function between 0 and pi (analytic result is 2).")]
        public ScalarValue Function(MatrixValue y, MatrixValue x)
        {
            var integral = new TrapezIntegrator(y);
            return integral.Integrate(x);
        }
    }
}
