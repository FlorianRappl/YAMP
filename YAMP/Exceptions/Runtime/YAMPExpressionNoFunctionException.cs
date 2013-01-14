using System;

namespace YAMP
{
    class YAMPExpressionNoFunctionException : YAMPRuntimeException
    {
        public YAMPExpressionNoFunctionException()
            : base("The given expression cannot be used as a function.")
        {
        }
    }
}
