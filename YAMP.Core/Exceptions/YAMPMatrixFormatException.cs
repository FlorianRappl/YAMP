namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// The matrix format exception.
    /// </summary>
    public class YAMPMatrixFormatException : YAMPRuntimeException
    {
        internal YAMPMatrixFormatException(String format)
            : base("The provided matrix has to be {0}.", format)
        {
        }
    }
}
