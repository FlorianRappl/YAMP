namespace YAMP.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using YAMP.Errors;

    /// <summary>
    /// Represents the exception that will be thrown if parse errors occured.
    /// </summary>
    public class YAMPParseException : YAMPException
    {
        /// <summary>
        /// Creates a new YAMP Parse Exception.
        /// </summary>
        /// <param name="engine">The engine where this happend.</param>
        public YAMPParseException(ParseEngine engine) 
            : base("The query can not run, since the parser encountered {0} error(s).", engine.ErrorCount)
        {
            Errors = engine.Errors;
        }

        /// <summary>
        /// Returns an enumerable of errors.
        /// </summary>
        public IEnumerable<YAMPParseError> Errors
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns a string representation of the errors.
        /// </summary>
        /// <returns>The string with the errors.</returns>
        public override String ToString()
        {
            var sb = new StringBuilder();

            foreach (var error in Errors)
            {
                sb.AppendLine(error.ToString());
            }
            
            return sb.ToString();
        }
    }
}
