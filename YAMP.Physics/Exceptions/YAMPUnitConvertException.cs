namespace YAMP.Exceptions
{
    using System;

    class YAMPUnitConvertException : YAMPException
    {
        public YAMPUnitConvertException(String expr)
            : base("Could not convert the given expression ({0}) to a number.", expr)
        {

        }
    }
}
