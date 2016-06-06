namespace YAMP
{
    using System;

    /// <summary>
    /// Abstract base class for any operator mapping (e.g., binary).
    /// </summary>
    public abstract class OperatorMapping
    {
        /// <summary>
        /// Computes the type distance between the type and the instance.
        /// </summary>
        protected static MapHit TypeDistance(Type basis, Object instance)
        {
            if (basis.IsInstanceOfType(instance))
            {
                if (instance.GetType() == basis)
                {
                    return MapHit.Direct;
                }

                return MapHit.Indirect;
            }

            return MapHit.Miss;
        }
    }
}
