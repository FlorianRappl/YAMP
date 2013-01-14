using System;

namespace YAMP
{
    class YAMPNonNumericException : YAMPRuntimeException
    {
        public YAMPNonNumericException()
            : base("A matrix can only contain numeric values.")
        {
        }
    }
}
