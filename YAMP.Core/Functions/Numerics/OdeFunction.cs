using System;
using YAMP.Numerics;

namespace YAMP
{
	[Description("Solves one dimensional ordinary differential equations in the form x'(t) = f(t, x(t)).")]
	[Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/Ordinary_differential_equation")]
    sealed class OdeFunction : SystemFunction
	{
		[Description("Searches for a solution of the differential equation x'(t) = f(t, x) for a given a lambda expression f with two arguments t and x within the range specified as a vector and the starting value of x(t) at t(0) (the first value for t).")]
		[Example("ode((t, x) => -x, 0:0.01:2, 1)", "Solves the DEQ x'(t) + x(t) = 0 and gets the solution vector, which is exp(-t) within the specified point range.")]
		[Example("ode((t, x) => x - t, 0:0.01:5, 1.5)", "Solves the DEQ x'(t) = x(t) - t and gets the solution vector, which is 1 / 2 * exp(t) + t + 1 within the specified point range.")]
		public MatrixValue Function(FunctionValue deq, MatrixValue points, ScalarValue x0)
		{
			Func<double, double, double> lambda = (t, x) => 
			{
                var av = new ArgumentsValue(new ScalarValue(t), new ScalarValue(x));
				var sv = deq.Perform(Context, av);

                if (!(sv is ScalarValue))
                    throw new YAMPArgumentInvalidException(Name, sv.Header, 1);

				return ((ScalarValue)sv).Re;
			};

			var euler = new RungeKutta(lambda, points[1].Re, points[points.Length].Re, x0.Re, points.Length - 1);
			return new MatrixValue(euler.Result);
		}
	}
}
