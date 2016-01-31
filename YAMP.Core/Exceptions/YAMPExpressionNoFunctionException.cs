namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// The expression no function exception.
    /// </summary>
    public class YAMPExpressionNoFunctionException : YAMPRuntimeException
    {
        public YAMPExpressionNoFunctionException()
            : base("The given expression cannot be used as a function.")
        {
        }
    }
}
