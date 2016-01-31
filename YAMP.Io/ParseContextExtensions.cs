namespace YAMP.Io
{
    using System;

    /// <summary>
    /// Useful extensions for the parse context.
    /// </summary>
    public static class ParseContextExtensions
    {
        /// <summary>
        /// Loads the workspace from the given file.
        /// </summary>
        /// <param name="fromFileName">The path to the file.</param>
        public static void Load(this ParseContext context, String fromFileName)
        {
            var lf = new LoadFunction(context);
            lf.Function(new StringValue(fromFileName));
        }

        /// <summary>
        /// Saves the workspace in the given file.
        /// </summary>
        /// <param name="toFileName">The path to the file.</param>
        public static void Save(this ParseContext context, String toFileName)
        {
            var variables = context.Variables;
            SaveFunction.Save(toFileName, variables);
        }
    }
}
