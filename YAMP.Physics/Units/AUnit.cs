using System;
using YAMP;

namespace YAMP.Physics
{
    /// <summary>
    /// Ampere (SI unit) -- unit for the current
    /// </summary>
    class AUnit : PhysicalUnit
    {
        public AUnit()
        {
        }

        protected override PhysicalUnit Create()
        {
            return new AUnit();
        }
    }
}
