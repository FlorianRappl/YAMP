namespace YAMP.Errors
{
    using System;

    /// <summary>
    /// The single else error.
    /// </summary>
    public class YAMPSingleElseError : YAMPParseError
    {
        public YAMPSingleElseError(Int32 line, Int32 column)
            : base(line, column, "Cannot use two consecutive else blocks. Did you miss an if before?")
        {
        }

        public YAMPSingleElseError(ParseEngine pe)
            : this(pe.CurrentLine, pe.CurrentColumn)
        {
        }
    }
}
