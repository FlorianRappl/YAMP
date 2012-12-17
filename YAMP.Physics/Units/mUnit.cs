using System;
using YAMP;

namespace YAMP.Physics
{
    /// <summary>
    /// Meters (SI unit) -- unit of length
    /// </summary>
    class mUnit : PhysicalUnit
    {
        public mUnit()
        {
            Add("yd", 1.0936).Add("in", 39.370).Add("ft", 3.2808).Add("Å", 1e10);
        }

        protected override PhysicalUnit Create()
        {
            return new mUnit();
        }
    }
}
