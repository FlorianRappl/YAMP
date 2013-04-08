using System;

namespace YAMP
{
	/// <summary>
	/// Gets the value of delta.
	/// </summary>
	[Description("The Feigenbaum constant delta is the limiting ratio of each bifurcation interval to the next between every period doubling, of a one-parameter map.")]
    [Kind(PopularKinds.Constant)]
    [Link("http://en.wikipedia.org/wiki/Feigenbaum_constant")]
	class DeltaConstant : BaseConstant
	{
        static readonly ScalarValue delta = new ScalarValue(4.66920160910299067185320382046620161);

		public override Value Value
		{
			get { return delta; }
		}
	}
}
