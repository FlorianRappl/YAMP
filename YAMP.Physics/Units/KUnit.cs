using System;
using YAMP;

namespace YAMP.Physics
{
    /// <summary>
    /// Kelvin (SI unit) -- unit of temperature
    /// </summary>
    class KUnit : PhysicalUnit
    {
        public KUnit()
        {
            Add("°C", 1.0, -273.16);
            Add("°F", 9.0 / 5.0, -460.0);
        }

        protected override PhysicalUnit Create()
        {
            return new KUnit();
        }
    }
}
