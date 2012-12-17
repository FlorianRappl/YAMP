using System;

namespace YAMP.Physics
{
    /// <summary>
    /// Coloumb (SI unit) -- unit for the charge
    /// </summary>
    class CUnit : CombinedUnit
    {
        public CUnit() : base("A * s", 1.0)
        {
        }
    }

    /// <summary>
    /// Speed of light (natural unit) -- unit for the velocity
    /// </summary>
    class cUnit : CombinedUnit
    {
        public cUnit() : base("m / s", 299792458.0)
        {
        }
    }
}