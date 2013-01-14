using System;

namespace YAMP
{
    class YAMPUnitConvertException : YAMPException
    {
        public YAMPUnitConvertException(string expr)
            : base("Could not convert the given expression ({0}) to a number.", expr)
        {

        }
    }
}
