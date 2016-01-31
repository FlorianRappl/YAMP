namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// The expression no function exception.
    /// </summary>
    public class YAMPExpressionNoFunctionException : YAMPRuntimeException
    {
        internal YAMPExpressionNoFunctionException()
            : base("The given expression cannot be used as a function.")
        {
        }
    }
}
