using System;
using YAMP;

namespace YAMP.Physics
{
    /// <summary>
    /// Grams (CGS unit and with k prefix SI unit) -- unit of mass
    /// </summary>
    class gUnit : PhysicalUnit
    {
        public gUnit()
        {
            Add("lb", 0.002205);
        }

        protected override PhysicalUnit Create()
        {
            return new gUnit();
        }
    }

    /// <summary>
    /// Gauss (derived CGS unit) -- unit of the magnetic field
    /// </summary>
    class GUnit : CombinedUnit
    {
        public GUnit() : base("kg / (A * s^2)", 1e-4)
        {
        }
    }
}
