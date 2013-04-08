using System;

namespace YAMP
{
	/// <summary>
	/// Gets Catalan's constant.
	/// </summary>
	[Description("In mathematics, Catalan's constant G, which occasionally appears in estimates in combinatorics, is defined by G = beta(2), where beta is the Dirichlet beta function.")]
    [Kind(PopularKinds.Constant)]
    [Link("http://en.wikipedia.org/wiki/Catalan_constant")]
	class CatalanConstant : BaseConstant
	{
        static readonly ScalarValue beta = new ScalarValue(0.915965594177219015054604);

		public override Value Value
		{
			get { return beta; }
		}
	}
}
