using System;

namespace YAMP.Physics
{
    /// <summary>
    /// Volts (derived SI unit) -- unit of voltage
    /// </summary>
    class VUnit : CombinedUnit
    {
        public VUnit() : base("kg * m^2 / (A * s^3)", 1.0)
        {
        }
    }
}
