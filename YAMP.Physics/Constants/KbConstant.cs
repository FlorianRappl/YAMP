using System;
using YAMP;

namespace YAMP.Physics
{
	[Description("The Boltzmann constant (k or kB) is a physical constant relating energy at the individual particle level with temperature. It is the gas constant R divided by the Avogadro constant NA. The value is given in J / K.")]
	[Kind(PopularKinds.Constant)]
	class KbConstant : BaseConstant
	{
		static readonly UnitValue kb = new UnitValue(1.380648813e-23, "J / K");

		public override Value Value
		{
			get { return kb; }
		}
	}
}
