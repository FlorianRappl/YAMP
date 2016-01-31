namespace YAMP.Io
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Class that describes the current parse context (available functions, constants, variables, ...).
    /// </summary>
    public class DiskFunctionLoader : IFunctionLoader
    {
        readonly List<FunctionBuffer> _buffer;
        readonly ParseContext _context;

        public DiskFunctionLoader(ParseContext context)
        {
            _buffer = new List<FunctionBuffer>();
            _context = context;
        }

        /// <summary>
        /// Tries to load a function from a given file.
        /// </summary>
        /// <param name="symbolName">The name of the function (equals the name of the file).</param>
        /// <returns>The function (if found) or NULL.</returns>
        public IFunction Load(String symbolName)
        {
            var script = String.Empty;

            var function = new FunctionBuffer
            {
                Directory = Environment.CurrentDirectory,
                FunctionName = symbolName
            };

            if (!File.Exists(function.FileName))
            {
                return null;
            }

            for (var i = _buffer.Count - 1; i >= 0; i--)
            {
                if (!_buffer[i].Directory.Equals(Environment.CurrentDirectory, StringComparison.CurrentCultureIgnoreCase))
                {
                    _buffer.RemoveAt(i);
                }
            }

            var original = _buffer.
                Where(m => m.FileName.Equals(function.FileName, StringComparison.CurrentCultureIgnoreCase)).
                FirstOrDefault();

            try
            {
                function.LastUpdated = File.GetLastWriteTime(function.FileName);

                if (original != null)
                {
                    if (function.LastUpdated.CompareTo(original.LastUpdated) <= 0)
                    {
                        return original.Lookup();
                    }

                    _buffer.Remove(original);
                }

                script = File.ReadAllText(function.FileName);
            }
            catch
            {
                if (_buffer != null)
                {
                    return original.Lookup();
                }

                return null;
            }

            if (!String.IsNullOrEmpty(script))
            {
                function.Context = new ParseContext(_context);
                var query = new QueryContext(function.Context, script);
                var parser = query.Parser;
                parser.Parse();

                if (!parser.HasErrors)
                {
                    _buffer.Add(function);
                    return function.Lookup();
                }
            }

            return null;
        }
    }
}
