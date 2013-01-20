using System;

namespace YAMP
{
	/// <summary>
	/// Gets the value of euler's number.
	/// </summary>
	[Description("The number e is an important mathematical constant, approximately equal to 2.71828, that is the base of the natural logarithm.")]
    [Kind(PopularKinds.Constant)]
    [Link("http://en.wikipedia.org/wiki/E_(mathematical_constant)")]
	class EConstant : BaseConstant
	{
		static readonly ScalarValue e = new ScalarValue(Math.E);

		public override Value Value
		{
			get { return e; }
		}
	}
}
