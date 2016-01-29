using System;

namespace YAMP
{
    class YAMPWrongLengthException : YAMPRuntimeException
    {
        public YAMPWrongLengthException(int provided, int required)
            : base("The provided vector must have at least {0} element(s). You provided {1} element(s).", required, provided)
        {
        }
    }
}
