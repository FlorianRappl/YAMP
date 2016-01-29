using System;

namespace YAMP
{
    class YAMPFunctionNameMissing : YAMPParseError
    {
        public YAMPFunctionNameMissing(int line, int column)
            : base(line, column, "Functions without names are not allowed. Consider using lambda expressions for such scenarios.")
        {
        }

        public YAMPFunctionNameMissing(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
