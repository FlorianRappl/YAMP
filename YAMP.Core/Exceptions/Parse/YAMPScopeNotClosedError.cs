using System;

namespace YAMP
{
    class YAMPScopeNotClosedError : YAMPParseError
    {
        public YAMPScopeNotClosedError(int line, int column) :
            base(line, column, "The scope (curly bracket) starting at line {0}, column {1} has not been properly closed.", line, column)
        {
        }

        public YAMPScopeNotClosedError(ParseEngine pe) :
            this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
