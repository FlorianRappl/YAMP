namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// The non-numeric exception.
    /// </summary>
    public class YAMPNonNumericException : YAMPRuntimeException
    {
        internal YAMPNonNumericException()
            : base("A matrix can only contain numeric values.")
        {
        }
    }
}
