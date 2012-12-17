using System;
using YAMP;

namespace YAMP.Physics
{
    /// <summary>
    /// Weber (derived SI unit) -- unit of the magnetic flux
    /// </summary>
    class WbUnit : CombinedUnit
    {
        public WbUnit() : base("kg * m^2 / (A * s^2)", 1.0)
        {
        }
    }
}
