using System;

namespace YAMP
{
    /// <summary>
    /// The base class for any runtime exception. 
    /// </summary>
    public class YAMPRuntimeException : YAMPException
    {
        /// <summary>
        /// Creates a new runtime exception.
        /// </summary>
        /// <param name="msg">The message to show.</param>
        public YAMPRuntimeException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        /// Creates a new runtime exception.
        /// </summary>
        /// <param name="msg">The message to show.</param>
        /// <param name="args">The arguments for the message.</param>
        public YAMPRuntimeException(string msg, params object[] args)
            : base(msg, args)
        {
        }
    }
}
