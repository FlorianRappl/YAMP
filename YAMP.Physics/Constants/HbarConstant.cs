using System;
using YAMP;

namespace YAMP.Physics
{
	[Description("The Planck constant (denoted h, also called Planck's constant) is a physical constant that is the quantum of action in quantum mechanics. In applications where frequency is expressed in terms of radians per second instead of cycles per second, it is often useful to absorb a factor of 2π into the Planck constant. The resulting constant is called the reduced Planck constant or Dirac constant. It is equal to the Planck constant divided by 2π, and is denoted ħ. The value is given in J * s.")]
	[Kind(PopularKinds.Constant)]
	class HbarConstant : BaseConstant
	{
		static readonly UnitValue planck = new UnitValue(1.05457172647e-34, "J * s");

		public override Value Value
		{
			get { return planck; }
		}
	}
}
