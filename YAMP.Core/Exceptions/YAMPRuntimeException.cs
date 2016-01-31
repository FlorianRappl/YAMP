namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// The base class for any runtime exception. 
    /// </summary>
    public class YAMPRuntimeException : YAMPException
    {
        /// <summary>
        /// Creates a new runtime exception.
        /// </summary>
        /// <param name="msg">The message to show.</param>
        public YAMPRuntimeException(String msg)
            : base(msg)
        {
        }

        /// <summary>
        /// Creates a new runtime exception.
        /// </summary>
        /// <param name="msg">The message to show.</param>
        /// <param name="args">The arguments for the message.</param>
        public YAMPRuntimeException(String msg, params Object[] args)
            : base(msg, args)
        {
        }
    }
}
