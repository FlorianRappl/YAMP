using System;
using YAMP;

namespace YAMP.Physics
{
    class KUnit : PhysicalUnit
    {
        public KUnit()
        {
            Add("°C", t => t - 273.16);
            Add("°F", t => 9.0 * t / 5.0 - 460);
        }

        protected override PhysicalUnit Create()
        {
            return new KUnit();
        }
    }
}
