namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// Class to use for invalid arguments (type-wise or value-wise).
    /// </summary>
    public class YAMPArgumentInvalidException : YAMPRuntimeException
    {
        /// <summary>
        /// Creates a new argument invalid exception.
        /// </summary>
        /// <param name="function">The function where this happened.</param>
        /// <param name="argument">The given argument.</param>
        public YAMPArgumentInvalidException(String function, String argument)
            : base("The argument {1} provided for the function {0} is not valid.", function, argument)
        {
        }

        /// <summary>
        /// Creates a new argument invalid exception.
        /// </summary>
        /// <param name="function">The function where this happened.</param>
        /// <param name="argumentType">The type of argument.</param>
        /// <param name="argumentNumber">The number of the argument.</param>
        public YAMPArgumentInvalidException(String function, String argumentType, Int32 argumentNumber)
            : base("The argument #{1} of type {2}, provided for the function {0}, is not valid.", function, argumentNumber, argumentType)
        {
        }

        /// <summary>
        /// Creates a new argument invalid exception.
        /// </summary>
        /// <param name="function">The function where this happened.</param>
        /// <param name="argumentType">The actual type of argument.</param>
        /// <param name="expectedType">The expected type of argument.</param>
        /// <param name="argumentNumber">The number of the argument.</param>
        public YAMPArgumentInvalidException(String function, String argumentType, String expectedType, Int32 argumentNumber)
            : base("The argument #{1} of type {2}, provided for the function {0}, is not valid. Expected was {3}.", function, argumentNumber, argumentType, expectedType)
        {
        }

        /// <summary>
        /// Creates a new argument invalid exception.
        /// </summary>
        /// <param name="function">The function where this happened.</param>
        /// <param name="argumentNumber">The number of the argument.</param>
        public YAMPArgumentInvalidException(String function, Int32 argumentNumber)
            : base("The argument #{1} provided for the function {0} is not valid.", function, argumentNumber)
        {
        }
    }
}
