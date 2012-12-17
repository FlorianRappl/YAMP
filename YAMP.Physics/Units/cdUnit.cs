using System;
using YAMP;

namespace YAMP.Physics
{
    /// <summary>
    /// Candela (SI unit) -- unit for the luminosity
    /// </summary>
    class cdUnit : PhysicalUnit
    {
        public cdUnit()
        {
        }

        protected override PhysicalUnit Create()
        {
            return new cdUnit();
        }
    }
}
