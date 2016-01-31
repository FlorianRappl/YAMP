namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// The argument number exception.
    /// </summary>
    public class YAMPArgumentNumberException : YAMPRuntimeException
    {
        public YAMPArgumentNumberException(String function, Int32 givenArguments, Int32 expectedArguments)
            : base("The closest overload for {0} takes {1} argument(s). You provided {2} argument(s).", function, expectedArguments, givenArguments)
        {
        }
    }
}
