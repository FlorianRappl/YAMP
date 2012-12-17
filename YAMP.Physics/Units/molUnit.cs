using System;
using YAMP;

namespace YAMP.Physics
{
    /// <summary>
    /// Mol (SI unit) -- unit of quantity in chemistry
    /// </summary>
    class molUnit : PhysicalUnit
    {
        public molUnit()
        {

        }

        protected override PhysicalUnit Create()
        {
            return new molUnit();
        }
    }
}
