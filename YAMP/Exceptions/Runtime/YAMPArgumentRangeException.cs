using System;

namespace YAMP
{
    /// <summary>
    /// This class should be used if an argument is out of range.
    /// </summary>
    public class YAMPArgumentRangeException : YAMPRuntimeException
    {
        public YAMPArgumentRangeException(string parameterName, double lowerBound, double upperBound)
            : base("The argument {0} was out of range. It has to be between {1} and {2}.", parameterName, lowerBound, upperBound)
        {
        }

        public YAMPArgumentRangeException(string parameterName, double lowerBound)
            : base("The argument {0} was out of range. It has to be greater than {1}.", parameterName, lowerBound)
        {
        }

        public YAMPArgumentRangeException(string parameterName, string boundaries)
            : base("The argument {0} was out of range. Boundaries: {1}.", parameterName, boundaries)
        {
        }

        public YAMPArgumentRangeException(string parameterName)
            : base("The argument {0} was out of range.", parameterName)
        {
        }
    }
}
