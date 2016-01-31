namespace YAMP.Errors
{
    using System;

    /// <summary>
    /// The keyword not possible error.
    /// </summary>
    public class YAMPKeywordNotPossible : YAMPParseError
    {
        internal YAMPKeywordNotPossible(Int32 line, Int32 column, String keyword)
            : base(line, column, "The {0} keyword cannot be used in the given context.", keyword)
        {
        }

        internal YAMPKeywordNotPossible(ParseEngine pe, String keyword)
            : this(pe.CurrentLine, pe.CurrentColumn, keyword)
        {
        }
    }
}
