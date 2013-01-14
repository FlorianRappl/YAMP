using System;

namespace YAMP
{
    class YAMPIfArgumentsMissing : YAMPParseError
    {
        public YAMPIfArgumentsMissing(int line, int column)
            : base(line, column, "The if keyword requires one (1) argument given in round brackets.")
        {
        }

        public YAMPIfArgumentsMissing(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
