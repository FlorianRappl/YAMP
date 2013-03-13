using System;
using YAMP;

namespace YAMP.Physics
{
	[Description("The gravitational constant denoted by letter G, is an empirical physical constant involved in the calculation(s) of gravitational force between two bodies. It usually appears in Sir Isaac Newton's law of universal gravitation, and in Albert Einstein's theory of general relativity. It is also known as the universal gravitational constant, Newton's constant, and colloquially as Big G. Its value is given in m^3 / kg / s^2.")]
	[Kind(PopularKinds.Constant)]
	class GConstant : BaseConstant
	{
		static readonly UnitValue g = new UnitValue(6.6738480e-11, "m^3 / (kg * s^2)");

		public override Value Value
		{
			get { return g; }
		}
	}
}
