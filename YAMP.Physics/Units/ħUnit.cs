using System;
using YAMP;

namespace YAMP.Physics
{
    /// <summary>
    /// Reduced plank constant (natural unit) -- unit of energy
    /// </summary>
    class ħUnit : CombinedUnit
    {
        public ħUnit() : base("J * s", 1.05457172647e-34)
        {
        }
    }

    /// <summary>
    /// Just a different spelling!
    /// </summary>
    class hbarUnit : ħUnit
    {
        public hbarUnit()
        {
        }
    }
}
