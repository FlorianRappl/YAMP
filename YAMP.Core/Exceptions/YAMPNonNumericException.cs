namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// The non-numeric exception.
    /// </summary>
    public class YAMPNonNumericException : YAMPRuntimeException
    {
        public YAMPNonNumericException()
            : base("A matrix can only contain numeric values.")
        {
        }
    }
}
