namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// The no series available exception.
    /// </summary>
    public class YAMPNoSeriesAvailableException : YAMPRuntimeException
    {
        public YAMPNoSeriesAvailableException(String message)
            : base(message)
        {
        }
    }
}
