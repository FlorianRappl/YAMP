using System;

namespace YAMP
{
    /// <summary>
    /// Class to use when an argument has the wrong-type and some other type was expected.
    /// </summary>
    public class YAMPArgumentWrongTypeException : YAMPRuntimeException
    {
        public YAMPArgumentWrongTypeException(string argumentType, string expectedType, string function)
            : base("The argument supplied for the function {2} of type {0} is not supported. Expected was a value of type {1}.", 
                argumentType, expectedType, function)
        {
        }

        public YAMPArgumentWrongTypeException(string argumentType, string[] expectedTypes, string function)
            : base("The argument supplied for the function {2} of type {0} is not supported. Possible values are {1}.", 
                argumentType, string.Join(", ", expectedTypes), function)
        {
        }
    }
}
