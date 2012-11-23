using System;

namespace YAMP
{
	[Description("Computes the greatest common divisor of the given elements.")]
	[Kind(PopularKinds.Function)]
	class GcdFunction : ArgumentFunction
	{
		[Description("Given a matrix of integers the greatest common divisor is computed.")]
		[Example("gcd([54, 24])", "Evaluates the list [ 54, 24 ] and returns the GCD , which is 6.")]
		[Example("gcd([54, 24, 42])", "Evaluates the list [ 54, 24, 42 ] and returns the GCD , which is 6.")]
		public ScalarValue Function(MatrixValue values)
		{
			if(values.Length == 0)
				return new ScalarValue();

			int gcd = values[1].IntValue;

			for (var i = 2; i <= values.Length; i++)
				gcd = GCD(gcd, values[i].IntValue);

			return new ScalarValue(gcd);
		}

		static int GCD(int A, int B)
		{
			if (A == B)
				return A;
			
			if (A == 1 || B == 1)
				return 1;
			
			if ((A % 2 == 0) && (B % 2 == 0))
				return 2 * GCD(A / 2, B / 2);
			else if ((A % 2 == 0) && (B % 2 != 0))
				return GCD(A / 2, B);
			else if ((A % 2 != 0) && (B % 2 == 0))
				return GCD(A, B / 2);
			
			if (A > B)
				return GCD((A - B) / 2, B);

			return GCD(A, (B - A) / 2);
		}  
	}
}
