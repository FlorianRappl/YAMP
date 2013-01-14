using System;

namespace YAMP
{
    class YAMPForArgumentsMissing : YAMPParseError
    {
        public YAMPForArgumentsMissing(int line, int column)
            : base(line, column, "The for keyword requires (3) arguments given in round brackets.")
        {
        }

        public YAMPForArgumentsMissing(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
