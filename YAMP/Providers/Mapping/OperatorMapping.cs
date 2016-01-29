using System;

namespace YAMP
{
    /// <summary>
    /// Abstract base class for any operator mapping (e.g. binary).
    /// </summary>
    abstract class OperatorMapping
    {
        protected static MapHit TypeDistance(Type basis, object instance)
        {
            if (basis.IsInstanceOfType(instance))
            {
                if (instance.GetType() == basis)
                    return MapHit.Direct;

                return MapHit.Indirect;
            }

            return MapHit.Miss;
        }
    }
}
