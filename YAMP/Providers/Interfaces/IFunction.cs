using System;

namespace YAMP
{
    /// <summary>
    /// Every function needs to implemented the IFunction interface.
    /// </summary>
	public interface IFunction
	{
        /// <summary>
        /// Invokes the function.
        /// </summary>
        /// <param name="context">The context where the invocation takes place.</param>
        /// <param name="argument">The arguments of the function.</param>
        /// <returns>The result of the function.</returns>
		Value Perform(ParseContext context, Value argument);
	}
}