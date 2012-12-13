using System;
using YAMP;

namespace YAMP.Physics
{
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
