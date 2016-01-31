namespace YAMP.Exceptions
{
    using System;

    class YAMPUnitParseException : YAMPException
    {
        public YAMPUnitParseException(Int32 col, String rest)
            : base ("Could not parse the given unit! Problem occured at char {0} in the beginning of {1}.", col, rest)
        {
        }
    }
}
