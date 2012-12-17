using System;
using YAMP;

namespace YAMP.Physics
{
    /// <summary>
    /// Henry (derived SI unit) -- unit of inductance
    /// </summary>
    class HUnit : CombinedUnit
    {
        public HUnit() : base("m^2 * kg / (s^2 * A^2)", 1.0)
        {
        }
    }
}
