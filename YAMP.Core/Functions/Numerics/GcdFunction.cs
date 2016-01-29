using System;
using YAMP.Numerics;

namespace YAMP
{
	[Description("Computes the greatest common divisor of the given elements.")]
	[Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/Greatest_common_divisor")]
    sealed class GcdFunction : ArgumentFunction
	{
		[Description("Given a matrix of integers the greatest common divisor is computed.")]
		[Example("gcd([54, 24])", "Evaluates the list [ 54, 24 ] and returns the GCD , which is 6.")]
		[Example("gcd([54, 24, 42])", "Evaluates the list [ 54, 24, 42 ] and returns the GCD , which is 6.")]
        public ScalarValue Function(MatrixValue values)
		{
			if(values.Length == 0)
                return new ScalarValue();

			int gcd = values[1].GetIntegerOrThrowException("values", Name);

			for (var i = 2; i <= values.Length; i++)
				gcd = Helpers.GCD(gcd, values[i].GetIntegerOrThrowException("values", Name));

            return new ScalarValue(gcd);
		}
	}
}
