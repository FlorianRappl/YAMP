namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// The function missing exception.
    /// </summary>
    public class YAMPMemberFunctionMissingException : YAMPRuntimeException
    {
        internal YAMPMemberFunctionMissingException(string fnName, string objName)
            : base("The function {0} could not be found in object {1}.", fnName, objName)
        {
        }
    }
}
