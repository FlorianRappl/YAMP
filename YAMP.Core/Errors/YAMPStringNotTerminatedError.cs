namespace YAMP.Errors
{
    using System;

    /// <summary>
    /// The string not terminated error.
    /// </summary>
    public class YAMPStringNotTerminatedError : YAMPParseError
    {
        public YAMPStringNotTerminatedError(Int32 line, Int32 column)
            : base(line, column, "The string starting at line {0}, column {1} has not been closed.", line, column)
        {
        }

        public YAMPStringNotTerminatedError(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
