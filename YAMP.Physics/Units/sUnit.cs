using System;
using YAMP;

namespace YAMP.Physics
{
    /// <summary>
    /// Second (SI unit) -- unit of time
    /// </summary>
    class sUnit : PhysicalUnit
    {
        public sUnit()
        {
            Add("d", 1.0 / 86400.0);
            Add("h", 1.0 / 3600.0);
            Add("yr", 1.0 / (86400.0 * 365.25)); 
        }

        protected override PhysicalUnit Create()
        {
            return new sUnit();
        }
    }
}
