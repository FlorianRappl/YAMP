namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// The function missing exception.
    /// </summary>
    public class YAMPSetsFunctionNotMemberException : YAMPRuntimeException
    {
        internal YAMPSetsFunctionNotMemberException(string fnName)
            : base("The function {0} must be called in the context of a Set object.", fnName)
        {
        }
    }
}
