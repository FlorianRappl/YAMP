namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// Class to use when an argument has the wrong-type and some other type was expected.
    /// </summary>
    public class YAMPArgumentWrongTypeException : YAMPRuntimeException
    {
        /// <summary>
        /// Creates a new instance of the argument wrong type exception.
        /// </summary>
        /// <param name="argumentType">The given argument type.</param>
        /// <param name="expectedType">The expected argument type.</param>
        /// <param name="function">The function where this happened.</param>
        public YAMPArgumentWrongTypeException(String argumentType, String expectedType, String function)
            : base("The argument supplied for the function {2} of type {0} is not supported. Expected was a value of type {1}.", 
                argumentType, expectedType, function)
        {
        }

        /// <summary>
        /// Creates a new instance of the argument wrong type exception.
        /// </summary>
        /// <param name="argumentType">The given argument type.</param>
        /// <param name="expectedType">The expected argument type.</param>
        /// <param name="function">The function where this happened.</param>
        /// <param name="argumentName">The name of the provided argument.</param>
        public YAMPArgumentWrongTypeException(String argumentType, String expectedType, String function, String argumentName)
            : base("The argument {3} supplied for the function {2} must be of type {1}.",
                argumentType, expectedType, function, argumentName)
        {
        }

        /// <summary>
        /// Creates a new instance of the argument wrong type exception.
        /// </summary>
        /// <param name="argumentType">The given argument type.</param>
        /// <param name="expectedTypes">A list of possible types.</param>
        /// <param name="function">The function where this happened.</param>
        public YAMPArgumentWrongTypeException(String argumentType, String[] expectedTypes, String function)
            : base("The argument supplied for the function {2} of type {0} is not supported. Possible values are {1}.",
                argumentType, String.Join(", ", expectedTypes), function)
        {
        }
    }
}
