namespace YAMP.Errors
{
    using System;

    /// <summary>
    /// A bracket error occurred.
    /// </summary>
    public class YAMPBracketEmptyError : YAMPParseError
    {
        public YAMPBracketEmptyError(Int32 line, Int32 column)
            : base(line, column, "An unexpected bracket has been found. Are you missing something?")
        {
        }

        public YAMPBracketEmptyError(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
