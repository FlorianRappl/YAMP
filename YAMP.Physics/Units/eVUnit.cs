using System;
using YAMP;

namespace YAMP.Physics
{
    /// <summary>
    /// Electron volt (derived SI unit, popular in atom physics, semiconductors, ...) -- unit of energy
    /// </summary>
    class eVUnit : CombinedUnit
    {
        public eVUnit() : base("kg * m^2 / s^2", 1.60217656535e-19)
        {
        }
    }
}
