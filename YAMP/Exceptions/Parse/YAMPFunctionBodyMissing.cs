using System;

namespace YAMP
{
    class YAMPFunctionBodyMissing : YAMPParseError
    {
        public YAMPFunctionBodyMissing(int line, int column)
            : base(line, column, "A function requires a body encapsulated in curly brackets.")
        {
        }

        public YAMPFunctionBodyMissing(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
