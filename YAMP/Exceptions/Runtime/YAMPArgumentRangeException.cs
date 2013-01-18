using System;

namespace YAMP
{
    /// <summary>
    /// This class should be used if an argument is out of range.
    /// </summary>
    public class YAMPArgumentRangeException : YAMPRuntimeException
    {
        /// <summary>
        /// Creates a new instance of the range exception.
        /// </summary>
        /// <param name="parameterName">The parameter where this happened.</param>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        public YAMPArgumentRangeException(string parameterName, double lowerBound, double upperBound)
            : base("The argument {0} was out of range. It has to be between {1} and {2}.", parameterName, lowerBound, upperBound)
        {
        }

        /// <summary>
        /// Creates a new instance of the range exception.
        /// </summary>
        /// <param name="parameterName">The parameter where this happened.</param>
        /// <param name="lowerBound">The lower bound.</param>
        public YAMPArgumentRangeException(string parameterName, double lowerBound)
            : base("The argument {0} was out of range. It has to be greater than {1}.", parameterName, lowerBound)
        {
        }

        /// <summary>
        /// Creates a new instance of the range exception.
        /// </summary>
        /// <param name="parameterName">The parameter where this happened.</param>
        /// <param name="boundaries">A string expressing the boundaries.</param>
        public YAMPArgumentRangeException(string parameterName, string boundaries)
            : base("The argument {0} was out of range. Boundaries: {1}.", parameterName, boundaries)
        {
        }

        /// <summary>
        /// Creates a new instance of the range exception.
        /// </summary>
        /// <param name="parameterName">The parameter where this happened.</param>
        public YAMPArgumentRangeException(string parameterName)
            : base("The argument {0} was out of range.", parameterName)
        {
        }
    }
}
