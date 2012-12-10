using System;
using YAMP;

namespace YAMP.Physics
{
	[Description("The elementary charge, usually denoted as e or q, is the electric charge carried by a single proton, or equivalently, the negation (opposite) of the electric charge carried by a single electron.[2] This elementary charge is a fundamental physical constant. To avoid confusion over its sign, e is sometimes called the elementary positive charge. The value is given in Coulombs.")]
	[Kind(PopularKinds.Constant)]
	class QConstant : BaseConstant
	{
		static readonly UnitValue charge = new UnitValue(1.60217656535e-19, "C");

		public override Value Value
		{
			get { return charge; }
		}
	}
}
