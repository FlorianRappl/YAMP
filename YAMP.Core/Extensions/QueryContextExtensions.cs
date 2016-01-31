namespace YAMP
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Useful extensions for QueryContext instances.
    /// </summary>
    public static class QueryContextExtensions
    {
        /// <summary>
        /// Begins the interpretation of the current parse tree.
        /// </summary>
        /// <param name="query">The query to extend.</param>
		public static void Run(this QueryContext query)
        {
            query.Run(new Dictionary<String, Value>());
        }

        /// <summary>
        /// Begins the interpretation of the current parse tree.
        /// </summary>
        /// <param name="query">The query to extend.</param>
        /// <param name="values">
        /// The values in an anonymous object - containing name - value pairs.
        /// </param>
        public static void Run(this QueryContext query, Object values)
        {
            var symbols = values.ToDictionary();
            query.Run(symbols);
        }
    }
}
