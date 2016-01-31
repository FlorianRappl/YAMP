namespace YAMP.Exceptions
{
    using System;

    class YAMPUnitBracketException : YAMPException
    {
        public YAMPUnitBracketException(String unit)
            : base("Could not parse the end of a bracket in the given unit {0}. Are you missing a closing bracket?", unit)
        {
        }
    }
}
