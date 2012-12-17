using System;
using YAMP;

namespace YAMP.Physics
{
    /// <summary>
    /// Joule (derived SI unit) -- unit of energy
    /// </summary>
    class JUnit : CombinedUnit
    {
        public JUnit() : base("kg * m^2 / s^2", 1.0)
        {
        }
    }
}
