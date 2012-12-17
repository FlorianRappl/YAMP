using System;
using YAMP;

namespace YAMP.Physics
{
	[Description("The Planck constant (denoted h, also called Planck's constant) is a physical constant that is the quantum of action in quantum mechanics. The Planck constant was first described as the proportionality constant between the energy (E) of a photon and the frequency (ν) of its associated electromagnetic wave. The value is given in J * s.")]
	[Kind(PopularKinds.Constant)]
	class HConstant : BaseConstant
	{
		static readonly UnitValue planck = new UnitValue(6.6260695729e-34, "J * s");

		public override Value Value
		{
			get { return planck; }
		}
	}
}
