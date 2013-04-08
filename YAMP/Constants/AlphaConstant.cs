using System;

namespace YAMP
{
	/// <summary>
	/// Gets the value of alpha.
	/// </summary>
	[Description("The Feigenbaum constant alpha is the ratio between the width of a tine and the width of one of its two subtines (except the tine closest to the fold).")]
	[Kind(PopularKinds.Constant)]
    [Link("http://en.wikipedia.org/wiki/Feigenbaum_constant")]
	class AlphaConstant : BaseConstant
	{
        static readonly ScalarValue alpha = new ScalarValue(2.50290787509589282228390287321821578);

		public override Value Value
		{
			get { return alpha; }
		}
	}
}
