using System;

namespace YAMP
{
    /// <summary>
    /// Defines a standard set of kinds of functions.
    /// </summary>
    public enum PopularKinds
    {
        /// <summary>
        /// Just a normal function.
        /// </summary>
        Function,
        /// <summary>
        /// A visualization related function for plotting or modifing plots.
        /// </summary>
        Plot,
        /// <summary>
        /// A special kind of function - a system function.
        /// </summary>
        System,
        /// <summary>
        /// This is not a function, but a constant.
        /// </summary>
        Constant,
        /// <summary>
        /// This is a random number generator with a specific distribution.
        /// </summary>
        Random,
        /// <summary>
        /// This is a trigonometric function.
        /// </summary>
        Trigonometric,
        /// <summary>
        /// This is a statistic function.
        /// </summary>
        Statistic,
        /// <summary>
        /// This is a logic function.
        /// </summary>
        Logic,
        /// <summary>
        /// This is a converter function.
        /// </summary>
        Conversion
    }
}
