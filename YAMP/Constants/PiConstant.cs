using System;

namespace YAMP
{
	/// <summary>
	/// Gets the value of Pi.
	/// </summary>
	[Description("The mathematical constant Pi is the ratio of a circle's circumference to its diameter.")]
	[Kind(PopularKinds.Constant)]
	class PiConstant : BaseConstant
	{
		static readonly ScalarValue pi = new ScalarValue(Math.PI);

		public override Value Value
		{
			get { return pi; }
		}
	}
}
