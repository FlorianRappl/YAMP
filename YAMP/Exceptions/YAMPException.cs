using System;

namespace YAMP
{
    /// <summary>
    /// Basic YAMP exception. This lets everyone know that the exception
    /// did not occur because something was fishy in the (C#) code, but
    /// rather in the query.
    /// </summary>
    public class YAMPException : Exception
    {
        /// <summary>
        /// Creates an anonymous YAMP exception.
        /// </summary>
        public YAMPException()
            : this("An unknown exception occured by executing your query.")
        {
        }

        /// <summary>
        /// Creates a YAMP exception with a simple message.
        /// </summary>
        /// <param name="message">Which message do you want to display?</param>
        public YAMPException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a YAMP exception with a formatted message.
        /// </summary>
        /// <param name="message">The associated message.</param>
        /// <param name="args">Some parameters for your message.</param>
        public YAMPException(string message, params object[] args) 
            : base(string.Format(message, args))
        {
        }
    }
}
