using System;
using YAMP;

namespace YAMP.Physics
{
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
