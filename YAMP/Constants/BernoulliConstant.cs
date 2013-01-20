using System;
using YAMP.Numerics;

namespace YAMP
{
	/// <summary>
	/// Gets the first values of the bernoulli series.
	/// </summary>
	[Description("In mathematics, the Bernoulli numbers Bn are a sequence of rational numbers with deep connections to number theory. The first 21 numbers are given in this vector. The Bernoulli numbers appear in the Taylor series expansions of the tangent and hyperbolic tangent functions, in formulas for the sum of powers of the first positive integers, in the Euler–Maclaurin formula, and in expressions for certain values of the Riemann zeta function.")]
    [Kind(PopularKinds.Constant)]
    [Link("http://en.wikipedia.org/wiki/Bernoulli_number")]
	class BernoulliConstant : BaseConstant
	{
		public override Value Value
		{
            get { return new MatrixValue(Helpers.BernoulliNumbers); }
		}
	}
}
