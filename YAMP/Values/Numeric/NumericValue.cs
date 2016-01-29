using System;

namespace YAMP
{
    /// <summary>
    /// Base class for numeric classes.
    /// </summary>
    public abstract class NumericValue : Value
    {
        /// <summary>
        /// Clears the contents of the numeric value.
        /// </summary>
        public abstract void Clear();
    }
}
