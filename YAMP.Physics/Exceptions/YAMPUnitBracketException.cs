using System;

namespace YAMP
{
    class YAMPUnitBracketException : YAMPException
    {
        public YAMPUnitBracketException(string unit)
            : base("Could not parse the end of a bracket in the given unit {0}. Are you missing a closing bracket?", unit)
        {
        }
    }
}
