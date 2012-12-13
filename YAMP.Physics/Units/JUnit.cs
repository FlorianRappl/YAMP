using System;
using YAMP;

namespace YAMP.Physics
{
    class JUnit : CombinedUnit
    {
        public JUnit() : base("kg * m^2 / s^2")
        {
            Add("erg", 1e7);
        }
    }
}
