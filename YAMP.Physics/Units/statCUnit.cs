using System;
using YAMP;

namespace YAMP.Physics
{
    /// <summary>
    /// StatCoulomb (CGS unit) -- unit of charge
    /// </summary>
    class statCUnit : CombinedUnit
    {
        public statCUnit() : base("A * s", 1.0 / 2997924580.0)
        {
        }
    }

    /// <summary>
    /// One Franklin is equivalent to one StatColoumb
    /// </summary>
    class FrUnit : statCUnit
    {
    }

    /// <summary>
    /// The electrostatic unit of charge is also equivalent to one StatColoumb
    /// </summary>
    class esuUnit : statCUnit
    {
    }
}
