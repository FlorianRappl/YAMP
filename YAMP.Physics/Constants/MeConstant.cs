using System;
using YAMP;

namespace YAMP.Physics
{
	[Description("The electron rest mass (symbol: me) is the mass of a stationary electron. It is one of the fundamental constants of physics, and is also very important in chemistry because of its relation to the Avogadro constant. Its value is given in kg.")]
	[Kind(PopularKinds.Constant)]
	class MeConstant : BaseConstant
	{
		readonly static UnitValue value = new UnitValue(9.1093821545e-31, "kg");

		public override Value Value
		{
			get { return value; }
		}
	}
}
