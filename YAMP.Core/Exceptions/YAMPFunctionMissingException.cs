namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// The function missing exception.
    /// </summary>
    public class YAMPFunctionMissingException : YAMPRuntimeException
    {
        internal YAMPFunctionMissingException(String name)
            : base("The function {0} could not be found.", name)
        {
        }
    }
}
