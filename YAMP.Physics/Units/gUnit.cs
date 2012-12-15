using System;
using YAMP;

namespace YAMP.Physics
{
    class gUnit : PhysicalUnit
    {
        public gUnit()
        {
            Add("lb", 0.002205);
        }

        protected override PhysicalUnit Create()
        {
            return new gUnit();
        }
    }
}
