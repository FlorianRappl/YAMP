using System;

namespace YAMP
{
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
        public YAMPArgumentInvalidException(string function, string argument)
            : base("The argument {1} provided for the function {0} is not valid.", function, argument)
        {
        }

        /// <summary>
        /// Creates a new argument invalid exception.
        /// </summary>
        /// <param name="function">The function where this happened.</param>
        /// <param name="argumentType">The type of argument.</param>
        /// <param name="argumentNumber">The number of the argument.</param>
        public YAMPArgumentInvalidException(string function, string argumentType, int argumentNumber)
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
        public YAMPArgumentInvalidException(string function, string argumentType, string expectedType, int argumentNumber)
            : base("The argument #{1} of type {2}, provided for the function {0}, is not valid. Expected was {3}.", function, argumentNumber, argumentType, expectedType)
        {
        }

        /// <summary>
        /// Creates a new argument invalid exception.
        /// </summary>
        /// <param name="function">The function where this happened.</param>
        /// <param name="argumentNumber">The number of the argument.</param>
        public YAMPArgumentInvalidException(string function, int argumentNumber)
            : base("The argument #{1} provided for the function {0} is not valid.", function, argumentNumber)
        {
        }
    }
}
