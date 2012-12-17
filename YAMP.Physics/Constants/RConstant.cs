using System;
using YAMP;

namespace YAMP.Physics
{
	[Description("The gas constant (also known as the molar, universal, or ideal gas constant, denoted by the symbol R or R) is a physical constant which is featured in many fundamental equations in the physical sciences, such as the ideal gas law and the Nernst equation. The value is given in J / K / mol")]
	[Kind(PopularKinds.Constant)]
	class RConstant : BaseConstant
	{
		readonly static UnitValue value = new UnitValue(8.314462175, "J / (K * mol)");

		public override Value Value
		{
			get { return value; }
		}
	}
}
