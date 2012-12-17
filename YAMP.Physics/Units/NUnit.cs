using System;
using YAMP;

namespace YAMP.Physics
{
    /// <summary>
    /// Newton (derived SI unit) -- unit of force
    /// </summary>
    class NUnit : CombinedUnit
    {
        public NUnit() : base("kg * m / s^2", 1.0)
        {
        }
    }
}
