using System;

namespace YAMP
{
    class YAMPWhileArgumentsMissing : YAMPParseError
    {
        public YAMPWhileArgumentsMissing(int line, int column)
            : base(line, column, "The while keyword requires one (1) argument given in round brackets.")
        {
        }

        public YAMPWhileArgumentsMissing(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
