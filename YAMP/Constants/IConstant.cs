using System;

namespace YAMP
{
	/// <summary>
	/// Gets the value of the imaginary constant.
	/// </summary>
	[Description("In mathematics, the imaginary unit or unit imaginary number allows the real number system to be extended to the complex number system.")]
    [Kind(PopularKinds.Constant)]
    [Link("http://en.wikipedia.org/wiki/Imaginary_unit")]
	class IConstant : BaseConstant
	{
		static readonly ScalarValue i = ScalarValue.I;

		public override Value Value
		{
			get { return i; }
		}
	}
}
