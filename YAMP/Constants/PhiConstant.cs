using System;

namespace YAMP
{
	/// <summary>
	/// Gets the value of the golden ratio.
	/// </summary>
	[Description("The golden ratio: two quantities are in the golden ratio if the ratio of the sum of the quantities to the larger quantity is equal to the ratio of the larger quantity to the smaller one.")]
    [Kind(PopularKinds.Constant)]
    [Link("http://en.wikipedia.org/wiki/Golden_ratio")]
	class PhiConstant : BaseConstant
	{
        static readonly ScalarValue phi = new ScalarValue(1.61803398874989484820458683436563811);

		public override Value Value
		{
			get { return phi; }
		}
	}
}
