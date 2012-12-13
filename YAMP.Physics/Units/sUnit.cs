using System;
using YAMP;

namespace YAMP.Physics
{
    class sUnit : PhysicalUnit
    {
        public sUnit()
        {
            Add("d", 0.00001157407);
            Add("yr", 3.16880878e-8); 
        }

        protected override PhysicalUnit Create()
        {
            return new sUnit();
        }
    }
}
