using System;
using YAMP;

namespace YAMP.Physics
{
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
