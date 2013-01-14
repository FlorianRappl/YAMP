using System;

namespace YAMP
{
    class YAMPSingleElseError : YAMPParseError
    {
        public YAMPSingleElseError(int line, int column)
            : base(line, column, "Cannot use two consecutive else blocks. Did you miss an if before?")
        {
        }

        public YAMPSingleElseError(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
