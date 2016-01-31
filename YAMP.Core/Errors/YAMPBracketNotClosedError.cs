namespace YAMP.Errors
{
    using System;

    /// <summary>
    /// A bracket error occurred.
    /// </summary>
    public class YAMPBracketNotClosedError : YAMPParseError
    {
        public YAMPBracketNotClosedError(Int32 line, Int32 column) :
            base(line, column, "Missing bracket terminator for bracket starting at line {0}, column {1}.", line, column)
        {
        }

        public YAMPBracketNotClosedError(ParseEngine pe) :
            this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
