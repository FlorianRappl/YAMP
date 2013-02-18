using System;

namespace YAMP
{
    class YAMPNoSeriesAvailableException : YAMPRuntimeException
    {
        public YAMPNoSeriesAvailableException(string message)
            : base(message)
        {
        }
    }
}
