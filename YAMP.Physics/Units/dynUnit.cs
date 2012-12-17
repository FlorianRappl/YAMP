using System;
using YAMP;

namespace YAMP.Physics
{
    /// <summary>
    /// One dyne (CGS unit) -- unit of force
    /// </summary>
    class dynUnit : CombinedUnit
    {
        public dynUnit() : base("kg * m / s^2", 1e-5)
        {
        }
    }
}
