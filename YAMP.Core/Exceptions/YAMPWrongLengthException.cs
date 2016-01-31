namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// The wrong length.
    /// </summary>
    public class YAMPWrongLengthException : YAMPRuntimeException
    {
        internal YAMPWrongLengthException(Int32 provided, Int32 required)
            : base("The provided vector must have at least {0} element(s). You provided {1} element(s).", required, provided)
        {
        }
    }
}
