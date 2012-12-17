using System;
using YAMP;

namespace YAMP.Physics
{
    /// <summary>
    /// Watts (derived SI unit) -- unit of power
    /// </summary>
    class WUnit : CombinedUnit
    {
        public WUnit() : base("kg * m^2 / s^3", 1.0)
        {
        }
    }
}
