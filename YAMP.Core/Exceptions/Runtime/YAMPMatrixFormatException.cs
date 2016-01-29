using System;

namespace YAMP
{
    class YAMPMatrixFormatException : YAMPRuntimeException
    {
        public YAMPMatrixFormatException(SpecialMatrixFormat format)
            : base("The provided matrix has to be {0}.", format.ToString())
        {
        }
    }
}
