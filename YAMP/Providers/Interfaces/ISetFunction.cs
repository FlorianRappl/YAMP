using System;

namespace YAMP
{
    /// <summary>
    /// This interface has to be implemented for values to indicate that using
    /// the value as a method can also be done in order to set something, e.g.
    /// for a matrix M to be used like M(2, 3) = 5.
    /// </summary>
    public interface ISetFunction
    {
        /// <summary>
        /// Invokes the function to set a certain value.
        /// </summary>
        /// <param name="context">The context where the invocation takes place.</param>
        /// <param name="indices">The parameter containing the indices.</param>
        /// <param name="values">The parameter containing the value(s).</param>
        /// <returns>Usually the modified instance is returned.</returns>
        Value Perform(ParseContext context, Value indices, Value values);
    }
}
