using System;

namespace YAMP
{
    /// <summary>
    /// Base class for the ParseContext.
    /// </summary>
    public abstract class BaseParseContext
    {
        /// <summary>
        /// Tries to load a function from a given file.
        /// </summary>
        /// <param name="symbolName">The name of the function (equals the name of the file).</param>
        /// <returns>The function (if found) or NULL.</returns>
        public virtual IFunction LoadFunction(string symbolName)
        {
            return null;
        }
    }
}
