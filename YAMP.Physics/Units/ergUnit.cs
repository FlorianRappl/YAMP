using System;
using YAMP;

namespace YAMP.Physics
{
    /// <summary>
    /// Unit of energy (CGS unit) -- used for energy
    /// </summary>
    class ergUnit : CombinedUnit
    {
        public ergUnit() : base("kg * m^2 / s^2", 1e-7)
        {
        }
    }
}
