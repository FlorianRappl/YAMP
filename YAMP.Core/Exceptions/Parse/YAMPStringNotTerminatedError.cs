using System;

namespace YAMP
{
    class YAMPStringNotTerminatedError : YAMPParseError
    {
        public YAMPStringNotTerminatedError(int line, int column)
            : base(line, column, "The string starting at line {0}, column {1} has not been closed.", line, column)
        {
        }

        public YAMPStringNotTerminatedError(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
