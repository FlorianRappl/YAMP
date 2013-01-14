using System;

namespace YAMP
{
    class YAMPUnitParseException : YAMPException
    {
        public YAMPUnitParseException(int col, string rest)
            : base ("Could not parse the given unit! Problem occured at char {0} in the beginning of {1}.", col, rest)
        {
        }
    }
}
