using System;

namespace YAMP
{
    class YAMPBracketEmptyError : YAMPParseError
    {
        public YAMPBracketEmptyError(int line, int column)
            : base(line, column, "An unexpected bracket has been found. Are you missing something?")
        {
        }

        public YAMPBracketEmptyError(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
