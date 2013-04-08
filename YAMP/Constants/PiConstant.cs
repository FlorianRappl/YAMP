using System;

namespace YAMP
{
	/// <summary>
	/// Gets the value of Pi.
	/// </summary>
	[Description("The mathematical constant Pi is the ratio of a circle's circumference to its diameter.")]
    [Kind(PopularKinds.Constant)]
    [Link("http://en.wikipedia.org/wiki/Pi")]
	class PiConstant : BaseConstant
	{
        static readonly ScalarValue pi = new ScalarValue(Math.PI);

		public override Value Value
		{
			get { return pi; }
		}
	}
}
