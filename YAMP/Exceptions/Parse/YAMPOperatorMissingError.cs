using System;

namespace YAMP
{
    class YAMPOperatorMissingError : YAMPParseError
    {
        public YAMPOperatorMissingError(int line, int column)
            : base(line, column, "An expected operator was not found.")
        {
        }

        public YAMPOperatorMissingError(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
