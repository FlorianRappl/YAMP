using System;

namespace YAMP
{
    class YAMPExpressionMissingError : YAMPParseError
    {
        public YAMPExpressionMissingError(int line, int column)
            : base(line, column, "Another expression was expected but nothing was found.")
        {
        }

        public YAMPExpressionMissingError(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
