using System;

namespace YAMP
{
    /// <summary>
    /// Marks a class to hold a certain constant defined by the two properties.
    /// </summary>
    public interface IConstants
    {
        /// <summary>
        /// The name of the constant.
        /// </summary>
		string Name { get; }

        /// <summary>
        /// The value of the constant.
        /// </summary>
		Value Value { get; }
    }
}
