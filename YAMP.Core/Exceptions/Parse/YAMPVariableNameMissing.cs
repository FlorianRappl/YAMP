using System;

namespace YAMP
{
    class YAMPVariableNameMissing : YAMPParseError
    {
        public YAMPVariableNameMissing(int line, int column)
            : base(line, column, "The let keyword can only be used in combination with a valid name for the variable.")
        {
        }

        public YAMPVariableNameMissing(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}

