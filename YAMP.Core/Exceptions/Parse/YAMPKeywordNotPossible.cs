using System;

namespace YAMP
{
    class YAMPKeywordNotPossible : YAMPParseError
    {
        public YAMPKeywordNotPossible(int line, int column, string keyword)
            : base(line, column, "The {0} keyword cannot be used in the given context.", keyword)
        {
        }

        public YAMPKeywordNotPossible(ParseEngine pe, string keyword)
            : this(pe.CurrentLine, pe.CurrentColumn, keyword)
        {
        }
    }
}
