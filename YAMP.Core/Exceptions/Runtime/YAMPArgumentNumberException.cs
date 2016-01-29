using System;

namespace YAMP
{
    class YAMPArgumentNumberException : YAMPRuntimeException
    {
        public YAMPArgumentNumberException(string function, int givenArguments, int expectedArguments)
            : base("The closest overload for {0} takes {1} argument(s). You provided {2} argument(s).", function, expectedArguments, givenArguments)
        {
        }
    }
}
