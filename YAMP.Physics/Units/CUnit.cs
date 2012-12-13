using System;

namespace YAMP.Physics
{
    class CUnit : CombinedUnit
    {
        public CUnit() : base("A * s")
        {
            Add("statC", 2997924580);
        }
    }
}