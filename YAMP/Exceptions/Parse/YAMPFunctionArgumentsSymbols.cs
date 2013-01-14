using System;

namespace YAMP
{
    class YAMPFunctionArgumentsSymbols : YAMPParseError
    {
        public YAMPFunctionArgumentsSymbols(int line, int column)
            : base(line, column, "A function requires a list of valid symbols as arguments. Only provide valid symbolnames as function arguments.")
        {
        }

        public YAMPFunctionArgumentsSymbols(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
