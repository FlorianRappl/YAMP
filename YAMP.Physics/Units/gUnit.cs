using System;
using YAMP;

namespace YAMP.Physics
{
    class gUnit : PhysicalUnit
    {
        public gUnit()
        {
            Add("lb", 2.205);
        }

        protected override PhysicalUnit Create()
        {
            return new gUnit();
        }
    }
}
