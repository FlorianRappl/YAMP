using System;

namespace YAMP
{
    class YAMPFunctionArgumentsMissing : YAMPParseError
    {
        public YAMPFunctionArgumentsMissing(int line, int column)
            : base(line, column, "Arguments for the function have not been specified. Round brackets are always required.")
        {
        }

        public YAMPFunctionArgumentsMissing(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
