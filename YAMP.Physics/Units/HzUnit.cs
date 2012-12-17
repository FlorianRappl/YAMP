using System;

namespace YAMP.Physics
{
    /// <summary>
    /// Hertz (derived SI unit) -- unit of times per second
    /// </summary>
    class HzUnit : CombinedUnit
    {
        public HzUnit() : base("1 / s", 1.0)
        {
        }
    }
}
