using System;

namespace YAMP
{
    class YAMPArgumentInvalidException : YAMPRuntimeException
    {
        public YAMPArgumentInvalidException(string function, string argument)
            : base("The argument {0} provided for the function {1} is not valid.", function, argument)
        {
        }

        public YAMPArgumentInvalidException(string function, string argumentType, int argumentNumber)
            : base("The argument #{0} of type {2}, provided for the function {1}, is not valid.", function, argumentNumber, argumentType)
        {
        }

        public YAMPArgumentInvalidException(string function, int argumentNumber)
            : base("The argument #{0} provided for the function {1} is not valid.", function, argumentNumber)
        {
        }
    }
}
