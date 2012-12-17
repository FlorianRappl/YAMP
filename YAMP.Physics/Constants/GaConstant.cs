using System;
using YAMP;

namespace YAMP.Physics
{
	[Description("In physics, gravitational acceleration is the acceleration on an object caused by gravity. Neglecting friction such as air resistance, all small bodies accelerate in a gravitational field at the same rate relative to the center of mass. The value is given in m / s / s.")]
	[Kind(PopularKinds.Constant)]
	class GaConstant : BaseConstant
	{
		readonly static UnitValue value = new UnitValue(9.80665, "m / s^2");

		public override Value Value
		{
			get { return value; }
		}
	}
}
