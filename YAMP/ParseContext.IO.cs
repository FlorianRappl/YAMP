namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Class that describes the current parse context (available functions, constants, variables, ...).
    /// </summary>
    public sealed partial class ParseContext
    {
        /// <summary>
        /// List to buffer previous file function calls.
        /// </summary>
        static List<FunctionBuffer> buffer = new List<FunctionBuffer>();

        #region Comfort Methods

        /// <summary>
        /// Loads the workspace from the given file.
        /// </summary>
        /// <param name="fromFileName">The path to the file.</param>
        public void Load(String fromFileName)
        {
            var lf = new LoadFunction();
            lf.Context = this;
            lf.Function(new StringValue(fromFileName));
        }

        /// <summary>
        /// Saves the workspace in the given file.
        /// </summary>
        /// <param name="toFileName">The path to the file.</param>
        public void Save(String toFileName)
        {
            SaveFunction.Save(toFileName, variables);
        }

        /// <summary>
        /// Tries to load a function from a given file.
        /// </summary>
        /// <param name="symbolName">The name of the function (equals the name of the file).</param>
        /// <returns>The function (if found) or NULL.</returns>
        public override IFunction LoadFunction(String symbolName)
        {
            if (!Parser.UseScripting)
                return null;

            var script = String.Empty;

            var function = new FunctionBuffer
            {
                Directory = Environment.CurrentDirectory,
                FunctionName = symbolName
            };

            if (!File.Exists(function.FileName))
                return null;

            for (var i = buffer.Count - 1; i >= 0; i--)
                if(!buffer[i].Directory.Equals(Environment.CurrentDirectory, StringComparison.CurrentCultureIgnoreCase))
                    buffer.RemoveAt(i);

            var original = buffer
                .Where(m => m.FileName.Equals(function.FileName, StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault();

            try
            {
                function.LastUpdated = File.GetLastWriteTime(function.FileName);

                if (original != null)
                {
                    if (function.LastUpdated.CompareTo(original.LastUpdated) <= 0)
                        return original.Lookup();

                    buffer.Remove(original);
                }

                script = File.ReadAllText(function.FileName);
            }
            catch
            {
                if (buffer != null)
                    return original.Lookup();

                return null;
            }

            if (!String.IsNullOrEmpty(script))
            {
                function.Context = new ParseContext(parent);
                var p = Parser.Parse(function.Context, script);

                if (!p.Context.Parser.HasErrors)
                {
                    try { p.Execute(); }
                    catch { return null; }

                    buffer.Add(function);
                    return function.Lookup();
                }
            }

            return null;
        }

        #endregion

        /// <summary>
        /// Class to buffer previous file function calls.
        /// </summary>
        class FunctionBuffer
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
}
