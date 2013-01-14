using System;

namespace YAMP
{
    class YAMPDoWhileNotEmptyError : YAMPParseError
    {
        public YAMPDoWhileNotEmptyError(int line, int column)
            : base(line, column, "A do-while construct requires an empty statement after the while condition. Consider inserting a semicolon after the condition.")
        {
        }

        public YAMPDoWhileNotEmptyError(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
