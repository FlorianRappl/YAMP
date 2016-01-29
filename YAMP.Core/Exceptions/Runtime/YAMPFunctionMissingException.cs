using System;

namespace YAMP
{
    class YAMPFunctionMissingException : YAMPRuntimeException
    {
        public YAMPFunctionMissingException(string name)
            : base("The function {0} could not be found.", name)
        {
        }
    }
}
