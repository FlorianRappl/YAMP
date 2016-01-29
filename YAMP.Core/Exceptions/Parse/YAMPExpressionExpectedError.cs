using System;

namespace YAMP
{
    class YAMPExpressionExpectedError : YAMPParseError
    {
        public YAMPExpressionExpectedError(int line, int column)
            : base(line, column, "An expression was expected but an operator was found.")
        {
        }

        public YAMPExpressionExpectedError(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
