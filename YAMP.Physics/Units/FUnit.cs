using System;
using YAMP;

namespace YAMP.Physics
{
    /// <summary>
    /// Farad (derived SI unit) -- unit of the capacity
    /// </summary>
    class FUnit : CombinedUnit
    {
        public FUnit() : base("s^4 * A^2 / (m^2 * kg)", 1)
        {
        }
    }
}
