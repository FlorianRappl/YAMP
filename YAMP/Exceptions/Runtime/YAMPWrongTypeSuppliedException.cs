using System;

namespace YAMP
{
    class YAMPWrongTypeSuppliedException : YAMPRuntimeException
    {
        public YAMPWrongTypeSuppliedException(string givenType, string expectedType)
            : base("The supplied value of type {0} is not supported. Expected was a value of type {1}.", givenType, expectedType)
        {
        }

        public YAMPWrongTypeSuppliedException(string expectedType)
            : base("The supplied type is not supported. Expected was {0}.", expectedType)
        {
        }
    }
}
