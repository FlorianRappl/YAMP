using System;
using YAMP;

namespace YAMP.Physics
{
    /// <summary>
    /// Ohms (derived SI unit) -- unit of electrical resistance
    /// </summary>
    class ΩUnit : CombinedUnit
    {
        public ΩUnit() : base("kg * m^2 / (s^3 * A^2)", 1.0)
        {
        }
    }

    /// <summary>
    /// Different spelling for the same unit -- R as Resistance
    /// </summary>
    class RUnit : ΩUnit
    {
    }
}
