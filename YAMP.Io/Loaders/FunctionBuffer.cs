namespace YAMP.Io
{
    using System;

    /// <summary>
    /// Class to buffer previous file function calls.
    /// </summary>
    sealed class FunctionBuffer
    {
        /// <summary>
        /// Gets or sets the time of the last update of the file.
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets the file name (functionname + ys as extension).
        /// </summary>
        public String FileName { get { return FunctionName + ".ys"; } }

        /// <summary>
        /// Gets or sets the directory that has been used.
        /// </summary>
        public String Directory { get; set; }

        /// <summary>
        /// Gets or sets the name of the function.
        /// </summary>
        public String FunctionName { get; set; }

        /// <summary>
        /// Gets or sets the used context.
        /// </summary>
        public ParseContext Context { get; set; }

        /// <summary>
        /// Requests the function to be looked up.
        /// </summary>
        /// <returns>The function or NULL, if the context did not contain the function.</returns>
        public IFunction Lookup()
        {
            return Context.FindFunction(FunctionName);
        }
    }
}
