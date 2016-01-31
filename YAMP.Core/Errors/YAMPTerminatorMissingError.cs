namespace YAMP.Errors
{
    using System;

    /// <summary>
    /// The terminator missing error.
    /// </summary>
    public class YAMPTerminatorMissingError : YAMPParseError
    {
        public YAMPTerminatorMissingError(Int32 line, Int32 column, Char terminator)
            : base(line, column, "The query has not been terminated. Missing terminator for {0} at line {1}, column {2}.", terminator, line, column)
        {
        }

        public YAMPTerminatorMissingError(ParseEngine pe, Char terminator)
            : this(pe.CurrentLine, pe.CurrentColumn, terminator)
        {
        }
    }
}
