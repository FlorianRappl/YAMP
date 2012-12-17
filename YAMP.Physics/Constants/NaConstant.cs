using System;
using YAMP;

namespace YAMP.Physics
{
	[Description("In chemistry and physics, the Avogadro constant (symbols: L, NA) is defined as the number of constituent particles (usually atoms or molecules) in one mole of a given substance. It is a dimensionless number and has the value 6.02214129(27) x 10^23 / mol.")]
	[Kind(PopularKinds.Constant)]
	class NaConstant : BaseConstant
	{
		static readonly UnitValue na = new UnitValue(6.0221412927e23, "1 / mol");

		public override Value Value
		{
			get { return na; }
		}
	}
}
