using System;
using YAMP;

namespace YAMP.Physics
{
	[Description("The speed of light in vacuum, commonly denoted c, is a universal physical constant important in many areas of physics. Its value is 299,792,458 metres per second, a figure that is exact because the length of the metre is defined from this constant and the international standard for time.[1] In imperial units this speed is approximately 186,282 miles per second. According to special relativity, c is the maximum speed at which all energy, matter, and information in the universe can travel. Its value is given in m / s.")]
	[Kind(PopularKinds.Constant)]
	class CConstant : BaseConstant
	{
		static readonly UnitValue c = new UnitValue(299792458, "m / s");

		public override Value Value
		{
			get { return c; }
		}
	}
}
