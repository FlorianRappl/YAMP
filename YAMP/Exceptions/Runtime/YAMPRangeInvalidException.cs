using System;

namespace YAMP
{
    class YAMPRangeInvalidException : YAMPRuntimeException
    {
        public YAMPRangeInvalidException(string msg)
            : base("The given range value is invalid, since {0}.", msg)
        {
        }
    }
}
