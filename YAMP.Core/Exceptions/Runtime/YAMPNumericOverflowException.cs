using System;

namespace YAMP
{
    class YAMPNumericOverflowException : YAMPRuntimeException
    {
        public YAMPNumericOverflowException(string function)
            : base("Numeric overflow in the {0} function.", function)
        {
        }
    }
}
