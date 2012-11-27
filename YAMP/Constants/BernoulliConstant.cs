using System;

namespace YAMP
{
	/// <summary>
	/// Gets the first values of the bernoulli series.
	/// </summary>
	[Description("In mathematics, the Bernoulli numbers Bn are a sequence of rational numbers with deep connections to number theory. The first 9 numbers are given in this vector. The Bernoulli numbers appear in the Taylor series expansions of the tangent and hyperbolic tangent functions, in formulas for the sum of powers of the first positive integers, in the Euler–Maclaurin formula, and in expressions for certain values of the Riemann zeta function.")]
	[Kind(PopularKinds.Constant)]
	class BernoulliConstant : BaseConstant
	{
		public override Value Value
		{
			get { return new MatrixValue(Bernoulli); }
		}

		internal static readonly double[] Bernoulli = new double[] {
            1.0, 1.0 / 6.0, -1.0 / 30.0, 1.0 / 42.0, -1.0 / 30.0, 5.0 / 66.0, -691.0 / 2730.0, 7.0 / 6.0, -3617.0 / 510.0
        };
	}
}
