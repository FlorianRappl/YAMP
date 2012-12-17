using System;
using YAMP;

namespace YAMP.Physics
{
    /// <summary>
    /// Tesla (derived SI unit) -- unit for the magnetic flux density
    /// </summary>
    class TUnit : CombinedUnit
    {
        public TUnit() : base("kg / (A * s^2)", 1.0)
        {
        }
    }
}
