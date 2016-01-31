namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// Wrong type supplied.
    /// </summary>
    public class YAMPWrongTypeSuppliedException : YAMPRuntimeException
    {
        public YAMPWrongTypeSuppliedException(String givenType, String expectedType)
            : base("The supplied value of type {0} is not supported. Expected was a value of type {1}.", givenType, expectedType)
        {
        }

        public YAMPWrongTypeSuppliedException(String expectedType)
            : base("The supplied type is not supported. Expected was {0}.", expectedType)
        {
        }
    }
}
