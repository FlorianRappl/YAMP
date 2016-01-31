namespace YAMP
{
    using System;

    /// <summary>
    /// Defines the interface for loading external functions.
    /// </summary>
    public interface IFunctionLoader
    {
        /// <summary>
        /// Loads the function corresponding to the given symbol.
        /// </summary>
        /// <param name="symbolName">The symbol to resolve.</param>
        /// <returns>The function, if any.</returns>
        IFunction Load(String symbolName);
    }
}
