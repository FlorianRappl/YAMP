using System;

namespace YAMP
{
	/// <summary>
	/// Gets the value of gamma(1).
	/// </summary>
	[Description("The Euler–Mascheroni constant (also called Euler's constant) is a mathematical constant recurring in analysis and number theory.")]
    [Kind(PopularKinds.Constant)]
    [Link("http://en.wikipedia.org/wiki/Euler–Mascheroni_constant")]
	class Gamma1Constant : BaseConstant
	{
        static readonly ScalarValue gamma = new ScalarValue(0.57721566490153286060651209008240243);

		public override Value Value
		{
			get { return gamma; }
		}
	}
}
