using System;

namespace YAMP
{
    class YAMPBracketNotClosedError : YAMPParseError
    {
        public YAMPBracketNotClosedError(int line, int column) :
            base(line, column, "Missing bracket terminator for bracket starting at line {0}, column {1}.", line, column)
        {
        }

        public YAMPBracketNotClosedError(ParseEngine pe) :
            this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
