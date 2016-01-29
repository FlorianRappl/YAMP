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

        public YAMPExpressionMissingError(ParseEngine pe, Operator op, int found)
            : base(pe.CurrentLine, pe.CurrentColumn, "The {0} operator requires {1} expression(s), but only {2} expression(s) have been found.", op.Op, op.Expressions, found)
        {
        }
    }
}
