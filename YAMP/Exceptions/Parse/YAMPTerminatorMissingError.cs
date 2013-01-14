using System;

namespace YAMP
{
    class YAMPTerminatorMissingError : YAMPParseError
    {
        public YAMPTerminatorMissingError(int line, int column, char terminator)
            : base(line, column, "The query has not been terminated. Missing terminator for {0} at line {1}, column {2}.", terminator, line, column)
        {
        }

        public YAMPTerminatorMissingError(ParseEngine pe, char terminator)
            : this(pe.CurrentLine, pe.CurrentColumn, terminator)
        {
        }
    }
}
