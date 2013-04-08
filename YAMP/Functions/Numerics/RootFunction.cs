using System;
using YAMP.Numerics.Optimization;

namespace YAMP
{
	[Description("A root-finding algorithm is a numerical method, or algorithm, for finding a value x such that f(x) = 0, for a given function f. Such an x is called a root of the function f.")]
	[Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/Zero_of_a_function")]
    sealed class RootFunction : SystemFunction
	{
		[Description("This function calls finds the root of f(x) that is the closest to the given value of x. The output is a value x0, which has the property that f(x0) = 0. There might be more roots depending on the starting value of x.")]
		[Example("root(x => x^2+x, -2)", "Returns the value of -1, which has been found as a root of the function f(x) = x^2 + x. The starting value of x has been chosen to -2.")]
		[Example("root(x => x^2+x, 1)", "Returns the value of 0, which has been found as a root of the function f(x) = x^2 + x. The starting value of x has been chosen to 0.")]
		public ScalarValue Function(FunctionValue f, ScalarValue x)
		{
			Func<double, double> lambda = t =>
			{
                var sv = f.Perform(Context, new ScalarValue(t));

				if (!(sv is ScalarValue))
					throw new YAMPArgumentInvalidException(Name, sv.Header, 1);

				return ((ScalarValue)sv).Re;
			};

			var newton = new NewtonMethod(lambda, x.Re, 0.00001);
            return new ScalarValue(newton.Result[0, 0]);
		}
	}
}
