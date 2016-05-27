namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// The function missing exception.
    /// </summary>
    public class YAMPSetsFunctionMissingException : YAMPRuntimeException
    {
        internal YAMPSetsFunctionMissingException(string fnName, string objName)
            : base("The function {0} could not be found in object {1}.", fnName, objName)
        {
        }
    }
}
